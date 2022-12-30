using ToDo_List_with_additions.Models;

namespace ToDo_List_with_additions.Services
{
    public interface IUsersService
    {
        public List<UserModel> Get();
        public UserModel GetUser(string id);
        public UserModel Register(UserModel user);
        public UserModel Edit(UserModel user);
        public UserModel Delete(string id);
        public UserModel Login(string login, string password);
        public UserModel CheckLogin(string login);
    }
}
