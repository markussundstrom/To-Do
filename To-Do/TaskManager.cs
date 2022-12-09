using System.Text.Json;
using System.Text.Json.Serialization;

namespace To_Do
{
    public class TaskManager
    {
        private List<TaskList> _lists;
        private FileOps _file;

        public TaskManager()
        {
            _file = new FileOps();
            _lists = new List<TaskList>();
            string json = _file.GetTaskFileContent();
            _lists.AddRange(JsonSerializer.Deserialize<List<TaskList>>(json, 
                            new JsonSerializerOptions() {PropertyNameCaseInsensitive=true, WriteIndented=true }));
        }

        public List<TaskList> GetLists()
        {
            List<TaskList> listsCopy = _lists.ToList();
            return listsCopy;
        }

        public void AddTaskList(string name)
        {
            _lists.Add(new TaskList(name));
        }

        public void AddSubtask(string name, int listIndex, int taskIndex)
        {
            _lists[listIndex].Tasks[taskIndex].AddSubtask(name);
        }

        public void DeleteTaskList(TaskList list)
        {
            _lists.Remove(list);
        }

        public void SetListTitle(int listIndex, string newTitle)
        {
            _lists[listIndex].Title = newTitle;
        }
                
        public void SetTaskTitle(int listIndex, int taskIndex, string newTitle)
        {
            _lists[listIndex].Tasks[taskIndex].Title = newTitle;
        }

        public void ToggleSubtaskComplete(int listIndex, int taskIndex, int subtaskIndex)
        {
            _lists[listIndex].Tasks[taskIndex].ToggleSubtaskCompleted(subtaskIndex);
        }

        public void SetTaskPriority(int listIndex, int taskIndex, int priority)
        {
            _lists[listIndex].Tasks[taskIndex].SetPriority(priority);
        }

        public void ShutdownTaskManager()
        {
            string jsonstring = JsonSerializer.Serialize(_lists, new JsonSerializerOptions() { WriteIndented = true });
            _file.PutTaskFileContent(jsonstring);
        }

    }

}