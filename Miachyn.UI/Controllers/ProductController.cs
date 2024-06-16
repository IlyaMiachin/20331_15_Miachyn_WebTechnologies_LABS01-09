using Miachyn.Domain.Entities;
using Miachyn.UI.Services;
using Miachyn.UI.Services.CategoryService;
using Miachyn.UI.Services.FurnitureService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Miachyn.UI.Controllers
{
    [Authorize]
    public class ProductController(ICategoryService categoryService, IFurnitureService furnitureService) : Controller
    {
        //public async Task<IActionResult> Index()
        //{
        //    var productResponse = await furnitureService.GetFurnitureListAsync(null);
        //    if (!productResponse.Success)
        //        return NotFound(productResponse.ErrorMessage);
        //    return View(productResponse.Data.Items);
        //}
        [Route("Catalog")]
        [Route("Catalog/{category}")]
        //public async Task<IActionResult> Index(string? category)
        public async Task<IActionResult> Index(string? category, int pageNo = 1)
        {
            // Получить список категорий
            var categoriesResponse = await categoryService.GetCategoryListAsync();
            // Если список не получен, вернуть код 404
            if (!categoriesResponse.Success)
                return NotFound(categoriesResponse.ErrorMessage);
            // Передать список категорий во ViewData
            ViewData["categories"] = categoriesResponse.Data;
            // Передать во ViewData имя текущей категории
            var currentCategory = category ==  null ? "Все" : categoriesResponse.Data.FirstOrDefault(c => c.NormalizedName == category)?.Name;
            ViewData["currentCategory"] = currentCategory;
            var productResponse = await furnitureService.GetFurnitureListAsync(category, pageNo);
            if (!productResponse.Success)
                ViewData["Error"] = productResponse.ErrorMessage;
            //return View(productResponse.Data.Items);
            return View(productResponse.Data);
        }
    }
}
