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
    }
}
