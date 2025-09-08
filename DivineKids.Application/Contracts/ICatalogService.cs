using DivineKids.Application.Features.Dtos;

namespace DivineKids.Application.Contracts;
public interface ICatalogService
{
    Task<CatalogTreeDto> GetCatalogTreeAsync(CancellationToken cancellationToken);
}
