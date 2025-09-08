using DivineKids.Application.Contracts;
using DivineKids.Application.Features.Categories.Commands;
using DivineKids.Application.Features.Dtos;
using DivineKids.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace DivineKids.Application.Features.Categories;
public sealed class CategoryService(IGenericRepository<Category> repository, ILogger<CategoryService> logger) : ICategoryService
{
    private readonly IGenericRepository<Category> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<CategoryService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<CategoryDto> CreateAsync(CreateCategoryCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);
        if (string.IsNullOrWhiteSpace(command.Name))
            throw new ArgumentException("Name is required.", nameof(command));

        var entity = new Category(command.Name, command.MainCategoryId, command.Description, command.ImageUrl);
        await _repository.AddAsync(entity, cancellationToken);

        _logger.LogInformation("Created Category with Id {Id}", entity.Id);

        return MapToDto(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await _repository.DeleteAsync(id, cancellationToken);
        _logger.LogInformation("Deleted Category with Id {Id}", id);
        return true;
    }

    public async Task<IReadOnlyList<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return [.. entities.Select(MapToDto)];
    }

    public async Task<CategoryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity is null ? null : MapToDto(entity);
    }

    public async Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);
        if (string.IsNullOrWhiteSpace(command.Name))
            throw new ArgumentException("Name is required.", nameof(command));

        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity is null) return null;

        entity.SetName(command.Name);
        entity.SetDescription(command.Description);
        entity.SetImageUrl(command.ImageUrl);
        entity.SetMainCategory(command.MainCategoryId);

        await _repository.UpdateAsync(entity, cancellationToken);

        _logger.LogInformation("Updated MainCategory with Id {Id}", entity.Id);

        return MapToDto(entity);
    }

    private static CategoryDto MapToDto(Category entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return new CategoryDto(
            entity.Id,
            entity.Name,
            entity.Description,
            entity.ImageUrl,
            entity.MainCategoryId,
            entity.CreatedDate,
            entity.ModifiedDate
        );
    }
}
