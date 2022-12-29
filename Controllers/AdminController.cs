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


        public ActionResult Index()
        {
            var users = _adminService.GetUsers();
            var model = new UsersModel
            {
                Users = users
            };
            return View(model);

        }

    

        public ActionResult Todos(string id)
        {
            var todos = _adminService.GetToDos(id);
            var model = new ToDosAdminModel
            {
                Todos = todos,
                userId = id
            };
            return View(model);
        }

        public ActionResult Create(string id)
        {
            var model = new ToDoModel
            {
                UserId = id
            };
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string userId, DateTime date, string content, int importance, bool done)
        {
         
            if (ModelState.IsValid)
            {

                
                var toDo = new ToDoModel()
                {
                    Date = date.AddHours(1),
                    Content = content,
                    Importance = importance,
                    Done = done
                };
                
                _toDosService.Create(toDo);
                _statisticsService.IncrementNotDone(userId, importance);
                return RedirectToAction(nameof(Todos));
            }
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserModel user)
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
        public IActionResult EditUser(string id)
        {
            
            var user = _usersService.GetUser(id);
            
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
        public IActionResult EditUser(string id, string firstName, string lastName, string nickname, string login, string password)
        {
          
            var user = _usersService.GetUser(id);
            user.FirstName = firstName;
            user.LastName = lastName;
            user.Nickname = nickname;
            user.Login= login;
            user.Password = password;
            _usersService.Edit(user);
            return RedirectToAction(nameof(Index));
        }
        public ActionResult DeleteUser(string id)
        {

            _usersService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        public ActionResult EditToDo(string id)
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
        public ActionResult EditToDo(string id, DateTime date, string content, int importance, bool done)
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
                    _statisticsService.IncrementPostponed(HttpContext.Session.GetString("userId"), toDo.Importance);

                }

                return RedirectToAction(nameof(Todos));
            }
            return View();
        }
        public ActionResult DeleteToDo(string id)
        {
          
            _toDosService.Delete(id);
            return RedirectToAction(nameof(Todos));
        }
    }
}
