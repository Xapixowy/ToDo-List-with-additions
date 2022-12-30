using Microsoft.AspNetCore.Mvc;
using ToDo_List_with_additions.Models;
using ToDo_List_with_additions.Services;

namespace ToDo_List_with_additions.Controllers
{
    public class APIController : Controller
    {
        private readonly IAPIService _apiService;
        public APIController(IAPIService apiService)
        {
            _apiService = apiService;

        }
        public IActionResult GetUsers()
        {
            var users = _apiService.GetUsers();
            return Json(users);
        }
        public IActionResult GetUser(string login, string id)
        {
            try
            {
                var user = _apiService.GetUser(id, login);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                return Json(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }
        [HttpPost]
        public IActionResult CreateUser(string login, string password, string firstname, string lastname, string nickname)
        {
            try
            {
                if (login == null || password == null || firstname == null || lastname == null)
                {
                    throw new Exception("Login, password, firstname and lastname are required");
                }
                var userExists = _apiService.CheckLogin(login);
                if (userExists != null)
                {
                    throw new Exception("Login already exists");
                }
                var model = new UserModel()
                {
                    Login = login,
                    Password = password,
                    FirstName = firstname,
                    LastName = lastname,
                    Nickname = nickname
                };
                var user = _apiService.UserCreate(model);
                return Json(user);

            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }
        [HttpPut]

        public IActionResult EditUser(string id, string login, string property, string value)
        {
            try
            {
                var user = _apiService.GetUser(id, login);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                if (property == "login")
                {
                    var userExists = _apiService.CheckLogin(value);
                    if (userExists != null)
                    {
                        throw new Exception("Login already Exists");
                    }
                    user.Login = value;
                }
                else if (property == "password")
                {
                    user.Password = value;
                }
                else if (property == "firstname")
                {
                    user.FirstName = value;
                }
                else if (property == "lastname")
                {
                    user.LastName = value;
                }
                else if (property == "nickname")
                {
                    user.Nickname = value;
                }
                else
                {
                    throw new Exception("Property not found");
                }
                _apiService.UserEdit(user);

                return Json(user);


            }
            catch (Exception err)
            {
                return BadRequest(err.Message);

            }
        }
        [HttpDelete]

        public IActionResult DeleteUser(string id, string login)
        {
            try
            {
                var user = _apiService.GetUser(id, login);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                var userDelete = _apiService.UserDelete(user.Id);
                return Json(userDelete);
            }
            catch (Exception err)
            {
                return NotFound(err.Message);
            }

        }
    }
}
