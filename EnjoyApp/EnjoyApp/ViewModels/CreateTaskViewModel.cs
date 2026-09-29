using EnjoyApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace EnjoyApp.ViewModels
{
    public class CreateTaskViewModel
    {
        public ICommand AddTaskCommand { get; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public EventHandler<TaskItem>? TaskCreated;
        public ObservableCollection<TimeOnly> AvailableTimes { get; } = new();

        public CreateTaskViewModel()
        {
            for (int hour = 0; hour < 24; hour++)
            {
                AvailableTimes.Add(new TimeOnly(hour, 0));
                AvailableTimes.Add(new TimeOnly(hour, 30));
            }
            AddTaskCommand = new RelayCommand(AddTask);
        }

        public void AddTask()
        {
            TaskItem task = new TaskItem
            {
                Id = Guid.NewGuid(),
                Name = Name,
                Description = Description,
                StartTime = StartTime,
                EndTime = EndTime,
                IsCompleted = false

            };
            TaskCreated?.Invoke(this, task);
        }
    }
}
