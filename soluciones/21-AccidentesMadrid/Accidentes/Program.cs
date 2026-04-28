using System.Globalization;
using System.Text;
using Accidentes.Models;
using Accidentes.Services;
using Accidentes.Storages;
using AccidentesMadrid.Models;
using AccidentesMadrid.Repositories;
using AccidentesMadrid.Services;
using Microsoft.Extensions.Configuration;
using Serilog;
using static System.Console;

var loggerConfiguration = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console(
        outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

Log.Logger = loggerConfiguration;

// --- CARGA DE CONFIGURACIÓN (appsettings.json) ---
// En el software profesional nunca hardcodea parámetros.
var config = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", false, true)
    .Build();

var añoActual = config.GetValue<int>("Analisis:AñoActual");
var añoAnterior = config.GetValue<int>("Analisis:AñoAnterior");
var carpetaGraficas = config.GetValue<string>("Graficas:DirectorioSalida") ?? "graphs";

Title = $"Accidentes Madrid {añoActual} - Análisis de Datos";
OutputEncoding = Encoding.UTF8;

Main(añoActual, añoAnterior, carpetaGraficas);

Log.CloseAndFlush();
return;

void Main(int actual, int anterior, string dirGraficas) {
    // --- INICIALIZACIÓN DE SERVICIOS ---
    IAccidentesStorage storage = new AccidentesCsvStorage();
    // Inyectamos el nombre del directorio desde la configuración
    IAccidentesGraphicsService graphics = new ScottPlotGraphicsService(dirGraficas);

    // Configuración Año Actual
    IAccidentesRepository repoActual = new AccidentesRepository();
    var serviceActual = new AccidentesService(repoActual, storage);
    serviceActual.CargarAño(actual);

    var dataActual = serviceActual
        .GetAll()
        .ToList();

    var totalActual = dataActual.Count;

    WriteLine("==========================================================================");
    WriteLine($"   ACCIDENTES MADRID {actual} - INFORME COMPLETO DE DATOS");
    WriteLine("==========================================================================");
    WriteLine();
    WriteLine($"[DATOS] Registros totales cargados ({actual}): {totalActual:N0}");
    WriteLine();

    // --------------------------------------------------------------------------
    // BLOQUE 1: ESTADÍSTICAS GENERALES (Año Actual)
    // --------------------------------------------------------------------------

    WriteLine("========================================");
    WriteLine("  1. CONSULTA: 5 primeros accidentes");
    WriteLine("========================================");
    var primerosCinco = dataActual
        .Take(5);

    foreach (var a in primerosCinco)
        WriteLine($"  {a.NumExpediente} - {a.Fecha:d} - {a.Distrito}");
    WriteLine();

    WriteLine("========================================");
    WriteLine("  2. CONSULTA: Con alcohol o drogas");
    WriteLine("========================================");
    var conSustanciasActual = dataActual
        .Where(a => a.PositivoAlcohol || a.PositivoDroga)
        .ToList();

    WriteLine($"  Total: {conSustanciasActual.Count}");
    foreach (var a in conSustanciasActual.Take(5))
        WriteLine($"  {a.NumExpediente} - {a.Distrito} (Alcohol: {a.PositivoAlcohol} Drogas: {a.PositivoDroga})");
    WriteLine();

    WriteLine("========================================");
    WriteLine("  3. CONSULTA: Alcohol Y drogas");
    WriteLine("========================================");
    var alcoholYDrogas = dataActual
        .Count(a => a.PositivoAlcohol && a.PositivoDroga);

    WriteLine($"  Total casos positivos en ambos: {alcoholYDrogas}");
    WriteLine();

    WriteLine("========================================");
    WriteLine($"  4. CONSULTA: Por sexo ({actual})");
    WriteLine("========================================");
    // NOTA PEDAGÓGICA:
    // Aquí usamos .ToDictionary() porque es más óptimo para conteos simples.
    var porSexo = dataActual
        .GroupBy(a => a.Sexo)
        .ToDictionary(
            g => g.Key,
            g => g.Count()
        );

    foreach (var item in porSexo)
        WriteLine($"  {item.Key,-15} | {item.Value,5}");
    WriteLine();

    WriteLine("========================================");
    WriteLine($"  5. CONSULTA: Por meses ({actual})");
    WriteLine("========================================");
    // NOTA PEDAGÓGICA:
    // Aquí NO usamos .ToDictionary() sino .Select() + .ToList().
    var porMesActual = dataActual
        .GroupBy(a => a.Fecha.Month)
        .OrderBy(g => g.Key)
        .Select(g => new {
            Mes = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(
                CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key)),
            Total = g.Count()
        })
        .ToList();

    foreach (var item in porMesActual)
        WriteLine($"  {item.Mes,-15} | {item.Total,5} accidentes");
    WriteLine();

    WriteLine("========================================");
    WriteLine("  6. CONSULTA: Mes con más accidentes");
    WriteLine("========================================");
    var mesMasAccidentes = porMesActual
        .MaxBy(x => x.Total);

    WriteLine($"  {mesMasAccidentes!.Mes}: {mesMasAccidentes.Total} accidentes");
    WriteLine();

    WriteLine("========================================");
    WriteLine("  7. CONSULTA: Por tipo de vehículo (Top 10)");
    WriteLine("========================================");
    var porVehiculo = dataActual
        .GroupBy(a => a.TipoVehiculo)
        .Select(g => new {
            Vehiculo = g.Key,
            Total = g.Count()
        })
        .OrderByDescending(x => x.Total)
        .Take(10)
        .ToList();

    foreach (var item in porVehiculo)
        WriteLine($"  {item.Vehiculo,-25} | {item.Total,5}");
    WriteLine();

    WriteLine("========================================");
    WriteLine("  8. CONSULTA: Búsqueda Calle Leganés");
    WriteLine("========================================");
    var calleLeganes = dataActual
        .Where(a => a.Localizacion.Contains("leganes", StringComparison.OrdinalIgnoreCase))
        .Take(5);

    foreach (var a in calleLeganes)
        WriteLine($"  {a.Localizacion}");
    WriteLine();

    WriteLine("========================================");
    WriteLine("  9. CONSULTA: Por distrito (Filtrado)");
    WriteLine("========================================");
    var porDistritoActual = dataActual
        .Where(a => !int.TryParse(a.Distrito, out _))
        .GroupBy(a => a.Distrito)
        .Select(g => new {
            Distrito = g.Key,
            Total = g.Count()
        })
        .OrderByDescending(x => x.Total)
        .ToList();

    foreach (var item in porDistritoActual)
        WriteLine($"  {item.Distrito,-25} | {item.Total,5}");
    WriteLine();

    WriteLine("========================================");
    WriteLine($"  10. CONSULTA: Accidentes en USERA ({actual})");
    WriteLine("========================================");
    var enUsera = dataActual
        .Count(a => a.Distrito.Contains("USERA"));

    WriteLine($"  Total: {enUsera}");
    WriteLine();

    WriteLine("========================================");
    WriteLine("  11. CONSULTA: Stats por distrito");
    WriteLine("========================================");
    var maxD = porDistritoActual.MaxBy(x => x.Total);
    var minD = porDistritoActual.MinBy(x => x.Total);
    var avgD = porDistritoActual.Average(x => x.Total);

    WriteLine($"  Max: {maxD!.Distrito} ({maxD.Total})");
    WriteLine($"  Min: {minD!.Distrito} ({minD.Total})");
    WriteLine($"  Avg: {avgD:F2}");
    WriteLine();

    WriteLine("========================================");
    WriteLine("  13. CONSULTA: Noche de Finde + Alcohol");
    WriteLine("========================================");
    var nocheFinde = dataActual
        .Count(a => a.PositivoAlcohol &&
                    a.Fecha.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday &&
                    (a.Hora.Hours >= 22 || a.Hora.Hours <= 6));

    WriteLine($"  Total casos críticos detectados: {nocheFinde}");
    WriteLine();

    WriteLine("========================================");
    WriteLine("  14. CONSULTA: Gravedad de Lesiones");
    WriteLine("========================================");
    var porGravedad = dataActual
        .GroupBy(a => a.Gravedad)
        .Select(g => new {
            G = g.Key,
            Total = g.Count()
        })
        .OrderByDescending(x => x.Total)
        .ToList();

    foreach (var item in porGravedad)
        WriteLine($"  {item.G,-20} | {item.Total,5}");
    WriteLine();

    WriteLine("========================================");
    WriteLine("  15. CONSULTA: Fallecidos");
    WriteLine("========================================");
    var fallecidosActual = dataActual
        .Where(a => a.Gravedad == Gravedad.Fallecido)
        .ToList();

    WriteLine($"  Total fallecidos: {fallecidosActual.Count}");
    foreach (var a in fallecidosActual.Take(5))
        WriteLine($"  {a.NumExpediente} - {a.Distrito}");
    WriteLine();

    WriteLine("========================================");
    WriteLine("  17. CONSULTA: Por Meteorología");
    WriteLine("========================================");
    var meteoValida = new[] { "Despejado", "Nublado", "Lluvia débil", "LLuvia intensa", "Granizando", "Nevando" };
    var porMeteo = dataActual
        .Where(a => meteoValida.Contains(a.EstadoMeteorologico))
        .GroupBy(a => a.EstadoMeteorologico)
        .Select(g => new {
            M = g.Key,
            Total = g.Count()
        })
        .OrderByDescending(x => x.Total)
        .ToList();

    foreach (var item in porMeteo)
        WriteLine($"  {item.M,-20} | {item.Total,5}");
    WriteLine();

    WriteLine("========================================");
    WriteLine("  19. CONSULTA: Sustancias (ToDictionary)");
    WriteLine("========================================");
    var porSust = dataActual
        .GroupBy(a => {
            if (a.PositivoAlcohol && a.PositivoDroga) return "Ambos";
            if (a.PositivoAlcohol) return "Solo Alcohol";
            if (a.PositivoDroga) return "Solo Drogas";
            return "Ninguna";
        })
        .ToDictionary(
            g => g.Key,
            g => g.Count()
        );

    foreach (var item in porSust)
        WriteLine($"  {item.Key,-15} | {item.Value,5}");
    WriteLine();

    WriteLine("========================================");
    WriteLine("  20. CONSULTA: Distrito con más Alcohol");
    WriteLine("========================================");
    var distAlc = dataActual
        .Where(a => a.PositivoAlcohol && !int.TryParse(a.Distrito, out _))
        .GroupBy(a => a.Distrito)
        .Select(g => new {
            D = g.Key,
            T = g.Count()
        })
        .MaxBy(x => x.T);

    WriteLine($"  {distAlc!.D} con {distAlc.T} positivos.");
    WriteLine();

    // --------------------------------------------------------------------------
    // BLOQUE 2: ANÁLISIS POR SEGURIDAD VIAL (Impacto Social)
    // --------------------------------------------------------------------------

    WriteLine("========================================");
    WriteLine("  23. CONSULTA: Niños y Mayores de 65 años");
    WriteLine("========================================");
    var vulnerables = dataActual
        .Where(a => a.RangoEdad.Contains("Menor", StringComparison.OrdinalIgnoreCase) ||
                    a.RangoEdad.Contains("65") ||
                    a.RangoEdad.Contains("74") ||
                    a.RangoEdad.Contains("Más de 74"))
        .GroupBy(a => a.Gravedad)
        .Select(g => new {
            Gravedad = g.Key,
            Total = g.Count()
        })
        .OrderByDescending(x => x.Total)
        .ToList();

    foreach (var v in vulnerables) WriteLine($"    {v.Gravedad,-20} | {v.Total,5} personas vulnerables");
    WriteLine();

    WriteLine("========================================");
    WriteLine("  24. CONSULTA: Letalidad por Vehículo (%)");
    WriteLine("========================================");
    var letalidad = dataActual
        .GroupBy(a => a.TipoVehiculo)
        .Select(g => new {
            V = string.IsNullOrWhiteSpace(g.Key) ? "Descon." : g.Key,
            T = g.Count(),
            GF = g.Count(x => x.Gravedad is Gravedad.Grave or Gravedad.Fallecido)
        })
        .Where(x => x.T > 50)
        .Select(x => new {
            x.V,
            P = (double)x.GF / x.T * 100
        })
        .OrderByDescending(x => x.P)
        .Take(5)
        .ToList();

    foreach (var l in letalidad) WriteLine($"    {l.V,-25} | {l.P,6:F2}% riesgo grave/muerte");
    WriteLine();

    WriteLine("========================================");
    WriteLine("  25. CONSULTA: La Hora Bruja (Finde/Noche)");
    WriteLine("========================================");
    var hBruja = dataActual
        .Where(a => a.Fecha.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday &&
                    a.Hora.Hours >= 0 && a.Hora.Hours <= 6 &&
                    (a.PositivoAlcohol || a.PositivoDroga))
        .GroupBy(a => a.Distrito)
        .Select(g => new {
            D = g.Key,
            T = g.Count()
        })
        .OrderByDescending(x => x.T)
        .Take(5)
        .ToList();

    foreach (var h in hBruja)
        WriteLine($"    {h.D,-25} | {h.T,3} casos críticos");
    WriteLine();

    WriteLine("========================================");
    WriteLine("  26. CONSULTA: Puntos Negros de Madrid");
    WriteLine("========================================");
    var pNegros = dataActual
        .GroupBy(a => a.Localizacion)
        .Select(g => new {
            C = g.Key,
            T = g.Count()
        })
        .OrderByDescending(x => x.T)
        .Take(5)
        .ToList();

    foreach (var p in pNegros) WriteLine($"    {p.C,-50} | {p.T,4} implicados");
    WriteLine();

    WriteLine("========================================");
    WriteLine("  27. CONSULTA: Peatones en Peligro");
    WriteLine("========================================");
    var atrops = dataActual
        .Where(a => a.TipoAccidente == TipoAccidente.AtropelloPersona && !int.TryParse(a.Distrito, out _))
        .GroupBy(a => a.Distrito)
        .Select(g => new {
            D = g.Key,
            T = g.Count()
        })
        .OrderByDescending(x => x.T)
        .Take(5)
        .ToList();

    foreach (var a in atrops) WriteLine($"    {a.D,-25} | {a.T,4} atropellos");
    WriteLine();

    WriteLine("========================================");
    WriteLine("  28. CONSULTA: Impacto Meteorológico");
    WriteLine("========================================");
    var meteoAcc = dataActual
        .Where(a => a.EstadoMeteorologico is "Despejado" or "LLuvia intensa")
        .GroupBy(a => new { a.EstadoMeteorologico, a.TipoAccidente })
        .Select(g => new {
            M = g.Key.EstadoMeteorologico,
            T = g.Key.TipoAccidente,
            Count = g.Count()
        })
        .OrderBy(x => x.M)
        .ThenByDescending(x => x.Count)
        .GroupBy(x => x.M)
        .Select(g => new {
            Clima = g.Key,
            Top = g.First()
        })
        .ToList();

    foreach (var c in meteoAcc) WriteLine($"    Con {c.Clima,-15} el accidente más común es {c.Top.T} ({c.Top.Count})");
    WriteLine();

    WriteLine("========================================");
    WriteLine("  29. CONSULTA: Jóvenes (18-24) y Sustancias");
    WriteLine("========================================");
    var jovSust = dataActual
        .Count(a => a.RangoEdad.Contains("18 a 24") && (a.PositivoAlcohol || a.PositivoDroga));

    WriteLine($"    {jovSust} jóvenes dieron positivo tras un accidente.");
    WriteLine();

    WriteLine("========================================");
    WriteLine("  30. CONSULTA: Roles por Género");
    WriteLine("========================================");
    var roles = dataActual
        .Where(a => a.Sexo != Sexo.NoAsignado)
        .GroupBy(a => new { a.Sexo, a.TipoPersona })
        .Select(g => new {
            g.Key.Sexo,
            g.Key.TipoPersona,
            T = g.Count()
        })
        .OrderByDescending(x => x.T)
        .Take(4)
        .ToList();

    foreach (var r in roles) WriteLine($"    {r.Sexo,-10} como {r.TipoPersona,-10} | {r.T,6} implicados");
    WriteLine();

    WriteLine("========================================");
    WriteLine("  31. CONSULTA: Peligro Hora Punta (L-V)");
    WriteLine("========================================");
    var hPunta = dataActual
        .Count(a => a.Fecha.DayOfWeek != DayOfWeek.Saturday &&
                    a.Fecha.DayOfWeek != DayOfWeek.Sunday &&
                    a.Hora.Hours >= 7 &&
                    a.Hora.Hours <= 9);

    var porHPunta = (double)hPunta / totalActual * 100;
    WriteLine($"    {hPunta} implicados ({porHPunta:F2}%) en entrada al trabajo.");
    WriteLine();

    WriteLine("========================================");
    WriteLine("  32. CONSULTA: Tasa de Supervivencia");
    WriteLine("========================================");
    var ilesActual = dataActual
        .Count(a => a.Gravedad == Gravedad.SinAsistencia);

    var porIlesActual = (double)ilesActual / totalActual * 100;
    WriteLine($"    El {porIlesActual:F2}% de los implicados resultaron ilesos.");
    WriteLine($"    Dato Crítico: {totalActual - ilesActual} personas necesitaron asistencia sanitaria.");
    WriteLine();

    // --------------------------------------------------------------------------
    // BLOQUE 3: COMPARATIVA INTERANUAL (Actual vs Anterior)
    // --------------------------------------------------------------------------

    WriteLine();
    WriteLine("==========================================================================");
    WriteLine($"   ANÁLISIS COMPARATIVO INTERANUAL: {actual} vs {anterior}");
    WriteLine("==========================================================================");
    WriteLine();

    // Carga independiente Año Anterior
    IAccidentesRepository repoAnterior = new AccidentesRepository();
    var serviceAnterior = new AccidentesService(repoAnterior, storage);
    serviceAnterior.CargarAño(anterior);

    var dataAnterior = serviceAnterior
        .GetAll()
        .ToList();

    // Métricas Resumen
    ImprimirFilaComparativa(
        "Total Implicados",
        dataAnterior.Count,
        totalActual,
        anterior, actual
    );

    ImprimirFilaComparativa(
        "Total Fallecidos",
        dataAnterior.Count(a => a.Gravedad == Gravedad.Fallecido),
        fallecidosActual.Count,
        anterior, actual
    );

    ImprimirFilaComparativa(
        "Positivos Sustancias",
        dataAnterior.Count(a => a.PositivoAlcohol || a.PositivoDroga),
        conSustanciasActual.Count,
        anterior, actual
    );

    // COMPARATIVA POR MESES
    WriteLine("=====================================================");
    WriteLine($"  COMPARATIVA MENSUAL ({anterior} vs {actual})");
    WriteLine("=====================================================");
    WriteLine($"  {"MES",-15} | {anterior,8} | {actual,8} | {"VAR.",8}");
    WriteLine("  ---------------------------------------------------");

    var mesesAnterior = dataAnterior
        .GroupBy(a => a.Fecha.Month)
        .ToDictionary(g => g.Key, g => g.Count());

    var mesesActual = dataActual
        .GroupBy(a => a.Fecha.Month)
        .ToDictionary(g => g.Key, g => g.Count());

    for (var m = 1; m <= 12; m++) {
        var nombreMes = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(
            CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m));

        var cAnt = mesesAnterior.ContainsKey(m) ? mesesAnterior[m] : 0;
        var cAct = mesesActual.ContainsKey(m) ? mesesActual[m] : 0;
        var diff = cAct - cAnt;

        WriteLine($"  {nombreMes,-15} | {cAnt,8:N0} | {cAct,8:N0} | {diff,8:+0;-0;0}");
    }

    WriteLine();

    // COMPARATIVA POR DISTRITOS
    WriteLine("=====================================================");
    WriteLine($"  COMPARATIVA POR DISTRITOS ({anterior} vs {actual})");
    WriteLine("=====================================================");
    WriteLine($"  {"DISTRITO",-25} | {anterior,8} | {actual,8} | {"VAR.",8}");
    WriteLine("  ---------------------------------------------------------------");

    var distAnterior = dataAnterior
        .Where(a => !int.TryParse(a.Distrito, out _))
        .GroupBy(a => a.Distrito)
        .ToDictionary(g => g.Key, g => g.Count());

    var distActual = dataActual
        .Where(a => !int.TryParse(a.Distrito, out _))
        .GroupBy(a => a.Distrito)
        .ToDictionary(g => g.Key, g => g.Count());

    var todosDistritos = distAnterior.Keys
        .Union(distActual.Keys)
        .OrderBy(d => d);

    foreach (var d in todosDistritos) {
        var cAnt = distAnterior.ContainsKey(d) ? distAnterior[d] : 0;
        var cAct = distActual.ContainsKey(d) ? distActual[d] : 0;
        var diff = cAct - cAnt;

        WriteLine($"  {d,-25} | {cAnt,8:N0} | {cAct,8:N0} | {diff,8:+0;-0;0}");
    }

    WriteLine();

    // --------------------------------------------------------------------------
    // BLOQUE 4: GENERACIÓN DE GRÁFICAS ESTADÍSTICAS
    // --------------------------------------------------------------------------
    WriteLine();
    WriteLine("==========================================================================");
    WriteLine("   BLOQUE 4: GENERACIÓN DE GRÁFICAS ESTADÍSTICAS");
    WriteLine("==========================================================================");
    WriteLine($"Generando archivos visuales en la carpeta /{dirGraficas}...");

    // Gráficas Año Actual
    graphics.GenerarTarta(
        $"Distribución por Sexo ({actual})",
        $"04_sexo_{actual}.png",
        porSexo.Keys.Select(k => k.ToString()),
        porSexo.Values.Select(v => (double)v)
    );

    graphics.GenerarBarras(
        $"Evolución Mensual ({actual})",
        $"05_meses_{actual}.png",
        porMesActual.Select(x => x.Mes),
        porMesActual.Select(x => (double)x.Total)
    );

    graphics.GenerarBarras(
        $"Top 10 Vehículos ({actual})",
        $"07_vehiculos_{actual}.png",
        porVehiculo.Select(x => string.IsNullOrWhiteSpace(x.Vehiculo) ? "Otro" : x.Vehiculo),
        porVehiculo.Select(x => (double)x.Total)
    );

    graphics.GenerarTarta(
        $"Gravedad Lesiones ({actual})",
        $"14_gravedad_{actual}.png",
        porGravedad.Select(x => x.G.ToString()),
        porGravedad.Select(x => (double)x.Total)
    );

    // Gráficas Comparativas
    var etiquetasMesesAbrev = Enumerable.Range(1, 12)
        .Select(m => CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(m))
        .ToArray();
    var valsM_Ant = Enumerable.Range(1, 12).Select(m => mesesAnterior.ContainsKey(m) ? (double)mesesAnterior[m] : 0);
    var valsM_Act = Enumerable.Range(1, 12).Select(m => mesesActual.ContainsKey(m) ? (double)mesesActual[m] : 0);

    graphics.GenerarBarrasComparativas(
        $"Evolución Mensual {anterior} vs {actual}",
        $"C1_meses_{anterior}_{actual}.png",
        etiquetasMesesAbrev,
        valsM_Ant, anterior.ToString(),
        valsM_Act, actual.ToString()
    );

    var topDists = distActual.OrderByDescending(x => x.Value).Take(10).Select(x => x.Key).ToList();
    var valsD_Ant = topDists.Select(d => distAnterior.ContainsKey(d) ? (double)distAnterior[d] : 0);
    var valsD_Act = topDists.Select(d => distActual.ContainsKey(d) ? (double)distActual[d] : 0);

    graphics.GenerarBarrasComparativas(
        $"Top 10 Distritos {anterior} vs {actual}",
        $"C2_distritos_{anterior}_{actual}.png",
        topDists,
        valsD_Ant, anterior.ToString(),
        valsD_Act, actual.ToString()
    );

    WriteLine();
    WriteLine("==========================================================================");
    WriteLine("   INFORME FINALIZADO CON ÉXITO");
    WriteLine("==========================================================================");
}

void ImprimirFilaComparativa(string titulo, double valAnt, double valAct, int añoAnt, int añoAct) {
    var diff = valAct - valAnt;
    var porc = valAnt != 0 ? diff / valAnt * 100 : 0;
    var tendencia = diff > 0 ? "📈 AUMENTA" : diff < 0 ? "📉 DISMINUYE" : "➖ SIN CAMBIOS";

    WriteLine($"  {titulo,-30}:");
    WriteLine($"    {añoAnt}: {valAnt,8:N0} | {añoAct}: {valAct,8:N0}");
    WriteLine($"    Resultado: {tendencia} ({porc:+0.00;-0.00;0}%)");
    WriteLine();
}