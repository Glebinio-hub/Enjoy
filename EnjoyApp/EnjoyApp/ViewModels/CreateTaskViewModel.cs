using EnjoyApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace EnjoyApp.ViewModels
{
    public class CreateTaskViewModel:INotifyPropertyChanged
    {
        public ICommand AddTaskCommand { get; }
        private string name = "";

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Name))
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
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(EndTime))
                        );
                }
            }
        }

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

        public void Reset()
        {
            Name = "";
            Description = "";
            StartTime = null;
            EndTime = null;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
