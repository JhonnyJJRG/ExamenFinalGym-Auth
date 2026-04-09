using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ExamenFinalGym.Models
{
    public class Usuario
    {
        // Llave primaria por convención (UserId)
        public int UserId { get; set; }
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
        public string UserName { get; set; } = "";

        [StringLength(100)]
        public string NormalizedUserName { get; set; } = "";

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        [StringLength(256, ErrorMessage = "El correo no puede tener más de 256 caracteres.")]
        public string Email { get; set; } = "";

        [StringLength(256)]
        public string NormalizedEmail { get; set; } = "";

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(512, ErrorMessage = "La contraseña excede la longitud permitida.")]
        public string PasswordHash { get; set; } = ""; 

        [Phone(ErrorMessage = "El formato del número de teléfono no es válido.")]
        [StringLength(25, ErrorMessage = "El número de teléfono no puede tener más de 25 caracteres.")]
        public string? PhoneNumber { get; set; } 
        public bool IsActive { get; set; } = true;

        public DateTime? LastLoginAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}