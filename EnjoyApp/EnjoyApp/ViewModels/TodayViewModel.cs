using Avalonia.Platform;
using Avalonia.Threading;
using EnjoyApp.Models;
using EnjoyApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
namespace EnjoyApp.ViewModels

{
    public class TodayViewModel : INotifyPropertyChanged
    {

        private readonly TaskService taskService;
        public string UserName { get; } = "Gleb";
        private TaskItem? displayedTask;
        public string Greeting { get; } = "Good morning,";

        public TimeOnly WakeUpTime { get; } = new TimeOnly(8, 0);
        public TimeOnly BedTime { get; } = new TimeOnly(0, 0);

        public bool HasCurrentTask => CurrentTask != null;

        static DateTime date = DateTime.Now;

        public string Date { get; } = date.ToString("dddd, d MMMM", new CultureInfo("en-US"));

        public TaskItem? DisplayedTask
        {
            get => displayedTask;
            private set
            {
                if (ReferenceEquals(displayedTask, value))
                    return;

                displayedTask = value;

                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs(nameof(DisplayedTask)));
            }
        }

        public double TasksProgress
        {
            get
            {
                var (planned, completed) = taskService.GetDailyTaskSummary(DateOnly.FromDateTime(DateTime.Today));

                if (planned == 0)
                {
                    return 0;
                }

                return (double)completed / planned * 220;
            }
        }

        public string RemainingTasksText
        {
            get
            {
                var (planned, completed) = taskService.GetDailyTaskSummary(DateOnly.FromDateTime(DateTime.Today));
                int remaining = planned - completed;

                if (planned == 0)
                {
                    return "Твой день — чистый лист ✨";
                }

                if (completed == planned)
                {
                    return "На сегодня всё! ✓\n Ты разобрался с запланированным. Можно выдохнуть.";
                }

                return remaining switch
                {
                    1 => "Осталась 1 задача",
                    2 or 3 or 4 => $"Осталось {remaining} задачи",
                    _ => $"Осталось {remaining} задач"
                };
            }
        }

        public int DayProgress
        {
            get
            {
                TimeSpan now = DateTime.Now.TimeOfDay;

                TimeSpan dayLength;

                if (BedTime <= WakeUpTime)
                {
                    dayLength =
                        (TimeSpan.FromDays(1) - WakeUpTime.ToTimeSpan())
                        + BedTime.ToTimeSpan();
                }
                else
                {
                    dayLength =
                        BedTime.ToTimeSpan()
                        - WakeUpTime.ToTimeSpan();
                }

                TimeSpan elapsed;

                if (now >= WakeUpTime.ToTimeSpan())
                {
                    elapsed = now - WakeUpTime.ToTimeSpan();
                }
                else
                {
                    elapsed =
                        (TimeSpan.FromDays(1) - WakeUpTime.ToTimeSpan())
                        + now;
                }

                if (elapsed >= dayLength)
                {
                    return 100;
                }

                return (int)(
                    elapsed.TotalMinutes
                    / dayLength.TotalMinutes
                    * 100
                );
            }
        }


        public string Quote { get; } = "The best way to get started is to quit talking and begin doing.";

        public TaskItem? CurrentTask
        {
            get
            {
                TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);

                return (Tasks.Where(task => (task.Date == DateOnly.FromDateTime(DateTime.Today)) && task.StartTime <= now && task.EndTime >= now && !task.IsCompleted)).OrderBy(task => task.EndTime).FirstOrDefault();
            }
        }

        private readonly DispatcherTimer timer;

        public ObservableCollection<TaskItem> Tasks => taskService.Tasks;

        public string CompletedTasksText
        {
            get
            {
                var (planned, completed) = taskService.GetDailyTaskSummary(DateOnly.FromDateTime(DateTime.Today));

                if (planned != 0)
                {
                    return $"Выполнено: {completed} из {planned} задач";
                }
                else
                {
                    return "У вас еще нет активных задач на сегодня!";
                }

            }
        }

        public TodayViewModel(TaskService taskService)
        {

            this.taskService = taskService;
            DisplayedTask = CurrentTask;
            foreach (var task in taskService.Tasks)
            {
                task.PropertyChanged += OnTaskPropertyChanged;
            }
            taskService.Tasks.CollectionChanged += OnTasksCollectionChanged;

            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMinutes(1)
            };

            timer.Tick += (_, _) =>
            {
                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs(nameof(DayProgress))
                    );
                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs(nameof(CurrentTask))
                    );
                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs(nameof(HasCurrentTask))
                    );
            };
            timer.Start();


        }



        public void AddTask(TaskItem task)
        {
            taskService.AddTask(task);

            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(nameof(CurrentTask))
                );
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(nameof(HasCurrentTask))
                );
            PropertyChanged?.Invoke(
               this,
               new PropertyChangedEventArgs(nameof(CompletedTasksText))
               );
        }

        public void DeleteTask(TaskItem task)
        {
            taskService.DeleteTask(task);

            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(nameof(CurrentTask))
                );
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(nameof(HasCurrentTask))
                );
            PropertyChanged?.Invoke(
               this,
               new PropertyChangedEventArgs(nameof(CompletedTasksText))
               );
            PropertyChanged?.Invoke(
               this,
               new PropertyChangedEventArgs(nameof(TasksProgress))
);

            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(nameof(RemainingTasksText))
            );

        }

        public void CompleteTask(TaskItem task)
        {
            taskService.SetCompleted(task, true);
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(nameof(CurrentTask))
                );

            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(nameof(HasCurrentTask))
                );
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(nameof(CompletedTasksText))
                );

            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(nameof(TasksProgress))
                );

            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(nameof(RemainingTasksText))
            );

        }

        public void RefreshTasks()
        {

            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(nameof(CurrentTask))
                );
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(nameof(HasCurrentTask))
                );
            PropertyChanged?.Invoke(
               this,
               new PropertyChangedEventArgs(nameof(CompletedTasksText))
               );
            PropertyChanged?.Invoke(
                this,
                 new PropertyChangedEventArgs(nameof(TasksProgress))
                );

            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(nameof(RemainingTasksText))
            );
        }
        private void OnTaskPropertyChanged(
    object? sender,
    PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TaskItem.IsCompleted))
            {
                RefreshTasks();
            }
        }

        private void OnTasksCollectionChanged(
    object? sender,
    NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (TaskItem newTask in e.NewItems)
                {
                    newTask.PropertyChanged += OnTaskPropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (TaskItem oldTask in e.OldItems)
                {
                    oldTask.PropertyChanged -= OnTaskPropertyChanged;
                }
            }

            RefreshTasks();
        }

        public void UpdateDisplayedTask()
        {
            DisplayedTask = CurrentTask;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

    }
}
