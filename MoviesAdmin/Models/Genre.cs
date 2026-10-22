using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class Genre
    {
        public int GenreId { get; set; }

        [Display(Name = "Genre")]
        [StringLength(100)]
        [Required]
        public string Title { get; set; }
    }
}
