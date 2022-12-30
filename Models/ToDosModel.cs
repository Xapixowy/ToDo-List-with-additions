namespace ToDo_List_with_additions.Models
{
    public class ToDosModel
    {
        public string UserName { get; set; }
        public string UserNickname { get; set; }
        public List<ToDoModel> ToDosToday { get; set; }
        public List<ToDoModel> ToDosOthers { get; set; }
        public List<ToDoModel> ToDosDone { get; set; }
    }
}
