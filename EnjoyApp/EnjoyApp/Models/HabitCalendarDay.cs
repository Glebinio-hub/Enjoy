using System;
using System.Collections.Generic;
using System.Text;

namespace EnjoyApp.Models
{
    public class HabitCalendarDay
    {
        public DateOnly? Date { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsPlaceholder => Date is null;

        public int? DayNumber => Date?.Day;

        public bool IsToday => Date == DateOnly.FromDateTime(DateTime.Today);

        public bool IsUnavailable { get; set; }
    }
}
