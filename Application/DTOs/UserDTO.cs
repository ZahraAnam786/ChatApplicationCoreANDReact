using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs 
{
    public record UserDTO(long UserID, string Image, string UserName, string Email);
    public record RegisterDTO(string UserName, string Image, string Email, DateTime CreatedDate, string Password);
    public record LoginDTO(string Email, string Password);
}

