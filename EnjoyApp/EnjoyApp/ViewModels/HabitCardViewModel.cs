using EnjoyApp.Models;
using EnjoyApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnjoyApp.ViewModels
{
    public class HabitCardViewModel : INotifyPropertyChanged
    {

        private readonly HabitItem habit;
        private readonly DateOnly date;
        private readonly HabitService habitService;
        public Guid HabitId => habit.Id;


        public int CompletedLast7Days =>
    habit.GetCompletedDaysCount(
        DateOnly.FromDateTime(DateTime.Today),
        7);
        public string WeeklyProgressText =>
    $"{CompletedLast7Days} / 7 days";


        public string Name => habit.Name;

        public bool IsCompletedToday
        {
            get
            {
                return habit.IsCompletedOnDate(date);
            }
        }

        public List<HabitWeekDay> WeekDays =>
    Enumerable.Range(0, 7)
        .Select(offset =>
        {
            var date = DateOnly.FromDateTime(DateTime.Today)
                .AddDays(-6 + offset);

            return new HabitWeekDay
            {
                DayNumber = date.Day,
                IsCompleted = habit.IsCompletedOnDate(date),
                Date = date,
            };
        })
        .ToList();

        public string WeekRangeText
        {
            get
            {
                var end = DateOnly.FromDateTime(DateTime.Today);
                var start = end.AddDays(-6);

                var culture = new System.Globalization.CultureInfo("ru-RU");

                if (start.Month == end.Month && start.Year == end.Year)
                    return $"{start.Day}–{end.ToString("d MMMM", culture)}";

                return $"{start.ToString("d MMM", culture)} – {end.ToString("d MMM", culture)}";
            }
        }

        public HabitCardViewModel(HabitItem habit, DateOnly date, HabitService habitService)
        {
            this.habit = habit;
            this.date = date;
            this.habitService = habitService;
        }


        public string StatusText =>
     IsCompletedToday ? "На сегодня отмечено" : "Можно отметить";


        public async Task ToggleCompletionAsync()
        {
            await habitService.ToggleHabitCompletion(HabitId, date);
            OnPropertyChanged(nameof(IsCompletedToday));
            OnPropertyChanged(nameof(StatusText));
            OnPropertyChanged(nameof(CompletedLast7Days));
            OnPropertyChanged(nameof(WeeklyProgressText));
            OnPropertyChanged(nameof(WeekDays));
            OnPropertyChanged(nameof(WeekRangeText));
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName)
            );
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}



