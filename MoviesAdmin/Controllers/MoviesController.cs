
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAdmin.Models;
using MoviesAdmin.Services;
using System.Reflection.Metadata;

public class MoviesController : Controller
{
    private readonly MoviesAdminContext _context;

    private readonly IAzureStorageService _blobService; 

    public MoviesController(MoviesAdminContext context, IAzureStorageService blobService)
    {   
        _context = context;
        _blobService = blobService;
    }

    // GET: MOVIES
    public async Task<IActionResult> Index(string searchString)
    {

        var movies = _context.Movie.AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            movies = movies.Where(m => m.Title.Contains(searchString) || m.Genre.Contains(searchString));
        }

        return View(await movies.OrderBy(m => m.ReleaseDate).ToListAsync()); // Sorted by ReleaseDate by default
    }

    // GET: MOVIES/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movie
            .FirstOrDefaultAsync(m => m.Id == id);
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

    // POST: MOVIE/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Genre,Rating,Synopsis,ReleaseDate,Runtime,ImageUrl")] Movie movie, IFormFile imageFile) // collect inputs and file input from form
    {
        // Check if the model state is valid before adding the movie to the database
        if (ModelState.IsValid)
        {
            // Save file in Azure Storage
            string imageUrl = await _blobService.UploadFileAsync(imageFile, imageFile.FileName); // use the original filename
            // Save image Url in Movie record
            movie.ImageUrl = imageUrl;

            // Save record in database
            _context.Add(movie);
            await _context.SaveChangesAsync();

            // Redirect to the Index action after successfully creating the movie
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
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Title,Genre,Rating,Synopsis,ReleaseDate,Runtime,ImageUrl")] Movie movie)
    {
        if (id != movie.Id)
        {
            return NotFound();
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
                if (!MovieExists(movie.Id))
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
            .FirstOrDefaultAsync(m => m.Id == id);
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
        return _context.Movie.Any(e => e.Id == id);
    }

}