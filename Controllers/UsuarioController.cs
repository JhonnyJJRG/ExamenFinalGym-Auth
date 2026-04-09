using Microsoft.AspNetCore.Mvc;
using ExamenFinalGym.Data;
using ExamenFinalGym.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExamenFinalGym.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult Get()
        {
            var usuarios = _context.Usuarios.ToList(); // SELECT * FROM usuarios;
            return Ok(usuarios);
        }

        [Authorize(Roles = "ADMIN,ENTRENADOR,SOCIO")]
        [HttpGet("{id}")]
        public IActionResult GetOnly(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }

       
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Usuario usuario)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // Simulamos la fecha de creación en caso de que no venga en el JSON
                usuario.CreatedAt = DateTime.Now;

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
                transaction.Commit();
                
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine("ROLLBACK ejecutado");

                return BadRequest(new
                {
                    mensaje = "Error al registrar usuario",
                    detalle = ex.Message
                });
            }
        }

        // PUT actualizar un usuario
        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Usuario usuario)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var usuarioExistente = _context.Usuarios.Find(id);

                if (usuarioExistente == null)
                {
                    return NotFound();
                }

                // Actualizar los campos
                usuarioExistente.UserName = usuario.UserName;
                usuarioExistente.NormalizedUserName = usuario.NormalizedUserName;
                usuarioExistente.Email = usuario.Email;
                usuarioExistente.NormalizedEmail = usuario.NormalizedEmail;
                usuarioExistente.PhoneNumber = usuario.PhoneNumber;
                usuarioExistente.IsActive = usuario.IsActive;
                usuarioExistente.UpdatedAt = DateTime.Now;

                // Nota: El PasswordHash normalmente no se actualiza por aquí directamente, 
                // pero lo incluimos para seguir el patrón CRUD básico del profesor.
                if (!string.IsNullOrEmpty(usuario.PasswordHash))
                {
                    usuarioExistente.PasswordHash = usuario.PasswordHash;
                }

                _context.SaveChanges();
                transaction.Commit();
                return Ok(usuarioExistente);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return BadRequest(new
                {
                    mensaje = "Error al actualizar usuario",
                    detalle = ex.Message
                });
            }
        }

        // DELETE eliminar (o dar de baja) un usuario
        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var usuarioExistente = _context.Usuarios.Find(id);
                if (usuarioExistente == null)
                {
                    return NotFound();
                }

                _context.Usuarios.Remove(usuarioExistente);
                _context.SaveChanges();
                transaction.Commit();
                return Ok("Usuario eliminado");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return BadRequest(new
                {
                    mensaje = "Error al eliminar usuario",
                    detalle = ex.Message
                });
            }
        }
    }
}