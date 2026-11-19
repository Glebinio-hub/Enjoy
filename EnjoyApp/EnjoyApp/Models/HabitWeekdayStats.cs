using System;
using System.Collections.Generic;
using System.Text;

namespace EnjoyApp.Models
{
    public class HabitWeekdayStats
    {
        public DayOfWeek DayOfWeek { get; set; }

        public int CompletedCount { get; set; }

        public int AvailableCount { get; set; }

        public double CompletionPercentage => AvailableCount == 0 ? 0 : (double)CompletedCount / AvailableCount * 100;

        public bool HasData => AvailableCount > 0;
        public string ShortDayName =>
    DayOfWeek.ToString()[..3];



    }
}
