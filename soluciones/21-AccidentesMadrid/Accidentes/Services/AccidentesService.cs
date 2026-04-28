using Accidentes.Storages;
using AccidentesMadrid.Models;
using AccidentesMadrid.Repositories;
using AccidentesMadrid.Services;
using Serilog;

namespace Accidentes.Services;

public class AccidentesService(
    IAccidentesRepository repository,
    IAccidentesStorage storage
) : IAccidentesService {
    private readonly ILogger _logger = Log.ForContext<AccidentesService>();

    public void CargarAño(int año) {
        var path = $"data/{año}_Accidentalidad.csv";
        _logger.Information("Cargando flujo de accidentes para el año {Año}", año);

        var flujoAccidentes = storage.Cargar(path);
        repository.AddRange(flujoAccidentes);
    }

    public IEnumerable<Accidente> GetAll() {
        _logger.Debug("Obteniendo todos los accidentes");
        return repository.GetAll();
    }

    public Accidente? GetByNumExpediente(string numExpediente) {
        _logger.Debug("Buscando accidente: {NumExpediente}", numExpediente);
        return repository.GetByKey(numExpediente);
    }
}