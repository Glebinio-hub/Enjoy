using EnjoyApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Globalization;
using System.ComponentModel;
using Avalonia.Threading;
using System.Threading;
using Avalonia.Platform;
using EnjoyApp.Services;
namespace EnjoyApp.ViewModels
   
{
    public class TodayViewModel : INotifyPropertyChanged
    {

        private readonly TaskService taskService;
        public string UserName { get; } = "Глеб";

        public string Greeting { get; } = "Good morning,";

        public TimeOnly WakeUpTime { get; } = new TimeOnly(8, 0);
        public TimeOnly BedTime { get; } = new TimeOnly(0, 0);

        public bool HasCurrentTask => CurrentTask != null;

        static DateTime date = DateTime.Now;

        public string Date { get; } = date.ToString("dddd, d MMMM", new CultureInfo("en-US"));

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
                foreach (TaskItem item in Tasks)
                {
                    if (item.StartTime <= now && item.EndTime >= now)
                    {
                        return item;
                    }
                }

                return null;
            }
        }

        private readonly DispatcherTimer timer;

        public ObservableCollection<TaskItem> Tasks => taskService.Tasks;

        public TodayViewModel(TaskService taskService)
        {

            this.taskService = taskService;

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

        public TodayViewModel()
    : this(new TaskService())
        {
        }

        public void AddTask(TaskItem task)
        {
            taskService.Tasks.Add(task);

            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(nameof(CurrentTask))
                );
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(nameof(HasCurrentTask))
                );
        }



        public event PropertyChangedEventHandler? PropertyChanged;

    }
}
