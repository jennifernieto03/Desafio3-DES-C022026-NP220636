using Desafio03.Domain.Entities;

namespace Desafio03.Application.Interfaces.Services;

public interface IPedidoService
{
    Task<IReadOnlyList<Pedido>> ObtenerTodosAsync();
    Task<Pedido?> ObtenerPorIdAsync(int id);
    Task<Pedido> CrearAsync(Pedido pedido);
    Task<Pedido?> ActualizarAsync(int id, Pedido datos);
    Task<bool> EliminarAsync(int id);
}
