using System;
using System.Collections.Generic;
using System.Text;

namespace EnjoyApp.Models
{
    public class HabitWeekDay
    {
        public int DayNumber { get; set; }

        public bool IsCompleted { get; set; }

        public string Mark => IsCompleted ? "✔" : "-";

        public DateOnly Date { get; set; }
    }
}
