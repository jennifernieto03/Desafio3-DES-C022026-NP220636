namespace Desafio03.Application.DTOs;

public record PedidoResponseDto(
    int Id,
    DateTime FechaPedido,
    int Cantidad,
    int ClienteId,
    string? ClienteNombre,
    int ProductoId,
    string? ProductoNombre,
    decimal? PrecioUnitario);
