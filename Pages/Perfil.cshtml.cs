using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PESCAPP.Models;

namespace PESCAPP.Pages
{
    [Authorize]
    public class PerfilModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public PerfilModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public DatosPerfil Input { get; set; } = new();

        public string Correo { get; set; } = string.Empty;
        public string TipoUsuario { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            var usuario = await _userManager.GetUserAsync(User);

            if (usuario == null)
                return Challenge();

            Input.FullName = usuario.FullName;
            Input.PhoneNumber = usuario.PhoneNumber ?? "";
            Correo = usuario.Email ?? "";
            TipoUsuario = usuario.UserType;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var usuario = await _userManager.GetUserAsync(User);

            if (usuario == null)
                return Challenge();

            Correo = usuario.Email ?? "";
            TipoUsuario = usuario.UserType;

            if (!ModelState.IsValid)
                return Page();

            usuario.FullName = Input.FullName;
            usuario.PhoneNumber = Input.PhoneNumber;

            var resultado = await _userManager.UpdateAsync(usuario);

            if (!resultado.Succeeded)
            {
                foreach (var error in resultado.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                return Page();
            }

            TempData["MensajePerfil"] = "Tu perfil se actualizó correctamente.";

            return RedirectToPage();
        }

        public class DatosPerfil
        {
            [Required(ErrorMessage = "El nombre es obligatorio.")]
            [StringLength(100)]
            public string FullName { get; set; } = string.Empty;

            [Phone(ErrorMessage = "Ingresa un teléfono válido.")]
            public string? PhoneNumber { get; set; }
        }
    }
}
