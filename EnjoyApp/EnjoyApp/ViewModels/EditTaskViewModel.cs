using Avalonia.Animation;
using EnjoyApp.Models;
using EnjoyApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
namespace EnjoyApp.ViewModels
{
    public class EditTaskViewModel : INotifyPropertyChanged
    {
        public TaskItem Task { get; }
        public TaskService TaskService { get; }
        public ICommand RemoveSubTaskCommand { get; }
        private string name = "";
        public ObservableCollection<SubTaskItem> SubTasks { get; } = new();

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




        public EditTaskViewModel(TaskItem task, TaskService taskService)
        {
            Task = task;
            TaskService = taskService;
            Name = task.Name;
            Description = task.Description;
            StartTime = task.StartTime;
            EndTime = task.EndTime;

            foreach (var subTask in task.SubTasks)
            {
                SubTasks.Add(subTask);
            }

            RemoveSubTaskCommand = new RelayCommand(
    parameter => RemoveSubTask((SubTaskItem)parameter));


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

        private void RemoveSubTask(SubTaskItem subTask)
        {
            SubTasks.Remove(subTask);
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
