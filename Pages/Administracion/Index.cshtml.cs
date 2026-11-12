using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PESCAPP.Data;
using PESCAPP.Models;

namespace PESCAPP.Pages.Administracion
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<ApplicationUser> Usuarios { get; set; } = new();

        public List<PESCAPP.Models.Producto> Productos { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var usuario = await _userManager.GetUserAsync(User);

            if (usuario == null || usuario.UserType != "Administrador")
            {
                return Forbid();
            }

            Usuarios = await _userManager.Users
                .OrderBy(u => u.FullName)
                .ToListAsync();

            Productos = await _context.Productos
                .Include(p => p.Vendedor)
                .OrderByDescending(p => p.FechaPublicacion)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostCambiarDisponibilidadAsync(int id)
        {
            var usuario = await _userManager.GetUserAsync(User);

            if (usuario == null || usuario.UserType != "Administrador")
            {
                return Forbid();
            }

            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
            {
                return NotFound();
            }

            producto.Disponible = !producto.Disponible;
            producto.FechaActualizacion = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostCambiarTipoUsuarioAsync(string id, string nuevoTipo)
        {
            var administrador = await _userManager.GetUserAsync(User);

            if (administrador == null || administrador.UserType != "Administrador")
            {
                return Forbid();
            }

            if (nuevoTipo != "Comprador" && nuevoTipo != "Vendedor")
            {
                return BadRequest();
            }

            var usuario = await _userManager.FindByIdAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            if (usuario.UserType == "Administrador")
            {
                return BadRequest();
            }

            usuario.UserType = nuevoTipo;

            var resultado = await _userManager.UpdateAsync(usuario);

            if (!resultado.Succeeded)
            {
                return BadRequest();
            }

            return RedirectToPage();
        }
    }
}
