using Microsoft.AspNetCore.Mvc;
using MySchool.Models;

namespace MySchool.Controllers
{
    public class TeachersController : Controller
    {
        private readonly AppDbContext _context;

        public TeachersController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var teachers = _context.Teachers.ToList();
            return View(teachers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Teacher teacher)
        {
            _context.Teachers.Add(teacher);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {
            var teacherInDb = _context.Teachers.Find(id);
            return View(teacherInDb);
        }

        [HttpPost]
        public IActionResult Update(int id, Teacher teacher)
        {
            var teacherInDb = _context.Teachers.Find(id);
            teacherInDb.Name = teacher.Name;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var teacherInDb = _context.Teachers.Find(id);
            _context.Teachers.Remove(teacherInDb);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
