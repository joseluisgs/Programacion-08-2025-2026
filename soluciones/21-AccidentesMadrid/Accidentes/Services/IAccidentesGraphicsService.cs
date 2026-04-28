namespace AccidentesMadrid.Services;

/// <summary>
///     Define los servicios necesarios para la representación visual de datos estadísticos.
/// </summary>
public interface IAccidentesGraphicsService {
    /// <summary>
    ///     Genera un gráfico de barras simple para representar frecuencias o rankings.
    ///     Ideal para visualizar comparativas entre categorías independientes (ej: Accidentes por Distrito).
    /// </summary>
    /// <param name="titulo">Título principal del gráfico.</param>
    /// <param name="nombreFichero">Nombre del archivo PNG resultante (se guardará en el directorio de salida).</param>
    /// <param name="etiquetas">Nombres de las categorías para el eje X.</param>
    /// <param name="valores">Valores numéricos asociados a cada categoría.</param>
    void GenerarBarras(string titulo, string nombreFichero, IEnumerable<string> etiquetas, IEnumerable<double> valores);

    /// <summary>
    ///     Genera un gráfico de tarta (pie chart) para representar proporciones respecto a un total.
    ///     Ideal para visualizar distribuciones porcentuales (ej: Porcentaje por Sexo o Lesividad).
    /// </summary>
    /// <param name="titulo">Título principal del gráfico.</param>
    /// <param name="nombreFichero">Nombre del archivo PNG resultante.</param>
    /// <param name="etiquetas">Nombres de los sectores de la tarta.</param>
    /// <param name="valores">Valores numéricos que determinan el tamaño de cada sector.</param>
    void GenerarTarta(string titulo, string nombreFichero, IEnumerable<string> etiquetas, IEnumerable<double> valores);

    /// <summary>
    ///     Genera un gráfico de barras agrupadas para comparar dos series de datos distintas sobre las mismas categorías.
    ///     Ideal para análisis evolutivos o comparativas interanuales.
    /// </summary>
    /// <param name="titulo">Título principal del gráfico.</param>
    /// <param name="nombreFichero">Nombre del archivo PNG resultante.</param>
    /// <param name="etiquetas">Categorías compartidas por ambas series (ej: Meses).</param>
    /// <param name="valoresSerie1">Valores de la primera serie (ej: Datos del año pasado).</param>
    /// <param name="etiquetaSerie1">Nombre identificativo de la primera serie para la leyenda.</param>
    /// <param name="valoresSerie2">Valores de la segunda serie (ej: Datos del año actual).</param>
    /// <param name="etiquetaSerie2">Nombre identificativo de la segunda serie para la leyenda.</param>
    void GenerarBarrasComparativas(
        string titulo,
        string nombreFichero,
        IEnumerable<string> etiquetas,
        IEnumerable<double> valoresSerie1,
        string etiquetaSerie1,
        IEnumerable<double> valoresSerie2,
        string etiquetaSerie2
    );
}