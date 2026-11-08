using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAdmin;
using MoviesAdmin.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;


[Authorize(Roles = "Critic")]
public class CriticReviewsController : Controller
{
    private readonly MoviesAdminContext _context;

    private string? _userId;

    public CriticReviewsController(MoviesAdminContext context)
    {
        _context = context;
    }


    // User is not available when contructor is called.
    // OnActionExecuting() is called after constructor is called, but before action method is called.
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        _userId = User.GetUserId();
    }


    // GET: CriticReviews
    public async Task<IActionResult> Index()
    {
        var reviews = await _context.CriticReview
            .Where(r => r.CreatedBy == _userId)
            .Include(r => r.Movie)
            .ToListAsync();


        return View(reviews);

    }

    // GET: CriticReviews/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var review = await _context.CriticReview
            .Where(r => r.CreatedBy == _userId)
            .Include(r => r.Movie)
            .FirstOrDefaultAsync(r => r.Id == id);
        return review == null ? NotFound() : View(review);
    }

    // GET: CriticReviews/Create
    public async Task<IActionResult> Create(int movieId)
    {
        if (!await _context.Movie.AnyAsync(m => m.Id == movieId))
        {
            return NotFound();
        }
            

        if (await _context.CriticReview.AnyAsync(r => r.MovieId == movieId && r.CreatedBy == _userId))
        {
            return NotFound();
        }
            

        ViewBag.MovieId = movieId;
        return View();
    }

    // POST: CriticReviews/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MovieId,Description,Rating,IsPublished")] CriticReview review)
    {
        review.CreatedBy = _userId!;
        review.CreatedDate = DateTime.Now;

        if (!await _context.Movie.AnyAsync(m => m.Id == review.MovieId))
        {
            return NotFound();
        }

        if (await _context.CriticReview.AnyAsync(r => r.MovieId == review.MovieId && r.CreatedBy == _userId))
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _context.Add(review);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(review);
    }

    // GET: CriticReviews/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var review = await _context.CriticReview
            .Where(r => r.CreatedBy == _userId && !r.IsPublished)
            .Include(r => r.Movie)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (review == null)
        {
            return NotFound();
        }

        return View(review);
    }

    // POST: CriticReviews/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,MovieId,Description,Rating,IsPublished,CreatedBy,CreatedDate")] CriticReview review)
    {
        if (id != review.Id) return NotFound();

        var stored = await _context.CriticReview // store a copy of original values
            .AsNoTracking() // makes it read-only, 
            .FirstOrDefaultAsync(r => r.Id == review.Id && r.CreatedBy == _userId);

        if (stored == null || stored.IsPublished) return NotFound(); // if its published dont let them edit

        // copy the values that they can't change
        review.CreatedBy = stored.CreatedBy;
        review.CreatedDate = stored.CreatedDate;
        review.MovieId = stored.MovieId;

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(review);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CriticReviewExists(review.Id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        review.Movie = (await _context.Movie.FindAsync(stored.MovieId))!;
        return View(review);
    }

    // GET: CriticReviews/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var review = await _context.CriticReview
            .Include(r => r.Movie)
            .Where(r => r.CreatedBy == _userId && !r.IsPublished)
            .FirstOrDefaultAsync(r => r.Id == id);
        return review == null ? NotFound() : View(review);
    }

    // POST: CriticReviews/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var review = await _context.CriticReview
            .Where(r => r.CreatedBy == _userId)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (review != null)
        {
            _context.CriticReview.Remove(review);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }


    private bool CriticReviewExists(int id) => _context.CriticReview.Any(r => r.Id == id);
}

