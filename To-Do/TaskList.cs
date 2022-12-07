namespace To_Do
{
    public class TaskList
    {
        public string Title { get; set; }
        public List<Task> Tasks { get; set; }
        public DateTime Created { get, private set; }

        public TaskList()
        {

        }

        public TaskList(string title, List<Task> tasks)
        {
            Title = title;
            Tasks = tasks;
            Created = DateTime.Now;
        }

        public void RemoveTask(Task task)
        {
            Tasks.Remove(task);
        }
    }
}