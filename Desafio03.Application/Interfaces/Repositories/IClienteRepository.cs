using Desafio03.Domain.Entities;

namespace Desafio03.Application.Interfaces.Repositories;

public interface IClienteRepository
{
    Task<IReadOnlyList<Cliente>> ObtenerTodosAsync();
    Task<Cliente?> ObtenerPorIdAsync(int id);
    Task<bool> EmailExisteAsync(string email, int? excluirId = null);
    Task<bool> TienePedidosAsync(int clienteId);
    Task<Cliente> CrearAsync(Cliente cliente);
    Task ActualizarAsync(Cliente cliente);
    Task EliminarAsync(Cliente cliente);
}
