using AccidentesMadrid.Models;
using Serilog;

namespace AccidentesMadrid.Repositories;

public sealed class AccidentesRepository : IAccidentesRepository {
    // Eliminamos Singleton para permitir múltiples instancias (comparativas)

    // Usamos List para no perder registros duplicados por expediente (implicados)
    private readonly List<Accidente> _accidentes = new();
    private readonly ILogger _logger = Log.ForContext<AccidentesRepository>();

    public AccidentesRepository() {
        _logger.Information("Repositorio inicializado");
    }

    public IEnumerable<Accidente> GetAll() {
        return _accidentes;
    }

    public Accidente? GetByKey(string numExpediente) {
        return _accidentes.FirstOrDefault(a => a.NumExpediente == numExpediente);
    }

    public IEnumerable<Accidente> GetByExpediente(string numExpediente) {
        return _accidentes.Where(a => a.NumExpediente == numExpediente);
    }

    public void Save(Accidente accidente) {
        _accidentes.Add(accidente);
    }

    public void AddRange(IEnumerable<Accidente> accidentes) {
        _accidentes.AddRange(accidentes);
        _logger.Information("Repositorio actualizado. Total registros: {Count}", _accidentes.Count);
    }

    public void Clear() {
        _accidentes.Clear();
        _logger.Information("Repositorio vaciado");
    }
}