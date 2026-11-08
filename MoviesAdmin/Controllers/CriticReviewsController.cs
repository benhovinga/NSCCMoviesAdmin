
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviesAdmin;
using MoviesAdmin.Models;

[Authorize]
public class CriticReviewsController : Controller
{
    private readonly MoviesAdminContext _context;

    private string? _userId;

    public CriticReviewsController(MoviesAdminContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);
        _userId = User.GetUserId();
    }

    // GET: CRITICREVIEWS
    [Authorize(Roles = "Critic")]
    public async Task<IActionResult> Index()    
    {
        // Only show reviews for the current user.
        var reviews = await _context.CriticReview
            .Where(m => m.CreatedById == _userId)
            .Include(m => m.Movie)
            .ToListAsync();

        return View(reviews);
    }

    // GET: CRITICREVIEWS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var criticreview = await _context.CriticReview
            .Include(m => m.Movie)
            .FirstOrDefaultAsync(m => m.CriticReviewId == id);

        if (criticreview == null)
        {
            return NotFound();
        }

        // Review must be either published or the current user owns the review.
        if (!criticreview.IsPublished && criticreview.CreatedById != _userId)
        {
            return Unauthorized();
        }

        return View(criticreview);
    }

    // GET: CRITICREVIEWS/Create
    [Authorize(Roles = "Critic")]
    public IActionResult Create()
    {
        // Add a dropdown field for all Movies
        ViewBag.MovieId = new SelectList(
            _context.Movie.ToList(),
            nameof(Movie.MovieId),
            nameof(Movie.Title)
        );

        return View();
    }

    // POST: CRITICREVIEWS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Critic")]
    public async Task<IActionResult> Create([Bind("MovieId,Review,Rating,IsPublished")] CriticReview criticreview)
    {
        if (_userId == null)
        {
            return BadRequest();
        }

        // Assign the current user to the new review.
        criticreview.CreatedById = _userId;
        ModelState.Remove(nameof(CriticReview.CreatedById));

        if (ModelState.IsValid)
        {
            _context.Add(criticreview);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Add a dropdown field for all Movies
        ViewBag.MovieId = new SelectList(
            _context.Movie.ToList(),
            nameof(Movie.MovieId),
            nameof(Movie.Title),
            criticreview.MovieId
        );

        return View(criticreview);
    }

    // GET: CRITICREVIEWS/Edit/5
    [Authorize(Roles = "Critic")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var criticreview = await _context.CriticReview.FindAsync(id);
        if (criticreview == null)
        {
            return NotFound();
        }

        // The current user must own the review to edit it.
        if (criticreview.CreatedById != _userId)
        {
            return Unauthorized();
        }

        // Cannot edit a published review, redirect to its details page.
        if (criticreview.IsPublished)
        {
            return RedirectToAction(nameof(Details), new { id });
        }

        // Add a dropdown field for all Movies
        ViewBag.MovieId = new SelectList(
            _context.Movie.ToList(),
            nameof(Movie.MovieId),
            nameof(Movie.Title)
        );

        return View(criticreview);
    }

    // POST: CRITICREVIEWS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Critic")]
    public async Task<IActionResult> Edit(int? id, [Bind("CriticReviewId,MovieId,Review,Rating,IsPublished")] CriticReview criticreview)
    {
        if (id != criticreview.CriticReviewId)
        {
            return NotFound();
        }

        var existingReview = await _context.CriticReview
            .FirstOrDefaultAsync(m => m.CriticReviewId == id);

        if (existingReview == null)
        {
            return NotFound();
        }

        // The current user must own the review to edit it.
        if (existingReview.CreatedById != _userId)
        {
            return Unauthorized();
        }

        // Cannot edit a published review, redirect to its details page.
        if (criticreview.IsPublished)
        {
            return RedirectToAction(nameof(Details), new { id });
        }

        ModelState.Remove(nameof(CriticReview.CreatedById));

        if (ModelState.IsValid)
        {
            try
            {
                // Only update specific fields
                existingReview.MovieId = criticreview.MovieId;
                existingReview.Review = criticreview.Review;
                existingReview.Rating = criticreview.Rating;
                existingReview.IsPublished = criticreview.IsPublished;
                
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CriticReviewExists(criticreview.CriticReviewId))
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


        // Add a dropdown field for all Movies
        ViewBag.MovieId = new SelectList(
            _context.Movie.ToList(),
            nameof(Movie.MovieId),
            nameof(Movie.Title),
            criticreview.MovieId
        );

        return View(criticreview);
    }

    // GET: CRITICREVIEWS/Delete/5
    [Authorize(Roles = "Critic")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var criticreview = await _context.CriticReview
            .Include(m => m.Movie)
            .FirstOrDefaultAsync(m => m.CriticReviewId == id);

        if (criticreview == null)
        {
            return NotFound();
        }

        // The current user must own the review to delete it.
        if (criticreview.CreatedById != _userId)
        {
            return Unauthorized();
        }

        // Cannot delete a published review, redirect to its details page.
        if (criticreview.IsPublished)
        {
            return RedirectToAction(nameof(Details), new { id });
        }

        return View(criticreview);
    }

    // POST: CRITICREVIEWS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Critic")]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var criticreview = await _context.CriticReview.FindAsync(id);
        if (criticreview != null)
        {
            _context.CriticReview.Remove(criticreview);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CriticReviewExists(int? id)
    {
        return _context.CriticReview.Any(e => e.CriticReviewId == id);
    }
}
