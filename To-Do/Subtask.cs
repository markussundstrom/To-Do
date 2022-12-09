namespace To_Do
{
    public class Subtask
    {
        public string Title { get; set; } = "";
        public bool Completed { get; set; }

        public event EventHandler CompletedChanged;

        public Subtask(string title)
        {
            Title = title;
            Completed = false;
        }

        public void ToggleCompleted()
        {
            Completed = !Completed;
            CompletedChanged(this, null);
        }
    }
}
