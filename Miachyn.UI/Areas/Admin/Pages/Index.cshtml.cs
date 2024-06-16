using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Miachyn.Domain.Entities;
using Miachyn.UI.Services.FurnitureService;
using Microsoft.AspNetCore.Authorization;

namespace Miachyn.UI.Areas.Admin.Pages
{
    [Authorize(Policy = "admin")]
    public class IndexModel : PageModel
    {       
        private readonly IFurnitureService _furnitureService;

        public IndexModel(IFurnitureService furnitureService)
        {
            _furnitureService = furnitureService;
        }

        public List<Furniture> Furniture { get;set; } = default!;
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; } = 1;

        public async Task OnGetAsync(int? pageNo = 1)
        {
            var response = await _furnitureService.GetFurnitureListAsync(null, pageNo.Value);
            if (response.Success)
            {
                Furniture = response.Data.Items;
                CurrentPage = response.Data.CurrentPage;
                TotalPages = response.Data.TotalPages;
            }
        }
    }
}
