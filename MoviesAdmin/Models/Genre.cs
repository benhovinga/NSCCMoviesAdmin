using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [Display(Name = "Genre")]
        [StringLength(100)]
        [Required]
        public string Title { get; set; }
    }
}
