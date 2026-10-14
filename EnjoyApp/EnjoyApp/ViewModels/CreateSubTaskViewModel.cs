using EnjoyApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;

namespace EnjoyApp.ViewModels
{
    public class CreateSubTaskViewModel : INotifyPropertyChanged
    {
        public ICommand AddSubTaskCommand { get; }
        public event EventHandler<SubTaskItem>? SubTaskCreated;
        public ObservableCollection<TimeOnly> AvailableTimes { get; } = new();
        private string name = "";
        public string Name
        {
            get => name;
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

        private TimeOnly? startTime;

        public TimeOnly? StartTime
        {
            get => startTime;
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
            get => endTime;
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

        public CreateSubTaskViewModel()
        {
            for (int hour = 0; hour < 24; hour++)
            {
                AvailableTimes.Add(new TimeOnly(hour, 0));
                AvailableTimes.Add(new TimeOnly(hour, 30));
            }
            AddSubTaskCommand = new RelayCommand(AddSubTask);
        }

        public void AddSubTask()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                ValidationMessage = "Enter a task name";
                return;
            }
            if(StartTime.HasValue && EndTime.HasValue && EndTime.Value <= StartTime.Value)
            {
                ValidationMessage = "End time must be after start time";
                return;
            }
            SubTaskItem subTask = new SubTaskItem
            {
                Id = Guid.NewGuid(),
                Name = Name,
                StartTime = StartTime,
                EndTime = EndTime,
                IsCompleted = false

            };
            SubTaskCreated?.Invoke(this, subTask);
        }

        public void Reset()
        {
            Name = "";
            StartTime = null;
            EndTime = null;
        }





        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
