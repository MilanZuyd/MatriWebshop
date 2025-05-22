using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using DataAccessLayer;


public class PartsModel : PageModel
{
    private readonly MatrixIncDbContext _context;

    public PartsModel(MatrixIncDbContext context)
    {
        _context = context;
    }

    public IList<Part> Parts { get; set; }

    public async Task OnGetAsync()
    {
        Parts = await _context.Parts
            .Include(p => p.Products)
            .ToListAsync();
    }
}
