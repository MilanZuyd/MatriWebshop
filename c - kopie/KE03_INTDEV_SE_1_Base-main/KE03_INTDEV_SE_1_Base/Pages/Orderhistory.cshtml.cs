using DataAccessLayer;   // <-- Add this to fix the error
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class OrderhistoryModel : PageModel
    {
        private readonly MatrixIncDbContext _context;

        public List<Order> Orders { get; set; } = new();

        public OrderhistoryModel(MatrixIncDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            Orders = _context.Orders
                .Include(o => o.Products)
                .ToList();
        }
    }
}
