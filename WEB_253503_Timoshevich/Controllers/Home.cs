using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253503_Timoshevich.Models;

namespace WEB_253503_Timoshevich.Controllers
{
	public class Home : Controller
	{
        // GET: Home
        [ViewData]
        public List<ListDemo> ListDemos { get; set; } = new() {
        new ListDemo { Id = 0, Name = "Item-1" },
        new ListDemo { Id = 1, Name = "Item-2" },
        new ListDemo { Id = 2, Name = "Item-3" }

    };
        public ActionResult Index()
		{
			ViewData["LabTitle"] = "Лабораторная работа №2";
            ViewBag.SelectList = new SelectList(ListDemos, "Id", "Name");

            return View();
		}

		// GET: Home/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: Home/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Home/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: Home/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: Home/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: Home/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: Home/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}
