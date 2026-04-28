using RepositorioBinario.Models;
using RepositorioBinario.Repositories;

// Limpiar archivos anteriores para empezar de cero
LimpiarArchivos();

Console.WriteLine("=== PRUEBA DE REPOSITORIOS BINARIOS ===");
Console.WriteLine();

// Lista de 10 personas para probar
var personasPrueba = new List<Persona> {
    new(0, "Juan Pérez", 25, "juan@email.com"),
    new(0, "María García", 30, "maria@email.com"),
    new(0, "Carlos López", 45, "carlos@email.com"),
    new(0, "Ana Martínez", 28, "ana@email.com"),
    new(0, "Pedro Sánchez", 35, "pedro@email.com"),
    new(0, "Laura Rodríguez", 22, "laura@email.com"),
    new(0, "Miguel González", 50, "miguel@email.com"),
    new(0, "Carmen Fernández", 33, "carmen@email.com"),
    new(0, "David Díaz", 27, "david@email.com"),
    new(0, "Elena Torres", 41, "elena@email.com")
};

// Ejecutar pruebas con cada repositorio
// NOTA: PersonasSerialRepository no funciona en .NET 9+ porque BinaryFormatter fue eliminado
#pragma warning disable CS8321 // Función local no usada
// ProbarRepositorioSerial(personasPrueba);
#pragma warning restore CS8321
Console.WriteLine("--- PersonasSerialRepository DESHABILITADO (BinaryFormatter eliminado en .NET 9+) ---");
Console.WriteLine();

ProbarRepositorioSecuencial(personasPrueba);
Console.WriteLine();

ProbarRepositorioRandomAccess(personasPrueba);
Console.WriteLine();

Console.WriteLine("=== PRUEBAS COMPLETADAS ===");

/// <summary>
/// Limpia los archivos de datos para empezar de cero.
/// </summary>
void LimpiarArchivos() {
    if (Directory.Exists("Data")) {
        var archivos = Directory.GetFiles("Data");
        foreach (var archivo in archivos) {
            File.Delete(archivo);
            Console.WriteLine($"  [LIMPIEZA] Eliminado: {archivo}");
        }
    }
    Console.WriteLine();
}


#if false
void ProbarRepositorioSerial(List<Persona> personas) {
    Console.WriteLine("--- REPOSITORIO SERIAL (BinaryFormatter) ---");

    var repo = new PersonasSerialRepository();

    // CREATE - Insertar todas las personas
    Console.WriteLine("\n[CREATE] Insertando personas...");
    foreach (var persona in personas) {
        var creada = repo.Create(persona);
        Console.WriteLine($"  Creada: {creada?.Nombre} (ID: {creada?.Id})");
    }

    // READ - Obtener todas
    Console.WriteLine("\n[READ] Obteniendo todas las personas...");
    var todas = repo.GetAll();
    foreach (var p in todas)
        Console.WriteLine($"  {p.Id}: {p.Nombre}, {p.Edad} años, {p.Email}");

    // READ - Obtener por ID
    Console.WriteLine("\n[READ] Obteniendo persona con ID 3...");
    var persona3 = repo.GetById(3);
    Console.WriteLine($"  Encontrada: {persona3?.Nombre}");

    // UPDATE - Actualizar una persona
    Console.WriteLine("\n[UPDATE] Actualizando persona con ID 2...");
    var actualizada = repo.Update(2, new Persona(2, "María García Modificada", 31, "maria.modificada@email.com"));
    Console.WriteLine($"  Actualizada: {actualizada?.Nombre}");

    // DELETE - Eliminar una persona
    Console.WriteLine("\n[DELETE] Eliminando persona con ID 5...");
    var eliminada = repo.Delete(5);
    Console.WriteLine($"  Eliminada: {eliminada?.Nombre}");

    // READ - Verificar eliminación
    Console.WriteLine("\n[READ] Personas después de eliminar...");
    Console.WriteLine($"  Total: {repo.GetAll().Count()} personas");
}
#endif


void ProbarRepositorioSecuencial(List<Persona> personas) {
    Console.WriteLine("--- REPOSITORIO SECUENCIAL (BinaryReader/Writer) ---");

    var repo = new PersonasSecuencialRepository();

    // CREATE
    Console.WriteLine("\n[CREATE] Insertando 10 personas...");
    foreach (var persona in personas) {
        var creada = repo.Create(persona);
        Console.WriteLine($"  ✓ {creada?.Nombre} (ID: {creada?.Id})");
    }

    // READ - Obtener todas
    Console.WriteLine("\n[READ] Obteniendo todas las personas...");
    var todas = repo.GetAll();
    foreach (var p in todas)
        Console.WriteLine($"  ID {p.Id}: {p.Nombre}, {p.Edad} años, {p.Email}");
    Console.WriteLine($"  >> Total: {todas.Count()} personas");

    // READ - Obtener por ID específico
    Console.WriteLine("\n[READ] Buscando persona con ID 7...");
    var persona7 = repo.GetById(7);
    Console.WriteLine($"  >> {(persona7 != null ? $"Encontrada: {persona7.Nombre}" : "No encontrada")}");

    // UPDATE - Actualizar
    Console.WriteLine("\n[UPDATE] Actualizando persona con ID 4...");
    var actualizada = repo.Update(4, new Persona(4, "Ana Martínez MODIFICADA", 29, "ana.nueva@email.com"));
    Console.WriteLine($"  >> {(actualizada != null ? $"OK: {actualizada.Nombre}" : "Error: No se encontró")}");

    // Verificar actualización
    Console.WriteLine("\n[VERIFY] Verificando actualización...");
    var persona4Actualizada = repo.GetById(4);
    Console.WriteLine($"  >> Persona ID 4 ahora es: {persona4Actualizada?.Nombre}");

    // DELETE - Eliminar
    Console.WriteLine("\n[DELETE] Eliminando persona con ID 8...");
    var eliminada = repo.Delete(8);
    Console.WriteLine($"  >> {(eliminada != null ? $"OK: {eliminada.Nombre} eliminada" : "Error: No se encontró")}");

    // READ - Verificar después de eliminar
    Console.WriteLine("\n[READ] Personas después de eliminar...");
    var final = repo.GetAll();
    foreach (var p in final)
        Console.WriteLine($"  ID {p.Id}: {p.Nombre}");
    Console.WriteLine($"  >> Total: {final.Count()} personas (antes había 10, ahora {final.Count()})");
}

void ProbarRepositorioRandomAccess(List<Persona> personas) {
    Console.WriteLine("--- REPOSITORIO RANDOM ACCESS (Índice + Huecos) ---");

    var repo = new PersonasRandomAccessRepository();

    // CREATE
    Console.WriteLine("\n[CREATE] Insertando 10 personas...");
    foreach (var persona in personas) {
        var creada = repo.Create(persona);
        Console.WriteLine($"  ✓ {creada?.Nombre} (ID: {creada?.Id})");
    }

    // READ - Obtener todas
    Console.WriteLine("\n[READ] Obteniendo todas las personas...");
    var todas = repo.GetAll();
    foreach (var p in todas)
        Console.WriteLine($"  ID {p.Id}: {p.Nombre}, {p.Edad} años, {p.Email}");
    Console.WriteLine($"  >> Total: {todas.Count()} personas");

    // READ - Obtener por ID específico (acceso directo O(1))
    Console.WriteLine("\n[READ] Buscando persona con ID 6 (acceso directo O(1))...");
    var persona6 = repo.GetById(6);
    Console.WriteLine($"  >> {(persona6 != null ? $"Encontrada: {persona6.Nombre}" : "No encontrada")}");

    // UPDATE - Actualizar
    Console.WriteLine("\n[UPDATE] Actualizando persona con ID 1...");
    var actualizada = repo.Update(1, new Persona(1, "Juan Pérez MODIFICADO", 26, "juan.nuevo@email.com"));
    Console.WriteLine($"  >> {(actualizada != null ? $"OK: {actualizada.Nombre}" : "Error: No se encontró")}");

    // Verificar actualización
    Console.WriteLine("\n[VERIFY] Verificando actualización...");
    var persona1Actualizada = repo.GetById(1);
    Console.WriteLine($"  >> Persona ID 1 ahora es: {persona1Actualizada?.Nombre}");

    // DELETE - Eliminar
    Console.WriteLine("\n[DELETE] Eliminado persona con ID 9...");
    var eliminada = repo.Delete(9);
    Console.WriteLine($"  >> {(eliminada != null ? $"OK: {eliminada.Nombre} eliminada" : "Error: No se encontró")}");

    // READ - Verificar después de eliminar
    Console.WriteLine("\n[READ] Personas después de eliminar...");
    var final = repo.GetAll();
    foreach (var p in final)
        Console.WriteLine($"  ID {p.Id}: {p.Nombre}");
    Console.WriteLine($"  >> Total: {final.Count()} personas (antes había 10, ahora {final.Count()})");
}