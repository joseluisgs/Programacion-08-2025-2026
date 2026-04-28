using AccidentesMadrid.Models;

namespace AccidentesMadrid.Services;

public interface IAccidentesService {
    void CargarAño(int año);
    IEnumerable<Accidente> GetAll();
    Accidente? GetByNumExpediente(string numExpediente);
}