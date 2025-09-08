using DivineKids.Application.Contracts;
using DivineKids.Application.Features.Dtos;
using DivineKids.Application.Features.Prodoucts.Commands;
using DivineKids.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace DivineKids.Application.Features.Prodoucts;
public sealed class ProductService(IGenericRepository<Product> genericRepository, ILogger<ProductService> logger) : IProductService
{
    private readonly IGenericRepository<Product> _repository = genericRepository ?? throw new ArgumentNullException(nameof(genericRepository));
    private readonly ILogger<ProductService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<ProductDto> CreateProductAsync(CreateProductCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        if (string.IsNullOrWhiteSpace(command.Name)) throw new ArgumentException("Name is required.", nameof(command));

        var entity = new Product(command.Name, command.Price, command.CategoryId, command.WeightKg ?? 0, command.LengthCm,
            command.WidthCm ?? 0, command.HeightCm, command.Description, command.ImageUrl);

        await _repository.AddAsync(entity, cancellationToken);

        _logger.LogInformation("Created Product with Id {Id}", entity.Id);

        return MapToDto(entity);
    }

    public async Task<bool> DeleteProductAsync(int id, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(id, cancellationToken);
        _logger.LogInformation("Deleted Category with Id {Id}", id);
        return true;
    }

    public async Task<IReadOnlyList<ProductDto>> GetAllProductsAsync(CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return [.. entities.Select(MapToDto)];
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity is null ? null : MapToDto(entity);
    }

    public async Task<ProductDto> UpdateProductAsync(int id, UpdateProductCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        if (string.IsNullOrWhiteSpace(command.Name))
            throw new ArgumentException("Name is required.", nameof(command));

        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        ArgumentNullException.ThrowIfNull(entity);

        entity.SetName(command.Name);
        entity.SetDescription(command.Description);
        entity.SetImageUrl(command.ImageUrl);
        entity.SetCategory(command.CategoryId);
        entity.SetRequiresShipping(command.RequiresShipping);
        entity.SetWeightKg(command.WeightKg);
        entity.SetDimensions(entity.LengthCm, entity.WidthCm, entity.HeightCm);
        entity.SetPrice(command.Price);

        await _repository.UpdateAsync(entity, cancellationToken);

        _logger.LogInformation("Updated Product with Id {Id}", entity.Id);

        return MapToDto(entity);
    }

    private static ProductDto MapToDto(Product entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Price = entity.Price,
        CategoryId = entity.CategoryId,
        WeightKg = entity.WeightKg,
        LengthCm = entity.LengthCm,
        HeightCm = entity.HeightCm,
        Description = entity.Description,
        ImageUrl = entity.ImageUrl,
        CreatedDate = entity.CreatedDate,
        ModifiedDate = entity.ModifiedDate,
        RequiresShipping = entity.RequiresShipping,
        WidthCm = entity.WidthCm
    };
}
