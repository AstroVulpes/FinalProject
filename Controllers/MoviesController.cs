using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProject.Data;
using FinalProject.Models;

namespace FinalProject.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Movies.ToListAsync());
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            // Retrieve all comments for the movie
            var comments = await _context.Comments
                .Where(c => c.MovieId == id) // Filter comments by MovieId
                .ToListAsync();

            // Create a view model to hold both movie and comments
            var viewModel = new MovieDetailsViewModel
            {
                Movie = movie,
                Comments = comments
            };

            return View(viewModel); // Pass the view model to the view
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ThumbnailName,Director,Starring,Production,Distribution,ReleaseDate,Runtime,Country,Language,Description,UserId")] Movie movie, IFormFile thumbnail)
        {
            const string defaultThumbnail = "Missing-image-232x150.png"; // Replace with your actual default image file name or path
            if (ModelState.IsValid)
            {
                // Check if a file has been uploaded
                if (thumbnail != null && thumbnail.Length > 0)
                {
                    // Generate a unique filename
                    var fileName = Path.GetFileName(thumbnail.FileName);
                    var filePath = Path.Combine("wwwroot/uploads", fileName);

                    // Save the file
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await thumbnail.CopyToAsync(stream);
                    }

                    // Set the ThumbnailName after saving the file
                    movie.ThumbnailName = fileName;

                }
                else
                {
                    // Set ThumbnailName to the default if no file uploaded
                    movie.ThumbnailName = defaultThumbnail;
                }
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ThumbnailName,Director,Starring,Production,Distribution,ReleaseDate,Runtime,Country,Language,Description,UserId")] Movie movie, IFormFile thumbnail)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Check if a file has been uploaded
                    if (thumbnail != null && thumbnail.Length > 0)
                    {
                        // Generate a unique filename
                        var fileName = Path.GetFileName(thumbnail.FileName);
                        var filePath = Path.Combine("wwwroot/uploads", fileName);

                        // Save the file
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await thumbnail.CopyToAsync(stream);
                        }

                        // Set the ThumbnailName after saving the file
                        movie.ThumbnailName = fileName;

                    }
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

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
