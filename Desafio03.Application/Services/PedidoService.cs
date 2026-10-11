using Desafio03.Application.Interfaces.Repositories;
using Desafio03.Application.Interfaces.Services;
using Desafio03.Application.Validation;
using Desafio03.Domain.Entities;

namespace Desafio03.Application.Services;

public class PedidoService(
    IClienteRepository clientes,
    IProductoRepository productos,
    IPedidoRepository pedidos) : IPedidoService
{
    public Task<IReadOnlyList<Pedido>> ObtenerTodosAsync() => pedidos.ObtenerTodosAsync();

    public Task<Pedido?> ObtenerPorIdAsync(int id) => pedidos.ObtenerPorIdAsync(id);

    public async Task<Pedido> CrearAsync(Pedido pedido)
    {
        PrepararFecha(pedido);
        ValidacionDominio.Validar(pedido);

        var cliente = await clientes.ObtenerPorIdAsync(pedido.ClienteId)
            ?? throw new InvalidOperationException("El cliente indicado no existe.");
        var producto = await productos.ObtenerPorIdAsync(pedido.ProductoId)
            ?? throw new InvalidOperationException("El producto indicado no existe.");

        if (producto.Stock < pedido.Cantidad)
            throw new InvalidOperationException($"Stock insuficiente. Disponible: {producto.Stock}; solicitado: {pedido.Cantidad}.");

        producto.Stock -= pedido.Cantidad;
        pedido.Cliente = cliente;
        pedido.Producto = producto;
        return await pedidos.CrearConProductoAsync(pedido, producto);
    }

    public async Task<Pedido?> ActualizarAsync(int id, Pedido datos)
    {
        PrepararFecha(datos);
        ValidacionDominio.Validar(datos);

        var actual = await pedidos.ObtenerPorIdAsync(id);
        if (actual is null) return null;

        var clienteNuevo = await clientes.ObtenerPorIdAsync(datos.ClienteId)
            ?? throw new InvalidOperationException("El cliente indicado no existe.");

        var productoAnterior = await productos.ObtenerPorIdAsync(actual.ProductoId)
            ?? throw new InvalidOperationException("El producto original del pedido ya no existe.");
        var productoNuevo = actual.ProductoId == datos.ProductoId
            ? productoAnterior
            : await productos.ObtenerPorIdAsync(datos.ProductoId)
                ?? throw new InvalidOperationException("El producto indicado no existe.");

        if (actual.ProductoId == datos.ProductoId)
        {
            var stockDisponible = productoAnterior.Stock + actual.Cantidad;
            if (stockDisponible < datos.Cantidad)
                throw new InvalidOperationException($"Stock insuficiente. Disponible para este pedido: {stockDisponible}; solicitado: {datos.Cantidad}.");
            productoNuevo.Stock = stockDisponible - datos.Cantidad;
        }
        else
        {
            if (productoNuevo.Stock < datos.Cantidad)
                throw new InvalidOperationException($"Stock insuficiente. Disponible: {productoNuevo.Stock}; solicitado: {datos.Cantidad}.");
            productoAnterior.Stock += actual.Cantidad;
            productoNuevo.Stock -= datos.Cantidad;
        }

        actual.FechaPedido = datos.FechaPedido;
        actual.Cantidad = datos.Cantidad;
        actual.ClienteId = datos.ClienteId;
        actual.Cliente = clienteNuevo;
        actual.ProductoId = datos.ProductoId;
        actual.Producto = productoNuevo;

        var productosAActualizar = new[] { productoAnterior, productoNuevo }
            .DistinctBy(p => p.Id)
            .ToArray();
        await pedidos.ActualizarConProductosAsync(actual, productosAActualizar);
        return await pedidos.ObtenerPorIdAsync(id);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var pedido = await pedidos.ObtenerPorIdAsync(id);
        if (pedido is null) return false;

        var producto = await productos.ObtenerPorIdAsync(pedido.ProductoId)
            ?? throw new InvalidOperationException("El producto asociado al pedido no existe.");
        producto.Stock += pedido.Cantidad;
        await pedidos.EliminarConProductoAsync(pedido, producto);
        return true;
    }

    private static void PrepararFecha(Pedido pedido)
    {
        if (pedido.FechaPedido == default) pedido.FechaPedido = DateTime.UtcNow;
    }
}
