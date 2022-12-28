using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Security.Policy;
using ToDo_List_with_additions.Models;
using ToDo_List_with_additions.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ToDo_List_with_additions.Controllers
{
    public class ToDoController : Controller
    {
		private readonly ILogger<ToDoController> _logger;
		private readonly IToDosService _toDosService;
		public ToDoController(ILogger<ToDoController> logger, IToDosService toDosService)
		{
			_logger = logger;
			_toDosService = toDosService;
		}
        public ActionResult Index()
        {
			if (HttpContext.Session.GetString("userId") == null)
			{
				return RedirectToAction("Login", "User");
			}
			Console.WriteLine("Index ToDo:" + HttpContext.Session.GetString("userId"));
			var userId = HttpContext.Session.GetString("userId");
            var model = new ToDosModel()
            {
                ToDosToday = _toDosService.GetToday(userId),
                ToDosOthers = _toDosService.GetOthers(userId),
                ToDosDone = _toDosService.GetDone(userId)
            };
            return View(model);
        }
        public ActionResult Create()
        {
			return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DateTime date, string content, int importance, bool done)
        {
            Console.WriteLine("Create ToDo:" + HttpContext.Session.GetString("userId"));
			if (HttpContext.Session.GetString("userId") == null)
			{
				return RedirectToAction("Login", "User");
			}
			if (ModelState.IsValid)
            {
                var userId = HttpContext.Session.GetString("userId");
                var toDo = new ToDoModel()
                {
                    UserId = userId,
					Date = date.AddHours(1),
				    Content = content,
                    Importance = importance,
                    Done = done
                };
				_toDosService.Create(toDo);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public ActionResult Edit(string id)
        {
			if (HttpContext.Session.GetString("userId") == null)
			{
				return RedirectToAction("Login", "User");
			}
			var toDo = _toDosService.GetToDo(id);
            var model = new ToDoModel()
            {
                Id = toDo.Id,
                UserId = toDo.UserId,
                Date = toDo.Date,
                Content = toDo.Content,
                Importance = toDo.Importance,
                Done = toDo.Done
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, DateTime date, string content, int importance, bool done)
        {
			if (HttpContext.Session.GetString("userId") == null)
			{
				return RedirectToAction("Login", "User");
			}
			if (ModelState.IsValid)
            {
                Console.WriteLine("Edit ToDo:" + HttpContext.Session.GetString("userId"));
                var toDo = _toDosService.GetToDo(id);
                toDo.Date = date.AddHours(1);
                toDo.Content = content;
                toDo.Done = done;
                toDo.Importance = importance;
				_toDosService.Edit(toDo);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public ActionResult Delete(string id)
        {
			if (HttpContext.Session.GetString("userId") == null)
			{
				return RedirectToAction("Login", "User");
			}
			_toDosService.Delete(id);
            return RedirectToAction(nameof(Index));
		}
    }
}
