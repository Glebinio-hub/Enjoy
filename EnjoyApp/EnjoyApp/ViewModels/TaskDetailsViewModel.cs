using EnjoyApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnjoyApp.ViewModels
{
    public class TaskDetailsViewModel
    {
        public TaskItem Task { get; }
        public TaskDetailsViewModel(TaskItem task)
        {
            Task = task;
        }

        public TaskDetailsViewModel()
        {
                
        }
    }
}
