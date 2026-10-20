
using EnjoyApp.Models;
using EnjoyApp.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace EnjoyApp.ViewModels
{
    public class ScheduleViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly TaskService taskService;

        private DateTimeOffset? _selectedDate = DateTimeOffset.Now;

        public DateTimeOffset? SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate == value)
                    return;

                _selectedDate = value;

                OnPropertyChanged(nameof(SelectedDate));

                UpdateTasksForSelectedDate();
            }
        }

        public ObservableCollection<TaskItem> TasksForSelectedDate { get; }
            = new();

        public ScheduleViewModel(TaskService taskService)
        {
            this.taskService = taskService;

            UpdateTasksForSelectedDate();
        }

        private void UpdateTasksForSelectedDate()
        {
            TasksForSelectedDate.Clear();

            if (SelectedDate == null)
                return;

            DateOnly selectedDate = DateOnly.FromDateTime(
                SelectedDate.Value.DateTime);

            var tasks = taskService.Tasks
                .Where(task => task.Date == selectedDate)
                .OrderBy(task => task.StartTime);

            foreach (var task in tasks)
            {
                TasksForSelectedDate.Add(task);
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }
    }
}
