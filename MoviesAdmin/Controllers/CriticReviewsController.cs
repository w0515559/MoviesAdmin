using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAdmin;
using MoviesAdmin.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;


[Authorize(Roles ="Critic")]
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
        var reviews = _context.CriticReview
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
            .Include(r => r.Movie)
            .FirstOrDefaultAsync(r => r.Id == id);
        return review == null ? NotFound() : View(review);
    }

    // GET: CriticReviews/Create
    public async Task<IActionResult> Create()
    {
        await PopulateMoviesAsync();
        return View();
    }

    // POST: CriticReviews/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Description,Rating,IsPublished")] CriticReview review)
    {
        review.CreatedBy = _userId!;
        review.CreatedDate = DateTime.Now;


        if (ModelState.IsValid)
        {
            _context.Add(review);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        await PopulateMoviesAsync(review.MovieId);

        return View(review);
    }

    // GET: CriticReviews/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) 
            return NotFound(); 

        var review = await _context.CriticReview.FindAsync(id);

        if (review == null) 
            return NotFound();

        await PopulateMoviesAsync(review.MovieId);

        return View(review);
    }

    // POST: CriticReviews/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Description,Rating,IsPublished")] CriticReview review)
    {
        if (id != review.Id) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(review);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!CriticReviewExists(review.Id))
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        await PopulateMoviesAsync(review.MovieId);
        return View(review);
    }

    // GET: CriticReviews/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var review = await _context.CriticReview.Include(r => r.Movie).FirstOrDefaultAsync(r => r.Id == id);
        return review == null ? NotFound() : View(review);
    }

    // POST: CriticReviews/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var review = await _context.CriticReview.FindAsync(id);
        if (review != null)
        {
            _context.CriticReview.Remove(review);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool CriticReviewExists(int id) => _context.CriticReview.Any(r => r.Id == id);

    private async Task PopulateMoviesAsync(int? selectedMovieId = null)
    {
        ViewData["MovieId"] = new SelectList(await _context.Movie.OrderBy(m => m.Title).ToListAsync(), "Id", "Title", selectedMovieId);
    }
}
