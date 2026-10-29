using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviesAdmin.Models;

public class CriticReviewsController : Controller
{
    private readonly MoviesAdminContext _context;

    public CriticReviewsController(MoviesAdminContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index() => View(await _context.CriticReview.Include(r => r.Movie).ToListAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var review = await _context.CriticReview.Include(r => r.Movie).FirstOrDefaultAsync(r => r.Id == id);
        return review == null ? NotFound() : View(review);
    }

    public async Task<IActionResult> Create()
    {
        await PopulateMoviesAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MovieId,Description,Rating,IsPublished,CreatedBy,CreatedDate")] CriticReview review)
    {
        if (ModelState.IsValid)
        {
            _context.Add(review);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        await PopulateMoviesAsync(review.MovieId);
        return View(review);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var review = await _context.CriticReview.FindAsync(id);
        if (review == null) return NotFound();
        await PopulateMoviesAsync(review.MovieId);
        return View(review);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,MovieId,Description,Rating,IsPublished,CreatedBy,CreatedDate")] CriticReview review)
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

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var review = await _context.CriticReview.Include(r => r.Movie).FirstOrDefaultAsync(r => r.Id == id);
        return review == null ? NotFound() : View(review);
    }

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
