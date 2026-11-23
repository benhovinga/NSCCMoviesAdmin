using Dapper;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.DTOs;
using System.Data;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IDbConnection _db;

        // Constructor
        public MoviesController(IDbConnection db)
        {
            _db = db;
        }

        // GET api/movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetAllMovies()
        {
            // Create 2 movies and return them as a list
            var movies = await _db.QueryAsync<Movie>("SELECT * FROM Movie");

            return Ok(movies);  // 200 OK
        }

        // GET api/movies/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovieById(int id)
        {
            var sql = @"SELECT m.*, g.*, r.*, u.*
                FROM Movie m
                INNER JOIN Genre g ON m.GenreId = g.GenreId
                LEFT OUTER JOIN CriticReview r ON m.MovieId = r.MovieId
                INNER JOIN UserSnapshot u ON u.UserId = r.CreatedById
                WHERE m.MovieId = @Id";

            Movie? movie = null;

            await _db.QueryAsync<Movie, Genre, CriticReview, User, Movie>(
                sql,
                (m, g, r, u) =>
                {
                    if (movie == null)
                    {
                        movie = m;
                        movie.Genre = g;
                        movie.CriticReviews = new List<CriticReview>();
                    }

                    if (r != null)
                    {
                        r.User = u;
                        movie.CriticReviews.Add(r);
                    }

                    return movie;
                },
                new { Id = id },
                splitOn: "GenreId,CriticReviewId,UserId"
                );

            return Ok(movie);
        }
    }
}
