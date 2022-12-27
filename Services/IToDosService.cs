using ToDo_List_with_additions.Models;

namespace ToDo_List_with_additions.Services
{
    public interface IToDosService
    {
        public List<ToDoModel> Get(string userId);
        public ToDoModel GetToDo(string id);
        public ToDoModel Create(ToDoModel toDo);
        public ToDoModel Edit(ToDoModel toDo);
        public ToDoModel Delete(string id);
    }
}
