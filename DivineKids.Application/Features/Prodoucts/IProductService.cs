using DivineKids.Application.Features.Dtos;

namespace DivineKids.Application.Features.Prodoucts;
public interface IProductService
{
    Task<ProductDto> CreateProductAsync(Commands.CreateProductCommand command, CancellationToken cancellationToken);
    Task<ProductDto> UpdateProductAsync(int id, Commands.UpdateProductCommand command, CancellationToken cancellationToken);
    Task<bool> DeleteProductAsync(int id, CancellationToken cancellationToken);
    Task<ProductDto?> GetProductByIdAsync(int id, CancellationToken cancellationToken);
    Task<IReadOnlyList<ProductDto>> GetAllProductsAsync(CancellationToken cancellationToken);
}
