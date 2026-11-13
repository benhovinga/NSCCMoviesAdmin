using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class UserSnapshot
    {
        [Key]
        [StringLength(50)]
        public string UserId { get; set; } = string.Empty;

        [StringLength(150)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;
    }
}
