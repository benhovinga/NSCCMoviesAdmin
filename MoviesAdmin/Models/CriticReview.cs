using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesAdmin.Models
{
    public class CriticReview
    {
        public int CriticReviewId { get; set; }

        [ForeignKey(nameof(Movie))]
        [Display(Name = "Movie")]
        public int MovieId { get; set; }

        [ValidateNever]
        public Movie Movie { get; set; } = null!; // Navigation property

        [Required]
        public string Review { get; set; } = string.Empty;

        [Display(Name = "Star Rating")]
        [Range(1, 5)]
        [Required]
        public int Rating { get; set; }

        [Display(Name = "Published")]
        [Required]
        public bool IsPublished { get; set; } = false;

        [Display(Name = "Created By")]
        [Required]
        public string CreatedById { get; set; } = string.Empty;

        [Display(Name = "Created On")]
        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
