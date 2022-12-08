namespace To_Do
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TaskManager taskmanager = new TaskManager();
            //            Ui ui = new Ui(taskmanager);
            Display display = new Display();
            TaskList? selectedList = null;
            Task? selectedTask  = null;
            Subtask? selectedSubtask = null;
            int itemCount = 0;
            int state = (int)State.Lists;
            bool running = true;
            display.AppName = "To-Do";
            
            while (running)
            {
                switch (state)
                {
                    case (int)State.Lists:
                        List<TaskList> lists = taskmanager.GetLists();
                        itemCount = lists.Count;
                        if (itemCount == 0)
                        {
                            selectedList = null;
                        }
                        else if (!lists.Contains(selectedList))
                        {
                            selectedList = lists[0];
                        }
                        display.Context = "Tasklists";
                        display.Help = "Keys: q: quit, a: add list, x: delete list, <Enter>: view list, j/k: Change selection";
                        display.DrawScreen();
                        display.RenderList(lists, selectedList);
                        break;

                    case (int)State.Tasks:
                        List<Task> tasks = selectedList.GetTaskList();
                        itemCount = tasks.Count;
                        if (itemCount == 0)
                        {
                            selectedTask = null;
                        }
                        else if (!tasks.Contains(selectedTask))
                        {
                            selectedTask = tasks[0];
                        }
                        display.Context = $"Tasks in {selectedList.Title}";
                        display.Help = "Keys: a: add task, x: delete task, t: edit list title, <Enter>: view list, j/k: Change selection, b: go back";
                        display.DrawScreen();
                        display.RenderList(tasks, selectedTask);
                        break;

                    case (int)State.Taskview:
                        List<Subtask> subtasks = selectedTask.GetSubtaskList();
                        itemCount = subtasks.Count;
                        if (itemCount == 0)
                        {
                            selectedSubtask = null;
                        }
                        else if (!subtasks.Contains(selectedSubtask))
                        {
                            selectedSubtask = subtasks[0];
                        }
                        display.Context = $"Viewing task \"{selectedTask.Title}\"";
                        display.Help = "Keys: t: edit task title, c: toggle task complete, p: change priority, b: go back\n" +
                                        "a: add new subtask, x: delete subtask, C: toggle subtask complete";
                        display.DrawScreen();
                        display.RenderTaskview(selectedTask, selectedSubtask);
                        break;
                }

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

                    case 'a':
                        string input = "";
                        if (state == (int)State.Lists)
                        {
                            display.ShowMessage("Enter name of new list:");
                            input = Console.ReadLine();
                            if (!String.IsNullOrEmpty(input))
                            {
                                taskmanager.AddTaskList(input);
                            }
                        }
                        else if (state == (int)State.Tasks)
                        {
                            display.ShowMessage("Enter name of new task:");
                            input = Console.ReadLine();
                            if (!String.IsNullOrEmpty(input))
                            {
                                taskmanager.AddTask(input, selectedList);
                            }
                        }
                        else if (state == (int)State.Taskview)
                        {
                            display.ShowMessage("Enter name of new subtask:");
                            input = Console.ReadLine();
                            if (!String.IsNullOrEmpty(input))
                            {
                                taskmanager.AddSubtask(input, selectedList, selectedTask);
                            }
                        }
                        break;

                    case 'x':
                        if (itemCount > 0)
                        {
                            if (state == (int)State.Lists)
                            {
                                display.ShowMessage("Delete selected list? (y/n)");
                                if (GetConfirmation())
                                {
                                    taskmanager.DeleteTaskList(selectedList);
                                }
                            }
                            else if (state == (int)State.Tasks)
                            {
                                display.ShowMessage("Delete selected task? (y/n)");
                                if (GetConfirmation())
                                {
                                    taskmanager.DeleteTask(selectedList, selectedTask);
                                }
                            }
                            else if (state == (int)State.Taskview)
                            {
                                display.ShowMessage("Delete selected subtask? (y/n)");
                                if (GetConfirmation())
                                {
                                    taskmanager.DeleteSubtask(selectedList, selectedTask, selectedSubtask);
                                }
                            }
                        }
                        break;

                    case (char)13: //enter
                        if (state < 4 && itemCount > 0)
                        {
                            state <<= 1;
                        }
                        break;

                    case 'k':
                        if (itemCount > 0)
                        {
                            if (state == (int)State.Lists)
                            {
                                selectedList = (selectedList == 0) ? itemCount - 1 : selectedList - 1;
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
        public enum State
        {
            Lists = 1,
            Tasks = 2,
            Taskview = 4,
        }

        public static bool GetConfirmation()
        {
            Console.CursorVisible = false;
            while (true)
            {
                char choice = (Console.ReadKey(true).KeyChar);
                if (choice == 'y')
                {
                    return true;
                }
                else if (choice == 'n')
                {
                    return false;
                }
            }
        }
    }
}