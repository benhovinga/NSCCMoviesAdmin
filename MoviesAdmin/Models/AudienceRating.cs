using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    [Index(nameof(Title), IsUnique = true)]
    public class AudienceRating
    {
        public int AudienceRatingId { get; set; }

        [Display(Name = "Rating")]
        [StringLength(20)]
        [Required]
        public string Title { get; set; } = string.Empty;

        [ValidateNever]
        public ICollection<Movie> Movies { get; } = new List<Movie>(); // Navigation Property
    }
}
