public interface IPeliculaService
{
    // Método para obtener una lista de todas las películas
    public IEnumerable<Pelicula> GetAll();

    // Método para obtener una película específica por su ID
    public Pelicula? GetById(int id);

    // Método para crear una nueva película usando un objeto de transferencia de datos (DTO)
    public Pelicula Create(PeliculaDTO p);

    // Método para eliminar una película de la base de datos usando su ID
    public void Delete(int id);

    // Método para actualizar una película existente usando su ID y los nuevos datos de la película
    public Pelicula? Update(int id, Pelicula p);

    // Método para obtener todas las críticas asociadas a una película específica por su ID
    public IEnumerable<Critica> GetCriticas(int id);
}
