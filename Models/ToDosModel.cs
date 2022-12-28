namespace ToDo_List_with_additions.Models
{
    public class ToDosModel
    {
        public List<ToDoModel> ToDosToday { get; set; }
        public List<ToDoModel> ToDosOthers { get; set; }
        public List<ToDoModel> ToDosDone { get; set; }
    }
}
