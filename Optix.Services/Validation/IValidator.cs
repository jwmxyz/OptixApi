namespace Optix.Services.Validation;

public interface IValidator<T>
{
    /// <summary>
    /// Method used to validate some enity
    /// </summary>
    /// <param name="entity">the entity that we wish to validate.</param>
    /// <param name="validationErrors">Any errors that were part of the validation.</param>
    /// <returns>true if the object T is valid; false otherwise.</returns>
    bool Validate(T entity, out List<string> validationErrors);
}