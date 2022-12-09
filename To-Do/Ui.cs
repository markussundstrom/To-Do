namespace To_Do
{
    public class Ui
    {
        public enum State
        {
            Overview = 1,
            Listview = 2,
            Taskview = 4,
        }

        private TaskManager _taskman;
        private Display _display = new Display();
        private int _state;
        private bool _running;
        private TaskList? _selectedList = null;
        private Task? _selectedTask = null;
        private Subtask? _selectedSubtask = null;
        private List<dynamic> _currentEnum = new List<dynamic>;

        public Ui(string appname, TaskManager taskmanager)
        {
            _taskman = taskmanager;
            _display.AppName = appname;
        }

        public void Run()
        {
            _state = (int)State.Overview;
            _running = true;
            while (_running)
            {
                switch (_state)
                {
                    case (int)State.Overview:
                        Overview();
                        OverviewGetInput();
                        break;

                    case (int)State.Listview:
                        Listview();
                        ListviewGetInput();
                        break;

                    case (int)State.Taskview:
                        Taskview();
                        TaskviewGetInput();
                        break;
                }
            }
        }

        private void Overview()
        {
            _currentEnum.Clear();
            _currentEnum.AddRange(_taskman.GetLists());
            if (_currentEnum.Count == 0)
            {
                _selectedList = null;
            }
            else if (!_currentEnum.Contains(_selectedList))
            {
                _selectedList = _currentEnum[0];
            }
            _display.Context = "Tasklists";
            _display.Help = "Keys: q: quit, a: add list, x: delete list, <Enter>: view list, j/k: Change selection";
            _display.DrawScreen();
            _display.RenderList(_currentEnum, _selectedList);
        }

        private void Listview()
        {
            List<Task> tasks = _selectedList.GetTaskList();
            if (tasks.Count == 0)
            {
                _selectedTask = null;
            }
            else if (!tasks.Contains(_selectedTask))
            {
                _selectedTask = tasks[0];
            }
            _display.Context = $"Tasks in {_selectedList.Title}";
            _display.Help = "Keys: a: add task, x: delete task, t: edit list title, <Enter>: view list, j/k: Change selection, b: go back";
            _display.DrawScreen();
            _display.RenderList(tasks, _selectedTask);
        }

        private void Taskview()
        {
            List<Subtask> subtasks = _selectedTask.GetSubtaskList();
            if (subtasks.Count == 0)
            {
                _selectedSubtask = null;
            }
            else if (!subtasks.Contains(_selectedSubtask))
            {
                _selectedSubtask = subtasks[0];
            }
            _display.Context = $"Viewing task \"{_selectedTask.Title}\"";
            _display.Help = "Keys: t: edit task title, c: toggle task complete, p: change priority, b: go back\n" +
                            "a: add new subtask, x: delete subtask, C: toggle subtask complete";
            _display.DrawScreen();
            _display.RenderTaskview(_selectedTask, _selectedSubtask);
        }

        private void OverviewGetInput()
        {
            switch (Console.ReadKey(true).KeyChar)
            {
                case 'q':
                    _display.ShowMessage("Do you want to quit? (y/n)");
                    if (GetConfirmation())
                    {
                        _running = false;
                    }
                    break;

                case 'a':
                    _display.ShowMessage("Enter name of new list:");
                    string input = Console.ReadLine();
                    if (!String.IsNullOrEmpty(input))
                    {
                        _taskman.AddTaskList(input);
                    }
                    break;
                case 'x':
                    if (_selectedList != null)
                    {
                        _display.ShowMessage("Delete selected list? (y/n)");
                        if (GetConfirmation())
                        {
                            _taskman.DeleteTaskList(_selectedList);
                        }
                    }
                    break;
                case (char)13:
                    if (_selectedList != null)
                    {
                        _state <<= 1;
                    }
                    break;
                case 'k':
                    MoveSelection(ref _selectedList, -1);
                    break;
                case 'j':
                    MoveSelection(ref _selectedList, 1);
                    break;
            }
        }

        private void ListviewGetInput()
        {
            switch (Console.ReadKey(true).KeyChar)
            {
                case 'a':
                    _display.ShowMessage("Enter name of new task:");
                    string input = Console.ReadLine();
                    if (!String.IsNullOrEmpty(input))
                    {
                        _selectedList.AddTask(input);
                    }
                    break;
                case 'x':
                    if (_selectedTask != null)
                    {
                        _display.ShowMessage("Delete selected task? (y/n)");
                        if (GetConfirmation())
                        {
                            _selectedList.DeleteTask(_selectedTask);
                        }
                    }
                    break;

                case 't':
                    if (_selectedList != null)
                    {
                        EditTitle(_selectedList);
                    }
                    break;

                case (char)13:
                    if (_selectedTask != null)
                    {
                        _state <<= 1;
                    }
                    break;

                case 'k':
                    MoveSelection(ref _selectedTask, -1);
                    break;

                case 'j':
                    MoveSelection(ref _selectedTask, 1);
                    break;
                case 'b':
                    _state >>= 1;
                    break;
            }
        }

        private void TaskviewGetInput()
        {
            switch (Console.ReadKey(true).KeyChar)
            {
                case 'a':
                    _display.ShowMessage("Enter name of new subtask:");
                    string input = Console.ReadLine();
                    if (!String.IsNullOrEmpty(input))
                    {
                        _selectedTask.AddSubtask(input);
                    }
                    break;
                case 'x':
                    if (_selectedSubtask != null)
                    {
                        display.ShowMessage("Delete selected subtask? (y/n)");
                        if (GetConfirmation())
                        {
                            _selectedTask.DeleteSubtask(_selectedSubtask);
                        }
                    }
                    break;

                case 'c':
                    try
                    {
                        _selectedTask.ToggleCompleted();
                    }
                    catch
                    {
                        _display.ShowMessage("Unable to set task as completed, does it have subtasks? Press enter to continue.");
                        Console.ReadLine();
                    }
                    break;

                case 'C':
                    if (_selectedSubtask != null)
                    {
                        _selectedSubtask.ToggleCompleted();
                    }
                    break;

                case 'k':
                    MoveSelection(ref _selectedSubtask, -1);
                    break;

                case 'j':
                    MoveSelection(ref _selectedSubtask, 1);
                    break;
                case 'b':
                    _state >>= 1;
                    break;

                case 'p':
                    _display.ShowMessage("Enter new priority (1-3), leave empty to cancel:");
                    while (true)
                    {
                        string prioInput = Console.ReadLine();
                        if (String.IsNullOrEmpty(prioInput))
                        {
                        break;
                        }
                        try
                        {
                            _selectedTask.SetPriority(Int32.Parse(prioInput));
                            break;
                        }
                        catch
                        {
                            _display.ShowMessage("Priority needs to be within range 1-3");
                        }
                    }
                    break;
            }
        }

        private bool GetConfirmation()
        {
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

        private void MoveSelection<T>(ref T selection, int direction)
        {
            if (selection == null)
            {
                return;
            }
            int index = _currentEnum.IndexOf(selection);
            index += direction;
            index = ((index % _currentEnum.Count) + _currentEnum.Count) % _currentEnum.Count;
            selection = _currentEnum[index];
        }

        private void EditTitle<T>(T selected)
        {
            _display.ShowMessage($"Enter new title of item. Leave empty to cancel edit");
            string titleInput = Console.ReadLine();
            if (!String.IsNullOrEmpty(titleInput))
            {
                _display.ShowMessage($"Change title of item to {titleInput}? (y/n)");
                if (GetConfirmation())
                {
                    selected.SetTitle()
                    {
                        taskmanager.SetTaskTitle(selectedList, selectedTask, titleInput);
                    }
                }
            }
        }
    }
}