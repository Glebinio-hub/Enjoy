using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Pipes;
using System.Text;
using System.ComponentModel;
namespace EnjoyApp.Models
{
    public class TaskItem : INotifyPropertyChanged
    {
        private bool isCompleted;

        public bool IsCompleted
        {
            get { return isCompleted; }
            set
            {
                if (isCompleted != value)
                {
                    isCompleted = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsCompleted))
                    );
                }

            }
        }


        private DateOnly date = DateOnly.FromDateTime(DateTime.Today);

        public DateOnly Date
        {
            get { return date; }
            set
            {
                if (date != value)
                {
                    date = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Date))
                    );
                }
            }
        }


        private TimeOnly? startTime;

        public TimeOnly? StartTime
        {
            get { return startTime; }
            set
            {
                if (startTime != value)
                {
                    startTime = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(StartTime))
                    );

                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(TimeRange))
                    );
                }
            }
        }

        private TimeOnly? endTime;

        public TimeOnly? EndTime
        {
            get { return endTime; }
            set
            {
                if (endTime != value)
                {
                    endTime = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(EndTime))
                    );

                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(TimeRange))
                    );

                }
            }

        }

        public bool HasTime => StartTime.HasValue || EndTime.HasValue;

        public string TimeRangeText
        {
            get
            {
                var start = StartTime?.ToString("HH:mm");
                var end = EndTime?.ToString("HH:mm");

                if (start is not null && end is not null)
                    return $"{start} – {end}";

                return start ?? end ?? string.Empty;
            }
        }
        private string name;

        public string Name
        {
            get { return name; }
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

        private string description;

        public string Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    description = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Description))
                    );
                }
            }
        }

        private Guid id;

        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }


        public ObservableCollection<SubTaskItem> SubTasks { get; set; }
     = new ObservableCollection<SubTaskItem>();

        public string TimeRange => $"{StartTime:HH\\:mm}-{EndTime:HH\\:mm}";
        public event PropertyChangedEventHandler? PropertyChanged;

    }
}
