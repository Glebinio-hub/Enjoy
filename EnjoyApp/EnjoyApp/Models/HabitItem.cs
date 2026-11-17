using Avalonia.Controls.Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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


        public Guid Id { get; set; } = Guid.NewGuid();


        public DateOnly CreatedAt { get; set; } =
     DateOnly.FromDateTime(DateTime.Today);


        private readonly HashSet<DateOnly> completedDates = new();





        public event PropertyChangedEventHandler PropertyChanged;


        public bool IsCompletedOnDate(DateOnly date)
        {
            return completedDates.Contains(date);
        }

        public void ToggleCompletion(DateOnly date)
        {
            if ( completedDates.Contains(date))
            {
                completedDates.Remove(date);    
            }
            else
            {
                completedDates.Add(date);
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName)
            );
        }

        public int GetCompletedDaysCount(DateOnly endDate, int days)
        {
            if (days <= 0)
                return 0;

            return completedDates.Count(date =>
                date >= endDate.AddDays(-days + 1) &&
                date <= endDate);
        }

        public List<DateOnly> GetCompletedDates()
        {
            return completedDates.OrderBy(d => d).ToList();
        }

        public void RestoreCompletedDates(
            IEnumerable<DateOnly> dates)
        {
            completedDates.Clear();

            foreach (var date in dates)
            {
                completedDates.Add(date);
            }
        }
    }
}