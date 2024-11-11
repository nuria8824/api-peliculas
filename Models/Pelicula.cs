using System.Text.Json.Serialization;
//Este modelo representa la estructura de la entidad Pelicula y 
//proporciona las propiedades y métodos necesarios para manejar 
//los datos relacionados con películas en la aplicación.
public class Pelicula
{
    // Propiedad que representa el identificador único de la película
    public int Id { get; set; }

    // Propiedad que almacena el título de la película, puede ser nula
    public string? Titulo { get; set; }

    // Propiedad que almacena una breve descripción de la película, puede ser nula
    public string? Descripcion { get; set; }

    // Propiedad que almacena la fecha de lanzamiento de la película como cadena, puede ser nula
    public string? FechaLanzamiento { get; set; }

    // Propiedad que representa la lista de críticas asociadas a la película
    // Se ignora en la serialización JSON para evitar ciclos o excesivos datos anidados en la respuesta
    [JsonIgnore]
    public virtual List<Critica> Criticas { get; set; }

    // Constructor sin parámetros necesario para inicializar el modelo sin datos
    public Pelicula()
    {

    }

    // Constructor con parámetros para inicializar una instancia de Pelicula con valores
    public Pelicula(int id, string titulo, string descripcion, string fechaLanzamiento)
    {
        Id = id;
        Titulo = titulo;
        Descripcion = descripcion;
        FechaLanzamiento = fechaLanzamiento;
    }

    // Sobreescribe el método ToString para devolver una representación en texto de la instancia
    public override string ToString()
    {
        return $"Id:{Id}, {Titulo}, {Descripcion}, {FechaLanzamiento}";
    }
}
