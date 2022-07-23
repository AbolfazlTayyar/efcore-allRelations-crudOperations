using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySchool.Models;

namespace MySchool.Controllers
{
	public class StudentsController : Controller
	{
		private readonly AppDbContext _context;

		public StudentsController(AppDbContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			var students = _context.Students.Include(s => s.Teacher).ToList();
			return View(students);
		}

		public IActionResult Create()
		{
			ViewBag.Courses = new SelectList(_context.Courses.ToList(), "Id", "Name");
			ViewBag.Teachers = new SelectList(_context.Teachers.ToList(), "Id", "Name");

			return View();
		}

		[HttpPost]
		public IActionResult Create(AddStudentVM vm)
		{
			var newStudent = new Student()
			{
				FirstName = vm.FirstName,
				LastName = vm.LastName,
				DateOfBirth = vm.DateOfBirth,
				TeacherId = vm.TeacherId
			};

			_context.Students.Add(newStudent);
			_context.SaveChanges();

			foreach (var courseId in vm.CourseIds)
			{
				var newStudentCourse = new StudentCourse()
				{
					StudentId = newStudent.Id,
					CourseId = courseId
				};

				_context.StudentCourses.Add(newStudentCourse);
			}

			_context.SaveChanges();

			return RedirectToAction("Index");
		}

		public IActionResult Update(int id)
		{
			var student = _context.Students
				.Include(s => s.StudentCourses)
				.ThenInclude(s => s.Course)
				.FirstOrDefault(s => s.Id == id);

			//var variable = student.StudentCourses.Select(sc => sc.CourseId).ToList();

			var response = new AddStudentVM()
			{
				FirstName = student.FirstName,
				LastName = student.LastName,
				DateOfBirth = student.DateOfBirth,
				TeacherId = student.TeacherId,
				CourseIds = student.StudentCourses.Select(sc => sc.CourseId).ToList()
			};

			ViewBag.Courses = new SelectList(_context.Courses.ToList(), "Id", "Name");
			ViewBag.Teachers = new SelectList(_context.Teachers.ToList(), "Id", "Name");

			return View(response);
		}

		[HttpPost]
		public IActionResult Update(int id, AddStudentVM vm)
		{
			var studentInDb = _context.Students.FirstOrDefault(s => s.Id == id);

			studentInDb.FirstName = vm.FirstName;
			studentInDb.LastName = vm.LastName;
			studentInDb.DateOfBirth = vm.DateOfBirth;
			studentInDb.TeacherId = vm.TeacherId;

			_context.SaveChanges();

			var existingCourseInDb = _context.StudentCourses.Where(sc => sc.StudentId == vm.Id).ToList();
			_context.StudentCourses.RemoveRange(existingCourseInDb);
			_context.SaveChanges();

			foreach (var courseId in vm.CourseIds)
			{
				var newStudentCourse = new StudentCourse()
				{
					StudentId = vm.Id,
					CourseId = courseId
				};

				_context.StudentCourses.Add(newStudentCourse);
			}

			_context.SaveChanges();

			return RedirectToAction(nameof(Index));
		}

		public IActionResult Delete(int id)
		{
			var studentInDb = _context.Students.FirstOrDefault(s => s.Id == id);

			_context.Students.Remove(studentInDb);
			_context.SaveChanges();

			return RedirectToAction(nameof(Index));
		}
	}
}
