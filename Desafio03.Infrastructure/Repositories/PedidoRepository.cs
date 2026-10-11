using Desafio03.Application.Interfaces.Repositories;
using Desafio03.Domain.Entities;
using Desafio03.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Desafio03.Infrastructure.Repositories;

public class PedidoRepository(AppDbContext context) : IPedidoRepository
{
    public async Task<IReadOnlyList<Pedido>> ObtenerTodosAsync() => await context.Pedidos
        .AsNoTracking()
        .Include(x => x.Cliente)
        .Include(x => x.Producto)
        .OrderByDescending(x => x.FechaPedido)
        .ToListAsync();

    public Task<Pedido?> ObtenerPorIdAsync(int id) => context.Pedidos
        .Include(x => x.Cliente)
        .Include(x => x.Producto)
        .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<Pedido> CrearConProductoAsync(Pedido pedido, Producto producto)
    {
        context.Productos.Update(producto);
        context.Pedidos.Add(pedido);
        // Un único SaveChanges agrupa la creación del pedido y el descuento de stock.
        await context.SaveChangesAsync();
        return pedido;
    }

    public async Task ActualizarConProductosAsync(Pedido pedido, IReadOnlyCollection<Producto> productos)
    {
        foreach (var producto in productos) context.Productos.Update(producto);
        context.Pedidos.Update(pedido);
        await context.SaveChangesAsync();
    }

    public async Task EliminarConProductoAsync(Pedido pedido, Producto producto)
    {
        context.Productos.Update(producto);
        context.Pedidos.Remove(pedido);
        await context.SaveChangesAsync();
    }
}
