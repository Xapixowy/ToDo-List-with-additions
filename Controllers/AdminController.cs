using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization.Attributes;
using ToDo_List_with_additions.Models;
using ToDo_List_with_additions.Services;

namespace ToDo_List_with_additions.Controllers
{
    public class AdminController : Controller
    {
        private readonly UsersService usersService;
        public AdminController(UsersService usersService)
        {
            this.usersService = usersService;
        }
        public ActionResult Index()
        {
            var model = new UsersModel()
            {
                Users = usersService.Get()
            };
            return View(model);
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
