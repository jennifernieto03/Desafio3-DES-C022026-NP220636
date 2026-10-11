using Desafio03.Application.DTOs;
using Desafio03.Application.Interfaces.Services;
using Desafio03.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Desafio03.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController(IProductoService servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductoResponseDto>>> ObtenerTodos()
    {
        var productos = await servicio.ObtenerTodosAsync();
        return Ok(productos.Select(Mapear));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductoResponseDto>> ObtenerPorId(int id)
    {
        var producto = await servicio.ObtenerPorIdAsync(id);
        return producto is null ? NotFound(new { error = "Producto no encontrado." }) : Ok(Mapear(producto));
    }

    [HttpPost]
    public async Task<ActionResult<ProductoResponseDto>> Crear([FromBody] ProductoRequestDto dto)
    {
        var producto = await servicio.CrearAsync(new Producto
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            Precio = dto.Precio,
            Stock = dto.Stock
        });
        return CreatedAtAction(nameof(ObtenerPorId), new { id = producto.Id }, Mapear(producto));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductoResponseDto>> Actualizar(int id, [FromBody] ProductoRequestDto dto)
    {
        var producto = await servicio.ActualizarAsync(id, new Producto
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            Precio = dto.Precio,
            Stock = dto.Stock
        });
        return producto is null ? NotFound(new { error = "Producto no encontrado." }) : Ok(Mapear(producto));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var eliminado = await servicio.EliminarAsync(id);
        return eliminado ? NoContent() : NotFound(new { error = "Producto no encontrado." });
    }

    private static ProductoResponseDto Mapear(Producto x) => new(x.Id, x.Nombre, x.Descripcion, x.Precio, x.Stock);
}
