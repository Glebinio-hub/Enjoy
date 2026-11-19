using EnjoyApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace EnjoyApp.ViewModels
{
    public class HabitDetailsViewModel : INotifyPropertyChanged
    {

        public ICommand NextMonthCommand { get; }
        public ICommand PreviousMonthCommand { get; }
        private readonly HabitItem habit;

        private DateOnly selectedMonth = new DateOnly(DateTime.Today.Year, DateTime.Today.Month, 1);

        public string HabitDescription => habit.Description;
        public List<HabitWeekdayStats> WeekdayStats => GetWeekdayStats();

        public bool HasHabitDescription =>
    !string.IsNullOrWhiteSpace(HabitDescription);

        public DateOnly SelectedMonth
        {
            get { return selectedMonth; }
            set
            {
                var firstDay = new DateOnly(value.Year, value.Month, 1);

                if (selectedMonth != firstDay)
                {
                    selectedMonth = firstDay;

                    OnPropertyChanged(nameof(SelectedMonth));
                    OnPropertyChanged(nameof(CalendarDays));
                    OnPropertyChanged(nameof(MonthTitle));
                    OnPropertyChanged(nameof(CompletedDaysInSelectedMonth));
                    OnPropertyChanged(nameof(CompletionPercentage));
                }
            }
        }
        public string MonthTitle
        {
            get
            {
                var culture = CultureInfo.CurrentUICulture;

                var title = SelectedMonth.ToString("MMMM yyyy", culture);

                return culture.TextInfo.ToTitleCase(title);

            }
        }

        public int CompletedDaysInSelectedMonth
        {
            get
            {
                var period = GetAvailablePeriod();

                if(period is null)
                    return 0;

                var (periodStart, periodEnd) = period.Value;

                int days = periodEnd.DayNumber
                         - periodStart.DayNumber + 1;

                return habit.GetCompletedDaysCount(periodEnd, days);
            }
        }

        public double CompletionPercentage
        {
            get

            {
                var period = GetAvailablePeriod();

                if (period is null)
                    return 0;

                var (periodStart, periodEnd) = period.Value;
                
                int availableDays = periodEnd.DayNumber
                          - periodStart.DayNumber + 1;

                return (double)CompletedDaysInSelectedMonth
                     / availableDays * 100;
            }
        }



        public List<HabitCalendarDay> CalendarDays =>
    GetMonthDays(SelectedMonth.Year, SelectedMonth.Month);

        public HabitDetailsViewModel(HabitItem habit)
        {
            this.habit = habit;
            NextMonthCommand = new RelayCommand(NextMonth);
            PreviousMonthCommand = new RelayCommand(PreviousMonth);
        }


        public void NextMonth()
        {
            SelectedMonth = SelectedMonth.AddMonths(1);
        }

        public void PreviousMonth()
        {
            SelectedMonth = SelectedMonth.AddMonths(-1);
        }

        private (DateOnly Start, DateOnly End)? GetAvailablePeriod()
        {
            var monthStart = SelectedMonth;
            var monthEnd = SelectedMonth.AddMonths(1).AddDays(-1);

            var today = DateOnly.FromDateTime(DateTime.Today);
            var periodStart = monthStart > habit.CreatedAt
                ? monthStart
                : habit.CreatedAt;

            var periodEnd = monthEnd < today
                ? monthEnd
                : today;

            if (periodEnd < periodStart)
                return null;

            return (periodStart, periodEnd);
        }


        public string HabitName => habit.Name;

        public List<HabitCalendarDay> GetMonthDays(int year, int month)
        {
            var firstDay = new DateOnly(year, month, 1);

            int daysInMonth = DateTime.DaysInMonth(year, month);

            int offset = ((int)firstDay.DayOfWeek + 6) % 7;

            var days = new List<HabitCalendarDay>();



            for (int i = 0; i < offset; i++)
            {
                days.Add(new HabitCalendarDay
                {
                    Date = null,
                    IsCompleted = false
                });
            }
            var today = DateOnly.FromDateTime(DateTime.Today);
            for (int day = 1; day <= daysInMonth; day++)
            {
                var date = new DateOnly(year, month, day);

                days.Add(new HabitCalendarDay
                {
                    Date = date,
                    IsCompleted = habit.IsCompletedOnDate(date),
                    IsUnavailable = date < habit.CreatedAt || date > today
                });
            }

            return days;
        }


        public List<HabitWeekdayStats> GetWeekdayStats()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var weekdayStats =
                new Dictionary<DayOfWeek, HabitWeekdayStats>();

            foreach (var day in Enum.GetValues<DayOfWeek>())
            {
                weekdayStats[day] = new HabitWeekdayStats
                {
                    DayOfWeek = day
                };
            }

            for (var date = habit.CreatedAt;
                 date <= today;
                 date = date.AddDays(1))
            {
                var weekday = date.DayOfWeek;
                var isCompleted = habit.IsCompletedOnDate(date);

                var stat = weekdayStats[weekday];

                stat.AvailableCount++;

                if (isCompleted)
                {
                    stat.CompletedCount++;
                }
            }

            return weekdayStats.Values
                .OrderBy(stat => ((int)stat.DayOfWeek + 6) % 7)
                .ToList();
        }


        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
