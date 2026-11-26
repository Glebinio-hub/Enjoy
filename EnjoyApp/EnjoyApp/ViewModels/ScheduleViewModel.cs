
using EnjoyApp.Models;
using EnjoyApp.Services;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace EnjoyApp.ViewModels
{
    public class ScheduleViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public DateOnly NewTaskDate { get; private set; }
        private readonly TaskService taskService;

        private DateTimeOffset? _selectedDate = DateTimeOffset.Now;
        private ScheduleDateItem? _selectedDateItem;
        private string newTaskName = string.Empty;

        public string NewTaskName
        {
            get { return newTaskName; }
            set
            {
                if (newTaskName == value)
                    return;

                newTaskName = value;
                OnPropertyChanged(nameof(NewTaskName));
            }
        }

        private bool isRegeneratingDates;

        public ObservableCollection<ScheduleDateItem> Dates { get; }
            = new();

        public ObservableCollection<TaskItem> TasksForSelectedDate { get; }
            = new();
        public bool IsCreateTaskOpen { get; set; }

        private bool isCalendarOpen;
        private DateTime calendarMonth = new(
            DateTime.Today.Year,
            DateTime.Today.Month,
            1);
        public int CompletedTaskCount => TasksForSelectedDate.Count(x => x.IsCompleted == true);

        public string CompletedTasksText
        {
            get
            {
                int planned = TasksForSelectedDate.Count;
                int completed = CompletedTaskCount;

                if (planned != 0)
                {
                    return $"{completed} / {planned} done";
                }
                else
                {
                    return "0 tasks";
                }

            }
        }
        public bool IsCalendarOpen
        {
            get => isCalendarOpen;
            private set
            {
                if (isCalendarOpen == value)
                    return;

                isCalendarOpen = value;
                OnPropertyChanged(nameof(IsCalendarOpen));
            }
        }

        public string CalendarMonthTitle =>
            calendarMonth.ToString(
                "MMMM yyyy",
                CultureInfo.GetCultureInfo("en-US"));

        public ObservableCollection<ScheduleCalendarDay>
            CalendarDays
        { get; } = new();

        public void OpenCalendar()
        {
            var date = SelectedDate?.Date ?? DateTime.Today;

            calendarMonth = new DateTime(
                date.Year, date.Month, 1);

            GenerateCalendarDays();
            IsCalendarOpen = true;
            OnPropertyChanged(nameof(CalendarMonthTitle));
        }

        public void CloseCalendar()
        {
            IsCalendarOpen = false;
        }

        public void ChangeCalendarMonth(int offset)
        {
            calendarMonth = calendarMonth.AddMonths(offset);

            OnPropertyChanged(nameof(CalendarMonthTitle));
            GenerateCalendarDays();
        }

        public void SelectCalendarDate(DateTime date)
        {
            SelectedDate = new DateTimeOffset(date);
            IsCalendarOpen = false;
        }

        private void GenerateCalendarDays()
        {
            CalendarDays.Clear();

            int offset =
                ((int)calendarMonth.DayOfWeek + 6) % 7;

            for (int i = 0; i < offset; i++)
            {
                CalendarDays.Add(new ScheduleCalendarDay());
            }

            int count = DateTime.DaysInMonth(
                calendarMonth.Year,
                calendarMonth.Month);

            for (int day = 1; day <= count; day++)
            {
                var date = new DateTime(
                    calendarMonth.Year,
                    calendarMonth.Month,
                    day);

                CalendarDays.Add(new ScheduleCalendarDay
                {
                    Date = date,
                    IsSelected = SelectedDate?.Date == date,
                    IsToday = date == DateTime.Today
                });
            }
        }

        public DateTimeOffset? SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate == value)
                    return;

                _selectedDate = value;

                OnPropertyChanged(nameof(SelectedDate));
                OnPropertyChanged(nameof(PickerDate));
                OnPropertyChanged(nameof(SelectedMonthLabel));
                OnPropertyChanged(nameof(SelectedDateHeading));
                OnPropertyChanged(nameof(IsToday));
                var matchingDate = value.HasValue
                    ? Dates.FirstOrDefault(
                        date => date.Date.Date == value.Value.Date)
                    : null;

                if (value.HasValue && matchingDate is null)
                {
                    GenerateDates(value.Value);
                }
                else
                {
                    _selectedDateItem = matchingDate;
                    OnPropertyChanged(nameof(SelectedDateItem));

                    UpdateDateSelection();
                }

                UpdateTasksForSelectedDate();
            }
        }


        public DateTime? PickerDate
        {
            get => SelectedDate?.Date;
            set
            {
                if (!value.HasValue)
                    return;

                if (SelectedDate?.Date == value.Value.Date)
                    return;

                SelectedDate = new DateTimeOffset(value.Value.Date);
            }
        }

        public bool IsToday =>
            SelectedDate?.Date == DateTime.Today;

        public ScheduleDateItem? SelectedDateItem
        {
            get => _selectedDateItem;
            set
            {
                if (ReferenceEquals(_selectedDateItem, value))
                    return;


                if (isRegeneratingDates)
                    return;

                if (value is null)
                    return;

                _selectedDateItem = value;

                OnPropertyChanged(nameof(SelectedDateItem));

                if (!SelectedDate.HasValue ||
                    SelectedDate.Value.Date != value.Date.Date)
                {
                    SelectedDate = value.Date;
                }
                else
                {
                    UpdateDateSelection();
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
            GenerateDates(DateTimeOffset.Now);
            GenerateCalendarDays();

            UpdateTasksForSelectedDate();

            taskService.Tasks.CollectionChanged +=
                OnTasksCollectionChanged;
        }


        private void GenerateDates(DateTimeOffset centerDate)
        {
            isRegeneratingDates = true;

            try
            {
                Dates.Clear();

                for (int offset = -30; offset <= 60; offset++)
                {
                    Dates.Add(
                        new ScheduleDateItem(
                            centerDate.AddDays(offset)));
                }

                _selectedDateItem = Dates.FirstOrDefault(
                    date => SelectedDate.HasValue &&
                            date.Date.Date == SelectedDate.Value.Date);

                OnPropertyChanged(nameof(SelectedDateItem));

                UpdateDateSelection();
            }
            finally
            {
                isRegeneratingDates = false;
            }
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
            foreach (var task in TasksForSelectedDate)
            {
                task.PropertyChanged -= OnTaskPropertyChanged;
            }

            TasksForSelectedDate.Clear();

            if (SelectedDate.HasValue)
            {
                DateOnly selectedDate = DateOnly.FromDateTime(
                    SelectedDate.Value.DateTime);

                var tasks = taskService.Tasks
                    .Where(task => task.Date == selectedDate)
                    .OrderBy(task => task.StartTime == null)
                    .ThenBy(task => task.StartTime)
                    .ThenBy(task => task.Name); ;

                foreach (var task in tasks)
                {
                    TasksForSelectedDate.Add(task);
                    task.PropertyChanged += OnTaskPropertyChanged;
                }
            }

            OnPropertyChanged(nameof(HasTasks));
            OnPropertyChanged(nameof(HasNoTasks));
            OnPropertyChanged(nameof(CompletedTaskCount));
            OnPropertyChanged(nameof(CompletedTasksText));
        }

        private void OnTasksCollectionChanged(
            object? sender,
            NotifyCollectionChangedEventArgs e)
        {
            UpdateTasksForSelectedDate();
        }

        public void OpenCreateTask()
        {
            NewTaskName = string.Empty;
            if (SelectedDate.HasValue)
            {
                NewTaskDate = DateOnly.FromDateTime(
                    SelectedDate.Value.DateTime);
            }
            else
            {
                NewTaskDate = DateOnly.FromDateTime(
                    DateTime.Today);
            }
            IsCreateTaskOpen = true;
            OnPropertyChanged(nameof(NewTaskDate));
            OnPropertyChanged(nameof(IsCreateTaskOpen));

        }

        public void CancelCreateTask()
        {
            if (!IsCreateTaskOpen)
                return;

            IsCreateTaskOpen = false;
            OnPropertyChanged(nameof(IsCreateTaskOpen));
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));

        }

        private void OnTaskPropertyChanged(
    object? sender,
    PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TaskItem.IsCompleted))
            {
                OnPropertyChanged(nameof(CompletedTaskCount));
                OnPropertyChanged(nameof(CompletedTasksText));
            }
        }
    }
}
