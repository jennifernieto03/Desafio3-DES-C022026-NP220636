using Desafio03.Domain.Entities;

namespace Desafio03.Application.Interfaces.Services;

public interface IProductoService
{
    Task<IReadOnlyList<Producto>> ObtenerTodosAsync();
    Task<Producto?> ObtenerPorIdAsync(int id);
    Task<Producto> CrearAsync(Producto producto);
    Task<Producto?> ActualizarAsync(int id, Producto datos);
    Task<bool> EliminarAsync(int id);
}
