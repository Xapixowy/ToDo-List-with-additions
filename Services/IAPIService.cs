using ToDo_List_with_additions.Models;

namespace ToDo_List_with_additions.Services
{
    public interface IAPIService
    {
        public List<UserModel> GetUsers();
        public UserModel GetUser(string id, string login);
        public UserModel CheckLogin(string login);
        public UserModel UserCreate(UserModel user);
        public UserModel UserEdit(UserModel user);
        public UserModel UserDelete(string id);
    }
}
