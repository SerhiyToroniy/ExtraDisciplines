using ExtraDisciplines.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ExtraDisciplines.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _dbContext;

        public AdminController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            // Get all courses from the database
            var courses = await _dbContext.Courses.ToListAsync();

            return View(courses);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Course course)
        {
            if (ModelState.IsValid)
            {
                // Add the course to the database
                _dbContext.Courses.Add(course);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(course);
        }

        public async Task<IActionResult> Edit(int id)
        {
            // Find the course by id
            var course = await _dbContext.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update the course in the database
                    _dbContext.Courses.Update(course);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
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

            return View(course);
        }

        public async Task<IActionResult> Delete(int id)
        {
            // Find the course by id
            var course = await _dbContext.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Find the course by id
            var course = await _dbContext.Courses.FindAsync(id);

            // Remove the course from the database
            _dbContext.Courses.Remove(course);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Ratings(int id)
        {
            // Find the course by id
            var course = await _dbContext.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            // Get the list of students who registered for the course and order them by average score
            var students = await _dbContext.Enrollments
                .Where(e => e.CourseId == id)
                .Include(e => e.Student)
                .OrderByDescending(e => e.Score)
                .ToListAsync();

            return View(students);
        }

        public async Task<IActionResult> SetMaxCapacity(int id)
        {
            // Find the course by id
            var course = await _dbContext.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> SetMaxCapacity(int id, int maxCapacity)
        {
            // Find the course by id
            var course = await _dbContext.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            course.MaxCapacity = maxCapacity;

            // Update the course in the database
            _dbContext.Courses.Update(course);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _dbContext.Courses.Any(e => e.Id == id);
        }
    }
}
