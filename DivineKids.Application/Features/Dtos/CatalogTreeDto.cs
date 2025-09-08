using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivineKids.Application.Features.Dtos;
public class CatalogTreeDto
{
    public List<MainCategoryNode> MainCategories { get; init; } = new();
}
