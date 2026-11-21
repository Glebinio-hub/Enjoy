using System;
using System.Collections.Generic;
using System.Text;

namespace EnjoyApp.Models
{
    public class ScheduleCalendarDay
    {
        public DateTime? Date { get; set; }

        public int? DayNumber => Date?.Day;

        public bool IsPlaceholder => Date is null;

        public bool IsSelected { get; set; }

        public bool IsToday { get; set; }
    }
}
