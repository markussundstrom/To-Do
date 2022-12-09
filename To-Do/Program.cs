namespace To_Do
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TaskManager taskmanager = new TaskManager();
            Ui ui = new Ui("To-Do", taskmanager);
            TaskList? selectedList = null;
            Task? selectedTask  = null;
            Subtask? selectedSubtask = null;
            int itemCount = 0;
            int state = (int)State.Lists;
            bool running = true;
            display.AppName = "To-Do";

            ui.Run();

            while (running)
            {

                switch (Console.ReadKey(true).KeyChar)
                {



                    case 't':
                        if (itemCount <= 0)
                        {
                            break;
                        }
                        string editing = "";
                        if (state == (int)State.Tasks)
                        {
                            editing = "list";
                        }
                        else if (state == (int)State.Taskview)
                        {
                            editing = "task";
                        }
                        else
                        {
                            break;
                        }
                        break;

                }
                taskmanager.ShutdownTaskManager();
            }
        }
    }
}