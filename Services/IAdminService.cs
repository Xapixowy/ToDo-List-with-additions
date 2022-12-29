using ToDo_List_with_additions.Models;

namespace ToDo_List_with_additions.Services
{
    public interface IAdminService
    {

        public List<UserModel> GetUsers();

        public List<ToDoModel> GetToDos(string userId);
        public UserModel GetUser(string login);

    }
}
