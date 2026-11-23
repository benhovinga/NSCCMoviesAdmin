namespace MoviesAPI.DTOs
{
    public class CriticReview
    {
        public int CriticReviewId { get; set; }
        public int MovieId { get; set; }
        public string Review { get; set; } = string.Empty;
        public int Rating { get; set; }
        public bool IsPublished { get; set; }
        public string CreatedById { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }

        public User User { get; set; } = null!;
    }
}
