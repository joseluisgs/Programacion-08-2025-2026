using RepositorioBinario.Models;

namespace RepositorioBinario.Repositories;

/// <summary>
///     Repositorio que persiste personas en un archivo binario.
///     Utiliza BinaryReader/BinaryWriter para serialización manual campo a campo.
/// </summary>
/// <remarks>
///     PROS:
///     - Portabilidad: Los datos son tipos primitivos simples (int, string) que cualquier lenguaje puede leer.
///     - Control total: Puedes definir exactamente el formato y orden de los datos.
///     - Legibilidad: Se puede inspeccionar el archivo con un editor hexadecimal.
///     - Compatibilidad: Fácil de leer desde otros lenguajes (Kotlin, Java, Python, C++, etc.)
///     - Rendimiento: Escritura y lectura muy rápidas.
///     CONTRAS:
///     - Requiere escribir el código de lectura/escritura manualmente.
///     - Si cambias la estructura del modelo, debes actualizar el código de serialización.
///     - El archivo no es auto-descriptivo (necesitas conocer el orden de los campos).
///     MULTILENGUAJE:
///     - Kotlin/Java: DataInputStream.readInt(), readUTF() para leer los mismos datos.
///     - Python: struct.unpack() o lectura directa de bytes.
///     - C/C++: Lectura directa de tipos primitivos del archivo.
///     - Cualquier lenguaje que maneje bytes puede interpretar este formato.
/// </remarks>
public class PersonasSecuencialRepository : IPersonasRepository {
    // Constante con la ruta del archivo binario donde se guardan los datos de las personas
    private const string FilePath = "Data/personas_secuencial.dat";

    // Variable estática (compartida entre todas las instancias) para generar IDs únicos
    private static int _nextId = 1;

    // Diccionario en memoria que actúa como caché de las personas
    // Clave: ID de la persona, Valor: objeto Persona
    private readonly Dictionary<int, Persona> _personas;

    /// <summary>
    ///     Constructor que carga los datos del archivo binario al iniciar.
    /// </summary>
    public PersonasSecuencialRepository() {
        // Crear directorio Data si no existe
        if (!Directory.Exists("Data"))
            Directory.CreateDirectory("Data");
        
        // Al crear el repositorio, carregamos los datos del archivo
        _personas = Load();
    }

    /// <summary>
    ///     Recupera todas las personas del repositorio.
    /// </summary>
    public IEnumerable<Persona> GetAll() {
        return _personas.Values;
    }

    /// <summary>
    ///     Busca una persona por su ID.
    /// </summary>
    /// <param name="id">ID de la persona a buscar.</param>
    /// <returns>La persona encontrada o null si no existe.</returns>
    public Persona? GetById(int id) {
        return _personas.GetValueOrDefault(id);
    }

    /// <summary>
    ///     Crea una nueva persona, asignándole un ID automático.
    /// </summary>
    /// <param name="entity">Persona a crear (sin ID).</param>
    /// <returns>La persona creada con el ID asignado.</returns>
    public Persona? Create(Persona entity) {
        // Generamos un nuevo ID y avanzamos el contador estático
        var id = _nextId++;
        // Creamos una copia del registro con el nuevo ID (using 'with')
        var nuevaPersona = entity with { Id = id };
        // Añadimos al diccionario
        _personas[id] = nuevaPersona;
        // Guardamos en archivo
        Save();
        return nuevaPersona;
    }

    /// <summary>
    ///     Actualiza los datos de una persona existente.
    /// </summary>
    /// <param name="id">ID de la persona a actualizar.</param>
    /// <param name="entity">Nuevos datos de la persona.</param>
    /// <returns>La persona actualizada o null si no existe.</returns>
    public Persona? Update(int id, Persona entity) {
        // Verificamos que la persona exista
        if (!_personas.ContainsKey(id))
            return null;

        // Creamos copia con el ID correcto
        var actualizada = entity with { Id = id };
        // Actualizamos en el diccionario
        _personas[id] = actualizada;
        // Guardamos en archivo
        Save();
        return actualizada;
    }

    /// <summary>
    ///     Elimina una persona del repositorio.
    /// </summary>
    /// <param name="id">ID de la persona a eliminar.</param>
    /// <returns>La persona eliminada o null si no existía.</returns>
    public Persona? Delete(int id) {
        // TryRemove retorna la persona eliminada si existía
        if (!_personas.Remove(id, out var persona))
            return null;

        // Guardamos en archivo
        Save();
        return persona;
    }

    /// <summary>
    ///     Lee todas las personas del archivo binario.
    ///     Formato del archivo:
    ///     - Entero: cantidad de personas
    ///     - Entero: siguiente ID a asignar
    ///     - Por cada persona: Id, Nombre, Edad, Email
    /// </summary>
    private Dictionary<int, Persona> Load() {
        if (!File.Exists(FilePath))
            return new Dictionary<int, Persona>();

        using var stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
        using var reader = new BinaryReader(stream);

        var cantidad = reader.ReadInt32();
        _nextId = reader.ReadInt32();

        var personas = new Dictionary<int, Persona>();

        for (var i = 0; i < cantidad; i++) {
            var id = reader.ReadInt32();
            var nombre = reader.ReadString();
            var edad = reader.ReadInt32();
            var email = reader.ReadString();

            personas[id] = new Persona(id, nombre, edad, email);
        }

        return personas;
    }

    /// <summary>
    ///     Guarda todo el diccionario de personas en el archivo binario.
    ///     Se llama después de cada modificación (Create, Update, Delete).
    /// </summary>
    private void Save() {
        // Creamos el archivo (sobrescribe si existe)
        using var stream = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
        // Creamos un BinaryWriter para escribir tipos primitivos
        using var writer = new BinaryWriter(stream);

        // Escribimos la cantidad de personas
        writer.Write(_personas.Count);
        // Escribimos el siguiente ID a usar
        writer.Write(_nextId);

        // Escribimos cada persona campo por campo
        foreach (var persona in _personas.Values) {
            writer.Write(persona.Id);
            writer.Write(persona.Nombre);
            writer.Write(persona.Edad);
            writer.Write(persona.Email);
        }
    }
}