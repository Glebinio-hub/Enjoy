using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using EnjoyApp.Models;

namespace EnjoyApp.Services
{
    public class TaskStorage
    {
        private string PathToSFolder;
        private string AppFolder;
        private string PathToFile;

        public TaskStorage()
        {
            PathToSFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            AppFolder = Path.Combine(PathToSFolder, "EnjoyApp");
            Directory.CreateDirectory(AppFolder);
            PathToFile = Path.Combine(AppFolder, "tasks.json");
            System.Diagnostics.Debug.WriteLine(PathToFile);
        }

        public void SaveTasks(ObservableCollection<TaskItem> tasks)
        {

            System.Diagnostics.Debug.WriteLine("SAVE TASKS");
            var json = System.Text.Json.JsonSerializer.Serialize(tasks);
            File.WriteAllText(PathToFile, json);
            System.Diagnostics.Debug.WriteLine(json);
        }

        public ObservableCollection<TaskItem> LoadTasks()
        {
            if (File.Exists(PathToFile))
            {
                var json = File.ReadAllText(PathToFile);
                var tasks = System.Text.Json.JsonSerializer.Deserialize<ObservableCollection<TaskItem>>(json);
                return tasks??new ObservableCollection<TaskItem>();
            }
            return new ObservableCollection<TaskItem>();
        }
    }
}
