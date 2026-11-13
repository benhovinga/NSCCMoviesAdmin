namespace MoviesAPI.DTOs
{
    public class Movie
    {
        public int MovieId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Synopsis { get; set; } = string.Empty;
        public int GenreId { get; set; }
        public int AudienceRatingId { get; set; }
        public int Runtime { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public string Director { get; set; } = string.Empty;
        public string Producer { get; set; } = string.Empty;
        public string Distributor { get; set; } = string.Empty;
        public string OriginalLanguage { get; set; } = string.Empty;
        public string PosterUrl { get; set; } = string.Empty;
    }
}
