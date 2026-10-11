using Desafio03.Application.Interfaces.Repositories;
using Desafio03.Domain.Entities;
using Desafio03.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Desafio03.Infrastructure.Repositories;

public class ProductoRepository(AppDbContext context) : IProductoRepository
{
    public async Task<IReadOnlyList<Producto>> ObtenerTodosAsync() =>
        await context.Productos.AsNoTracking().OrderBy(x => x.Nombre).ToListAsync();

    public Task<Producto?> ObtenerPorIdAsync(int id) => context.Productos.FirstOrDefaultAsync(x => x.Id == id);

    public Task<bool> TienePedidosAsync(int productoId) => context.Pedidos.AnyAsync(x => x.ProductoId == productoId);

    public async Task<Producto> CrearAsync(Producto producto)
    {
        context.Productos.Add(producto);
        await context.SaveChangesAsync();
        return producto;
    }

    public async Task ActualizarAsync(Producto producto)
    {
        context.Productos.Update(producto);
        await context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Producto producto)
    {
        context.Productos.Remove(producto);
        await context.SaveChangesAsync();
    }
}
