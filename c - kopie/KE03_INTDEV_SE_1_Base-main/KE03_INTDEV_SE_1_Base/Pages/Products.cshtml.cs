using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using DataAccessLayer;


public class ProductsModel : PageModel
{
    private readonly MatrixIncDbContext _context;

    public ProductsModel(MatrixIncDbContext context)
    {
        _context = context;
    }

    public List<Product> Products { get; set; }

    public async Task OnGetAsync()
    {
        Products = await _context.Products.ToListAsync();
    }
}
