using Microsoft.EntityFrameworkCore;
using ExamenFinalGym.Models;

namespace ExamenFinalGym.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Role> Roles {get; set;}
        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        // Esto es NECESARIO para que la base de datos funcione correctamente
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().HasKey(u => u.UserId);
            modelBuilder.Entity<Role>().HasKey(r => r.RoleId);
            // Configura la llave primaria compuesta de UserRoles
            modelBuilder.Entity<Usuario>()
            .ToTable("Users") // <--- Esto soluciona el error actual
            .HasKey(u => u.UserId);

            modelBuilder.Entity<UserRole>()
            .ToTable("UserRoles")
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        // 4. ---> REGLAS DE NAVEGACIÓN (Para evitar el error UsuarioUserId) <---
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Usuario)       // Un UserRole tiene un Usuario
            .WithMany(u => u.UserRoles)     // Un Usuario tiene muchos UserRoles
            .HasForeignKey(ur => ur.UserId); // La llave foránea es UserId

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)          // Un UserRole tiene un Role
            .WithMany(r => r.UserRoles)     // Un Role tiene muchos UserRoles
            .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<Role>()
            .ToTable("Roles")
            .HasKey(r => r.RoleId);

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            base.OnModelCreating(modelBuilder);
        }  

    }
    
}