using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySchool.Models;

namespace MySchool.Controllers
{
	public class ContactsInfoController : Controller
	{
		private readonly AppDbContext _context;

		public ContactsInfoController(AppDbContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			var contactsInfo = _context.ContactsInfo.Include(c => c.Student).ToList();

			return View(contactsInfo);
		}

		public IActionResult Create()
		{
			var studentsInContactsInfo = _context.ContactsInfo.Select(c => c.StudentId).ToList();

			var dropdown = new StudentsDropdownVm()
			{
				Students = _context.Students.Where(s => !studentsInContactsInfo.Contains(s.Id)).ToList()
			};

			ViewBag.Students = new SelectList(dropdown.Students, "Id", "LastName");

			return View();
		}

		[HttpPost]
		public IActionResult Create(AddContactInfoVM vm)
		{
			var newContactInfo = new ContactInfo()
			{
				PhoneNumber = vm.PhoneNumber,
				City = vm.City,
				StudentId = vm.StudentId
			};

			_context.ContactsInfo.Add(newContactInfo);
			_context.SaveChanges();

			return RedirectToAction(nameof(Index));
		}

		public IActionResult Update(int id)
		{
			var contactInfoInDb = _context.ContactsInfo.Find(id);

			var respones = new AddContactInfoVM()
			{
				City = contactInfoInDb.City,
				PhoneNumber = contactInfoInDb.PhoneNumber,
				StudentId = contactInfoInDb.StudentId
			};

			var studentsInContactsInfo = _context.ContactsInfo
				.Where(c => c.StudentId != contactInfoInDb.StudentId)
				.Select(c=>c.StudentId)
				.ToList();

			var dropdown = new StudentsDropdownVm()
			{
				Students = _context.Students.Where(s => !studentsInContactsInfo.Contains(s.Id)).ToList()
			};

			ViewBag.Students = new SelectList(dropdown.Students, "Id", "LastName");

			return View(respones);
		}

		[HttpPost]
		public IActionResult Update(int id, AddContactInfoVM vm)
		{
			var contactInfoInDb = _context.ContactsInfo.Find(id);

			contactInfoInDb.City = vm.City;
			contactInfoInDb.PhoneNumber = vm.PhoneNumber;
			contactInfoInDb.StudentId = vm.StudentId;

			_context.SaveChanges();

			return RedirectToAction(nameof(Index));
		}

		public IActionResult Delete(int id)
		{
			var contactInfoInDb = _context.ContactsInfo.Find(id);

			_context.ContactsInfo.Remove(contactInfoInDb);
			_context.SaveChanges();

			return RedirectToAction(nameof(Index));
		}
	}
}
