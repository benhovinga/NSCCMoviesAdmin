using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesAdmin.Models
{
    public class Movie
    {
        public int MovieId { get; set; }

        [StringLength(150)]
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Synopsis { get; set; } = string.Empty;

        //[Required]
        //public string Genre { get; set; }

        [ForeignKey("Genre")]
        public int GenreId { get; set; }

        [ValidateNever]
        public Genre Genre { get; set; } = null!; // Navigation property

        [Required]
        public string Rating { get; set; } = string.Empty;

        [Display(Name = "Runtime (minutes)")]
        [Range(0, double.PositiveInfinity)]
        [Required]
        public int Runtime { get; set; }

        [Display(Name = "Release Date")]
        [Required]
        public DateOnly ReleaseDate { get; set; }

        [Required]
        public string Director { get; set; } = string.Empty;

        [Required]
        public string Producer { get; set; } = string.Empty;

        [Required]
        public string Distributor { get; set; } = string.Empty;

        [Display(Name = "Original Language")]
        [Required]
        public string OriginalLanguage { get; set; } = string.Empty;

        [Display(Name = "Poster Image")]
        [Required]
        public string PosterUrl { get; set; } = string.Empty;
    }
}
