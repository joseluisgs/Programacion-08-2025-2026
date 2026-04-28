using RepositorioBinario.Models;

namespace RepositorioBinario.Repositories;

/// <summary>
///     Repositorio que persiste personas usando acceso aleatorio con archivos separados.
///     Utiliza: personas.dat (heap), personas.idx (índice), personas.frx (huecos libres).
/// </summary>
/// <remarks>
///     PROS:
///     - Rendimiento: Solo lee/escribe lo modificado, no todo el archivo.
///     - Escalabilidad: Funciona con millones de registros sin cargar todo en memoria.
///     - Persistencia parcial: Si falla, puedes recuperar datos/índice/huecos por separado.
///     - Espacio: Reutiliza huecos sin reescribir todo.
///     - Acceso directo: El índice permite saltar al registro sin escanear.
///     - Portabilidad: Los archivos son legibles desde cualquier lenguaje.
///     CONTRAS:
///     - Complejidad: Más difícil de implementar y mantener.
///     - Consistencia: Debes mantener sincronización entre los tres archivos.
///     - Fragmentación: Con muchas modificaciones pueden acumularse huecos pequeños.
///     - Más archivos: Tres archivos en lugar de uno.
///     COMPARACIÓN CON OTRAS VERSIONES:
///     - PersonasBinaryRepository: Lee todo el archivo en cada operación O(n).
///     - PersonasSerialRepository: Lee/escribe todo el diccionario en cada operación O(n).
///     - PersonasRandomAccessRepository: Solo lee/escribe lo necesario O(1) por ID.
///     OPTIMIZACIÓN:
///     Si tienes 1M de registros, las versiones anteriores cargan TODO en memoria.
///     Esta versión solo carga el índice (~8MB para 1M de entradas) y accede directo al archivo.
///     Además, en modificaciones solo escribes los bytes del registro, no todo el diccionario.
///     Ideal para sistemas con muchas modificaciones y grandes volúmenes de datos.
///     MULTILENGUAJE:
///     - El archivo de datos (personas.dat) usa formato simple: string + int + string.
///     - Cualquier lenguaje puede leerlo interpretando: texto (con longitud en bytes .NET), edad, texto.
///     - Kotlin: DataInputStream.readUTF(), readInt().
///     - Java: DataInputStream igual.
///     - Python: struct o lectura directa de tipos .NET.
///     - El índice y huecos también son tipos primitivos simples.
/// </remarks>
public class PersonasRandomAccessRepository : IPersonasRepository {
    // ============================================================
    // CONSTANTES Y RUTAS DE ARCHIVOS
    // ============================================================

    /// <summary>
    ///     Archivo que contiene los registros de personas (el heap).
    ///     Formato: secuencia de registros de tamaño variable.
    ///     Cada registro contiene: nombre (string), edad (int), email (string).
    /// </summary>
    private const string FileDatos = "Data/personas.dat";

    /// <summary>
    ///     Archivo índice que mapea ID -> (offset, longitud).
    ///     Permite acceso directo a cada registro sin escanear.
    ///     Formato: [nextId][cantidad][(id, offset, longitud)*]
    /// </summary>
    private const string FileIndices = "Data/personas.idx";

    /// <summary>
    ///     Archivo de huecos libres (free list).
    ///     Lista de posiciones y longitudes de registros borrados.
    ///     Formato: [cantidad][(posicion, longitud)*]
    /// </summary>
    private const string FileHuecos = "Data/personas.frx";

    /// <summary>
    ///     Umbral de fragmentación (30%).
    ///     Si los huecos ocupan más del 30% del tamaño total del archivo, se compacta.
    ///     Esto evita que se acumulen demasiados huecos pequeños inútiles.
    /// </summary>
    private const double FragmentationThreshold = 0.3;

    // ============================================================
    // ATRIBUTOS
    // ============================================================

    /// <summary>
    ///     Contador estático para generar IDs únicos.
    ///     Se comparte entre todas las instancias y se persiste en el índice.
    /// </summary>
    private static int _nextId = 1;

    /// <summary>
    ///     Lista de huecos libres (free list).
    ///     Cada entrada representa un espacio dejado por un registro borrado.
    ///     Estructura: List
    ///     <(posicion, longitud)>
    ///         - posicion: offset en bytes donde empieza el hueco
    ///         - longitud: tamaño del hueco en bytes
    ///         Cuando se crea un nuevo registro:
    ///         1. Buscar en _huecos un hueco del tamaño adecuado
    ///         2. Si existe, reutilizarlo (escribir ahí)
    ///         3. Si no existe, escribir al final del archivo
    ///         Esto evita tener que reescribir todo el archivo en cada inserción.
    /// </summary>
    private readonly List<(long posicion, int longitud)> _huecos;

    /// <summary>
    ///     Índice en memoria: Diccionario que mapea ID de persona a su posición en el archivo.
    ///     Estructura: Dictionary
    ///     <id, ( offset, length)>
    ///         - offset: posición en bytes donde empieza el registro en personas.dat
    ///         - length: tamaño en bytes del registro completo
    ///         Este índice es el que permite el acceso aleatorio O(1):
    ///         Para buscar la persona con ID=5:
    ///         1. Consultar _indice[5] → obtener (offset, length)
    ///         2. file.Seek(offset) → positioned en el registro
    ///         3. Leer length bytes → obtener la persona
    /// </summary>
    private readonly Dictionary<int, (long offset, int length)> _indice;

    // ============================================================
    // CONSTRUCTOR
    // ============================================================

    /// <summary>
    ///     Constructor del repositorio.
    ///     Al crear una instancia, carga los archivos existentes en memoria.
    ///     Si no existen los archivos, crea estructuras vacías.
    /// </summary>
    public PersonasRandomAccessRepository() {
        // Crear directorio Data si no existe
        if (!Directory.Exists("Data"))
            Directory.CreateDirectory("Data");

        // Inicializamos las estructuras en memoria vacías
        _indice = new Dictionary<int, (long, int)>();
        _huecos = new List<(long, int)>();

        // Cargamos los datos de los archivos (si existen)
        CargarArchivos();
    }

    // ============================================================
    // OPERACIONES CRUD
    // ============================================================

    /// <summary>
    ///     Recupera todas las personas del repositorio.
    ///     NOTA: Esta operación no es eficiente porque fuerza a leer cada registro.
    ///     En un sistema real, tendríamos un flag en el índice para saber si está borrado
    ///     o tendríamos una lista de IDs válidos.
    ///     Complejidad: O(n) donde n es el número de registros.
    /// </summary>
    public IEnumerable<Persona> GetAll() {
        var personas = new List<Persona>();

        // Iteramos sobre las claves del índice
        foreach (var id in _indice.Keys) {
            // Obtenemos cada persona usando GetById
            var persona = GetById(id);
            if (persona != null)
                personas.Add(persona);
        }

        return personas;
    }

    /// <summary>
    ///     Busca una persona por su ID usando acceso aleatorio.
    ///     ALGORITMO:
    ///     1. Consultar el índice para obtener (offset, length) de esa ID
    ///     2. Si no existe, retornar null
    ///     3. Seek al offset en el archivo de datos
    ///     4. Leer length bytes
    ///     5. Deserializar los bytes a Persona
    ///     Complejidad: O(1) - acceso directo, no hay que escanear nada.
    /// </summary>
    /// <param name="id">ID de la persona a buscar</param>
    /// <returns>La persona o null si no existe</returns>
    public Persona? GetById(int id) {
        // Paso 1: Consultar el índice
        if (!_indice.TryGetValue(id, out var slot))
            return null;

        try {
            // Paso 2: Abrir archivo de datos
            using var stream = new FileStream(FileDatos, FileMode.Open, FileAccess.Read);

            // Paso 3: Ir directamente a la posición del registro (acceso aleatorio)
            stream.Seek(slot.offset, SeekOrigin.Begin);

            // Paso 4: Leer los bytes del registro
            var bytes = new byte[slot.length];
            var totalLeido = 0;
            while (totalLeido < slot.length) {
                // Read puede no leer todos los bytes de una vez, usamos un loop
                var leido = stream.Read(bytes, totalLeido, slot.length - totalLeido);
                if (leido == 0) break; // Fin de archivo inesperado
                totalLeido += leido;
            }

            // Paso 5: Convertir bytes a objeto Persona (usando el ID correcto)
            var personaSinId = DeserializePersona(bytes);
            return personaSinId with { Id = id };
        }
        catch {
            return null;
        }
    }

    /// <summary>
    ///     Crea una nueva persona.
    ///     ALGORITMO:
    ///     1. Asignar un nuevo ID (incremental)
    ///     2. Serializar la persona a bytes
    ///     3. Buscar un hueco libre del tamaño adecuado
    ///     4. Si existe hueco, escribir ahí; si no, escribir al final
    ///     5. Actualizar el índice con la nueva posición
    ///     6. Si reutilizamos un hueco, eliminarlo de la lista
    ///     7. Guardar índice y huecos en disco
    ///     Complejidad: O(n) para buscar hueco + O(1) para escribir.
    /// </summary>
    /// <param name="entity">Persona a crear (sin ID)</param>
    /// <returns>Persona con el ID asignado</returns>
    public Persona? Create(Persona entity) {
        // Paso 1: Generar nuevo ID
        var id = _nextId++;

        // Crear la persona con el ID asignado
        var persona = entity with { Id = id };

        // Paso 2: Convertir a bytes
        var datos = SerializePersona(persona);

        // Paso 3: Buscar un hueco disponible
        var (offset, reutilizado) = BuscarHueco(datos.Length);

        // Paso 4: Escribir el registro
        using var stream = new FileStream(FileDatos, FileMode.OpenOrCreate, FileAccess.Write);

        // Si no reutilizamos un hueco, escribimos al final del archivo
        stream.Seek(0, SeekOrigin.End);
        if (!reutilizado)
            offset = stream.Position;

        // Escribir en la posición correspondiente
        stream.Seek(offset, SeekOrigin.Begin);
        stream.Write(datos, 0, datos.Length);

        // Paso 5: Actualizar índice con la posición del nuevo registro
        _indice[id] = (offset, datos.Length);

        // Paso 6: Si reutilizamos un hueco, eliminarlo de la lista
        if (reutilizado)
            _huecos.RemoveAt(BuscarIndiceHueco(offset));

        // Paso 7: Persistir los cambios
        GuardarIndices();
        GuardarHuecos();

        return persona;
    }

    /// <summary>
    ///     Actualiza una persona existente.
    ///     ALGORITMO:
    ///     1. Verificar que existe la persona
    ///     2. Serializar los nuevos datos
    ///     3. Si el tamaño es igual: sobrescribir en la misma posición
    ///     4. Si el tamaño es diferente: escribir al final y actualizar índice
    ///     Complejidad: O(1) si el tamaño es igual, O(n) si se recrea.
    /// </summary>
    /// <param name="id">ID de la persona a actualizar</param>
    /// <param name="entity">Nuevos datos</param>
    /// <returns>Persona actualizada o null si no existe</returns>
    public Persona? Update(int id, Persona entity) {
        // Verificar que existe
        if (!_indice.TryGetValue(id, out var slot))
            return null;

        // Preparar la persona con el ID correcto
        var nuevaPersona = entity with { Id = id };
        var nuevosDatos = SerializePersona(nuevaPersona);

        // Verificar si el tamaño es el mismo
        if (nuevosDatos.Length == slot.length) {
            // Sobrescribir en la misma posición (acceso directo)
            using var stream = new FileStream(FileDatos, FileMode.Open, FileAccess.Write);
            stream.Seek(slot.offset, SeekOrigin.Begin);
            stream.Write(nuevosDatos, 0, nuevosDatos.Length);
        }
        else {
            // El tamaño ha cambiado: escribir al final y actualizar índice
            // No hacemos delete porque el hueco se reutilizará automáticamente
            using var stream = new FileStream(FileDatos, FileMode.Open, FileAccess.Write);
            stream.Seek(0, SeekOrigin.End);
            var nuevaPosicion = stream.Position;
            stream.Write(nuevosDatos, 0, nuevosDatos.Length);

            // Actualizar el índice con la nueva posición
            _indice[id] = (nuevaPosicion, nuevosDatos.Length);
            GuardarIndices();
        }

        return nuevaPersona;
    }

    /// <summary>
    ///     Elimina una persona (borrado lógico + gestión de huecos).
    ///     ALGORITMO:
    ///     1. Verificar que existe la persona
    ///     2. Añadir su posición a la lista de huecos libres
    ///     3. Eliminar del índice
    ///     4. Guardar cambios
    ///     5. Comprobar fragmentación y compactar si es necesario
    ///     NOTA: Este es un borrado lógico, no se elimina el espacio físicamente.
    ///     El espacio queda disponible para reutilizarse en futuras creaciones.
    /// </summary>
    /// <param name="id">ID de la persona a eliminar</param>
    /// <returns>Persona eliminada o null si no existía</returns>
    public Persona? Delete(int id) {
        // Verificar que existe
        if (!_indice.TryGetValue(id, out var slot))
            return null;

        // Obtener los datos de la persona (para retornarla)
        var persona = GetById(id);
        if (persona == null)
            return null;

        // Añadir a la lista de huecos (espacio disponible para reutilizar)
        _huecos.Add((slot.offset, slot.length));

        // Eliminar del índice
        _indice.Remove(id);

        // Persistir los cambios
        GuardarIndices();
        GuardarHuecos();

        // Comprobar si hay demasiada fragmentación
        ComprobarFragmentacion();

        return persona;
    }

    // ============================================================
    // MÉTODOS DE CARGA Y GUARDADO
    // ============================================================

    /// <summary>
    ///     Método que coordina la carga de todos los archivos.
    ///     Se llama al iniciar el repositorio.
    /// </summary>
    private void CargarArchivos() {
        // Cargamos primero el índice y después los huecos
        CargarIndices();
        CargarHuecos();
    }

    /// <summary>
    ///     Carga el archivo de índice en memoria.
    ///     Formato del archivo personas.idx:
    ///     - 4 bytes: siguiente ID a asignar (_nextId)
    ///     - 4 bytes: cantidad de entradas en el índice
    ///     - Por cada entrada:
    ///     - 4 bytes: ID de la persona
    ///     - 8 bytes: offset (posición en el archivo de datos)
    ///     - 4 bytes: longitud del registro
    ///     Ejemplo para 3 personas:
    ///     [4][3][id1, off1, len1][id2, off2, len2][id3, off3, len3]
    /// </summary>
    private void CargarIndices() {
        // Si no existe el archivo, no hay nada que cargar
        if (!File.Exists(FileIndices))
            return;

        try {
            // Abrimos el archivo para lectura
            using var stream = new FileStream(FileIndices, FileMode.Open, FileAccess.Read);
            using var reader = new BinaryReader(stream);

            // Leemos el siguiente ID disponible
            _nextId = reader.ReadInt32();
            // Leemos la cantidad de entradas
            var cantidad = reader.ReadInt32();

            // Por cada entrada, leemos ID, offset y longitud
            for (var i = 0; i < cantidad; i++) {
                var id = reader.ReadInt32();
                var offset = reader.ReadInt64();
                var longitud = reader.ReadInt32();

                // Añadimos al diccionario en memoria
                _indice[id] = (offset, longitud);
            }
        }
        catch {
            // Si hay cualquier error (archivo corrupto), simplemente no cargamos nada
            // El repositorio funcionará como si estuviera vacío
        }
    }

    /// <summary>
    ///     Carga el archivo de huecos libres en memoria.
    ///     Formato del archivo personas.frx:
    ///     - 4 bytes: cantidad de huecos
    ///     - Por cada hueco:
    ///     - 8 bytes: posición (offset)
    ///     - 4 bytes: longitud
    ///     Estos huecos vienen de registros borrados anteriormente.
    ///     Se reutilizan cuando se crean nuevos registros del tamaño adecuado.
    /// </summary>
    private void CargarHuecos() {
        if (!File.Exists(FileHuecos))
            return;

        using var stream = new FileStream(FileHuecos, FileMode.Open, FileAccess.Read);
        using var reader = new BinaryReader(stream);

        var cantidad = reader.ReadInt32();
        for (var i = 0; i < cantidad; i++) {
            var posicion = reader.ReadInt64();
            var longitud = reader.ReadInt32();
            _huecos.Add((posicion, longitud));
        }
    }

    /// <summary>
    ///     Guarda el índice en el archivo.
    ///     Se llama después de cada operación que modifique el índice.
    ///     Importante: Escribe todo el índice de una vez.
    ///     En un sistema real, quizás solo escribiríamos las partes modificadas.
    /// </summary>
    private void GuardarIndices() {
        using var stream = new FileStream(FileIndices, FileMode.Create, FileAccess.Write);
        using var writer = new BinaryWriter(stream);

        // Escribimos el siguiente ID
        writer.Write(_nextId);
        // Escribimos la cantidad de entradas
        writer.Write(_indice.Count);

        // Escribimos cada entrada: ID + offset + longitud
        foreach (var kvp in _indice) {
            writer.Write(kvp.Key);
            writer.Write(kvp.Value.offset);
            writer.Write(kvp.Value.length);
        }
    }

    /// <summary>
    ///     Guarda la lista de huecos en el archivo.
    ///     Se llama después de cada Delete o cuando se reutiliza un hueco.
    /// </summary>
    private void GuardarHuecos() {
        using var stream = new FileStream(FileHuecos, FileMode.Create, FileAccess.Write);
        using var writer = new BinaryWriter(stream);

        // Escribimos la cantidad de huecos
        writer.Write(_huecos.Count);

        // Escribimos cada hueco: posición + longitud
        foreach (var (posicion, longitud) in _huecos) {
            writer.Write(posicion);
            writer.Write(longitud);
        }
    }

    // ============================================================
    // MÉTODOS AUXILIARES
    // ============================================================

    /// <summary>
    ///     Busca un hueco libre del tamaño adecuado.
    ///     ALGORITMO:
    ///     - Recorrer la lista de huecos
    ///     - Devolver el primer hueco cuya longitud sea >= al tamaño necesario
    ///     - Si no hay ninguno, devolver (0, false) indicando que hay que escribir al final
    ///     Nota: Esta implementación es simple (first-fit).
    ///     Podría mejorarse con: best-fit (el más pequeño que sirve),
    ///     ordenamiento de huecos, etc.
    /// </summary>
    /// <param name="longitud">Tamaño necesario en bytes</param>
    /// <returns>(offset, reutilizado) - si reutilizado es true, usar ese hueco</returns>
    private (long offset, bool reutilizado) BuscarHueco(int longitud) {
        // Buscar el primer hueco que sea suficientemente grande
        // for (var i = 0; i < _huecos.Count; i++)
        //     if (_huecos[i].longitud >= longitud)
        //         return (_huecos[i].posicion, true);

        // Vamos a usar Linq para encontrar el primer hueco adecuado,
        // Para no desperdiciar, buscamos el minimo que sea >= longitud
        var hueco = _huecos.Where(h => h.longitud >= longitud)
            .OrderBy(h => h.longitud) // Ordenamos por tamaño para encontrar el mejor
            .FirstOrDefault();

        if (hueco != default)
            return (hueco.posicion, true);


        // No hay hueco disponible, indicar que se escriba al final
        return (0, false);
    }

    /// <summary>
    ///     Busca el índice de un hueco en la lista por su posición.
    ///     Se usa para eliminar el hueco cuando se reutiliza.
    /// </summary>
    private int BuscarIndiceHueco(long posicion) {
        /*for (var i = 0; i < _huecos.Count; i++)
            if (_huecos[i].posicion == posicion)
                return i;
        return -1;*/

        // Usamos Linq para encontrar el índice del hueco por su posición
        var index = _huecos.FindIndex(h => h.posicion == posicion);
        return index;
    }

    /// <summary>
    ///     Serializa una persona a un array de bytes.
    ///     Usa BinaryWriter para escribir tipos primitivos de forma portable.
    ///     Formato: [nombre(string)][edad(int)][email(string)]
    ///     Cada string se escribe con su longitud precediendo los bytes (formato .NET).
    /// </summary>
    private byte[] SerializePersona(Persona p) {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);

        // Escribir campos en orden
        writer.Write(p.Nombre);
        writer.Write(p.Edad);
        writer.Write(p.Email);

        return ms.ToArray();
    }

    /// <summary>
    ///     Deserializa un array de bytes a una persona.
    ///     Debe leer los campos en el mismo orden que SerializePersona.
    /// </summary>
    private Persona DeserializePersona(byte[] datos) {
        using var ms = new MemoryStream(datos);
        using var reader = new BinaryReader(ms);

        var nombre = reader.ReadString();
        var edad = reader.ReadInt32();
        var email = reader.ReadString();

        // El ID se pone a 0 porque viene del archivo de datos (no tiene el ID)
        return new Persona(0, nombre, edad, email);
    }

    // ============================================================
    // COMPACTACIÓN / DEFRAGMENTACIÓN
    // ============================================================

    /// <summary>
    ///     Calcula el porcentaje de fragmentación del archivo.
    ///     Fórmula: (suma de tamaños de huecos) / (tamaño total del archivo)
    ///     Si el resultado es 0.4 (40%), significa que el 40% del archivo
    ///     son espacios vacíos dejado por registros borrados.
    ///     Ejemplo:
    ///     - Archivo de datos: 1000 bytes
    ///     - Huecos: 300 bytes
    ///     - Fragmentación: 300/1000 = 0.3 (30%)
    /// </summary>
    private double CalcularFragmentacion() {
        // Si no existe el archivo, no hay fragmentación
        if (!File.Exists(FileDatos))
            return 0;

        var tamanoArchivo = new FileInfo(FileDatos).Length;
        if (tamanoArchivo == 0)
            return 0;

        // Sumar todos los tamaños de huecos
        var tamanoHuecos = _huecos.Sum(h => h.longitud);

        // Calcular porcentaje
        return (double)tamanoHuecos / tamanoArchivo;
    }

    /// <summary>
    ///     Compacta el archivo de datos eliminando todos los huecos.
    ///     ALGORITMO:
    ///     1. Leer todos los registros existentes
    ///     2. Crear un nuevo archivo de datos vacío
    ///     3. Escribir todos los registros secuencialmente (sin huecos)
    ///     4. Actualizar el índice con las nuevas posiciones
    ///     5. Vaciar la lista de huecos
    ///     6. Guardar índice y huecos (vacíos)
    ///     Esta operación es costosa (O(n)) pero elimina la fragmentación.
    ///     Se usa solo cuando la fragmentación supera el umbral.
    /// </summary>
    private void Compactar() {
        // Si no hay registros, no hay nada que compactar
        if (_indice.Count == 0)
            return;

        // Paso 1: Leer todos los registros actuales
        var nuevosRegistros = new Dictionary<int, (Persona persona, byte[] datos)>();

        foreach (var id in _indice.Keys) {
            var persona = GetById(id);
            if (persona != null) {
                var datos = SerializePersona(persona);
                nuevosRegistros[id] = (persona, datos);
            }
        }

        // Paso 2-5: Reescribir todo el archivo sin huecos
        using var stream = new FileStream(FileDatos, FileMode.Create, FileAccess.Write);

        // Limpiar estructuras en memoria
        _indice.Clear();
        _huecos.Clear();

        // Escribir cada registro secuencialmente
        foreach (var (id, (_, datos)) in nuevosRegistros) {
            var offset = stream.Position;
            stream.Write(datos, 0, datos.Length);
            _indice[id] = (offset, datos.Length);
        }

        // Paso 6: Guardar los archivos actualizados
        GuardarIndices();
        GuardarHuecos();
    }

    /// <summary>
    ///     Comprueba si la fragmentación supera el umbral y compacta si es necesario.
    ///     Se llama después de cada Delete.
    ///     Umbral actual: 30% (definido en FragmentationThreshold)
    /// </summary>
    private void ComprobarFragmentacion() {
        // Calcular la fragmentación actual
        if (CalcularFragmentacion() > FragmentationThreshold)
            // Hay demasiada fragmentación: compactar
            Compactar();
    }
}