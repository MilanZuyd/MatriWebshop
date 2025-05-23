using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

public class ShoppingcartModel : PageModel
{
    public List<CartItem> CartItems { get; set; } = new();

    public void OnGet()
    {
        var sessionData = HttpContext.Session.GetString("Cart");
        if (!string.IsNullOrEmpty(sessionData))
        {
            CartItems = JsonSerializer.Deserialize<List<CartItem>>(sessionData) ?? new List<CartItem>();
        }
    }

    public class CartItem
    {
        public Product Product { get; set; } = new();
        public int Quantity { get; set; }
    }
}