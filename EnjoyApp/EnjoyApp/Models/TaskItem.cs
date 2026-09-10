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


        private TimeOnly startTime;

        public TimeOnly StartTime
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

        private TimeOnly endTime;

        public TimeOnly EndTime
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
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private Guid id;

        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        private Collection<SubTaskItem> subTasks = new();

        public Collection<SubTaskItem> SubTasks
        {
            get { return subTasks; }
            set { subTasks = value; }
        }

        public string TimeRange => $"{StartTime:HH\\:mm}-{EndTime:HH\\:mm}";
        public event PropertyChangedEventHandler? PropertyChanged;

    }
}
