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
namespace EnjoyApp.ViewModels
{
    public class TodayViewModel : INotifyPropertyChanged
    {
        public ICommand AddTaskCommand { get; }

        public string UserName { get; } = "Глеб";

        public string Greeting { get; } = "Good morning,";

        public TimeOnly WakeUpTime { get; } = new TimeOnly(8, 0);
        public TimeOnly BedTime { get; } = new TimeOnly(0, 0);
        TimeSpan dayLength;



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

                if(elapsed >= dayLength)
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

        public TaskItem CurrentTask { get; } = new();
        private readonly DispatcherTimer timer;

        public ObservableCollection<TaskItem> Tasks { get; } = new();

        public TodayViewModel()
        {
            AddTaskCommand = new RelayCommand(AddTask);

            Tasks.Add(new TaskItem
            {
                Name = "Lunch",
                Description = "Get lunch",
                Id = Guid.NewGuid(),
                StartTime = new TimeOnly(12, 0),
                EndTime = new TimeOnly(13, 0),
                IsCompleted = false
            });

            Tasks.Add(new TaskItem
            {
                Name = "Meeting",
                Description = "Meet with team",
                Id = Guid.NewGuid(),
                StartTime = new TimeOnly(14, 0),
                EndTime = new TimeOnly(15, 0),
                IsCompleted = false
            });

            Tasks.Add(new TaskItem
            {
                Name = "Workout",
                Description = "Go to the gym",
                Id = Guid.NewGuid(),
                StartTime = new TimeOnly(18, 0),
                EndTime = new TimeOnly(19, 0),
                IsCompleted = false
            });

            CurrentTask = Tasks[1];


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
            };
            timer.Start();


        }
        private void AddTask()
        {
            Tasks.Add(new TaskItem
            {
                Name = "New Task",
                Id = Guid.NewGuid(),
                Description = "",
                StartTime = new TimeOnly(0, 0),
                EndTime = new TimeOnly(0, 0),
                IsCompleted = false,
            });
        }



        public event PropertyChangedEventHandler? PropertyChanged;

    }
}
