using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public long UserID { get; set; }
        public string Image { get; set; }
        public string UserName { get; set; } = string.Empty; 
        public string Email { get; set; } = string.Empty; 
        public DateTime CreatedDate { get; set; }
        public string? Password { get; set; }
        public string SignalID { get; set; }
    }


}
