using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using EnjoyApp.Models;
using Avalonia.Metadata;

namespace EnjoyApp.Services
{


    public class TaskService
    {
        public ObservableCollection<TaskItem> Tasks { get; private set; }
        private TaskStorage taskStorage;
        public TaskService(TaskStorage taskStorage)
        {
            this.taskStorage = taskStorage;
            Tasks = taskStorage.LoadTasks();
        }

        public void AddTask(TaskItem task)
        {
            Tasks.Add(task);
            taskStorage.SaveTasks(Tasks);
        }

        public bool DeleteTask(TaskItem task)
        {
            bool isRemoved = Tasks.Remove(task);
            if (isRemoved)
            {
                taskStorage.SaveTasks(Tasks);
            }
            return isRemoved;
        }

        public void CompleteTask(TaskItem task)
        {
            task.IsCompleted = true;
            taskStorage.SaveTasks(Tasks);
        }
    }
}
