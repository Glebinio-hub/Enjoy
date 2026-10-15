using Avalonia.Animation;
using EnjoyApp.Models;
using EnjoyApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
namespace EnjoyApp.ViewModels
{
    public class EditTaskViewModel : INotifyPropertyChanged
    {
        public TaskItem Task { get; }
        public TaskService TaskService { get; }

        private string name = "";
        public ObservableCollection<SubTaskItem> SubTasks { get; } = new();
        public ObservableCollection<TimeOnly> AvailableTimes { get; } = new();

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    ValidationMessage = "";
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Name))
                        );
                }
            }
        }

        private string validationMessage = "";

        public string ValidationMessage
        {
            get => validationMessage;
            set
            {
                if (validationMessage != value)
                {
                    validationMessage = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(ValidationMessage))
                    );
                }
            }
        }

        private string description = "";

        public string Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    description = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Description))
                        );
                }
            }
        }

        private TimeOnly? startTime;

        public TimeOnly? StartTime
        {
            get { return startTime; }
            set
            {
                if (startTime != value)
                {
                    startTime = value;
                    ValidationMessage = "";
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(StartTime))
                        );
                }
            }
        }

        private TimeOnly? endTime;

        public TimeOnly? EndTime
        {
            get { return endTime; }
            set
            {
                if (endTime != value)
                {
                    endTime = value;
                    ValidationMessage = "";
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(EndTime))
                        );
                }
            }
        }




        public EditTaskViewModel(TaskItem task, TaskService taskService)
        {
            Task = task;
            TaskService = taskService;
            Name = task.Name;
            Description = task.Description;
            StartTime = task.StartTime;
            EndTime = task.EndTime;

            for (int hour = 0; hour < 24; hour++)
            {
                AvailableTimes.Add(new TimeOnly(hour, 0));
                AvailableTimes.Add(new TimeOnly(hour, 30));
            }

            foreach (var subTask in task.SubTasks)
            {
                SubTasks.Add(subTask);
            }
        }

        public void EditTask()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                ValidationMessage = "Enter a task name";
                return;
            }

            if (StartTime.HasValue && EndTime.HasValue &&
                EndTime.Value <= StartTime.Value)
            {
                ValidationMessage = "End time must be after start time";
                return;
            }

            Task.Name = Name;
            Task.Description = Description;
            Task.StartTime = StartTime;
            Task.EndTime = EndTime;


            Task.SubTasks.Clear();

            foreach (var subTask in SubTasks)
            {
                Task.SubTasks.Add(subTask);
            }

            TaskService.EditTask(Task);
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
