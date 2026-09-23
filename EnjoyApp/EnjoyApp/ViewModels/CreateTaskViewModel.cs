using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace EnjoyApp.ViewModels
{
    public class CreateTaskViewModel
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public ObservableCollection<TimeOnly> AvailableTimes { get; } = new();

        public CreateTaskViewModel()
        {
            for (int hour = 0; hour < 24; hour++)
            {
                AvailableTimes.Add(new TimeOnly(hour, 0));
                AvailableTimes.Add(new TimeOnly(hour, 30));
            }
        }
    }
}
