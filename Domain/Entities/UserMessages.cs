using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserMessages
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long SenderId { get; set; }

        [Required]
        public long ReceiverId { get; set; }

        public string Content { get; set; }

        [Required]
        public bool IsSender { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }
    }
}

