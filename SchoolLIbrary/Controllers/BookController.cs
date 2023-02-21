using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolLIbrary.Data.ContextClass;
using SchoolLIbrary.Models;
using SchoolLIbrary.Models.ViewModels;

namespace SchoolLIbrary.Controllers
{
    public class BookController : Controller
    {
        public class BooksController : Controller
        {
            private readonly LibraryDbContext _context;
            public BooksController(LibraryDbContext context)
            {
                _context = context;
            }

            // GET: /Books/
            public async Task<IActionResult> Index()
            {
                // Retrieve all books from the database
                var books = await _context.Materials.ToListAsync();

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
                    DateCreated = b.DateCreated.ToShortDateString()
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
                    DateCreated = book.DateCreated.ToShortDateString()  
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

            // POST: Book/Edit/5
            // Updates a book in the database with the data from the Edit view form submission
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, BookViewModel bookViewModel)
            {
                if (id != bookViewModel.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        // Find the book with the specified id in the database
                        var book = await _context.Materials.FindAsync(id);

                        // Update the book data with the values from the view model
                        book.Title = bookViewModel.Title;
                        book.Author = bookViewModel.Author;
                        book.Publisher = bookViewModel.Publisher;
                        book.Year = bookViewModel.Year;
                        book.Genre = bookViewModel.Genre;
                        book.MaterialType = bookViewModel.MaterialType;
                        book.Falculty = bookViewModel.Falculty;
                        book.Department = bookViewModel.Department;
                        book.Quantity = bookViewModel.Quantity;
                        //book.DateCreated = viewModel.DateCreated;

                        // Save changes to the database
                        await _context.SaveChangesAsync();
                        // Display a success message
                        TempData["Message"] = "Book has been edited successfully.";
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BookExists(bookViewModel.Id))
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
                return View(bookViewModel);
            }
            private bool BookExists(int id)
            {
                return _context.Materials.Any(e => e.Id == id);
            }

            // GET: Book/Delete/5
            public async Task<IActionResult> Delete(int? id)
            {
                // Check if the book ID is provided
                if (id == null)
                {
                    return NotFound();
                }

                // Retrieve the book from the database using the ID
                var book = await _context.Materials
                    .FirstOrDefaultAsync(m => m.Id == id);

                // Check if the book is found
                if (book == null)
                {
                    return NotFound();
                }

                // Map the book model to the view model
                var bookViewModel = new BookViewModel
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
                    DateCreated = book.DateCreated.ToShortDateString()
                };

                // Pass the view model to the view
                return View(bookViewModel);
            }

            // POST: Book/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                // Retrieve the book from the database using the ID
                var book = await _context.Materials.FindAsync(id);

                // Check if the book is found
                if (book == null)
                {
                    return NotFound();
                }

                // Remove the book from the database
                _context.Materials.Remove(book);
                await _context.SaveChangesAsync();

                // Display a success message
                TempData["Message"] = "Book has been successfully deleted.";

                // Redirect to the book index page
                return RedirectToAction(nameof(Index));
            }

        }
    }
}
