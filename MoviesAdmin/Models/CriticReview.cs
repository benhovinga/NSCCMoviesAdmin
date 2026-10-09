using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class CriticReview
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Star Rating")]
        [Range(1, 5)]
        [Required]
        public int Rating { get; set; }

        [Display(Name = "Published")]
        [Required]
        public bool IsPublished { get; set; } = false;

        [Display(Name = "Created By")]
        [Required]
        public string CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
