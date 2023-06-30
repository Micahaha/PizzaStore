using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PizzaStore.Data;
using PizzaStore.Models;
using PizzaStore.Services;

namespace PizzaStore.Controllers
{
    public class ShoppingController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly CartService _cartService;

        public ShoppingController(ApplicationDbContext db, CartService cartService)
        {

            _db = db;
            _cartService = cartService;
        }

        public IActionResult Index()
        {
            return View();
        }
       

        [HttpPost]
        public async Task<IActionResult> Order(Pizza pizza)
        {
            if (ModelState.IsValid) 
            {
                add_total(pizza);
              
                var matchingItem = _db.ShoppingCartItems.FirstOrDefault(x =>
                    x.Pizza.FinalPrice == pizza.FinalPrice &&
                    x.Pizza.isVegetarian == pizza.isVegetarian &&
                    x.Pizza.Topping == pizza.Topping);

                    if (matchingItem != null)
                    {
                        matchingItem.Quantity += 1;
                        _db.SaveChanges();
                    }
                
                 
              
                await _cartService.AddToCart(pizza, 1);
                _db.SaveChanges();
               

                await _db.SaveChangesAsync();
                return RedirectToAction("Cart");
            }
            
            return View(pizza);

        }

        [Authorize]
        public async Task<IActionResult> Cart()
        {
            var cart = await _cartService.GetCart();
            return View(cart);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var item = await _db.ShoppingCartItems.FindAsync(id);

            _db.ShoppingCartItems.Remove(item);

            if (item != null)
            {
                _db.ShoppingCartItems.Remove(item);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Cart");
        }

        [Authorize]
        public IActionResult Order() 
        {
            List<SelectListItem> options = new List<SelectListItem>()
            {
                new SelectListItem {Value = Topping.Pepperoni.ToString(), Text = "Pepperoni" },
                new SelectListItem {Value = Topping.Sardines.ToString(), Text = "Sardines" },
                new SelectListItem {Value = Topping.Onions.ToString(), Text = "Onions" },
                new SelectListItem {Value = Topping.Sausage.ToString(), Text = "Sausage" },
            };

            ViewData["Options"] = options;
            return View();
        }

        [Authorize]
        public IActionResult Checkout()
        {
            // ADD functionality to send to admin 
            return View();
        }


            public Pizza add_total(Pizza pizza) 
        {
            var base_price = 12.00M;

            if (pizza.isVegetarian)
            {
                base_price += 2.00m;
            }

            switch (pizza.Topping)
            {
                case Topping.Pepperoni:
                    base_price += 1.50M;
                    break;
                case Topping.Sardines:
                    base_price += 2.00M;
                    break;
                case Topping.Onions:
                    base_price += 1.50M;
                    break;
                case Topping.Sausage:
                    base_price += 1.20M;
                    break;
            }
            
            pizza.FinalPrice = base_price;


            return pizza;
        }
    }
}
