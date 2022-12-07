namespace To_Do
{
    public class Subtask
    {
        public string Title { get; set; } = "";
        public bool Completed { get; set; }

        public Subtask(string title)
        {
            Title = title;
            Completed = false;
        }
    }
}
