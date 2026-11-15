using System;
using System.Collections.Generic;
using System.Text;

namespace EnjoyApp.Models
{
    public class HabitData
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateOnly CreatedAt { get; set; }

        public List<DateOnly> CompletedDates { get; set; } = new();

    }
}
