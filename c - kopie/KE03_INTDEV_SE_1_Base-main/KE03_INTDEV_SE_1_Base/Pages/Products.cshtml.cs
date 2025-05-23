using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

public class ProductsModel : PageModel
{
    private readonly IProductRepository _productRepository;

    public ProductsModel(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public IEnumerable<Product> Products { get; set; } = new List<Product>();

    public void OnGet()
    {
        Products = _productRepository.GetAllProducts();
    }

    public IActionResult OnPostAddToCart(int productId, int quantity)
    {
        var product = _productRepository.GetProductById(productId);
        if (product == null || quantity < 1)
        {
            return BadRequest();
        }

        var sessionData = HttpContext.Session.GetString("Cart");
        var cart = string.IsNullOrEmpty(sessionData)
            ? new List<CartItem>()
            : JsonSerializer.Deserialize<List<CartItem>>(sessionData);

        var existingItem = cart!.FirstOrDefault(p => p.Product.Id == productId);
        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            cart.Add(new CartItem { Product = product, Quantity = quantity });
        }

        HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));

        return new JsonResult(new { success = true });
    }

    public class CartItem
    {
        public Product Product { get; set; } = new();
        public int Quantity { get; set; }
    }
}

