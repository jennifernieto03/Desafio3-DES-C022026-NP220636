using Desafio03.Application.DTOs;
using Desafio03.Application.Interfaces.Services;
using Desafio03.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Desafio03.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidosController(IPedidoService servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PedidoResponseDto>>> ObtenerTodos()
    {
        var pedidos = await servicio.ObtenerTodosAsync();
        return Ok(pedidos.Select(Mapear));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PedidoResponseDto>> ObtenerPorId(int id)
    {
        var pedido = await servicio.ObtenerPorIdAsync(id);
        return pedido is null ? NotFound(new { error = "Pedido no encontrado." }) : Ok(Mapear(pedido));
    }

    [HttpPost]
    public async Task<ActionResult<PedidoResponseDto>> Crear([FromBody] PedidoRequestDto dto)
    {
        var pedido = await servicio.CrearAsync(new Pedido
        {
            FechaPedido = dto.FechaPedido ?? DateTime.UtcNow,
            Cantidad = dto.Cantidad,
            ClienteId = dto.ClienteId,
            ProductoId = dto.ProductoId
        });
        return CreatedAtAction(nameof(ObtenerPorId), new { id = pedido.Id }, Mapear(pedido));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PedidoResponseDto>> Actualizar(int id, [FromBody] PedidoRequestDto dto)
    {
        var pedido = await servicio.ActualizarAsync(id, new Pedido
        {
            FechaPedido = dto.FechaPedido ?? DateTime.UtcNow,
            Cantidad = dto.Cantidad,
            ClienteId = dto.ClienteId,
            ProductoId = dto.ProductoId
        });
        return pedido is null ? NotFound(new { error = "Pedido no encontrado." }) : Ok(Mapear(pedido));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var eliminado = await servicio.EliminarAsync(id);
        return eliminado ? NoContent() : NotFound(new { error = "Pedido no encontrado." });
    }

    private static PedidoResponseDto Mapear(Pedido x) => new(
        x.Id,
        x.FechaPedido,
        x.Cantidad,
        x.ClienteId,
        x.Cliente?.Nombre,
        x.ProductoId,
        x.Producto?.Nombre,
        x.Producto?.Precio);
}
