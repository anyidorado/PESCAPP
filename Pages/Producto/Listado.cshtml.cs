using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PESCAPP.Data;
using PESCAPP.Models;
using Microsoft.AspNetCore.Authorization;

namespace PESCAPP.Pages.Producto
{
    [Authorize]
    public class ListadoModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ListadoModel(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<PESCAPP.Models.Producto> Productos { get; set; } = new();

        public string? UsuarioId { get; set; }

        public bool EsVendedor { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var usuario = await _userManager.GetUserAsync(User);

            if (usuario == null || usuario.UserType != "Vendedor")
            {
                return Forbid();
            }

            UsuarioId = usuario.Id;
            EsVendedor = true;

            Productos = await _context.Productos
                .Where(p => p.VendedorId == usuario.Id)
                .Include(p => p.Vendedor)
                .OrderByDescending(p => p.FechaPublicacion)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostCambiarDisponibilidadAsync(int id)
        {
            var usuario = await _userManager.GetUserAsync(User);

            if (usuario == null || usuario.UserType != "Vendedor")
            {
                return Forbid();
            }

            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.Id == id && p.VendedorId == usuario.Id);

            if (producto == null)
            {
                return NotFound();
            }

            producto.Disponible = !producto.Disponible;
            producto.FechaActualizacion = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}
