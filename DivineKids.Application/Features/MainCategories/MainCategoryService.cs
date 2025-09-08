using DivineKids.Application.Contracts;
using DivineKids.Application.Features.Dtos;
using DivineKids.Application.Features.MainCategories.Commands;
using DivineKids.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace DivineKids.Application.Features.MainCategories;

public sealed class MainCategoryService(IGenericRepository<MainCategory> repository, ILogger<MainCategoryService> logger) : IMainCategoryService
{
    private readonly IGenericRepository<MainCategory> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<MainCategoryService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<IReadOnlyList<MainCategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return [.. entities.Select(MapToDto)];
    }

    public async Task<MainCategoryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity is null ? null : MapToDto(entity);
    }

    public async Task<MainCategoryDto> CreateAsync(CreateMainCategoryCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);
        if (string.IsNullOrWhiteSpace(command.Name))
            throw new ArgumentException("Name is required.", nameof(command));

        var entity = new MainCategory(command.Name, command.Description, command.ImageUrl);
        await _repository.AddAsync(entity, cancellationToken);

        _logger.LogInformation("Created MainCategory with Id {Id}", entity.Id);

        return MapToDto(entity);
    }

    public async Task<MainCategoryDto?> UpdateAsync(int id, UpdateMainCategoryCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);
        if (string.IsNullOrWhiteSpace(command.Name))
            throw new ArgumentException("Name is required.", nameof(command));

        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity is null) return null;

        entity.SetName(command.Name);
        entity.SetDescription(command.Description);
        entity.SetImageUrl(command.ImageUrl);

        await _repository.UpdateAsync(entity, cancellationToken);

        _logger.LogInformation("Updated MainCategory with Id {Id}", entity.Id);

        return MapToDto(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await _repository.DeleteAsync(id, cancellationToken);
        _logger.LogInformation("Deleted MainCategory with Id {Id}", id);
        return true;
    }

    private static MainCategoryDto MapToDto(MainCategory entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return new MainCategoryDto
        (
            entity.Id,
            entity.Name,
            entity.Description,
            entity.ImageUrl,
            entity.CreatedDate,
            entity.ModifiedDate
        );
    }
}