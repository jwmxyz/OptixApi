using Optix.Services.Models.DTO;

namespace Optix.Services.Validation;

public class MovieSearchParamValidator : IValidator<MovieSearchParams>
{
    public bool Validate(MovieSearchParams entity, out List<string> validationErrors)
    {
        validationErrors = [];
        
        if (string.IsNullOrEmpty(entity.Title))
        {
            validationErrors.Add("Movie title cannot be null or empty.");
        }
        
        if (!string.IsNullOrEmpty(entity.SortBy) && !entity.ValidSortByOptions.Contains(entity.SortBy, StringComparer.InvariantCultureIgnoreCase))
        {
            validationErrors.Add($"Sortby can only contain {string.Join("," , entity.ValidSortByOptions)}");
        }

        if (!string.IsNullOrEmpty(entity.OrderBy) && !Constants.SearchConstants.OrderByConsts.Contains(entity.OrderBy, StringComparer.InvariantCultureIgnoreCase))
        {
            validationErrors.Add($"OrderBy can only contain {string.Join("," , Constants.SearchConstants.OrderByConsts)}");
        }
        
        if (entity.Limit < 1)
        {
            validationErrors.Add("Limit must be a positive integer.");
        }

        if (entity.Page < 1)
        {
            validationErrors.Add("Page must be a positive integer.");
        }
        
        return validationErrors.Count == 0;
    }
}