using System.ComponentModel.DataAnnotations;

namespace Desafio03.Application.DTOs;

public class PedidoRequestDto
{
    public DateTime? FechaPedido { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
    public int Cantidad { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un cliente válido.")]
    public int ClienteId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un producto válido.")]
    public int ProductoId { get; set; }
}
