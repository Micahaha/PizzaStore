using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PizzaStore.Data;
using PizzaStore.Models;

namespace PizzaStore.Services
{
    public class CartService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;


        public CartService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        private string GetCardId() 
        {
           // Generate a new GUID and store it as a cookie if not existing. 
            return _httpContextAccessor.HttpContext.Request.Cookies["CartId"] ?? Guid.NewGuid().ToString();
        }

        public async Task<Cart> GetCart() 
        {
            var cartId = GetCardId();
            var cart = await _context.Carts.Include(c => c.CartItems)
                .ThenInclude(ci => ci.Pizza).
                FirstOrDefaultAsync(c => c.CartId == cartId);

            if (cart == null) 
            {
                cart = new Cart { CartId = cartId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();

                _httpContextAccessor.HttpContext.Response.Cookies.Append("CartId", cartId, new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(7) });

            }

            return cart;
        }

        public async Task AddToCart(Pizza pizza, int quantity)
        {
            var cart = await GetCart();

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Pizza.PizzaId == pizza.PizzaId);
            if (cartItem == null)
            {
                cartItem = new CartItem { Pizza = pizza, Quantity = quantity };
                cart.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            await _context.SaveChangesAsync();
        }


    }
}
