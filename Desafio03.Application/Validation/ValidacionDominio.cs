using System.ComponentModel.DataAnnotations;

namespace Desafio03.Application.Validation;

public static class ValidacionDominio
{
    public static void Validar(object entidad)
    {
        ArgumentNullException.ThrowIfNull(entidad);
        var contexto = new ValidationContext(entidad);
        Validator.ValidateObject(entidad, contexto, validateAllProperties: true);
    }
}
