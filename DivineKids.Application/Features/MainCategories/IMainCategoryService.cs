using DivineKids.Application.Features.Dtos;
using DivineKids.Application.Features.MainCategories.Commands;

namespace DivineKids.Application.Features.MainCategories;

public interface IMainCategoryService
{
    Task<IReadOnlyList<MainCategoryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<MainCategoryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<MainCategoryDto> CreateAsync(CreateMainCategoryCommand command, CancellationToken cancellationToken = default);
    Task<MainCategoryDto?> UpdateAsync(int id, UpdateMainCategoryCommand command, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}