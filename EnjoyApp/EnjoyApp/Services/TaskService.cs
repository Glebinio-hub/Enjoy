using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using EnjoyApp.Models;

namespace EnjoyApp.Services
{


    public class TaskService
    {
        public ObservableCollection<TaskItem> Tasks { get; } = new();

        public void AddTask(TaskItem task)
        {
            Tasks.Add(task);
        }
    }
}
