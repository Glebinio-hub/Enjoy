using EnjoyApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EnjoyApp.ViewModels
{
    public class TodayViewModel
    {
        public ICommand AddTaskCommand { get;}

        public string UserName { get; } = "Глеб";

        public string Greeting { get; } = "Good morning";

        public ObservableCollection<TaskItem> Tasks { get;} = new();

        public TodayViewModel()
        {
            AddTaskCommand = new RelayCommand(AddTask);

            Tasks.Add(new TaskItem
                {
                Name = "Lunch",
                Description = "Get lunch",
                Id = Guid.NewGuid(),
                StartTime = new TimeOnly(12, 0),
                EndTime = new TimeOnly(13, 0),
                IsCompleted = false 
            });

            Tasks.Add(new TaskItem
            {
                Name = "Meeting",
                Description = "Meet with team",
                Id = Guid.NewGuid(),
                StartTime = new TimeOnly(14, 0),
                EndTime = new TimeOnly(15, 0),
                IsCompleted = false
            });

            Tasks.Add(new TaskItem
            {
                Name = "Workout",
                Description = "Go to the gym",
                Id = Guid.NewGuid(),
                StartTime = new TimeOnly(18, 0),
                EndTime = new TimeOnly(19, 0),
                IsCompleted = false
            });

        }
        private void AddTask()
        {
            Tasks.Add(new TaskItem
            {
                Name = "",
                Id = Guid.NewGuid(),
                Description = "",
                StartTime = new TimeOnly(0, 0),
                EndTime = new TimeOnly(0, 0),
                IsCompleted = false,
            });
        }
    }
}
