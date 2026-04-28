using AccidentesMadrid.Services;
using ScottPlot;
using ScottPlot.TickGenerators;
using Serilog;

namespace Accidentes.Services;

/// <summary>
///     Implementación del servicio de gráficos utilizando la librería ScottPlot 5.
///     Se encarga de la generación de archivos PNG estadísticos en un directorio gestionado.
/// </summary>
public class ScottPlotGraphicsService : IAccidentesGraphicsService {
    private readonly ILogger _logger = Log.ForContext<ScottPlotGraphicsService>();
    private readonly string _outputDir;

    /// <summary>
    ///     Inicializa el servicio y prepara el directorio de salida en la ruta del ejecutable.
    /// </summary>
    /// <param name="outputDirName">Nombre de la carpeta de gráficas. Se creará relativa al ejecutable.</param>
    public ScottPlotGraphicsService(string outputDirName = "graphs") {
        // AppDomain.CurrentDomain.BaseDirectory nos da la ruta real del binario (bin/Debug/net...)
        _outputDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, outputDirName);
        PrepararDirectorio();
    }

    /// <inheritdoc />
    public void GenerarBarras(string titulo, string nombreFichero, IEnumerable<string> etiquetas,
        IEnumerable<double> valores) {
        var plt = new Plot();
        var vals = valores.ToArray();
        var labels = etiquetas.ToArray();

        // Creamos la colección de barras
        var bars = vals
            .Select((v, i) => new Bar { Value = v, Position = i })
            .ToList();

        plt.Add.Bars(bars);

        // Título profesional
        plt.Title(titulo);
        plt.Axes.Title.Label.FontSize = 24;

        // Configuración de etiquetas en el eje X
        plt.Axes.Bottom.TickGenerator = new NumericManual(
            labels.Select((e, i) => new Tick(i, e)).ToArray()
        );
        plt.Axes.Bottom.TickLabelStyle.Rotation = 45;
        plt.Axes.Bottom.TickLabelStyle.Alignment = Alignment.UpperRight;

        Guardar(plt, nombreFichero);
    }

    /// <inheritdoc />
    public void GenerarTarta(string titulo, string nombreFichero, IEnumerable<string> etiquetas,
        IEnumerable<double> valores) {
        var plt = new Plot();
        var vals = valores.ToArray();
        var labels = etiquetas.ToArray();
        var total = vals.Sum();

        var pie = plt.Add.Pie(vals);

        // Asignamos etiquetas con porcentaje calculado
        for (var i = 0; i < pie.Slices.Count; i++) {
            var porcentaje = total > 0 ? vals[i] / total * 100 : 0;
            pie.Slices[i].Label = $"{labels[i]} ({porcentaje:F1}%)";
        }

        // Estética de tarta
        plt.Axes.Frameless(); // Quitamos ejes
        plt.Title(titulo);
        plt.Axes.Title.Label.IsVisible = true;
        plt.Axes.Title.Label.FontSize = 24;

        plt.ShowLegend(Alignment.LowerRight);

        Guardar(plt, nombreFichero);
    }

    /// <inheritdoc />
    public void GenerarBarrasComparativas(
        string titulo,
        string nombreFichero,
        IEnumerable<string> etiquetas,
        IEnumerable<double> valoresSerie1,
        string etiquetaSerie1,
        IEnumerable<double> valoresSerie2,
        string etiquetaSerie2) {
        var plt = new Plot();

        var v1 = valoresSerie1.ToArray();
        var v2 = valoresSerie2.ToArray();
        var labels = etiquetas.ToArray();
        var bars = new List<Bar>();

        for (var i = 0; i < labels.Length; i++) {
            // Barra de la primera serie (izquierda)
            bars.Add(new Bar {
                Position = i - 0.2,
                Value = v1[i],
                FillColor = Colors.LightBlue,
                Label = i == 0 ? etiquetaSerie1 : ""
            });

            // Barra de la segunda serie (derecha)
            bars.Add(new Bar {
                Position = i + 0.2,
                Value = v2[i],
                FillColor = Colors.DarkBlue,
                Label = i == 0 ? etiquetaSerie2 : ""
            });
        }

        plt.Add.Bars(bars);

        plt.Title(titulo);
        plt.Axes.Title.Label.FontSize = 24;

        plt.Axes.Bottom.TickGenerator = new NumericManual(
            labels.Select((e, i) => new Tick(i, e)).ToArray()
        );
        plt.Axes.Bottom.TickLabelStyle.Rotation = 45;
        plt.ShowLegend(Alignment.UpperRight);

        Guardar(plt, nombreFichero);
    }

    private void PrepararDirectorio() {
        try {
            if (Directory.Exists(_outputDir)) {
                _logger.Debug("Limpiando directorio de gráficas: {Dir}", _outputDir);
                Directory.Delete(_outputDir, true);
            }

            Directory.CreateDirectory(_outputDir);
            _logger.Information("Directorio de gráficas listo en: {Dir}", _outputDir);
        }
        catch (Exception ex) {
            _logger.Error(ex, "Error al preparar el directorio de gráficas");
        }
    }

    private void Guardar(Plot plt, string nombre) {
        var path = Path.Combine(_outputDir, nombre);
        plt.SavePng(path, 800, 600);
        _logger.Information("Archivo visual generado: {Path}", path);
    }
}