using ExtraDisciplines.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class StudentController : Controller
{
    private readonly AppDbContext _dbContext;
    private readonly UserManager<User> _userManager;


    public StudentController(AppDbContext dbContext, UserManager<User> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<IActionResult> Enroll(int courseId, string studentId, int score)
    {
        // Get the course and student by their respective IDs from the database
        var course = _dbContext.Courses.FirstOrDefault(c => c.Id == courseId);
        var student = _dbContext.Students.FirstOrDefault(c => c.UserId == studentId);


        if (course == null || student == null)
        {
            // Course or student not found, handle accordingly (e.g., show an error message)
            return RedirectToAction("Index", "Home");
        }

        var enroll = _dbContext.Enrollments.Where(c => c.StudentId == studentId && c.CourseId == courseId).FirstOrDefault();

        if (enroll == null)
        {
            if (course.MaxCapacity - course.AlreadyEnrolledCount > 0)
            {
                var enrollment = new Enrollment() { CourseId = courseId, StudentId = studentId, Student = student, Score = score, IsPassed = score > 50 };
                await _dbContext.Enrollments.AddAsync(enrollment);
                course.AlreadyEnrolledCount++;
                await _dbContext.SaveChangesAsync();
            }

        }

        // Redirect back to the course list or show a success message
        return RedirectToAction("Index", "Home");
    }

    public IActionResult MyDisciplines()
    {
        // Get the current user's identity
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Retrieve the user's selected disciplines from the database
        var disciplines = _dbContext.Enrollments
            .Where(e => e.Student.UserId == userId)
            .Join(
                _dbContext.Courses,
                enrollment => enrollment.CourseId,
                course => course.Id,
                (enrollment, course) => course
            )
            .ToList();

        return View(disciplines);
    }

    public async Task<IActionResult> Results()
    {
        var students = await _dbContext.Students
            .Where(s => s.Enrollments.Any(e => e.IsPassed))
            .ToListAsync();

        return View(students);
    }



}
