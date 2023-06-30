using System.ComponentModel.DataAnnotations;

namespace PizzaStore.Models
{
    public class CartItem
    {
        [Key]
        public string CartItemId { get; set; } = Guid.NewGuid().ToString();
        public Pizza Pizza { get; set; }
        public int Quantity { get; set; }
        public string CartId { get; set; }
    }
}
