using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class AudienceRating
    {
        public int AudienceRatingId { get; set; }

        [Display(Name = "Rating")]
        [StringLength(10)]
        [Required]
        public string Title { get; set; }
    }
}
