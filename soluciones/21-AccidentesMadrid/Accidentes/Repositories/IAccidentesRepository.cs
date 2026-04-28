using AccidentesMadrid.Models;

namespace AccidentesMadrid.Repositories;

public interface IAccidentesRepository : IRepository<Accidente, string> {
    void AddRange(IEnumerable<Accidente> accidentes);
    void Clear();
    IEnumerable<Accidente> GetByExpediente(string numExpediente);
}