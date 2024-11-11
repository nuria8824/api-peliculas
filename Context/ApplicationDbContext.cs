using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

// Clase que representa el contexto de base de datos para la autenticación y autorización
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    // Constructor que recibe las opciones del contexto y las pasa a la clase base
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}
