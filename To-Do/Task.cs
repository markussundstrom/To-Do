namespace To_Do
{
    public class Task
    {
        public string Title { get; set; }
        public bool Completed { get; set; }
        public int Priority { get; set; }
        public List<Subtask> Subtasks { get; set; }

        public Task()
        {
            //Priority = 3;
            Subtasks = new List<Subtask>();
        }

        public Task(string title)
        {
            Title = title;
            Completed = false;
            Priority = 3;
            Subtasks = new List<Subtask>();
        }

        public Task(string title, bool completed, int priority) : this(title, completed)
        {
            Priority = priority;
        }

        public List<Subtask> GetSubtaskList()
        {
            List<Subtask> subtaskListCopy = Subtasks.ToList();
            return subtaskListCopy;
        }

        public void SetPriority(int pri)
        {
            if (1 > pri || pri > 3)
            {
                throw new ArgumentOutOfRangeException();
            }
            Priority = pri;
        }

        public void ToggleCompleted()
        {
            if (Subtasks.Count > 0)
            {
                throw new InvalidOperationException();
            }
            Completed = !Completed;
        }

        public void AddSubtask(string name)
        {
            Subtask st = new Subtask(name);
            st.CompletedChanged += SubtaskCompletedChanged;
            Subtasks.Add(st);
            Completed = false;
        }

        public void DeleteSubtask(Subtask subtask)
        {
            Subtasks.Remove(subtask);
        }

        private void SubtaskCompletedChanged(object sender, EventArgs e)
        {
            if (Subtasks.Any(st => !st.Completed))
            {
                Completed = false;
            }
            else
            {
                Completed = true;
            }
        }


    }
}