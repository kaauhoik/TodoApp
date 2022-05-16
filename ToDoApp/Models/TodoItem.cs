namespace ToDoApp.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
     
        public string Description { get; set; } = string.Empty;

        public bool IsDone { get; set; }
    }
}
