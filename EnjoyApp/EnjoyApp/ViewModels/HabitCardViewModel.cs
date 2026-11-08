using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EnjoyApp.ViewModels
{
    public class HabitCardViewModel : INotifyPropertyChanged
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
        private bool isCompletedToday;

        public bool IsCompletedToday
        {
            get { return isCompletedToday; }
            set
            {
                if (isCompletedToday != value)
                {
                    isCompletedToday = value;
                    OnPropertyChanged(nameof(IsCompletedToday));
                    OnPropertyChanged(nameof(StatusText));
                }
            }
        }


        public string StatusText =>
     IsCompletedToday ? "На сегодня отмечено" : "Можно отметить";


        public void ToggleCompletion()
        {
            IsCompletedToday = !IsCompletedToday;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName)
            );
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}



