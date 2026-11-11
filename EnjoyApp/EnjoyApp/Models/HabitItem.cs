using Avalonia.Controls.Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EnjoyApp.Models
{
    public class HabitItem : INotifyPropertyChanged
    {

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


        private DateOnly createdAt;

        public DateOnly CreatedAt
        {
            get { return createdAt; }
            set { createdAt = value; }
        }


        private HashSet<DateOnly> CompletedDates { get; set; } = new HashSet<DateOnly>();





        public event PropertyChangedEventHandler PropertyChanged;


        public bool IsCompletedOnDate(DateOnly date)
        {
            return CompletedDates.Contains(date);
        }

        public void ToggleCompletion(DateOnly date)
        {
            if ( CompletedDates.Contains(date))
            {
                CompletedDates.Remove(date);
            }
            else
            {
                CompletedDates.Add(date);
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName)
            );
        }
    }
}