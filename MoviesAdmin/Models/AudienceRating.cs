using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class AudienceRating
    {
        public int Id { get; set; }

        [Display(Name = "Rating")]
        [StringLength(10)]
        [Required]
        public string Title { get; set; }
    }
}
