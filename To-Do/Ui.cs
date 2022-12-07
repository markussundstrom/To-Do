namespace To_Do
{
    public class Ui
    {
        private TaskManager _taskman;

        public Ui(TaskManager taskmanager)
        {
            _taskman = taskmanager;
        }

        public void ListOverview()
        {
            _taskman.GetLists();
        }

        private void RenderListItem(TaskList tasklist)
        {
            Console.WriteLine($"{tasklist.Created}\t{tasklist.Title}\t({tasklist.Tasks.Count})");
        }

        private void RenderListItem(Task task)
        {
    }
}