using Miachyn.Domain.Entities;
using Miachyn.Domain.Models;

namespace Miachyn.UI.Services.CategoryService
{
    public class MemoryCategoryService : ICategoryService
    {
        public Task<ResponseData<List<Category>>>
        GetCategoryListAsync()
        {
            var categories = new List<Category>
            {
                new Category {Id=1, Name="Стулья",
                    NormalizedName="chairs"},
                new Category {Id=2, Name="Столы и письменные столы",
                    NormalizedName="tables"},
                new Category {Id=3, Name="Стеллажи и книжные шкафы",
                    NormalizedName="shelves"},
                new Category {Id=4, Name="Комоды",
                    NormalizedName="drawers"}
            };
            var result = new ResponseData<List<Category>>();
            result.Data = categories;
            return Task.FromResult(result);
        }
    }
}
