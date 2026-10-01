using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [StringLength(150)]
        [Required]
        public required string Title { get; set; }

        [Required]
        public string Synopsis { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        public string Rating { get; set; }

        [Display(Name = "Runtime (minutes)")]
        [Range(0, double.PositiveInfinity)]
        [Required]
        public int Runtime { get; set; }

        [Display(Name = "Release Date")]
        [Required]
        public DateOnly ReleaseDate { get; set; }

        [Required]
        public string Director { get; set; }

        [Required]
        public string Producer { get; set; }

        [Required]
        public string Distributor { get; set; }

        [Display(Name = "Original Language")]
        [Required]
        public string OriginalLanguage { get; set; }
    }
}
