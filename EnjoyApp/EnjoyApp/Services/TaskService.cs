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

        public ObservableCollection<TaskItem> Tasks { get; } = new();

        public void AddTask(TaskItem task)
        {
            Tasks.Add(task);
        }

        public bool DeleteTask(TaskItem task)
        {
            bool isRemoved = Tasks.Remove(task);
            return isRemoved;
        }
    }
}
