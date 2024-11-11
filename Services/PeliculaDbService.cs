using Microsoft.EntityFrameworkCore;

// Servicio para manejar operaciones CRUD de películas en la base de datos
public class PeliculaDbService : IPeliculaService
{
    // Contexto de base de datos para acceder a la base de datos de películas
    private readonly PeliculasContext _context;

    // Constructor que recibe el contexto de la base de datos e inicializa el servicio
    public PeliculaDbService(PeliculasContext context)
    {
        _context = context;
    }

    // Método para crear una nueva película en la base de datos
    public Pelicula Create(PeliculaDTO p)
    {
        // Crear una nueva instancia de Pelicula usando los datos del objeto PeliculaDTO
        Pelicula pelicula = new()
        {
            Titulo = p.Titulo,
            Descripcion = p.Descripcion,
            FechaLanzamiento = p.FechaLanzamiento
        };

        // Agregar la nueva película al contexto de la base de datos
        _context.Peliculas.Add(pelicula);
        _context.SaveChanges(); // Guardar los cambios en la base de datos

        return pelicula; // Retornar la película creada
    }

    // Método para eliminar una película de la base de datos por su ID
    public void Delete(int id)
    {
        // Buscar la película en la base de datos usando su ID
        var p = _context.Peliculas.Find(id);

        // Eliminar la película del contexto de la base de datos
        _context.Peliculas.Remove(p);
        _context.SaveChanges(); // Guardar los cambios en la base de datos
    }

    // Método para obtener todas las películas almacenadas en la base de datos
    public IEnumerable<Pelicula> GetAll()
    {
        // Retorna la lista completa de películas
        return _context.Peliculas;
    }

    // Método para obtener una película específica por su ID
    public Pelicula? GetById(int id)
    {
        // Busca la película en la base de datos usando el ID
        return _context.Peliculas.Find(id);
    }

    // Método para obtener las críticas asociadas a una película específica
    public IEnumerable<Critica> GetCriticas(int id)
    {
        // Busca la película por ID e incluye las críticas relacionadas
        Pelicula p = _context.Peliculas.Include(p => p.Criticas).FirstOrDefault(x => x.Id == id);

        // Retorna la lista de críticas asociadas a la película
        return p.Criticas;
    }

    // Método para actualizar una película existente
    public Pelicula? Update(int id, Pelicula p)
    {
        // Marca la entidad película como modificada en el contexto
        _context.Entry(p).State = EntityState.Modified;
        _context.SaveChanges(); // Guardar los cambios en la base de datos

        return p; // Retorna la película actualizada
    }
}
