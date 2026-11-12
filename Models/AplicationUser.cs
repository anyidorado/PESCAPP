using Microsoft.AspNetCore.Identity;

namespace PESCAPP.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;

        public string UserType { get; set; } = "Comprador";
    }
}
