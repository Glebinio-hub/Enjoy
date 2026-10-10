using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EnjoyApp.Models
{
    public class SubTaskItem : INotifyPropertyChanged
    {
        private Guid id;

        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

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


        public event PropertyChangedEventHandler? PropertyChanged;
        public string TimeRange => $"{StartTime:HH\\:mm}-{EndTime:HH\\:mm}";

    }
}
