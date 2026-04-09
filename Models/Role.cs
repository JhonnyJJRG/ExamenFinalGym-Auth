using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace ExamenFinalGym.Models
{
    public class Role
    {
        public int RoleId { get; set; }

        [MaxLength(50)] // Según NVARCHAR(50) del script 
        public string Name { get; set; } = "";

        [MaxLength(50)]
        public string NormalizedName { get; set; } = "";

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

      // Propiedad de navegación (Relación N:N con Usuarios) 
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}