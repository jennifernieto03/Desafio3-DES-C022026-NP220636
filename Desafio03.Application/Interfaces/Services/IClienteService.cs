using Desafio03.Domain.Entities;

namespace Desafio03.Application.Interfaces.Services;

public interface IClienteService
{
    Task<IReadOnlyList<Cliente>> ObtenerTodosAsync();
    Task<Cliente?> ObtenerPorIdAsync(int id);
    Task<Cliente> CrearAsync(Cliente cliente);
    Task<Cliente?> ActualizarAsync(int id, Cliente datos);
    Task<bool> EliminarAsync(int id);
}
