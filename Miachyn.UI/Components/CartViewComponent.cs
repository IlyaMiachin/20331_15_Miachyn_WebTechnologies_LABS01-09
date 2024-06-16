using Miachyn.Domain.Models;
using Miachyn.UI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Miachyn.UI.Components
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.Get<Cart>("cart");
            return View(cart);
        }
    }
}
