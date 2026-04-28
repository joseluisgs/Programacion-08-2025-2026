using System.Text;
using Accidentes.Dto;
using Accidentes.Mappers;
using AccidentesMadrid.Models;
using Serilog;

namespace Accidentes.Storages;

public class AccidentesCsvStorage : IAccidentesStorage {
    private readonly ILogger _logger = Log.ForContext<AccidentesCsvStorage>();

    public IEnumerable<Accidente> Cargar(string path) {
        _logger.Information("Cargando accidentes desde: {Path}", path);

        if (!File.Exists(path)) {
            _logger.Error("Archivo no encontrado: {Path}", path);
            throw new FileNotFoundException($"Archivo no encontrado: {path}");
        }

        try {
            // File.ReadLines es Lazy: lee línea a línea sin cargar todo en RAM
            return File.ReadLines(path, Encoding.UTF8)
                .Skip(1)
                .Select(ParseLinea)
                //.Where(a => a != null)
                //.Select(a => a!); // El operador ! es seguro aquí porque filtramos null
                .OfType<Accidente>(); // Sustituye TClase por el tipo que devuelve ParseLinea
        }
        catch (Exception ex) {
            _logger.Error(ex, "Error al cargar CSV: {Message}", ex.Message);
            throw;
        }
    }

    private Accidente? ParseLinea(string linea) {
        // NOTA PARA EL ALUMNO: 
        // Usamos StringSplitOptions.TrimEntries para separar y recortar espacios en un solo paso.
        // Es más eficiente que usar .Select(p => p.Trim()).ToArray() porque crea un solo array en memoria,
        // lo cual es vital cuando procesamos miles de accidentes de Madrid.
        var partes = linea.Split(';', StringSplitOptions.TrimEntries);

        if (partes.Length < 18) return null; // Si faltan campos, devolvemos null para que el filtro OfType<Accidente>() los ignore

        var dto = new AccidenteCsvDto(
            partes[0], // num_expediente
            partes[1], // fecha
            partes[2], // hora
            partes[3], // localizacion
            partes[4], // numero
            partes[5], // cod_distrito
            partes[6], // distrito
            partes[7], // tipo_accidente
            partes[8], // estado_meteorologico
            partes[9], // tipo_vehiculo
            partes[10], // tipo_persona
            partes[11], // rango_edad
            partes[12], // sexo
            partes[13], // cod_lesividad
            partes[14], // lesividad
            partes[15], // coordenada_x_utm
            partes[16], // coordenada_y_utm
            partes[17], // positiva_alcohol
            partes.Length > 18 ? partes[18] : "" // positiva_droga
        );

        return dto.ToModel();
    }
}