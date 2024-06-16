using Miachyn.Domain.Models;
using Miachyn.UI.Extensions;
using Miachyn.UI.Services.FurnitureService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Miachyn.UI.Controllers
{
    public class CartController : Controller
    {
        private readonly IFurnitureService _furnitureService;
        private Cart _cart;

        public CartController(IFurnitureService furnitureService)
        {
            _furnitureService = furnitureService;
        }

        // GET: CartController
        public ActionResult Index()
        {
            _cart = HttpContext.Session.Get<Cart>("cart") ?? new();
            return View(_cart.CartItems);
        }

        [Route("[controller]/add/{id:int}")]
        public async Task<ActionResult> Add(int id, string returnUrl)
        {
            var data = await _furnitureService.GetFurnitureByIdAsync(id);
            if (data.Success)
            {
                _cart = HttpContext.Session.Get<Cart>("cart") ?? new();
                _cart.AddToCart(data.Data);
                HttpContext.Session.Set<Cart>("cart", _cart);
            }
            return Redirect(returnUrl);
        }

        [Route("[controller]/remove/{id:int}")]
        public ActionResult Remove(int id)
        {
            _cart = HttpContext.Session.Get<Cart>("cart") ?? new();
            _cart.RemoveItems(id);
            HttpContext.Session.Set<Cart>("cart", _cart);
            return RedirectToAction("index");
        }
    }
}
