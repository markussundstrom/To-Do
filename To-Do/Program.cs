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
                    case 'q':
                        if (state == (int)State.Lists)
                        {
                            display.ShowMessage("Do you want to quit? (y/n)");
                            if (GetConfirmation())
                            {
                                running = false;
                            }
                        }
                        break;


                    case 'k':
                        if (itemCount > 0)
                        {
                            if (state == (int)State.Lists)
                            {
                            }
                            else if (state == (int)State.Tasks)
                            {
                                selectedTask = (selectedTask == 0) ? itemCount - 1 : selectedTask - 1;
                            }
                            else if (state == (int)State.Taskview)
                            {
                                selectedSubtask = (selectedSubtask == 0) ? itemCount - 1 : selectedSubtask - 1;
                            }
                        }
                        break;

                    case 'j':
                        if (itemCount > 0)
                        {
                            if (state == (int)State.Lists)
                            {
                                selectedList = (selectedList == itemCount -1) ? 0 : selectedList + 1;
                            }
                            else if (state == (int)State.Tasks)
                            {
                                selectedTask = (selectedTask == itemCount -1) ? 0 : selectedTask + 1;
                            }
                            else if (state == (int)State.Taskview)
                            {
                                selectedSubtask = (selectedSubtask == itemCount - 1) ? 0 : selectedSubtask + 1;
                            }
                        }
                        break;

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
                        display.ShowMessage($"Enter new title of {editing}. Leave empty to cancel edit");
                        string titleInput = Console.ReadLine();
                        if (!String.IsNullOrEmpty(titleInput))
                        {
                            display.ShowMessage($"Change title of {editing} to {titleInput}? (y/n)");
                            if (GetConfirmation())
                            {
                                if (state == (int)State.Tasks)
                                {
                                    taskmanager.SetListTitle(selectedList, titleInput);
                                }
                                else if (state == (int)State.Taskview)
                                {
                                    taskmanager.SetTaskTitle(selectedList, selectedTask, titleInput);
                                }
                            }
                        }
                        break;

                    case 'c':
                        if (state == (int)State.Taskview)
                        {
                            try
                            {
                                taskmanager.ToggleTaskComplete(selectedList, selectedTask);
                            }
                            catch
                            {
                                display.ShowMessage("Unable to set task as completed, does it have subtasks? Press enter to continue.");
                                Console.ReadLine();
                            }
                        }
                        break;

                    case 'C':
                        if (state == (int)State.Taskview && selectedSubtask >= 0)
                        {
                            taskmanager.ToggleSubtaskComplete(selectedList, selectedTask, selectedSubtask);
                        }
                        break;

                    case 'p':
                        if (state == (int)State.Taskview)
                        {
                            display.ShowMessage("Enter new priority (1-3), leave empty to cancel:");
                            while (true)
                            {
                                string prioInput = Console.ReadLine();
                                if (String.IsNullOrEmpty(prioInput))
                                {
                                    break;
                                }
                                try
                                {
                                    taskmanager.SetTaskPriority(selectedList, selectedTask, Int32.Parse(prioInput));
                                    break;
                                }
                                catch
                                {
                                    display.ShowMessage("Priority needs to be within range 1-3");
                                }
                            }
                        }
                        break;

                    case 'b':
                        if (state > 1)
                        {
                            state >>= 1;
                        }
                        break;
                }
                taskmanager.ShutdownTaskManager();
            }
        }
    }
}