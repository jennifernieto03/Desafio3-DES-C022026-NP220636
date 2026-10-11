using Desafio03.Application.Interfaces.Repositories;
using Desafio03.Application.Interfaces.Services;
using Desafio03.Application.Validation;
using Desafio03.Domain.Entities;

namespace Desafio03.Application.Services;

public class ProductoService(IProductoRepository repositorio) : IProductoService
{
    public Task<IReadOnlyList<Producto>> ObtenerTodosAsync() => repositorio.ObtenerTodosAsync();

    public Task<Producto?> ObtenerPorIdAsync(int id) => repositorio.ObtenerPorIdAsync(id);

    public async Task<Producto> CrearAsync(Producto producto)
    {
        Normalizar(producto);
        ValidacionDominio.Validar(producto);
        return await repositorio.CrearAsync(producto);
    }

    public async Task<Producto?> ActualizarAsync(int id, Producto datos)
    {
        var actual = await repositorio.ObtenerPorIdAsync(id);
        if (actual is null) return null;

        Normalizar(datos);
        ValidacionDominio.Validar(datos);
        actual.Nombre = datos.Nombre;
        actual.Descripcion = datos.Descripcion;
        actual.Precio = datos.Precio;
        actual.Stock = datos.Stock;
        await repositorio.ActualizarAsync(actual);
        return actual;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var producto = await repositorio.ObtenerPorIdAsync(id);
        if (producto is null) return false;
        if (await repositorio.TienePedidosAsync(id))
            throw new InvalidOperationException("No se puede eliminar el producto porque tiene pedidos asociados.");

        await repositorio.EliminarAsync(producto);
        return true;
    }

    private static void Normalizar(Producto producto)
    {
        producto.Nombre = producto.Nombre.Trim();
        producto.Descripcion = producto.Descripcion.Trim();
    }
}
