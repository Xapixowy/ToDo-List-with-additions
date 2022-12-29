using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization.Attributes;
using ToDo_List_with_additions.Models;
using ToDo_List_with_additions.Services;

namespace ToDo_List_with_additions.Controllers
{
    public class UserController : Controller
    {
		private readonly ILogger<UserController> _logger;
		private readonly IUsersService _usersService;
        private readonly IStatisticsService _statisticsService;
        public UserController(ILogger<UserController> logger, IUsersService usersService, IStatisticsService statisticsService)
        {
            _logger = logger;
            _usersService = usersService;
            _statisticsService = statisticsService;
        }

        public ActionResult Index()
        {
            return View("Login");
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserModel user)
        {
            var userFromDb = _usersService.Login(user.Login, user.Password);
            if (userFromDb != null)
            {
                HttpContext.Session.SetString("userId", userFromDb.Id);
                return RedirectToAction("Index", "ToDo");
            }
            ViewBag.LoginError = "Login or Password is incorrect";
            return View();
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Remove("userId");
            return RedirectToAction(nameof(Login));
        }
     
        public ActionResult Register()
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
                return RedirectToAction("Index", "ToDo");
            }
            ViewBag.LoginError = "Login is already taken";
            return View();
        }
        public ActionResult Edit()
        {
            var userId = HttpContext.Session.GetString("userId");
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
        public ActionResult Edit(string firstname, string lastname, string nickname)
        {
            var userId = HttpContext.Session.GetString("userId");
            var user = _usersService.GetUser(userId);
            user.FirstName = firstname;
            user.LastName = lastname;
            user.Nickname = nickname;
			_usersService.Edit(user);
            return RedirectToAction(nameof(Index),"ToDo");
        }
        public ActionResult Delete(string id)
        {
			_usersService.Delete(id);
            return RedirectToAction(nameof(Index));
		}
    }
}
