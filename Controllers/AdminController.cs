using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization.Attributes;
using System;
using ToDo_List_with_additions.Models;
using ToDo_List_with_additions.Services;

namespace ToDo_List_with_additions.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IUsersService _usersService;
        private readonly IToDosService _toDosService;
        private readonly IStatisticsService _statisticsService;
        public AdminController(IAdminService adminService, IUsersService usersService, IToDosService toDosService, IStatisticsService statisticsService)
        {
            _adminService = adminService;
            _usersService = usersService;
            _toDosService = toDosService;
            _statisticsService = statisticsService;
        }
        public IActionResult Index()
        {
			HttpContext.Session.Remove("adminUserId");
			var users = _adminService.GetUsers();
            var model = new UsersModel
            {
                Users = users
            };
            return View(model);

        }
        public IActionResult ToDos(string id, bool fromUsers)
        {
            Console.WriteLine("fromUsers: " + fromUsers);
            if (fromUsers) HttpContext.Session.SetString("adminUserId", id);
            var userId = HttpContext.Session.GetString("adminUserId");
            Console.WriteLine("userId: " + userId);
            var todos = _adminService.GetToDos(userId);
            var model = new ToDosAdminModel
            {
                UserName = _usersService.GetUser(userId).FirstName + " " + _usersService.GetUser(userId).LastName,
                UserNickname = _usersService.GetUser(userId).Nickname,
                ToDos = todos,
            };
            return View(model);
        }
		public IActionResult UserCreate()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult UserCreate(UserModel user)
		{
			var userValidation = _usersService.CheckLogin(user.Login);
			if (userValidation == null)
			{
				_usersService.Register(user);
				_statisticsService.Create(user.Id);
				var userFromDb = _usersService.Login(user.Login, user.Password);
				HttpContext.Session.SetString("userId", userFromDb.Id);
				return RedirectToAction(nameof(Index));
			}
			ViewBag.LoginError = "Login is already taken";
			return View();
		}
		public IActionResult UserEdit(string id)
		{
            HttpContext.Session.SetString("adminUserId", id);
            var userId = HttpContext.Session.GetString("adminUserId");
            var user = _usersService.GetUser(userId);
			var model = new UserModel()
			{
				Id = user.Id,
				Login = user.Login,
				Password = user.Password,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Nickname = user.Nickname
			};
			return View(model);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult UserEdit(string firstName, string lastName, string nickname, string login, string password)
		{
            var id = HttpContext.Session.GetString("adminUserId");
            var user = _usersService.GetUser(id);
			user.FirstName = firstName;
			user.LastName = lastName;
			user.Nickname = nickname;
			user.Login = login;
			user.Password = password;
			_usersService.Edit(user);
			return RedirectToAction(nameof(Index));
		}
		public IActionResult UserDelete(string id, bool fromUsers)
		{
			_usersService.Delete(id);
			return RedirectToAction(nameof(Index));
		}
        
		public IActionResult ToDoCreate()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToDoCreate(DateTime date, string content, int importance)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.Session.GetString("adminUserId");
                var toDo = new ToDoModel()
                {
                    UserId = userId,
                    Date = date.AddHours(1),
                    Content = content,
                    Importance = importance,
                    Done = false
                };
                
                _toDosService.Create(toDo);
                _statisticsService.IncrementNotDone(userId, importance);
                return RedirectToAction(nameof(ToDos));
            }
            return View();
        }
        public IActionResult ToDoDone(string id)
        {
            var toDo = _toDosService.GetToDo(id);
            _toDosService.Done(id);
            _statisticsService.IncrementDone(HttpContext.Session.GetString("adminUserId"), toDo.Importance);
            _statisticsService.DecrementNotDone(HttpContext.Session.GetString("adminUserId"), toDo.Importance);
            return RedirectToAction(nameof(ToDos));
        }
        public IActionResult ToDoEdit(string id)
        {
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
        public IActionResult ToDoEdit(string id, DateTime date, string content, int importance, bool done)
        {
            if (ModelState.IsValid)
            {
                var toDoBase = _toDosService.GetToDo(id);
                var toDo = _toDosService.GetToDo(id);
                toDo.Date = date.AddHours(1);
                toDo.Content = content;
                toDo.Done = done;
                toDo.Importance = importance;
                _toDosService.Edit(toDo);
                if (toDoBase.Date != date)
                {
                    _statisticsService.IncrementPostponed(HttpContext.Session.GetString("adminUserId"), toDo.Importance);

                }
                return RedirectToAction(nameof(ToDos));
            }
            return View();
        }
        public IActionResult ToDoDelete(string id)
        {
            _toDosService.Delete(id);
            return RedirectToAction(nameof(ToDos));
        }
        public IActionResult ToDoUserEdit(string id)
        {
            var userId = HttpContext.Session.GetString("adminUserId");
            var user = _usersService.GetUser(userId);
            var model = new UserModel()
            {
                Id = user.Id,
                Login = user.Login,
                Password = user.Password,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Nickname = user.Nickname
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToDoUserEdit(string firstName, string lastName, string nickname, string login, string password)
        {
            var id = HttpContext.Session.GetString("adminUserId");
            var user = _usersService.GetUser(id);
            user.FirstName = firstName;
            user.LastName = lastName;
            user.Nickname = nickname;
            user.Login = login;
            user.Password = password;
            _usersService.Edit(user);
            return RedirectToAction(nameof(ToDos));
        }
        public IActionResult ToDoUserDelete()
        {
            var id = HttpContext.Session.GetString("adminUserId");
            _usersService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        
      
    }
}
