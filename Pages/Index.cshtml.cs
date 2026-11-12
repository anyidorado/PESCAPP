using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PESCAPP.Data;
using PESCAPP.Models;

namespace PESCAPP.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _context;

        public IndexModel(
            ILogger<IndexModel> logger,
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public List<PESCAPP.Models.Producto> Productos { get; set; } = new();

        public async Task OnGetAsync()
        {
            Productos = await _context.Productos
                .Include(p => p.Vendedor)
                .Where(p => p.Disponible && p.CantidadDisponible > 0)
                .OrderByDescending(p => p.FechaPublicacion)
                .ToListAsync();
        }
    }
}