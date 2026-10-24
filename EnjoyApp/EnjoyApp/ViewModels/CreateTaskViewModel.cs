using EnjoyApp.Models;
using EnjoyApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace EnjoyApp.ViewModels
{
    public class CreateTaskViewModel:INotifyPropertyChanged
    {
        public ICommand AddTaskCommand { get; }
        public ICommand RemoveSubTaskCommand { get; }
        private string name = "";
        public ObservableCollection<SubTaskItem> SubTasks { get; } = new();
        private TaskStorage taskStorage = new();

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

        private bool isTimePickerOpen;

        public bool IsTimePickerOpen
        {
            get => isTimePickerOpen;
            set
            {
                if (isTimePickerOpen == value) return;

                isTimePickerOpen = value;
                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs(nameof(IsTimePickerOpen)));
            }
        }

        private TimeOnly pickerTime = new(9, 0);

        public TimeOnly PickerTime
        {
            get => pickerTime;
            set
            {
                if (pickerTime == value) return;

                pickerTime = value;
                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs(nameof(PickerTime)));
            }
        }

        private bool isEditingStartTime;
        public event EventHandler<TaskItem>? TaskCreated;

        public CreateTaskViewModel()
        {
           
            AddTaskCommand = new RelayCommand(AddTask);
            RemoveSubTaskCommand = new RelayCommand(
    parameter => RemoveSubTask((SubTaskItem)parameter)
);
        }

        public void AddTask()
        {
            if (StartTime.HasValue && EndTime.HasValue && EndTime.Value <= StartTime.Value)
            {
                ValidationMessage = "End time must be after start time";
                return;
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                ValidationMessage = "Enter a task name";
                return;
            }
            ValidationMessage = "";
            TaskItem task = new TaskItem
            {
                Id = Guid.NewGuid(),
                Name = Name,
                Description = Description,
                StartTime = StartTime,
                EndTime = EndTime,
                IsCompleted = false,
                SubTasks = new ObservableCollection<SubTaskItem>(SubTasks.ToList())

            };
            TaskCreated?.Invoke(this, task);
        }

        private void RemoveSubTask(SubTaskItem subTask)
        {
            SubTasks.Remove(subTask);
        }
        public void Reset()
        {
            Name = "";
            Description = "";
            StartTime = null;
            EndTime = null;
            SubTasks.Clear();
        }

        public void OpenTimePicker(bool forStartTime)
        {
            isEditingStartTime = forStartTime;

            PickerTime = forStartTime
                ? StartTime ?? new TimeOnly(9, 0)
                : EndTime ?? new TimeOnly(10, 0);

            IsTimePickerOpen = true;
        }

        public void ConfirmTimePicker()
        {
            if (isEditingStartTime)
                StartTime = PickerTime;
            else
                EndTime = PickerTime;

            IsTimePickerOpen = false;
        }

        public void CancelTimePicker()
        {
            IsTimePickerOpen = false;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
