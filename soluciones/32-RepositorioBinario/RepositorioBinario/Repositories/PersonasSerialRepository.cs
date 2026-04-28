using System.Runtime.Serialization.Formatters.Binary;
using RepositorioBinario.Models;

namespace RepositorioBinario.Repositories;

/// <summary>
///     Repositorio que persiste personas usando serialización binaria automática.
///     Utiliza BinaryFormatter para serializar el diccionario completo.
/// </summary>
/// <remarks>
///     PROS:
///     - Simplicidad: No necesitas escribir código de lectura/escritura manual.
///     - autometadata: El propio formato incluye información de tipos y estructuras.
///     - Refactorización friendly: Si cambias el modelo, sigue funcionando (con cautions).
///     - Integración nativa: Forma parte del ecosistema .NET desde hace décadas.
///     CONTRAS:
///     - OBSOLETO: BinaryFormatter está marcado como obsoleto desde .NET 5 por problemas de seguridad. No se recomienda su
///     uso en producción.
///     - NO portable: Solo funciona en .NET. Olvídate de leerlo en Kotlin, Java, Python, etc.
///     - Formato cerrado: Depende de la implementación interna de .NET, que puede cambiar.
///     - Rendimiento: Más lento que la serialización manual.
///     - Seguridad: Vulnerable a ataques de deserialización maliciosos.
///     MULTILENGUAJE:
///     - IMPOSIBLE de leer desde otros lenguajes.
///     - Kotlin: No existe equivalente directo. Tendrías que reimplementar la especificación interna de .NET (imposible).
///     - Java: No compatible. Ni con ObjectInputStream ni con ningún otro mecanismo.
///     - Python: No hay librería que lo interprete.
///     - Este formato solo funciona dentro del ecosistema .NET.
/// </remarks>
public class PersonasSerialRepository : IPersonasRepository {
    // Ruta del archivo donde se guardan los datos de manera serializada
    private const string FilePath = "Data/personas_serial.dat";

    // Variable estática para generar IDs únicos (se guarda en el archivo)
    private static int _nextId = 1;

    // Diccionario en memoria con las personas
    private readonly Dictionary<int, Persona> _personas;

    /// <summary>
    ///     Constructor que carga los datos del archivo serializado al iniciar.
    /// </summary>
    public PersonasSerialRepository() {
        // Crear directorio Data si no existe
        if (!Directory.Exists("Data"))
            Directory.CreateDirectory("Data");
        
        _personas = Load();
    }

    /// <summary>
    ///     Recupera todas las personas del repositorio.
    /// </summary>
    public IEnumerable<Persona> GetAll() {
        return _personas.Values.ToList();
    }

    /// <summary>
    ///     Busca una persona por su ID.
    /// </summary>
    public Persona? GetById(int id) {
        return _personas.GetValueOrDefault(id);
    }

    /// <summary>
    ///     Crea una nueva persona con ID automático.
    /// </summary>
    public Persona? Create(Persona entity) {
        // Generamos nuevo ID
        var id = _nextId++;
        // Creamos copia con el ID asignado
        var nuevaPersona = entity with { Id = id };
        // Añadimos al diccionario
        _personas[id] = nuevaPersona;
        // Guardamos en archivo
        Save();
        return nuevaPersona;
    }

    /// <summary>
    ///     Actualiza una persona existente.
    /// </summary>
    public Persona? Update(int id, Persona entity) {
        if (!_personas.ContainsKey(id))
            return null;

        var actualizada = entity with { Id = id };
        _personas[id] = actualizada;
        Save();
        return actualizada;
    }

    /// <summary>
    ///     Elimina una persona.
    /// </summary>
    public Persona? Delete(int id) {
        if (!_personas.Remove(id, out var persona))
            return null;

        Save();
        return persona;
    }

    /// <summary>
    ///     Carga los datos desde el archivo binario usando deserialización.
    /// </summary>
    private Dictionary<int, Persona> Load() {
        if (!File.Exists(FilePath))
            return new Dictionary<int, Persona>();

        using var stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
        var formatter = new BinaryFormatter();
        var data = (SerialData?)formatter.Deserialize(stream);

        if (data != null) {
            _nextId = data.NextId;
            return data.Personas;
        }

        return new Dictionary<int, Persona>();
    }

    /// <summary>
    ///     Guarda todo el diccionario en el archivo usando serialización binaria.
    ///     Se llama después de cada modificación.
    /// </summary>
    private void Save() {
        // Creamos el archivo (sobrescribe si existe)
        using var stream = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
        // Creamos el formateador binario
        var formatter = new BinaryFormatter();
        // Serializamos el objeto SerialData que contiene todo
        formatter.Serialize(stream, new SerialData(_personas, _nextId));
    }

    /// <summary>
    ///     Clase auxiliar para serializar el diccionario y el contador de IDs.
    ///     Debe estar marcada como [Serializable] para que BinaryFormatter pueda serializarla.
    /// </summary>
    [Serializable]
    private class SerialData {
        public SerialData(Dictionary<int, Persona> personas, int nextId) {
            Personas = personas;
            NextId = nextId;
        }

        // Diccionario de personas a serializar
        public Dictionary<int, Persona> Personas { get; }

        // Siguiente ID a usar
        public int NextId { get; }
    }
}