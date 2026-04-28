using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using GestionAcademica.Cache;
using GestionAcademica.Config;
using GestionAcademica.Enums;
using GestionAcademica.Exceptions;
using GestionAcademica.Factories;
using GestionAcademica.Models;
using GestionAcademica.Repositories;
using GestionAcademica.Services;
using GestionAcademica.Storage.Json;
using GestionAcademica.Validators;
using Serilog;
using static System.Console;

// ====================================================================
// GESTIÓN ACADÉMICA - CONFIGURACIÓN INICIAL
// ====================================================================

var loggerConfiguration = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console(
        outputTemplate: "{Timestamp:HH:mm:ss.fff} [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

Log.Logger = loggerConfiguration;

Title = "🎓 Sistema de Gestión Académica - DAW";
OutputEncoding = Encoding.UTF8;
Clear();

Main();

Log.CloseAndFlush();
WriteLine("\n⌨️  Presiona una tecla para salir...");
ReadKey();
return;

// --------------------------------------------------------------------
// FLUJO PRINCIPAL
// --------------------------------------------------------------------

void Main() {
    // Inicialización del servicio con repositorio, validadores y caché LRU.
    // Inyectamos las dependencias manualmente.
    IPersonasService service = new PersonasService(
        PersonasRepository.Instance,
        new AcademiaJsonStorage(),
        new ValidadorEstudiante(),
        new ValidadorDocente(),
        new LruCache<int, Persona>(3)
    );


    // Metemos los datos de prueba...
    PersonasFactory.Seed().ToList().ForEach(p => service.Save(p));


    OpcionMenu opcion;
    const string RegexOpcionMenu = @"^([0-9]|1[0-5])$";

    WriteLine("🚀 SISTEMA DE GESTIÓN ACADÉMICA (ESTILO DAW)");
    WriteLine(new string('━', 45));

    do {
        MostrarMenu();

        var opcionStr = LeerCadenaValidada("👉 Seleccione una opción: ", RegexOpcionMenu, "Opción no válida (0-13).");
        var opcionValue = int.Parse(opcionStr);
        opcion = (OpcionMenu)opcionValue;

        switch (opcion) {
            case OpcionMenu.ListarTodas: ListarTodas(service); break;
            case OpcionMenu.BuscarDni: BuscarPorDniGeneral(service); break;
            case OpcionMenu.BuscarId: BuscarPorIdGeneral(service); break;
            case OpcionMenu.ListarEstudiantes: ListarEstudiantes(service); break;
            case OpcionMenu.AnadirEstudiante: AnadirNuevoEstudiante(service); break;
            case OpcionMenu.ActualizarEstudiante: ActualizarEstudiante(service); break;
            case OpcionMenu.EliminarEstudiante: EliminarEstudiante(service); break;
            case OpcionMenu.InformeEstudiantes: MostrarInformeEstudiantes(service); break;
            case OpcionMenu.ListarDocentes: ListarDocentes(service); break;
            case OpcionMenu.AnadirDocente: AnadirNuevoDocente(service); break;
            case OpcionMenu.ActualizarDocente: ActualizarDocente(service); break;
            case OpcionMenu.EliminarDocente: EliminarDocente(service); break;
            case OpcionMenu.InformeDocentes: MostrarInformeDocentes(service); break;
            case OpcionMenu.ImportarDatos: ImportarDatos(service); break;
            case OpcionMenu.ExportarDatos: ExportarDatos(service); break;
            case OpcionMenu.Salir: WriteLine("\n👋 Cerrando el sistema. ¡Hasta pronto!"); break;
        }

        if (opcion != OpcionMenu.Salir) {
            WriteLine("\n⌨️  Presione una tecla para continuar...");
            ReadKey();
        }
    } while (opcion != OpcionMenu.Salir);
}

void MostrarMenu() {
    WriteLine("\n📋 --- 1. OPERACIONES GENERALES ---");
    WriteLine(new string('─', 45));
    WriteLine($"  {(int)OpcionMenu.ListarTodas}. 👥 Listar todo el personal");
    WriteLine($"  {(int)OpcionMenu.BuscarDni}. 🔍 Buscar persona por DNI");
    WriteLine($"  {(int)OpcionMenu.BuscarId}. 🆔 Buscar persona por ID");

    WriteLine("\n🎓 --- 2. GESTIÓN DE ESTUDIANTES ---");
    WriteLine(new string('─', 45));
    WriteLine($"  {(int)OpcionMenu.ListarEstudiantes}. 📜 Listar Estudiantes");
    WriteLine($"  {(int)OpcionMenu.AnadirEstudiante}. ➕ Añadir Estudiante");
    WriteLine($"  {(int)OpcionMenu.ActualizarEstudiante}. 📝 Actualizar Estudiante");
    WriteLine($"  {(int)OpcionMenu.EliminarEstudiante}. 🗑️  Eliminar Estudiante");
    WriteLine($"  {(int)OpcionMenu.InformeEstudiantes}. 📊 Informe de Rendimiento");

    WriteLine("\n👨‍🏫 --- 3. GESTIÓN DE DOCENTES ---");
    WriteLine(new string('─', 45));
    WriteLine($"  {(int)OpcionMenu.ListarDocentes}. 📜 Listar Docentes");
    WriteLine($"  {(int)OpcionMenu.AnadirDocente}. ➕ Añadir Docente");
    WriteLine($"  {(int)OpcionMenu.ActualizarDocente}. 📝 Actualizar Docente");
    WriteLine($"  {(int)OpcionMenu.EliminarDocente}. 🗑️  Eliminar Docente");
    WriteLine($"  {(int)OpcionMenu.InformeDocentes}. 📈 Informe de Experiencia");

    WriteLine("\n💾 --- 4. IMPORTAR/EXPORTAR DATOS ---");
    WriteLine(new string('─', 45));
    WriteLine($"  {(int)OpcionMenu.ImportarDatos}. 📥 Importar desde Fichero");
    WriteLine($"  {(int)OpcionMenu.ExportarDatos}. 📤 Exportar a Fichero");

    WriteLine("\n🚪 --- 0. SALIR ---");
    WriteLine(new string('━', 45));
}

// ====================================================================
// MÉTODOS DE OPERACIÓN
// ====================================================================

void ListarTodas(IPersonasService service) {
    WriteLine("\n👥 --- LISTADO INTEGRAL DEL PERSONAL ---");
    WriteLine("⚙️  Criterios: 1.ID, 2.DNI, 3.Apellidos, 4.Nombre, 5.Ciclo");
    var op = LeerCadenaValidada("🎯 Seleccione criterio: ", "^[1-5]$", "Elija entre 1 y 5.");

    var criterio = op switch {
        "1" => TipoOrdenamiento.Id,
        "2" => TipoOrdenamiento.Dni,
        "3" => TipoOrdenamiento.Apellidos,
        "4" => TipoOrdenamiento.Nombre,
        _ => TipoOrdenamiento.Ciclo
    };

    var lista = service.GetAllOrderBy(criterio);
    ImprimirTablaPersonas(lista);
}

void BuscarPorDniGeneral(IPersonasService service) {
    WriteLine("\n🔍 --- BÚSQUEDA POR DNI ---");
    var dni = LeerDniValidado();
    try {
        var p = service.GetByDni(dni);
        ImprimirFichaPersona(p);
    }
    catch (PersonasException.NotFound ex) {
        WriteLine($"❌ ERROR: {ex.Message}");
    }
}

void BuscarPorIdGeneral(IPersonasService service) {
    WriteLine("\n🆔 --- BÚSQUEDA POR ID ---");
    var idStr = LeerCadenaValidada("Introduzca ID: ", @"^\d+$", "Debe ser un número entero.");
    try {
        var p = service.GetById(int.Parse(idStr));
        ImprimirFichaPersona(p);
    }
    catch (PersonasException.NotFound ex) {
        WriteLine($"❌ ERROR: {ex.Message}");
    }
}

void ListarEstudiantes(IPersonasService service) {
    WriteLine("\n🎓 --- LISTADO DE ESTUDIANTES ---");
    WriteLine("⚙️  Criterios: 1.ID, 2.DNI, 3.Apellidos, 4.Nombre, 5.Nota, 6.Curso, 7.Ciclo");
    var op = LeerCadenaValidada("🎯 Seleccione criterio: ", "^[1-7]$", "Elija entre 1 y 7.");

    var criterio = op switch {
        "1" => TipoOrdenamiento.Id,
        "2" => TipoOrdenamiento.Dni,
        "3" => TipoOrdenamiento.Apellidos,
        "4" => TipoOrdenamiento.Nombre,
        "5" => TipoOrdenamiento.Nota,
        "6" => TipoOrdenamiento.Curso,
        _ => TipoOrdenamiento.Ciclo
    };

    // El servicio se encarga de aplicar este filtro ANTES de ordenar.
    var lista = service.GetEstudiantesOrderBy(criterio);
    ImprimirTablaEstudiantes(lista);
}

void AnadirNuevoEstudiante(IPersonasService service) {
    WriteLine("\n➕ --- ALTA DE NUEVO ESTUDIANTE ---");
    var dni = LeerDniValidado();
    var nom = LeerCadenaValidada("👤 Nombre: ", @"^[a-zA-ZñÑáéíóúÁÉÍÓÚ\s]{2,30}$", "Mínimo 2 car.");
    var ape = LeerCadenaValidada("👤 Apellidos: ", @"^[a-zA-ZñÑáéíóúÁÉÍÓÚ\s]{2,50}$", "Mínimo 2 car.");
    var nota = LeerNotaValida();
    var ciclo = LeerCiclo();
    var curso = LeerCurso();

    var temp = new Estudiante
        { Dni = dni, Nombre = nom, Apellidos = ape, Calificacion = nota, Ciclo = ciclo, Curso = curso };
    WriteLine("\n👀 REVISE LOS DATOS:");
    ImprimirFichaPersona(temp);

    if (PedirConfirmacion("¿Confirmar alta?"))
        try {
            var creado = service.Save(temp);
            WriteLine("✅ Guardado con éxito.");
            ImprimirFichaPersona(creado);
        }
        catch (PersonasException.Validation ex) {
            ImprimirErroresValidacion(ex.Errores);
        }
        catch (PersonasException.AlreadyExists ex) {
            WriteLine($"❌ CONFLICTO: {ex.Message}");
        }
        catch (Exception ex) {
            WriteLine($"☠️ ERROR DESCONOCIDO: {ex.Message}");
        }
}

void ActualizarEstudiante(IPersonasService service) {
    WriteLine("\n📝 --- ACTUALIZACIÓN DE ESTUDIANTE ---");
    var dni = LeerDniValidado();
    try {
        var p = service.GetByDni(dni);
        if (p is not Estudiante est) {
            WriteLine("❌ ERROR: No es un Estudiante.");
            return;
        }

        ImprimirFichaPersona(est);
        var nNom = LeerCadenaValidada($"👤 Nombre [{est.Nombre}] (Enter mant.): ", @"^([a-zA-ZñÑáéíóúÁÉÍÓÚ\s]{2,30})?$",
            "Error.");
        var nApe = LeerCadenaValidada($"👤 Apellidos [{est.Apellidos}] (Enter mant.): ",
            @"^([a-zA-ZñÑáéíóúÁÉÍÓÚ\s]{2,50})?$", "Error.");
        var nota = PedirConfirmacion("¿Cambiar nota?") ? LeerNotaValida() : est.Calificacion;
        var ciclo = PedirConfirmacion("¿Cambiar ciclo?") ? LeerCiclo() : est.Ciclo;
        var curso = PedirConfirmacion("¿Cambiar curso?") ? LeerCurso() : est.Curso;

        var act = est with {
            Nombre = string.IsNullOrWhiteSpace(nNom) ? est.Nombre : nNom,
            Apellidos = string.IsNullOrWhiteSpace(nApe) ? est.Apellidos : nApe,
            Calificacion = nota, Ciclo = ciclo, Curso = curso
        };

        WriteLine("\n👀 REVISE CAMBIOS:");
        ImprimirFichaPersona(act);
        if (PedirConfirmacion("¿Actualizar?")) {
            var actualizado = service.Update(est.Id, act);
            WriteLine("✅ Actualizado.");
            ImprimirFichaPersona(actualizado);
        }
    }
    catch (PersonasException.Validation ex) {
        ImprimirErroresValidacion(ex.Errores);
    }
    catch (PersonasException.NotFound ex) {
        WriteLine($"❌ ERROR: {ex.Message}");
    }
    catch (Exception ex) {
        WriteLine($"☠️ ERROR DESCONOCIDO: {ex.Message}");
    }
}

void EliminarEstudiante(IPersonasService service) {
    WriteLine("\n🗑️  --- ELIMINACIÓN DE ESTUDIANTE ---");
    var dni = LeerDniValidado();
    try {
        var p = service.GetByDni(dni);
        if (p is not Estudiante) {
            WriteLine("❌ ERROR: No es un Estudiante.");
            return;
        }

        ImprimirFichaPersona(p);
        if (PedirConfirmacion($"¿Eliminar a {p.NombreCompleto}?")) {
            var eliminado = service.Delete(p.Id);
            WriteLine("✅ Borrado físicamente.");
            ImprimirFichaPersona(eliminado);
        }
    }
    catch (PersonasException.NotFound ex) {
        WriteLine($"❌ ERROR: {ex.Message}");
    }
    catch (Exception ex) {
        WriteLine($"☠️ ERROR DESCONOCIDO: {ex.Message}");
    }
}

void MostrarInformeEstudiantes(IPersonasService service) {
    WriteLine("\n📊 --- INFORME DE RENDIMIENTO ACADÉMICO ---");
    WriteLine("⚙️  Alcance: 1.Global, 2.Por Ciclo, 3.Por Curso, 4.Clase Específica");
    var alc = LeerCadenaValidada("🎯 Seleccione alcance: ", "^[1-4]$", "Elija entre 1 y 4.");

    Ciclo? fCiclo = null;
    Curso? fCurso = null;

    switch (alc) {
        case "2": fCiclo = LeerCiclo(); break;
        case "3": fCurso = LeerCurso(); break;
        case "4":
            fCiclo = LeerCiclo();
            fCurso = LeerCurso();
            break;
    }

    var inf = service.GenerarInformeEstudiante(fCiclo, fCurso);
    var desc = alc switch {
        "2" => $"Ciclo {fCiclo}", "3" => $"Curso {fCurso}", "4" => $"{fCurso}º {fCiclo}", _ => "Global"
    };

    WriteLine(new string('─', 65));
    WriteLine($"📍 ALCANCE: {desc}");
    WriteLine(
        $"👨‍🎓 Estudiantes: {inf.TotalEstudiantes} | 📈 Media: {inf.NotaMedia.ToString("F2", Configuracion.Locale)}");
    WriteLine(
        $"✅ Aprobados: {inf.Aprobados} ({inf.PorcentajeAprobados.ToString("F2", Configuracion.Locale)}%)");
    WriteLine(new string('─', 65));
    WriteLine("\n🏆 RANKING POR NOTA (DESCENDENTE):");
    ImprimirTablaEstudiantes(inf.PorNota);
}

void ListarDocentes(IPersonasService service) {
    WriteLine("\n👨‍🏫 --- LISTADO DE DOCENTES ---");
    WriteLine("⚙️  Criterios: 1.ID, 2.DNI, 3.Apellidos, 4.Nombre, 5.Experiencia, 6.Módulo, 7.Ciclo");
    var op = LeerCadenaValidada("🎯 Seleccione criterio: ", "^[1-7]$", "Elija entre 1 y 7.");

    var criterio = op switch {
        "1" => TipoOrdenamiento.Id,
        "2" => TipoOrdenamiento.Dni,
        "3" => TipoOrdenamiento.Apellidos,
        "4" => TipoOrdenamiento.Nombre,
        "5" => TipoOrdenamiento.Experiencia,
        "6" => TipoOrdenamiento.Modulo,
        _ => TipoOrdenamiento.Ciclo
    };

    var lista = service.GetDocentesOrderBy(criterio);
    ImprimirTablaDocentes(lista);
}

void AnadirNuevoDocente(IPersonasService service) {
    WriteLine("\n➕ --- ALTA DE NUEVO DOCENTE ---");
    var dni = LeerDniValidado();
    var nom = LeerCadenaValidada("👤 Nombre: ", @"^[a-zA-ZñÑáéíóúÁÉÍÓÚ\s]{2,30}$", "Mínimo 2 car.");
    var ape = LeerCadenaValidada("👤 Apellidos: ", @"^[a-zA-ZñÑáéíóúÁÉÍÓÚ\s]{2,50}$", "Mínimo 2 car.");
    var expStr = LeerCadenaValidada("⏳ Años de Experiencia: ", @"^\d+$", "Número entero.");
    var exp = int.Parse(expStr);
    var mod = LeerModulo();
    var ciclo = LeerCiclo();

    var temp = new Docente
        { Dni = dni, Nombre = nom, Apellidos = ape, Experiencia = exp, Especialidad = mod, Ciclo = ciclo };
    ImprimirFichaPersona(temp);

    if (PedirConfirmacion("¿Confirmar alta?"))
        try {
            var creado = service.Save(temp);
            WriteLine("✅ Guardado con éxito.");
            ImprimirFichaPersona(creado);
        }
        catch (PersonasException.Validation ex) {
            ImprimirErroresValidacion(ex.Errores);
        }
        catch (PersonasException.AlreadyExists ex) {
            WriteLine($"❌ CONFLICTO: {ex.Message}");
        }
        catch (Exception ex) {
            WriteLine($"☠️ ERROR DESCONOCIDO: {ex.Message}");
        }
}

void ActualizarDocente(IPersonasService service) {
    WriteLine("\n📝 --- ACTUALIZACIÓN DE DOCENTE ---");
    var dni = LeerDniValidado();
    try {
        var p = service.GetByDni(dni);
        if (p is not Docente doc) {
            WriteLine("❌ ERROR: No es un Docente.");
            return;
        }

        ImprimirFichaPersona(doc);
        var nNom = LeerCadenaValidada($"👤 Nombre [{doc.Nombre}] (Enter mant.): ", @"^([a-zA-ZñÑáéíóúÁÉÍÓÚ\s]{2,30})?$",
            "Error.");
        var nApe = LeerCadenaValidada($"👤 Apellidos [{doc.Apellidos}] (Enter mant.): ",
            @"^([a-zA-ZñÑáéíóúÁÉÍÓÚ\s]{2,50})?$", "Error.");
        var exp = PedirConfirmacion("¿Cambiar exp?")
            ? int.Parse(LeerCadenaValidada("⏳ Exp: ", @"^\d+$", "Num."))
            : doc.Experiencia;
        var mod = PedirConfirmacion("¿Cambiar mod?") ? LeerModulo() : doc.Especialidad;
        var ciclo = PedirConfirmacion("¿Cambiar ciclo?") ? LeerCiclo() : doc.Ciclo;

        var act = doc with {
            Nombre = string.IsNullOrWhiteSpace(nNom) ? doc.Nombre : nNom,
            Apellidos = string.IsNullOrWhiteSpace(nApe) ? doc.Apellidos : nApe,
            Experiencia = exp, Especialidad = mod, Ciclo = ciclo
        };

        ImprimirFichaPersona(act);
        if (PedirConfirmacion("¿Actualizar?")) {
            var actualizado = service.Update(doc.Id, act);
            WriteLine("✅ Actualizado.");
            ImprimirFichaPersona(actualizado);
        }
    }
    catch (PersonasException.Validation ex) {
        ImprimirErroresValidacion(ex.Errores);
    }
    catch (PersonasException.NotFound ex) {
        WriteLine($"❌ ERROR: {ex.Message}");
    }
    catch (Exception ex) {
        WriteLine($"☠️ ERROR DESCONOCIDO: {ex.Message}");
    }
}

void EliminarDocente(IPersonasService service) {
    WriteLine("\n🗑️  --- ELIMINACIÓN DE DOCENTE ---");
    var dni = LeerDniValidado();
    try {
        var p = service.GetByDni(dni);
        if (p is not Docente) {
            WriteLine("❌ ERROR: No es un Docente.");
            return;
        }

        ImprimirFichaPersona(p);
        if (PedirConfirmacion($"¿Eliminar a {p.NombreCompleto}?")) {
            var eliminado = service.Delete(p.Id);
            WriteLine("✅ Borrado.");
            ImprimirFichaPersona(eliminado);
        }
    }
    catch (PersonasException.NotFound ex) {
        WriteLine($"❌ ERROR: {ex.Message}");
    }
    catch (Exception ex) {
        WriteLine($"☠️ ERROR DESCONOCIDO: {ex.Message}");
    }
}

void MostrarInformeDocentes(IPersonasService service) {
    WriteLine("\n📈 --- INFORME DE CUADRO DOCENTE ---");
    WriteLine("⚙️  Alcance: 1.Global, 2.Por Ciclo");
    var alc = LeerCadenaValidada("🎯 Seleccione alcance: ", "^[1-2]$", "Elija entre 1 y 2.");

    Ciclo? fCiclo = null;
    if (alc == "2") fCiclo = LeerCiclo();

    var inf = service.GenerarInformeDocente(fCiclo);
    var desc = alc == "2" ? $"Ciclo {fCiclo}" : "Global";

    WriteLine(new string('─', 65));
    WriteLine($"📍 ALCANCE: {desc}");
    WriteLine(
        $"👨‍🏫 Docentes: {inf.TotalDocentes} | ⏳ Media: {inf.ExperienciaMedia.ToString("F2", Configuracion.Locale)} años");
    WriteLine(new string('─', 65));
    WriteLine("\n🏆 RANKING POR EXPERIENCIA (DESCENDENTE):");
    ImprimirTablaDocentes(inf.PorExperiencia);
}

void ImportarDatos(IPersonasService service) {
    WriteLine("\n📥 --- IMPORTAR DATOS DESDE FICHERO ---");
    if (PedirConfirmacion(
            $"Desea importar los datos desde el fichero: {Configuracion.AcademiaFile}\nEsta acción puede sobrescribir datos existentes. ¿Desea continuar?"))
        try {
            var importados = service.ImportarDatos();
            WriteLine($"✅ Importados {importados} registros.");
        }
        catch (Exception ex) {
            WriteLine($"☠️ ERROR AL IMPORTAR: {ex.Message}");
        }
}

void ExportarDatos(IPersonasService service) {
    WriteLine("\n📤 --- EXPORTAR DATOS A FICHERO ---");
    try {
        var exportados = service.ExportarDatos();
        WriteLine($"✅ Exportados {exportados} registros.");
    }
    catch (Exception ex) {
        WriteLine($"☠️ ERROR AL EXPORTAR: {ex.Message}");
    }
}

// ====================================================================
// RENDERIZADO UNIFICADO
// ====================================================================

void ImprimirTablaPersonas(IEnumerable<Persona> lista) {
    var line = new string('━', 105);
    WriteLine(line);
    WriteLine(
        $"{"🆔 ID",-5} | {"🆔 DNI",-10} | {"👤 Nombre Completo",-35} | {"📂 Ciclo",-8} | {"🎭 Tipo",-12}");
    WriteLine(line.Replace('━', '─'));
    foreach (var p in lista) {
        var (tipo, ciclo) = p switch {
            Estudiante e => ("🎓 Estudiante", e.Ciclo.ToString()), Docente d => ("👨‍🏫 Docente", d.Ciclo.ToString()),
            _ => ("❓", "N/A")
        };
        WriteLine($" {p.Id,-5} | {p.Dni,-10} | {p.NombreCompleto,-35} | {ciclo,-8} | {tipo}");
    }

    WriteLine(line);
}

void ImprimirTablaEstudiantes(IEnumerable<Estudiante> lista) {
    var line = new string('━', 125);
    WriteLine(line);
    WriteLine(
        $"{"🆔 ID",-5} | {"🆔 DNI",-10} | {"👤 Nombre Completo",-35} | {"📂 Ciclo",-10} | {"📅 Cur",-6} | {"📝 Nota",-7} | {"🎖️  Evaluación"}");
    WriteLine(line.Replace('━', '─'));
    foreach (var e in lista)
        WriteLine(
            $" {e.Id,-5} | {e.Dni,-10} | {e.NombreCompleto,-35} | {e.Ciclo,-10} | {(int)e.Curso,-6} | {e.Calificacion.ToString("F2", Configuracion.Locale),-7} | {e.CalificacionCualitativa}");
    WriteLine(line);
}

void ImprimirTablaDocentes(IEnumerable<Docente> lista) {
    var line = new string('━', 115);
    WriteLine(line);
    WriteLine(
        $"{"🆔 ID",-5} | {"🆔 DNI",-10} | {"👤 Nombre Completo",-35} | {"📂 Ciclo",-8} | {"⏳ Exp",-6} | {"📚 Especialidad"}");
    WriteLine(line.Replace('━', '─'));
    foreach (var d in lista)
        WriteLine(
            $" {d.Id,-5} | {d.Dni,-10} | {d.NombreCompleto,-35} | {d.Ciclo,-8} | {d.Experiencia,-6} | {d.Especialidad}");
    WriteLine(line);
}

void ImprimirFichaPersona(Persona p) {
    var line = new string('━', 65);
    WriteLine();
    WriteLine(line);
    WriteLine("  🆔 IDENTIDAD ACADÉMICA");
    WriteLine(line.Replace('━', '─'));
    WriteLine($"  🆔 ID:          {(p.Id == 0 ? "PENDIENTE" : p.Id)}");
    WriteLine($"  🆔 DNI:         {p.Dni}");
    WriteLine($"  👤 APELLIDOS:   {p.Apellidos}");
    WriteLine($"  👤 NOMBRE:      {p.Nombre}");

    if (p is Estudiante e) {
        WriteLine("  🎭 TIPO:        🎓 ESTUDIANTE");
        WriteLine($"  📝 NOTA:        {e.Calificacion.ToString("F2", Configuracion.Locale)}");
        WriteLine($"  🎖️  EVAL:        {e.CalificacionCualitativa}");
        WriteLine($"  📂 CICLO:       {e.Ciclo}");
        WriteLine($"  📅 CURSO:       {e.Curso}");
    }
    else if (p is Docente d) {
        WriteLine("  🎭 TIPO:        👨‍🏫 DOCENTE");
        WriteLine($"  ⏳ EXP:         {d.Experiencia} años");
        WriteLine($"  📚 MOD:         {d.Especialidad}");
        WriteLine($"  📂 CICLO:       {d.Ciclo}");
    }

    if (p.CreatedAt != default) {
        WriteLine(new string('─', 65));
        WriteLine($"  📅 ALTA (LOC):  {p.CreatedAt.ToLocalTime().ToString("g", Configuracion.Locale)}");
        WriteLine($"  🔄 MOD  (LOC):  {p.UpdatedAt.ToLocalTime().ToString("g", Configuracion.Locale)}");
        var estado = p.IsDeleted ? "❌ ELIMINADO" : "✅ ACTIVO";
        WriteLine($"  🚦 ESTADO:      {estado}");
    }

    WriteLine(line);
    WriteLine();
}

void ImprimirErroresValidacion(IEnumerable<string> errores) {
    WriteLine("\n⚠️  ERRORES DE VALIDACIÓN DETECTADOS:");
    foreach (var err in errores) WriteLine($"  ❌ {err}");
}

// ====================================================================
// APOYO (INPUT)
// ====================================================================

bool ValidarDniCompleto(string d) {
    if (!Regex.IsMatch(d, @"^(\d{8})([A-Z])$")) return false;
    var n = int.Parse(d.Substring(0, 8));
    return "TRWAGMYFPDXBNJZSQVHLCKE"[n % 23] == d[8];
}

string LeerDniValidado() {
    while (true) {
        Write("🆔 Introduzca DNI: ");
        var d = ReadLine()?.Trim().ToUpper() ?? "";
        if (ValidarDniCompleto(d)) return d;
        WriteLine("❌ ERROR: DNI inválido.");
    }
}

double LeerNotaValidada() {
    var sep = Configuracion.Locale.NumberFormat.NumberDecimalSeparator;
    while (true) {
        Write($"📝 Nota (0-10, use '{sep}'): ");
        var s = ReadLine()?.Trim().Replace(",", ".") ?? "";
        if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var n) && n >= 0 &&
            n <= 10) return n;
        WriteLine("❌ Error: Formato o rango incorrecto.");
    }
}

double LeerNotaValida() {
    return LeerNotaValidada();
} // Alias por compatibilidad

Ciclo LeerCiclo() {
    WriteLine("📂 Ciclos Disponibles: 1.DAM, 2.DAW, 3.ASIR");
    return (Ciclo)(int.Parse(LeerCadenaValidada("🎯 Elija Ciclo: ", @"^[1-3]$", "Seleccione entre 1 y 3.")) - 1);
}

Curso LeerCurso() {
    WriteLine("📅 Cursos Disponibles: 1.Primero, 2.Segundo");
    return (Curso)int.Parse(LeerCadenaValidada("🎯 Elija Curso: ", @"^[1-2]$", "Seleccione 1 o 2."));
}

string LeerModulo() {
    WriteLine(
        $"📚 Módulos: 1.{Modulos.Programacion}, 2.{Modulos.BasesDatos}, 3.{Modulos.Entornos}, 4.{Modulos.LenguajesMarcas}");
    return LeerCadenaValidada("🎯 Elija Módulo: ", @"^[1-4]$", "Seleccione entre 1 y 4.") switch {
        "1" => Modulos.Programacion,
        "2" => Modulos.BasesDatos,
        "3" => Modulos.Entornos,
        _ => Modulos.LenguajesMarcas
    };
}

string LeerCadenaValidada(string prompt, string regex, string error) {
    while (true) {
        Write(prompt);
        var input = ReadLine()?.Trim() ?? "";
        if (Regex.IsMatch(input, regex)) return input;
        WriteLine($"❌ ERROR: {error}");
    }
}

bool PedirConfirmacion(string mensaje) {
    Write($"\n⚠️  {mensaje} (S para confirmar): ");
    var res = char.ToUpper(ReadKey(false).KeyChar) == 'S';
    WriteLine();
    return res;
}