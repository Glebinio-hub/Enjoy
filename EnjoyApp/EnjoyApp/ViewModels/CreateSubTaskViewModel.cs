using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace EnjoyApp.ViewModels
{
    public class CreateSubTaskViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<TimeOnly> AvailableTimes { get; } = new();
        private string name = "";
        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Name))
                    );
                }
            }
        }

        private TimeOnly? startTime;

        public TimeOnly? StartTime
        {
            get => startTime;
            set
            {
                if (startTime != value)
                {
                    startTime = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(StartTime))
                        );
                }
            }
        }

        private TimeOnly? endTime;

        public TimeOnly? EndTime
        {
            get => endTime;
            set
            {
                if (endTime != value)
                {
                    endTime = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(EndTime))
                        );
                }
            }
        }

        public CreateSubTaskViewModel()
        {
            for (int hour = 0; hour < 24; hour++)
            {
                AvailableTimes.Add(new TimeOnly(hour, 0));
                AvailableTimes.Add(new TimeOnly(hour, 30));
            }
        }






        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
