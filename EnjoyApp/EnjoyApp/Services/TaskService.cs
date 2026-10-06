using Avalonia.Metadata;
using EnjoyApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

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
            Debug.WriteLine($"SERVICE START: {task.StartTime}");
            Debug.WriteLine($"SERVICE END: {task.EndTime}");
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

        public void SetCompleted(TaskItem task, bool isCompleted)
        {
            task.IsCompleted = isCompleted;
            taskStorage.SaveTasks(Tasks);
        }

        public void AddSubTask(TaskItem task, SubTaskItem subTask)
        {
            task.SubTasks.Add(subTask);
            taskStorage.SaveTasks(Tasks);
        }
    }
}
