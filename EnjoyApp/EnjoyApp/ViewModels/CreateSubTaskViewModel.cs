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
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(EndTime))
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






        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
