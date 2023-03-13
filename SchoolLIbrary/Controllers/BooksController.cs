using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolLIbrary.Data.ContextClass;
using SchoolLIbrary.Models;
using SchoolLIbrary.Models.ViewModels;

namespace SchoolLIbrary.Controllers
{
    public class BooksController : BaseController
    {
        private readonly LibraryDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BooksController(UserManager<ApplicationUser> userManager, LibraryDbContext context) : base(userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: /Books/
        public async Task<IActionResult> Index(string search)
        {
            // Retrieve all books from the database
            var books = await _context.Materials.ToListAsync();

            // Filter the books by the search string
            if (!string.IsNullOrEmpty(search))
            {
                books = books.Where(b => b.Title.Contains(search, StringComparison.OrdinalIgnoreCase) || b.Author.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Map the book objects to the BookViewModel for display
            var bookViewModels = books.Select(b => new BookViewModel
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Publisher = b.Publisher,
                Year = b.Year,
                Genre = b.Genre,
                MaterialType = b.MaterialType,
                Falculty = b.Falculty,
                Department = b.Department,
                Quantity = b.Quantity,
                DateCreated = b.DateCreated
            });

            // Pass the BookViewModels to the view for display
            return View(bookViewModels);
        }

        // GET: /Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the book with the given ID from the database
            var book = await _context.Materials.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            // Map the book object to the BookViewModel for display
            var bookViewModel = new BookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Publisher = book.Publisher,
                Year = book.Year,
                Genre = book.Genre,
                MaterialType = book.MaterialType,
                Falculty = book.Falculty,
                Department = book.Department,
                Quantity = book.Quantity,
                DateCreated = book.DateCreated
            };

            // Pass the BookViewModel to the view for display
            return View(bookViewModel);
        }

        // GET: /Books/Create
        public IActionResult Create()
        {
            // Return an empty BookViewModel to the view for display
            return View(new BookViewModel());
        }

        // POST: /Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Map the BookViewModel to a Book object for saving to the database
                var book = new MaterialModel
                {
                    Title = model.Title,
                    Author = model.Author,
                    Publisher = model.Publisher,
                    Year = model.Year,
                    Genre = model.Genre,
                    MaterialType = model.MaterialType,
                    Falculty = model.Falculty,
                    Department = model.Department,
                    Quantity = model.Quantity,
                    DateCreated = DateTime.Now
                };

                // Save the new book to the database
                _context.Materials.Add(book);
                await _context.SaveChangesAsync();

                //Display success message
                TempData["Message"] = "Book has been added successfully";

                return RedirectToAction(nameof(Index));
            }

            // If the model state is not valid, return the BookViewModel back to the view with validation errors
            return View(model);
        }
        // GET: Book/Edit/5
        // Returns the Edit view for a specific book
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Find the book with the specified id in the database
            var book = await _context.Materials.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            // Create a view model for the book data
            var bookViewModel = new BookViewModel()
            {
                Title = book.Title,
                Author = book.Author,
                Publisher = book.Publisher,
                Year = book.Year,
                Genre = book.Genre,
                MaterialType = book.MaterialType,
                Falculty = book.Falculty,
                Department = book.Department,
                Quantity = book.Quantity,
                //DateCreated = book.DateCreated
            };

            return View(bookViewModel);
        }

        // POST: Materials/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Edit(int id, BookViewModel bookViewModel)
        {
            if (id != bookViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var material = await _context.Materials.FindAsync(id);
                if (material == null)
                {
                    return NotFound();
                }

                material.Title = bookViewModel.Title;
                material.Author = bookViewModel.Author;
                material.Publisher = bookViewModel.Publisher;
                material.Year = bookViewModel.Year;
                material.Genre = bookViewModel.Genre;
                material.MaterialType = bookViewModel.MaterialType;
                material.Falculty = bookViewModel.Falculty;
                material.Department = bookViewModel.Department;
                material.Quantity = bookViewModel.Quantity;

                try
                {
                    _context.Materials.Update(material);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialExists(material.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                TempData["Message"] = "Material has been successfully updated.";

                return RedirectToAction(nameof(Index));
            }

            return View(bookViewModel);
        }
        private bool MaterialExists(int id)
        {
            return _context.Materials.Any(e => e.Id == id);
        }


        private bool BookExists(int id)
        {
            return _context.Materials.Any(e => e.Id == id);
        }

        // GET: Materials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await _context.Materials
                .FirstOrDefaultAsync(m => m.Id == id);

            if (material == null)
            {
                return NotFound();
            }

            var bookViewModel = new BookViewModel
            {
                Id = material.Id,
                Title = material.Title,
                Author = material.Author,
                Publisher = material.Publisher,
                Year = material.Year,
                Genre = material.Genre,
                MaterialType = material.MaterialType,
                Falculty = material.Falculty,
                Department = material.Department,
                Quantity = material.Quantity,
                DateCreated = material.DateCreated
            };

            return View(bookViewModel);
        }

        // POST: Materials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var material = await _context.Materials.FindAsync(id);

            if (material == null)
            {
                return NotFound();
            }

            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Material has been successfully deleted.";

            return RedirectToAction(nameof(Index));
        }


    }

}
