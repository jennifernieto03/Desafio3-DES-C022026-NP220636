using Desafio03.Domain.Entities;

namespace Desafio03.Application.Interfaces.Repositories;

public interface IPedidoRepository
{
    Task<IReadOnlyList<Pedido>> ObtenerTodosAsync();
    Task<Pedido?> ObtenerPorIdAsync(int id);
    Task<Pedido> CrearConProductoAsync(Pedido pedido, Producto producto);
    Task ActualizarConProductosAsync(Pedido pedido, IReadOnlyCollection<Producto> productos);
    Task EliminarConProductoAsync(Pedido pedido, Producto producto);
}
