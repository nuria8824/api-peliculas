//Este modelo define la estructura de la entidad Critica, que representa una reseña 
//o evaluación hecha por un usuario sobre una película.
public class Critica
{
    // Propiedad que representa el identificador único de la crítica
    public int Id { get; set; }

    // Propiedad que almacena la descripción o el texto de la crítica, puede ser nula
    public string? Descripcion { get; set; }

    // Propiedad que almacena el puntaje asignado a la película en la crítica
    public int Puntaje { get; set; }

    // Propiedad de navegación que representa la película a la que pertenece la crítica
    public Pelicula Pelicula { get; set; }

    // Propiedad que almacena el ID de la película asociada, utilizada como clave foránea
    public int PeliculaId { get; set; }

    // Propiedad de navegación que representa el usuario que hizo la crítica
    public ApplicationUser Usuario { get; set; }

    // Propiedad que almacena el ID del usuario que hizo la crítica, puede ser nula
    public string? UsuarioId { get; set; }
}
