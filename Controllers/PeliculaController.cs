using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/peliculas")]
public class PeliculaController : ControllerBase
{
    // Inyección de dependencia del servicio de películas
    private readonly IPeliculaService _peliculaService;

    // Constructor que recibe el servicio de películas y lo asigna a una propiedad privada
    public PeliculaController(IPeliculaService peliculaService)
    {
        _peliculaService = peliculaService;
    }

    // Endpoint para obtener todas las películas
    [HttpGet]
    [Authorize] // Requiere que el usuario esté autenticado
    public ActionResult<List<Pelicula>> GetAllPeliculas()
    {
        return Ok(_peliculaService.GetAll()); // Devuelve la lista de películas en un estado HTTP 200 OK
    }

    // Endpoint para obtener una película por su ID
    [HttpGet("{id}")]
    [Authorize] // Requiere autenticación
    public ActionResult<Pelicula> GetById(int id)
    {
        Pelicula? p = _peliculaService.GetById(id); // Busca la película por su ID
        if (p == null)
        {
            return NotFound("Pelicula no encontrada"); // Devuelve un error 404 si no se encuentra
        }
        return Ok(p); // Devuelve la película encontrada
    }

    // Endpoint para obtener todas las críticas de una película específica
    [HttpGet("{id}/criticas")]
    [Authorize] // Requiere autenticación
    public ActionResult<List<Critica>> GetCriticas(int id)
    {
        var p = _peliculaService.GetCriticas(id); // Obtiene las críticas de la película
        return Ok(p); // Devuelve las críticas en un estado HTTP 200 OK
    }

    // Endpoint para crear una nueva película (solo accesible por administradores)
    [HttpPost]
    [Authorize(Roles = "admin")] // Solo usuarios con rol "admin" pueden acceder
    public ActionResult<Pelicula> NuevaPelicula(PeliculaDTO p)
    {
        Pelicula _p = _peliculaService.Create(p); // Crea la nueva película usando los datos de PeliculaDTO
        return CreatedAtAction(nameof(GetById), new { id = _p.Id }, _p); // Devuelve la película creada con estado 201 Created
    }

    // Endpoint para eliminar una película por su ID (solo accesible por administradores)
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")] // Solo usuarios con rol "admin" pueden acceder
    public ActionResult Delete(int id)
    {
        var p = _peliculaService.GetById(id); // Verifica si la película existe

        if (p == null)
        {
            return NotFound("Pelicula no encontrada!!!"); // Retorna 404 si no se encuentra
        }

        _peliculaService.Delete(id); // Elimina la película
        return NoContent(); // Devuelve 204 No Content, indicando que se eliminó correctamente
    }

    // Endpoint para actualizar una película existente (solo accesible por administradores)
    [HttpPut("{id}")]
    [Authorize(Roles = "admin")] // Solo usuarios con rol "admin" pueden acceder
    public ActionResult<Pelicula> UpdatePelicula(int id, Pelicula updatedPelicula)
    {
        // Verifica que el ID en la URL coincida con el ID en el cuerpo de la solicitud
        if (id != updatedPelicula.Id)
        {
            return BadRequest("El ID de la pelicula en la URL no coincide con el ID de la pelicula en el cuerpo de la solicitud."); // Devuelve 400 Bad Request si no coinciden
        }
        
        var pelicula = _peliculaService.Update(id, updatedPelicula); // Actualiza la película

        if (pelicula is null)
        {
            return NotFound(); // Retorna 404 si la película a actualizar no se encuentra
        }
        
        return CreatedAtAction(nameof(GetById), new { id = pelicula.Id }, pelicula); // Devuelve la película actualizada con estado 201 Created
    }
}
