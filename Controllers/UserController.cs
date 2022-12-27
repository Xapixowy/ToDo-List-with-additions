using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization.Attributes;
using ToDo_List_with_additions.Models;
using ToDo_List_with_additions.Services;

namespace ToDo_List_with_additions.Controllers
{
    public class UserController : Controller
    {
        private readonly UsersService usersService;
        public UserController(UsersService usersService)
        {
            this.usersService = usersService;
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

            var userFromDb = usersService.Login(user.Login, user.Password);
            if (userFromDb != null)
            {
                HttpContext.Session.SetString("userId", userFromDb.Id);
                return RedirectToAction("Index", "ToDo");
            }
            return RedirectToAction(nameof(Login));
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
            usersService.Register(user);
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Edit(string id)
        {
            var user = usersService.GetUser(id);
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
        public ActionResult Edit(string id, string login, string password, string firstname, string lastname, string nickname)
        {
            var user = usersService.GetUser(id);
            user.Login = login;
            user.Password = password;
            user.FirstName = firstname;
            user.LastName = lastname;
            user.Nickname = nickname;
            usersService.Edit(user);
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Delete(string id)
        {
            usersService.Delete(id);
            return RedirectToAction(nameof(Index));
		}
    }
}
