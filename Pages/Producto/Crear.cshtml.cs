using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PESCAPP.Data;
using PESCAPP.Models;
using Microsoft.AspNetCore.Identity;

namespace PESCAPP.Pages.Producto
{
    public class CrearModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CrearModel(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public PESCAPP.Models.Producto Producto { get; set; } = new PESCAPP.Models.Producto();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var usuario = await _userManager.GetUserAsync(User);

            if (usuario == null || usuario.UserType != "Vendedor")
            {
                return Forbid();
            }

            Producto.VendedorId = usuario.Id;
            Producto.FechaPublicacion = DateTime.Now;
            Producto.FechaActualizacion = DateTime.Now;
            Producto.Disponible = true;

            _context.Productos.Add(Producto);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}
