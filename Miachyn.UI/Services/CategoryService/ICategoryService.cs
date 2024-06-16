using Miachyn.Domain.Entities;
using Miachyn.Domain.Models;

namespace Miachyn.UI.Services.CategoryService
{
    public interface ICategoryService
    {
        /// Получение списка всех категорий
        public Task<ResponseData<List<Category>>> GetCategoryListAsync();
    }
}
