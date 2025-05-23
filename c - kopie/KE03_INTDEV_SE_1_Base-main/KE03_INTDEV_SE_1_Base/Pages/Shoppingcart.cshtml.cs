using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DataAccessLayer.Interfaces;

public class ShoppingcartModel : PageModel
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public List<CartItem> CartItems { get; set; } = new();

    public ShoppingcartModel(IOrderRepository orderRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

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

    public IActionResult OnPostPurchase()
    {
        var sessionData = HttpContext.Session.GetString("Cart");
        if (!string.IsNullOrEmpty(sessionData))
        {
            CartItems = JsonSerializer.Deserialize<List<CartItem>>(sessionData) ?? new List<CartItem>();
        }

        if (!CartItems.Any())
        {
            TempData["Error"] = "Your cart is empty. Cannot place order.";
            return RedirectToPage();
        }

        var newOrder = new Order
        {
            OrderDate = DateTime.Now,
            CustomerId = 1 // Replace with real user ID when you have authentication
        };

        foreach (var item in CartItems)
        {
            var productFromDb = _productRepository.GetProductById(item.Product.Id);
            if (productFromDb != null)
            {
                for (int i = 0; i < item.Quantity; i++)
                {
                    newOrder.Products.Add(productFromDb);
                }
            }
        }

        try
        {
            _orderRepository.AddOrder(newOrder);
            HttpContext.Session.Remove("Cart");
            TempData["Message"] = "Order placed successfully!";
        }
        catch (Exception)
        {
            TempData["Error"] = "Error placing order. Please try again.";
        }

        return RedirectToPage();
    }

    public IActionResult OnPostRemoveItem(int productId)
    {
        var sessionData = HttpContext.Session.GetString("Cart");
        if (!string.IsNullOrEmpty(sessionData))
        {
            CartItems = JsonSerializer.Deserialize<List<CartItem>>(sessionData) ?? new List<CartItem>();
        }

        var itemToRemove = CartItems.FirstOrDefault(i => i.Product.Id == productId);
        if (itemToRemove != null)
        {
            CartItems.Remove(itemToRemove);
            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(CartItems));
            TempData["Message"] = "Item removed from cart.";
        }

        return RedirectToPage();
    }
}
