using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Miachyn.Domain.Entities;
using Miachyn.UI.Services.FurnitureService;

namespace Miachyn.UI.Areas.Admin.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IFurnitureService _furnitureService;

        public DeleteModel(IFurnitureService furnitureService)
        {
            _furnitureService = furnitureService;
        }

        [BindProperty]
        public Furniture Furniture { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var furniture = await _furnitureService.GetFurnitureByIdAsync(id.Value);

            if (furniture == null)
            {
                return NotFound();
            }
            else
            {
                Furniture = furniture.Data;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var furniture = await _furnitureService.GetFurnitureByIdAsync(id.Value);
            if (furniture != null)
            {
                Furniture = furniture.Data;
                _furnitureService.DeleteFurnitureAsync(Furniture.Id);
            }
            return RedirectToPage("./Index");
        }
    }
}
