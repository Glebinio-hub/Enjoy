using Avalonia.Metadata;
using EnjoyApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
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
            Tasks.Add(task);
            taskStorage.SaveTasks(Tasks);
        }


        public void EditTask(TaskItem task)
        {
            var existingTask = Tasks.FirstOrDefault(item => item.Id == task.Id);

            if (existingTask == null)
                return;

            existingTask.Name = task.Name;
            existingTask.Description = task.Description;
            existingTask.StartTime = task.StartTime;
            existingTask.EndTime = task.EndTime;
            existingTask.SubTasks = task.SubTasks;

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
        }
    }
}
