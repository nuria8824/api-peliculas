using System.ComponentModel.DataAnnotations;

public class CriticaDTO
{
    // Descripción o contenido de la crítica. Es un campo obligatorio.
    [Required(ErrorMessage = "El campo Descripcion es requerido")]
    public string? Descripcion { get; set; }

    // Puntaje o calificación de la película. Debe estar entre 1 y 5.
    [Range(1, 5, ErrorMessage = "El puntaje debe estar entre 1 y 5.")]
    public int Puntaje { get; set; }

    // ID de la película a la que pertenece la crítica
    public int PeliculaId { get; set; }
}
