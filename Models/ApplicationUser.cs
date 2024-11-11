using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    //[JsonIgnore]
    //public virtual List<Critica> Criticas { get; set; }
}

//Este modelo define al usuario de la aplicación extendiendo 
//IdentityUser para añadir funcionalidades o propiedades específicas en el futuro. 
//ApplicationUser puede ser utilizado en conjunto con ASP.NET Identity para manejar la autenticación, 
//autorización y la gestión de usuarios en la aplicación. La propiedad Criticas permitiría, si se descomenta, 
//acceder a todas las críticas creadas por el usuario.