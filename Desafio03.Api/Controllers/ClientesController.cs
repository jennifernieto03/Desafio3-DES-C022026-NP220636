using Desafio03.Application.DTOs;
using Desafio03.Application.Interfaces.Services;
using Desafio03.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Desafio03.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController(IClienteService servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClienteResponseDto>>> ObtenerTodos()
    {
        var clientes = await servicio.ObtenerTodosAsync();
        return Ok(clientes.Select(Mapear));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClienteResponseDto>> ObtenerPorId(int id)
    {
        var cliente = await servicio.ObtenerPorIdAsync(id);
        return cliente is null ? NotFound(new { error = "Cliente no encontrado." }) : Ok(Mapear(cliente));
    }

    [HttpPost]
    public async Task<ActionResult<ClienteResponseDto>> Crear([FromBody] ClienteRequestDto dto)
    {
        var cliente = await servicio.CrearAsync(new Cliente
        {
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            Email = dto.Email
        });
        return CreatedAtAction(nameof(ObtenerPorId), new { id = cliente.Id }, Mapear(cliente));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ClienteResponseDto>> Actualizar(int id, [FromBody] ClienteRequestDto dto)
    {
        var cliente = await servicio.ActualizarAsync(id, new Cliente
        {
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            Email = dto.Email
        });
        return cliente is null ? NotFound(new { error = "Cliente no encontrado." }) : Ok(Mapear(cliente));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var eliminado = await servicio.EliminarAsync(id);
        return eliminado ? NoContent() : NotFound(new { error = "Cliente no encontrado." });
    }

    private static ClienteResponseDto Mapear(Cliente x) => new(x.Id, x.Nombre, x.Apellido, x.Email);
}
