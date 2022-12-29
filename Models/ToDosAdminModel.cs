namespace ToDo_List_with_additions.Models
{
    public class ToDosAdminModel
    {
        public string UserName { get; set; }
        public string UserNickname { get; set; }
        public List<ToDoModel> ToDos {get; set;}
    }
}
