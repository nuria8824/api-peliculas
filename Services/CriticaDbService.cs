using Microsoft.EntityFrameworkCore;

// Servicio para manejar operaciones CRUD de críticas
public class CriticaDbService : ICriticaService
{
    // Contexto de la base de datos
    private readonly PeliculasContext _context;

    // Constructor que inicializa el contexto de base de datos
    public CriticaDbService(PeliculasContext context)
    {
        _context = context;
    }

    // Método para crear una nueva crítica
    public Critica Create(CriticaDTO c, string UsuarioId)
    {
        // Crear una nueva instancia de crítica con la información proporcionada
        var nuevaCritica = new Critica
        {
            Descripcion = c.Descripcion,
            Puntaje = c.Puntaje,
            PeliculaId = c.PeliculaId,
            UsuarioId = UsuarioId
        };

        // Agregar la nueva crítica al contexto de la base de datos
        _context.Criticas.Add(nuevaCritica);
        _context.SaveChanges(); // Guardar cambios en la base de datos

        return nuevaCritica; // Retornar la crítica creada
    }

    // Método para eliminar una crítica
    public bool Delete(int id, string UsuarioId)
    {
        // Buscar la crítica por su ID
        var critica = _context.Criticas.FirstOrDefault(c => c.Id == id);

        // Verificar si la crítica existe
        if (critica == null)
        {
            throw new Exception("Crítica no encontrada.");
        }

        // Verificar si la crítica pertenece al usuario autenticado
        if (critica.UsuarioId != UsuarioId)
        {
            return false; // El usuario no tiene permiso para eliminar la crítica
        }

        // Eliminar la crítica
        _context.Criticas.Remove(critica);
        _context.SaveChanges(); // Guardar los cambios en la base de datos

        return true; // Retornar verdadero si la eliminación fue exitosa
    }

    // Método para obtener todas las críticas
    public IEnumerable<Critica> GetAll()
    {
        // Retornar todas las críticas e incluir la información de la película relacionada
        return _context.Criticas.Include(la => la.Pelicula);
    }

    // Método para obtener una crítica por su ID
    public Critica? GetById(int id)
    {
        // Buscar la crítica en la base de datos usando el ID
        return _context.Criticas.Find(id);
    }

    // Método para actualizar una crítica
    public Critica Update(int id, CriticaDTO c, string UsuarioId)
    {
        // Buscar la crítica que se desea actualizar
        var criticaUpdate = _context.Criticas.FirstOrDefault(c => c.Id == id);

        // Verificar si la crítica existe
        if (criticaUpdate == null)
        {
            throw new Exception("La crítica no existe.");
        }

        // Verificar si el usuario autenticado es el dueño de la crítica
        if (criticaUpdate.UsuarioId != UsuarioId)
        {
            throw new UnauthorizedAccessException("No tienes permiso para modificar esta crítica.");
        }

        // Actualizar los campos de la crítica con la nueva información
        criticaUpdate.Descripcion = c.Descripcion;
        criticaUpdate.Puntaje = c.Puntaje;
        criticaUpdate.PeliculaId = c.PeliculaId;

        // Marcar la entidad como modificada y guardar los cambios
        _context.Entry(criticaUpdate).State = EntityState.Modified;
        _context.SaveChanges();

        return criticaUpdate; // Retornar la crítica actualizada
    }

    // Método para obtener todas las críticas de un usuario específico
    public IEnumerable<Critica> GetByUser(string UsuarioId)
    {
        // Buscar críticas que pertenezcan al usuario autenticado
        return _context.Criticas.Where(c => c.UsuarioId == UsuarioId).ToList();
    }
}
