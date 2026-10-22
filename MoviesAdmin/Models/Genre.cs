using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class Genre
    {
        public int GenreId { get; set; }

        [Display(Name = "Genre")]
        [StringLength(50)]
        [Required]
        public string Title { get; set; } = string.Empty;

        [ValidateNever]
        public ICollection<Movie> Movies { get; } = new List<Movie>();
    }
}
