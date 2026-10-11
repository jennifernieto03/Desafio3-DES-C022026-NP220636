using System.ComponentModel.DataAnnotations;
using Desafio03.Application.Services;
using Desafio03.Domain.Entities;
using Desafio03.Infrastructure.Persistence;
using Desafio03.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Desafio03.Tests;

public class PedidosTests
{
    private static AppDbContext CrearContexto()
    {
        var opciones = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase($"Desafio03_{Guid.NewGuid()}")
            .Options;
        return new AppDbContext(opciones);
    }

    private static (ClienteService Clientes, ProductoService Productos, PedidoService Pedidos) CrearServicios(AppDbContext db)
    {
        var clienteRepo = new ClienteRepository(db);
        var productoRepo = new ProductoRepository(db);
        var pedidoRepo = new PedidoRepository(db);
        return (
            new ClienteService(clienteRepo),
            new ProductoService(productoRepo),
            new PedidoService(clienteRepo, productoRepo, pedidoRepo));
    }

    private static Cliente ClienteValido() => new()
    {
        Nombre = "Ana",
        Apellido = "Lopez",
        Email = $"ana{Guid.NewGuid():N}@example.com"
    };

    private static Producto ProductoValido(int stock = 10) => new()
    {
        Nombre = "Teclado mecanico",
        Descripcion = "Teclado para pruebas unitarias",
        Precio = 35.50m,
        Stock = stock
    };

    [Fact]
    public async Task CrearCliente_DatosValidos_GuardaCliente()
    {
        using var db = CrearContexto();
        var servicios = CrearServicios(db);

        var creado = await servicios.Clientes.CrearAsync(ClienteValido());

        Assert.True(creado.Id > 0);
        Assert.Equal("Ana", creado.Nombre);
        Assert.Contains("@example.com", creado.Email);
    }

    [Fact]
    public void Cliente_DatosIncorrectos_AnotacionesDetectanErrores()
    {
        var cliente = new Cliente { Nombre = "", Apellido = "L", Email = "correo-invalido" };
        var resultados = new List<ValidationResult>();
        var valido = Validator.TryValidateObject(cliente, new ValidationContext(cliente), resultados, validateAllProperties: true);

        Assert.False(valido);
        Assert.Contains(resultados, x => x.MemberNames.Contains(nameof(Cliente.Nombre)));
        Assert.Contains(resultados, x => x.MemberNames.Contains(nameof(Cliente.Email)));
    }

    [Fact]
    public async Task CrearProducto_DatosValidos_GuardaProducto()
    {
        using var db = CrearContexto();
        var servicios = CrearServicios(db);

        var creado = await servicios.Productos.CrearAsync(ProductoValido());

        Assert.True(creado.Id > 0);
        Assert.Equal(35.50m, creado.Precio);
        Assert.Equal(10, creado.Stock);
    }

    [Theory]
    [InlineData(0.00, 10)]
    [InlineData(-1.00, 10)]
    [InlineData(35.50, -1)]
    public void Producto_PrecioOStockIncorrectos_AnotacionesDetectanErrores(double precio, int stock)
    {
        var producto = new Producto
        {
            Nombre = "Producto",
            Descripcion = "Descripción válida",
            Precio = (decimal)precio,
            Stock = stock
        };
        var resultados = new List<ValidationResult>();
        var valido = Validator.TryValidateObject(producto, new ValidationContext(producto), resultados, validateAllProperties: true);

        Assert.False(valido);
        if (precio <= 0)
            Assert.Contains(resultados, x => x.MemberNames.Contains(nameof(Producto.Precio)));
        if (stock < 0)
            Assert.Contains(resultados, x => x.MemberNames.Contains(nameof(Producto.Stock)));
    }

    [Fact]
    public async Task CrearPedido_CantidadDisponible_CreaPedidoYDescuentaStock()
    {
        using var db = CrearContexto();
        var servicios = CrearServicios(db);
        var cliente = await servicios.Clientes.CrearAsync(ClienteValido());
        var producto = await servicios.Productos.CrearAsync(ProductoValido(10));

        var pedido = await servicios.Pedidos.CrearAsync(new Pedido
        {
            Cantidad = 3,
            ClienteId = cliente.Id,
            ProductoId = producto.Id
        });

        Assert.True(pedido.Id > 0);
        Assert.NotEqual(default, pedido.FechaPedido);
        var productoActualizado = await servicios.Productos.ObtenerPorIdAsync(producto.Id);
        Assert.NotNull(productoActualizado);
        Assert.Equal(7, productoActualizado.Stock);
    }

    [Fact]
    public async Task ConsultarPedido_Existente_RetornaPedido()
    {
        using var db = CrearContexto();
        var servicios = CrearServicios(db);
        var cliente = await servicios.Clientes.CrearAsync(ClienteValido());
        var producto = await servicios.Productos.CrearAsync(ProductoValido());
        var creado = await servicios.Pedidos.CrearAsync(new Pedido
        {
            Cantidad = 2,
            ClienteId = cliente.Id,
            ProductoId = producto.Id
        });

        var consultado = await servicios.Pedidos.ObtenerPorIdAsync(creado.Id);

        Assert.NotNull(consultado);
        Assert.Equal(creado.Id, consultado.Id);
        Assert.Equal(2, consultado.Cantidad);
    }

    [Fact]
    public async Task ConsultarPedido_Inexistente_RetornaNulo()
    {
        using var db = CrearContexto();
        var servicios = CrearServicios(db);

        var consultado = await servicios.Pedidos.ObtenerPorIdAsync(98765);

        Assert.Null(consultado);
    }

    [Fact]
    public async Task CrearPedido_SinStockSuficiente_RechazaOperacion()
    {
        using var db = CrearContexto();
        var servicios = CrearServicios(db);
        var cliente = await servicios.Clientes.CrearAsync(ClienteValido());
        var producto = await servicios.Productos.CrearAsync(ProductoValido(2));

        var error = await Assert.ThrowsAsync<InvalidOperationException>(() => servicios.Pedidos.CrearAsync(new Pedido
        {
            Cantidad = 3,
            ClienteId = cliente.Id,
            ProductoId = producto.Id
        }));

        Assert.Contains("Stock insuficiente", error.Message);
        var productoActualizado = await servicios.Productos.ObtenerPorIdAsync(producto.Id);
        Assert.NotNull(productoActualizado);
        Assert.Equal(2, productoActualizado.Stock);
    }
}
