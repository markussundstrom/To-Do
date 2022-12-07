namespace To_Do
{
    public class TaskList
    {
        public string Title { get; set; }
        public List<Task> Tasks { get; set; }

        public TaskList()
        {
        }

        public TaskList(string title, List<Task> tasks)
        {
            Title = title;
            Tasks = tasks;
        }
    }
}