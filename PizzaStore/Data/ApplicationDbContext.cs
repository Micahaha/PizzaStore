using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PizzaStore.Models;

namespace PizzaStore.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }


        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<CartItem> ShoppingCartItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
    }
}