
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviesAdmin.Models;
using MoviesAdmin.Services;

public class MoviesController : Controller
{
    private readonly MoviesAdminContext _context;
    private readonly IAzureStorageService _storageService;

    public MoviesController(MoviesAdminContext context, IAzureStorageService azureStorageService)
    {
        _context = context;
        _storageService = azureStorageService;
    }

    // GET: MOVIES
    public async Task<IActionResult> Index()
    {
        var trails = await _context.Movie
            .OrderByDescending(t => t.ReleaseDate)
            .Include(m => m.Genre)
            .ToListAsync();

        return View(trails);
    }

    // GET: MOVIES/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movie
            .Include(m => m.Genre)
            .FirstOrDefaultAsync(m => m.MovieId == id);
        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // GET: MOVIES/Create
    public IActionResult Create()
    {
        // Add a dropdown field for all Genres
        ViewBag.GenreId = new SelectList(
            _context.Genre.ToList(),
            nameof(Genre.GenreId),
            nameof(Genre.Title)
        );
        return View();
    }

    // POST: MOVIES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MovieId,Title,Synopsis,GenreId,Rating,Runtime,ReleaseDate,Director,Producer,Distributor,OriginalLanguage,PosterUrl")] Movie movie, IFormFile PosterFile)
    {
        Console.WriteLine($"[DEBUG] POST PosterUrl: \"{movie.PosterUrl}\"");

        if (PosterFile != null && PosterFile.Length > 0)
        {
            // Remove the validation error generated before PosterUrl was assigned.
            ModelState.Remove(nameof(Movie.PosterUrl));

            try
            {
                // Upload poster to storage service with unique id
                Guid guid = Guid.NewGuid();
                movie.PosterUrl = await _storageService.UploadFileAsync(PosterFile, guid.ToString());
                Console.WriteLine($"[DEBUG] SET PosterUrl: \"{movie.PosterUrl}\"");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(Movie.PosterUrl), $"Error: Movie poster upload failed. {ex.Message}");
            }
        }

        if (ModelState.IsValid)
        {
            _context.Add(movie);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Rebuild the dropdown if validation fails
        ViewBag.GenreId = new SelectList(
            _context.Genre.ToList(),
            nameof(Genre.GenreId),
            nameof(Genre.Title),
            movie.GenreId
        );

        Console.WriteLine($"[DEBUG] VIEW PosterUrl: \"{movie.PosterUrl}\"");

        return View(movie);
    }

    // GET: MOVIES/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movie.FindAsync(id);
        if (movie == null)
        {
            return NotFound();
        }

        // Add a dropdown field for all Genres
        ViewBag.GenreId = new SelectList(
            _context.Genre.ToList(),
            nameof(Genre.GenreId),
            nameof(Genre.Title),
            movie.GenreId
        );

        return View(movie);
    }

    // POST: MOVIES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("MovieId,Title,Synopsis,GenreId,Rating,Runtime,ReleaseDate,Director,Producer,Distributor,OriginalLanguage,PosterUrl")] Movie movie, IFormFile? PosterFile)
    {
        if (id != movie.MovieId)
        {
            return NotFound();
        }

        // Upload new poster if one is set
        if (PosterFile != null && PosterFile.Length > 0)
        {
            // Remove the validation error generated before PosterUrl was assigned.
            ModelState.Remove(nameof(Movie.PosterUrl));

            try
            {
                // Upload poster to storage service with unique id
                Guid guid = Guid.NewGuid();
                movie.PosterUrl = await _storageService.UploadFileAsync(PosterFile, guid.ToString());
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(Movie.PosterUrl), $"Error: Movie poster upload failed. {ex.Message}");
            }
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(movie);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(movie.MovieId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // Rebuild the dropdown if validation fails
        ViewBag.GenreId = new SelectList(
            _context.Genre.ToList(),
            nameof(Genre.GenreId),
            nameof(Genre.Title),
            movie.GenreId
        );

        return View(movie);
    }

    // GET: MOVIES/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movie
            .FirstOrDefaultAsync(m => m.MovieId == id);
        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // POST: MOVIES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var movie = await _context.Movie.FindAsync(id);
        if (movie != null)
        {
            _context.Movie.Remove(movie);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MovieExists(int? id)
    {
        return _context.Movie.Any(e => e.MovieId == id);
    }
}
