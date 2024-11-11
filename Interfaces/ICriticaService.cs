public interface ICriticaService
{
    // Método para obtener una lista de todas las críticas
    public IEnumerable<Critica> GetAll();

    // Método para obtener una crítica específica por su ID
    public Critica? GetById(int id);

    // Método para crear una nueva crítica, que recibe los datos de la crítica y el ID del usuario
    public Critica Create(CriticaDTO c, string UsuarioId);

    // Método para eliminar una crítica por su ID, solo si el usuario autenticado es el propietario
    public bool Delete(int id, string UsuarioId);

    // Método para actualizar una crítica, que recibe los nuevos datos y el ID del usuario
    public Critica Update(int id, CriticaDTO c, string UsuarioId);

    // Método para obtener todas las críticas realizadas por un usuario específico
    public IEnumerable<Critica> GetByUser(string UsuarioId);
}
