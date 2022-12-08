namespace To_Do
{
    public class TaskList
    {
        public string Title { get; set; }
        public List<Task> Tasks { get; set; }
        public DateTime Created { get; private set; }

        public TaskList(string title)
        {
            Title = title;
            Tasks = new List<Task>();
            Created = DateTime.Now;
        }

        public List<Task> GetTaskList()
        {
            List<Task> taskListCopy = Tasks.ToList();
            return taskListCopy;
        }
        
        public void AddTask(string name)
        {
            Tasks.Add(new Task(name));
        }

        public void DeleteTask(Task task)
        {
            Tasks.Remove(task);
        }
    }
}