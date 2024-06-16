using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Miachyn.Domain.Entities;
using Miachyn.UI.Services.CategoryService;
using Miachyn.UI.Services.FurnitureService;

namespace Miachyn.UI.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IFurnitureService _furnitureService;
        private readonly ICategoryService _categoryService;

        public EditModel(IFurnitureService furnitureService, ICategoryService categoryService)
        {
            _furnitureService = furnitureService;
            _categoryService = categoryService;
        }

        [BindProperty]
        public Furniture Furniture { get; set; } = default!;
        public SelectList CategoryList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var furniture =  await _furnitureService.GetFurnitureByIdAsync(id.Value);
            if (furniture == null)
            {
                return NotFound();
            }
            Furniture = furniture.Data;
            var categoryListData = await _categoryService.GetCategoryListAsync();
            ViewData["CategoryId"] = new SelectList(categoryListData.Data, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _furnitureService.UpdateFurnitureAsync(Furniture.Id, Furniture, null);
            }
            catch (Exception)
            {
                if (!FurnitureExists(Furniture.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool FurnitureExists(int id)
        {
            return true;
        }
    }
}
