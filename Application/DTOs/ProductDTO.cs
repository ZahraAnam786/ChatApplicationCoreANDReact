using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    //record introduce in C#9 it simply the immutable and value based object
    public record ProductDTO(int Id, string Name, decimal Price);
}
