using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Miachyn.Domain.Entities;
using Miachyn.UI.Services.FurnitureService;
using Miachyn.UI.Services.CategoryService;

namespace Miachyn.UI.Areas.Admin.Pages
{
    public class CreateModel (ICategoryService categoryService, IFurnitureService furnitureService) : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            var categoryListData = await categoryService.GetCategoryListAsync();
            ViewData["CategoryId"] = new SelectList(categoryListData.Data, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Furniture Furniture { get; set; } = default!;

        [BindProperty]
        public IFormFile? Image { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await furnitureService.CreateFurnitureAsync(Furniture, Image);
            return RedirectToPage("./Index");
        }
    }
}
