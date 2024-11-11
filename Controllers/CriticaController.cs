using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize]
[Route("api/criticas")]
public class CriticaController : ControllerBase
{
    // Servicios y dependencias necesarios
    private readonly ICriticaService _criticaService;
    private readonly IPeliculaService _peliculaService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    // Constructor que inyecta los servicios y dependencias necesarios
    public CriticaController(ICriticaService criticaService, IPeliculaService peliculaService, UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _criticaService = criticaService;
        _peliculaService = peliculaService;
        _userManager = userManager;
        _configuration = configuration;
    }

    // Endpoint para obtener todas las críticas
    [HttpGet]
    public ActionResult<List<Critica>> GetAll()
    {
        try
        {
            return Ok(_criticaService.GetAll()); // Retorna todas las críticas
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e.Message);
            return Problem(detail: e.Message, statusCode: 500); // Devuelve un error 500 si ocurre una excepción
        }
    }

    // Endpoint para obtener una crítica por su ID
    [HttpGet("{id}")]
    public ActionResult<Critica> GetById(int id)
    {
        Critica? critica = _criticaService.GetById(id); // Busca la crítica por ID
        if (critica is null) return NotFound(); // Retorna 404 si no se encuentra
        return Ok(critica); // Retorna la crítica encontrada
    }

    // Endpoint para crear una nueva crítica
    [HttpPost]
    public async Task<IActionResult> NuevaCritica(CriticaDTO c)
    {
        // Obtener el usuario autenticado
        var userName = User.FindFirstValue(ClaimTypes.Name);
        var user = await _userManager.FindByNameAsync(userName);
        
        // Crear la crítica usando el ID del usuario autenticado
        Critica critica = _criticaService.Create(c, user.Id);
        return CreatedAtAction(nameof(GetById), new { id = critica.Id }, critica); // Retorna la crítica creada
    }
    
    // Endpoint para eliminar una crítica por su ID
    [HttpDelete]
    public async Task<ActionResult> Delete(int id)
    {
        // Obtener el usuario autenticado
        var userName = User.FindFirstValue(ClaimTypes.Name);
        var user = await _userManager.FindByNameAsync(userName);

        try
        {
            // Elimina la crítica si el usuario tiene permiso
            var deleted = _criticaService.Delete(id, user.Id);

            if (deleted)
            {
                return NoContent(); // Eliminación exitosa
            }

            return Forbid("No tienes permiso para eliminar esta crítica."); // El usuario no tiene permiso
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message); // Manejar errores generales
        }
    }
    
    // Endpoint para actualizar una crítica
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, CriticaDTO c)
    {
        // Obtener el usuario autenticado
        var userName = User.FindFirstValue(ClaimTypes.Name);
        var user = await _userManager.FindByNameAsync(userName);

        try
        {
            // Actualiza la crítica si el usuario tiene permiso
            Critica critica = _criticaService.Update(id, c, user.Id);
            if (critica is null) return NotFound(new { Message = $"No se pudo actualizar la critica con id: {id}" });
            return CreatedAtAction(nameof(GetById), new { id = critica.Id }, critica); // Retorna la crítica actualizada
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid("No tienes permiso para modificar esta crítica."); // El usuario no tiene permiso
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e.Message);
            return Problem(detail: e.Message, statusCode: 500); // Devuelve un error 500 si ocurre una excepción
        }
    }

    // Endpoint para obtener críticas de un usuario específico por su `UsuarioId`
    [HttpGet("byuser/{UsuarioId}")]
    public ActionResult<IEnumerable<Critica>> GetByUser(string UsuarioId)
    {
        // Obtiene las críticas del usuario
        var criticas = _criticaService.GetByUser(UsuarioId);

        if (criticas == null || !criticas.Any())
        {
            return NotFound($"No se encontraron críticas para el usuario con Id: {UsuarioId}"); // Retorna 404 si no se encuentran críticas
        }

        return Ok(criticas); // Retorna la lista de críticas
    }
}
