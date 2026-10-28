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
            CancelTimePicker();
            Name = "";
            StartTime = null;
            EndTime = null;
            ValidationMessage = string.Empty;
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
