using Desafio03.Domain.Entities;

namespace Desafio03.Application.Interfaces.Repositories;

public interface IProductoRepository
{
    Task<IReadOnlyList<Producto>> ObtenerTodosAsync();
    Task<Producto?> ObtenerPorIdAsync(int id);
    Task<bool> TienePedidosAsync(int productoId);
    Task<Producto> CrearAsync(Producto producto);
    Task ActualizarAsync(Producto producto);
    Task EliminarAsync(Producto producto);
}
