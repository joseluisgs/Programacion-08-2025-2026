using RepositorioBinario.Models;
using RepositorioBinario.Repositories.Common;

namespace RepositorioBinario.Repositories;

public interface IPersonasRepository : ICrudRepository<int, Persona>
{
}