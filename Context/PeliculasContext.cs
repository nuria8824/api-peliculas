using Microsoft.EntityFrameworkCore;

// Clase que representa el contexto de la base de datos para la aplicación de películas
public class PeliculasContext : DbContext
{
    // Tabla de películas
    public DbSet<Pelicula> Peliculas { get; set; }

    // Tabla de críticas
    public DbSet<Critica> Criticas { get; set; }

    // Constructor que recibe las opciones del contexto y las pasa a la clase base
    public PeliculasContext(DbContextOptions<PeliculasContext> options) : base(options)
    {
    }

    // Método para configurar las propiedades de las entidades al crear el modelo de la base de datos
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuración de la entidad Pelicula
        modelBuilder.Entity<Pelicula>(entity =>
        {
            // Titulo es requerido y tiene un máximo de 100 caracteres
            entity.Property(p => p.Titulo).IsRequired().HasMaxLength(100);

            // Descripcion es requerida y tiene un máximo de 255 caracteres
            entity.Property(p => p.Descripcion).IsRequired().HasMaxLength(255);

            // FechaLanzamiento es requerida
            entity.Property(p => p.FechaLanzamiento).IsRequired();
        });

        // Configuración de la entidad Critica
        modelBuilder.Entity<Critica>(entity =>
        {
            // Descripcion es requerida
            entity.Property(c => c.Descripcion).IsRequired();

            // Puntaje es requerido
            entity.Property(c => c.Puntaje).IsRequired();

            // Configura la relación Critica -> Pelicula: una crítica está asociada a una película
            entity.HasOne(c => c.Pelicula)
                .WithMany(p => p.Criticas)
                .HasForeignKey(c => c.PeliculaId)
                .IsRequired();

            // Configura la relación Critica -> Usuario: una crítica está asociada a un usuario
            entity.HasOne(c => c.Usuario)
                .WithMany()
                .HasForeignKey(c => c.UsuarioId)
                .IsRequired();

            // Crea un índice único en la combinación UsuarioId y PeliculaId para evitar críticas duplicadas de un usuario a la misma película
            entity.HasIndex(c => new { c.UsuarioId, c.PeliculaId }).IsUnique();
        });
    }
}
