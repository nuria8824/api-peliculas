using System.ComponentModel.DataAnnotations;

public class PeliculaDTO
{
    // Título de la película. Es un campo obligatorio.
    [Required(ErrorMessage = "El campo Titulo es requerido")]
    public string? Titulo { get; set; }

    // Descripción de la película. Es un campo obligatorio.
    [Required(ErrorMessage = "El campo Descripcion es requerido")]
    public string? Descripcion { get; set; }

    // Fecha de lanzamiento de la película. Es un campo obligatorio.
    [Required(ErrorMessage = "El campo FechaLanzamiento es requerido")]
    public string? FechaLanzamiento { get; set; }
}
