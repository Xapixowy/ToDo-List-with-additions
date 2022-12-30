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
        private readonly IStatisticsService _statisticsService;
        private readonly IUsersService _usersService;
        public ToDoController(ILogger<ToDoController> logger, IToDosService toDosService, IStatisticsService statisticsService, IUsersService usersService)
		{
			_logger = logger;
			_toDosService = toDosService;
            _statisticsService = statisticsService;
            _usersService = usersService;
        }
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetString("userId");
            Console.WriteLine("Index:" + userId);
            if (userId == null)
			{
				return RedirectToAction("Login", "User");
			}
            var model = new ToDosModel()
            {
                UserName = _usersService.GetUser(userId).FirstName + " " + _usersService.GetUser(userId).LastName,
                UserNickname = _usersService.GetUser(userId).Nickname,
                ToDosToday = _toDosService.GetToday(userId),
                ToDosOthers = _toDosService.GetOthers(userId),
                ToDosDone = _toDosService.GetDone(userId)
            };
            Console.WriteLine("ToDosToday(" + model.ToDosToday.Count + "):");
            foreach (var item in model.ToDosToday)
            {
                Console.WriteLine(item.Id);
            };
            Console.WriteLine("ToDosOthers(" + model.ToDosOthers.Count + "):");
            foreach (var item in model.ToDosOthers)
            {
                Console.WriteLine(item.Id);
            };
            Console.WriteLine("ToDosDone(" + model.ToDosDone.Count + "):");
            foreach (var item in model.ToDosDone)
            {
                Console.WriteLine(item.Id);
            };
            return View(model);
        }
        public IActionResult Create()
        {
            var userId = HttpContext.Session.GetString("userId");
            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }
            Console.WriteLine("Create:" + userId);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DateTime date, string content, int importance, bool done)
        {
            var userId = HttpContext.Session.GetString("userId");
            Console.WriteLine("Create (POST):" + userId);
            if (userId == null)
			{
				return RedirectToAction("Login", "User");
			}
			if (ModelState.IsValid)
            {
                var toDo = new ToDoModel()
                {
                    UserId = userId,
					Date = date.AddHours(1),
				    Content = content,
                    Importance = importance,
                    Done = done
                };
                 _toDosService.Create(toDo);
                 _statisticsService.IncrementNotDone(userId, toDo.Importance);
                 return RedirectToAction(nameof(Index));          
            }
            return View();
        }
        public IActionResult Edit(string id)
        {
            var userId = HttpContext.Session.GetString("userId");
            Console.WriteLine("Edit:" + userId);
            if (userId  == null)
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
        public IActionResult Edit(string id, DateTime date, string content, int importance, bool done)
        {
            var userId = HttpContext.Session.GetString("userId");
            Console.WriteLine("Edit (POST):" + userId);
            if (userId == null)
			{
				return RedirectToAction("Login", "User");
			}
			if (ModelState.IsValid)
            {
                var toDoBase = _toDosService.GetToDo(id);
                var toDo = _toDosService.GetToDo(id);
                toDo.Date = date.AddHours(1);
                toDo.Content = content;
                toDo.Done = done;
                toDo.Importance = importance;
				_toDosService.Edit(toDo);
                if (toDoBase.Date != date )
                {
                    _statisticsService.IncrementPostponed(userId, toDo.Importance);
                    
                }
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public IActionResult Delete(string id)
        {
            var userId = HttpContext.Session.GetString("userId");
            Console.WriteLine("Delete:" + userId);
            if (userId == null)
			{
				return RedirectToAction("Login", "User");
			}
			_toDosService.Delete(id);
            return RedirectToAction(nameof(Index));
		}

        public IActionResult Done(string id)
        {
            var userId = HttpContext.Session.GetString("userId");
            Console.WriteLine("Done:" + userId);
            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }
            var toDo = _toDosService.GetToDo(id);
          
            _toDosService.Done(toDo.Id);
            _statisticsService.IncrementDone(HttpContext.Session.GetString("userId"), toDo.Importance);
            _statisticsService.DecrementNotDone(HttpContext.Session.GetString("userId"), toDo.Importance);

            return RedirectToAction(nameof(Index));
        }
    }
}
