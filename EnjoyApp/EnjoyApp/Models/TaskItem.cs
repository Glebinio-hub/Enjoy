
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace EnjoyApp.Models
{
    public class TaskItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private bool isCompleted;

        public bool IsCompleted
        {
            get { return isCompleted; }
            set
            {
                if (isCompleted != value)
                {
                    isCompleted = value;
                    OnPropertyChanged(nameof(IsCompleted));
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
                    OnPropertyChanged(nameof(Date));
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

                    OnPropertyChanged(nameof(StartTime));
                    OnPropertyChanged(nameof(HasTime));
                    OnPropertyChanged(nameof(TimeRangeText));
                    OnPropertyChanged(nameof(TimeRange));
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

                    OnPropertyChanged(nameof(EndTime));
                    OnPropertyChanged(nameof(HasTime));
                    OnPropertyChanged(nameof(TimeRangeText));
                    OnPropertyChanged(nameof(TimeRange));
                }
            }
        }

        [JsonIgnore]
        public bool HasTime => StartTime.HasValue || EndTime.HasValue;

        [JsonIgnore]
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

        [JsonIgnore]
        public string TimeRange => TimeRangeText;

        private string name = string.Empty;

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        private string description = string.Empty;

        public string Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged(nameof(Description));
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

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName)
            );
        }
    }
}
