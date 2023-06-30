using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PizzaStore.Models
{
    public enum Topping
    {
        Pepperoni = 0,
        Sausage = 1,
        Sardines = 2,
        Onions = 3,
        Cheese = 4
    }

    public class Pizza
    {

        public int PizzaId { get; set; }
        [DisplayName("First Name")]
        public string? FirstName { get; set; }
        [DisplayName("Last Name")]
        public string? LastName { get; set;  }
        [DisplayName("Street")]
        public string Address { get; set; }
        [DisplayName("Add Veggies?")]
        public bool isVegetarian { get; set; }
        [Required]
        public decimal FinalPrice { get; set; }
        public Topping? Topping { get; set; }
       
    }

}
