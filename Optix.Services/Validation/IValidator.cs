namespace Optix.Services.Validation;

public interface IValidator<T>
{
    bool Validate(T entity, out List<string> validationErrors);
}