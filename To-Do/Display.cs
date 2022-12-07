using System.Diagnostics;

namespace To_Do
{
    public class Display
    {
        public string AppName { get; set; } = "";
        public string Context { get; set; } = "";
        public string Help { get; set; } = "";



        public void DrawScreen() 
        {

            Console.Clear();
            Console.CursorVisible = false;
            string title = $"{AppName}: {Context}";
            for (int i = 0; i < (Console.BufferWidth / 2) - (title.Length / 2); i++)
            {
                Console.CursorLeft = i;
                Console.CursorTop = 0;
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write(" ");
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(title);

            for (int i = (Console.BufferWidth / 2) + (title.Length / 2); i < Console.BufferWidth; i++)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write(" ");
            }

            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;

            Console.WriteLine(Help.PadRight(Console.WindowWidth, ' '));
            Console.ResetColor();
            Console.CursorTop = 3;
            Console.CursorLeft = 0;
        }

        public void RenderList(List<TaskList> lists, TaskList selected) 
        {
            Console.ResetColor();
            foreach (var item in lists)
            {
                RenderItem(item, item == selected);
            }
        }

        private void RenderItem(TaskList list, bool selected)
        {
            Console.ResetColor();
            if (selected)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
            }
            Console.WriteLine($"{list.Created}\t{list.Title}\t({list.Tasks.Count})");
            Console.ResetColor();
        }

        public void RenderTasks(TaskList list, int selected)
        {
            Console.ResetColor();
            for (int i = 0; i < list.Tasks.Count; i++)
            {
                var priocolor = ConsoleColor.White;
                if (list.Tasks[i].Completed)
                {
                    priocolor = ConsoleColor.Green;
                }
                else
                {
                    switch (list.Tasks[i].Priority)
                    {
                        case 1:
                            priocolor = ConsoleColor.Red;
                            break;
                        case 2:
                            priocolor = ConsoleColor.DarkYellow;
                            break;
                        case 3:
                            priocolor = ConsoleColor.Yellow;
                            break;
                    }
                }
                Console.ForegroundColor = priocolor;
                Console.Write((list.Tasks[i].Completed) ? "[X]" : "[-]");
                Console.ResetColor();
                if (selected == i)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                Console.WriteLine(list.Tasks[i].Title);
                Console.ResetColor();
            }
        }

        public void RenderTaskview(Task task, int selected)
        {
            Console.WriteLine(task.Title);
            Console.Write("Priority ");
            var priocolor = ConsoleColor.White;
            switch (task.Priority)
            {
                case 1:
                    priocolor = ConsoleColor.Red;
                    break;
                case 2:
                    priocolor = ConsoleColor.DarkYellow;
                    break;
                case 3:
                    priocolor = ConsoleColor.Yellow;
                    break;
            }
            Console.ForegroundColor = priocolor;
            Console.WriteLine(task.Priority);
            Console.ForegroundColor = (task.Completed) ? ConsoleColor.Green : priocolor;
            Console.WriteLine(task.Completed ? "Complete" : "Not Complete");
            Console.ResetColor();
            if (task.Subtasks.Count > 0)
            {
                Console.WriteLine("Subtasks:");
                for (int i = 0; i < task.Subtasks.Count; i++)
                {
                    if (i == selected)
                    {
                        Console.BackgroundColor = (task.Subtasks[i].Completed) ? ConsoleColor.DarkGray : ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.ForegroundColor = (task.Subtasks[i].Completed) ? ConsoleColor.DarkGray : ConsoleColor.White;
                    }
                    Console.Write((task.Subtasks[i].Completed) ? "[X]\t" : "[-]\t");
                    Console.WriteLine(task.Subtasks[i].Title);
                    Console.ResetColor();
                }
            }
        }

        public void ShowMessage(string text)
        {
            Console.CursorLeft = 0;
            Console.CursorTop = Console.WindowHeight - 8;
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(text.PadRight(Console.WindowWidth, ' '));
            Console.ResetColor();
            Console.CursorVisible = true;
        }
    }
}