namespace To_Do
{
    public class FileOps
    {
        private string _file;

        public FileOps()
        {
            _file = $"{Environment.CurrentDirectory}\\todo.json";
        }

        public string GetTaskFileContent()
        {
            if (File.Exists(_file))
            {
                using (StreamReader r = new StreamReader(_file))
                {
                    return r.ReadToEnd();
                }
            }
            else
            {
                return string.Empty;
            }
        }
        public void PutTaskFileContent(string jsonstring)
        {
            File.WriteAllText(_file, jsonstring);
        }
    }
}