using System.Text.Json.Serialization;

namespace ExamenFinalGym.Models
{
    public class UserRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        
       
        public Usuario? Usuario { get; set; }

        
        public Role? Role { get; set; }
    
        public DateTime AssignedAt { get; set; } = DateTime.Now;
    }
}