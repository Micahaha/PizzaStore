namespace PizzaStore.Models
{
    public class Cart
    {
        public string CartId { get; set; }
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
