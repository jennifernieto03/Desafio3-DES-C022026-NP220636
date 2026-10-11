using Desafio03.Application.Interfaces.Repositories;
using Desafio03.Application.Interfaces.Services;
using Desafio03.Application.Validation;
using Desafio03.Domain.Entities;

namespace Desafio03.Application.Services;

public class ClienteService(IClienteRepository repositorio) : IClienteService
{
    public Task<IReadOnlyList<Cliente>> ObtenerTodosAsync() => repositorio.ObtenerTodosAsync();

    public Task<Cliente?> ObtenerPorIdAsync(int id) => repositorio.ObtenerPorIdAsync(id);

    public async Task<Cliente> CrearAsync(Cliente cliente)
    {
        Normalizar(cliente);
        ValidacionDominio.Validar(cliente);
        if (await repositorio.EmailExisteAsync(cliente.Email))
            throw new InvalidOperationException("Ya existe un cliente registrado con ese correo electrónico.");

        return await repositorio.CrearAsync(cliente);
    }

    public async Task<Cliente?> ActualizarAsync(int id, Cliente datos)
    {
        var actual = await repositorio.ObtenerPorIdAsync(id);
        if (actual is null) return null;

        Normalizar(datos);
        ValidacionDominio.Validar(datos);
        if (await repositorio.EmailExisteAsync(datos.Email, id))
            throw new InvalidOperationException("Ya existe otro cliente registrado con ese correo electrónico.");

        actual.Nombre = datos.Nombre;
        actual.Apellido = datos.Apellido;
        actual.Email = datos.Email;
        await repositorio.ActualizarAsync(actual);
        return actual;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var cliente = await repositorio.ObtenerPorIdAsync(id);
        if (cliente is null) return false;
        if (await repositorio.TienePedidosAsync(id))
            throw new InvalidOperationException("No se puede eliminar el cliente porque tiene pedidos asociados.");

        await repositorio.EliminarAsync(cliente);
        return true;
    }

    private static void Normalizar(Cliente cliente)
    {
        cliente.Nombre = cliente.Nombre.Trim();
        cliente.Apellido = cliente.Apellido.Trim();
        cliente.Email = cliente.Email.Trim();
    }
}
