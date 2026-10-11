using Desafio03.Application.Interfaces.Repositories;
using Desafio03.Domain.Entities;
using Desafio03.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Desafio03.Infrastructure.Repositories;

public class ClienteRepository(AppDbContext context) : IClienteRepository
{
    public async Task<IReadOnlyList<Cliente>> ObtenerTodosAsync() =>
        await context.Clientes.AsNoTracking().OrderBy(x => x.Apellido).ThenBy(x => x.Nombre).ToListAsync();

    public Task<Cliente?> ObtenerPorIdAsync(int id) => context.Clientes.FirstOrDefaultAsync(x => x.Id == id);

    public Task<bool> EmailExisteAsync(string email, int? excluirId = null) =>
        context.Clientes.AnyAsync(x => x.Email == email && (!excluirId.HasValue || x.Id != excluirId.Value));

    public Task<bool> TienePedidosAsync(int clienteId) => context.Pedidos.AnyAsync(x => x.ClienteId == clienteId);

    public async Task<Cliente> CrearAsync(Cliente cliente)
    {
        context.Clientes.Add(cliente);
        await context.SaveChangesAsync();
        return cliente;
    }

    public async Task ActualizarAsync(Cliente cliente)
    {
        context.Clientes.Update(cliente);
        await context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Cliente cliente)
    {
        context.Clientes.Remove(cliente);
        await context.SaveChangesAsync();
    }
}
