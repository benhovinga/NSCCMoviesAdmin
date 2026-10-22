
using Microsoft.AspNetCore.Mvc;
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
        return View();
    }

    // POST: MOVIES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Synopsis,Genre,Rating,Runtime,ReleaseDate,Director,Producer,Distributor,OriginalLanguage")] Movie movie, IFormFile PosterFile)
    {
        if (PosterFile != null && PosterFile.Length > 0)
        {
            Guid gid = Guid.NewGuid();
            movie.PosterUrl = await _storageService.UploadFileAsync(PosterFile, gid.ToString());

            // Remove the validation error generated before PosterUrl was assigned.
            ModelState.Remove(nameof(Movie.PosterUrl));
        }

        if (ModelState.IsValid)
        {
            _context.Add(movie);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

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
        return View(movie);
    }

    // POST: MOVIES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Title,Synopsis,Genre,Rating,Runtime,ReleaseDate,Director,Producer,Distributor,OriginalLanguage")] Movie movie, IFormFile? PosterFile)
    {
        if (id != movie.MovieId)
        {
            return NotFound();
        }

        if (PosterFile != null && PosterFile.Length > 0)
        {
            Guid gid = Guid.NewGuid();
            movie.PosterUrl = await _storageService.UploadFileAsync(PosterFile, gid.ToString());
        }
        else
        {
            var existingMovie = await _context.Movie
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.MovieId == id);

            if (existingMovie == null)
            {
                return NotFound();
            }

            movie.PosterUrl = existingMovie.PosterUrl;
        }

        if (movie.PosterUrl != null && movie.PosterUrl != string.Empty)
        {
            // Remove the validation error generated before PosterUrl was assigned.
            ModelState.Remove(nameof(Movie.PosterUrl));
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
