using DivineKids.Application.Features.Categories.Commands;
using DivineKids.Application.Features.Dtos;

namespace DivineKids.Application.Features.Categories;
public interface ICategoryService
{
    Task<IReadOnlyList<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CategoryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<CategoryDto> CreateAsync(CreateCategoryCommand command, CancellationToken cancellationToken = default);
    Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryCommand command, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
