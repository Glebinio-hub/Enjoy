
using EnjoyApp.Models;
using EnjoyApp.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace EnjoyApp.ViewModels
{
    public class ScheduleViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly TaskService taskService;

        private DateTimeOffset? _selectedDate = DateTimeOffset.Now;
        private ScheduleDateItem? _selectedDateItem;

        public ObservableCollection<ScheduleDateItem> Dates { get; }
            = new();

        public ObservableCollection<TaskItem> TasksForSelectedDate { get; }
            = new();

        public DateTimeOffset? SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate == value)
                    return;

                _selectedDate = value;

                OnPropertyChanged(nameof(SelectedDate));
                OnPropertyChanged(nameof(SelectedMonthLabel));
                OnPropertyChanged(nameof(SelectedDateHeading));

                var matchingDate = value.HasValue
                    ? Dates.FirstOrDefault(
                        date => date.Date.Date == value.Value.Date)
                    : null;

                if (!ReferenceEquals(_selectedDateItem, matchingDate))
                {
                    _selectedDateItem = matchingDate;
                    OnPropertyChanged(nameof(SelectedDateItem));
                }

                UpdateDateSelection();
                UpdateTasksForSelectedDate();
            }
        }

        public ScheduleDateItem? SelectedDateItem
        {
            get => _selectedDateItem;
            set
            {
                if (ReferenceEquals(_selectedDateItem, value))
                    return;

                _selectedDateItem = value;

                OnPropertyChanged(nameof(SelectedDateItem));

                if (value is null)
                {
                    if (SelectedDate is not null)
                        SelectedDate = null;

                    return;
                }

                if (!SelectedDate.HasValue ||
                    SelectedDate.Value.Date != value.Date.Date)
                {
                    SelectedDate = value.Date;
                }
            }
        }

        public string SelectedMonthLabel =>
            (SelectedDate ?? DateTimeOffset.Now).ToString(
                "MMMM yyyy",
                CultureInfo.GetCultureInfo("en-US"));

        public string SelectedDateHeading =>
            (SelectedDate ?? DateTimeOffset.Now).ToString(
                "dddd, d MMMM",
                CultureInfo.GetCultureInfo("en-US"));

        public bool HasTasks => TasksForSelectedDate.Count > 0;

        public bool HasNoTasks => !HasTasks;

        public ScheduleViewModel(TaskService taskService)
        {
            this.taskService = taskService;

            DateTimeOffset today = new(DateTime.Today);

            for (int offset = -30; offset <= 60; offset++)
            {
                Dates.Add(
                    new ScheduleDateItem(today.AddDays(offset)));
            }

            _selectedDateItem = Dates.FirstOrDefault(
                date => date.Date.Date == DateTime.Today);

            UpdateDateSelection();
            UpdateTasksForSelectedDate();
        }

        private void UpdateDateSelection()
        {
            foreach (var date in Dates)
            {
                date.IsSelected =
                    SelectedDate.HasValue &&
                    date.Date.Date == SelectedDate.Value.Date;
            }
        }

        private void UpdateTasksForSelectedDate()
        {
            TasksForSelectedDate.Clear();

            if (SelectedDate.HasValue)
            {
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

            OnPropertyChanged(nameof(HasTasks));
            OnPropertyChanged(nameof(HasNoTasks));
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }
    }
}
