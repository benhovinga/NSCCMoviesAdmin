namespace MoviesAdmin.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Synopsis { get; set; }
        public string? Genre { get; set; }
        public string? Rating { get; set; }
        public int RuntimeMinutes { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public string? Director { get; set; }
        public string? Producer { get; set; }
        public string? Distributor  { get; set; }
        public string? OriginalLanguage { get; set; }
    }
}
