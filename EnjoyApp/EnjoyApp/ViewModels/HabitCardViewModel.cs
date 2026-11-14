using EnjoyApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EnjoyApp.ViewModels
{
    public class HabitCardViewModel : INotifyPropertyChanged
    {

        private readonly HabitItem habit;
        private readonly DateOnly date;

        public Guid HabitId => habit.Id;
        public HabitCardViewModel(HabitItem habit, DateOnly date)
        {
            this.habit = habit;
            this.date = date;
        }

        public string Name => habit.Name;

        public bool IsCompletedToday
        {
            get
            {
                return habit.IsCompletedOnDate(date);
            }
        }




        public string StatusText =>
     IsCompletedToday ? "На сегодня отмечено" : "Можно отметить";


        public void ToggleCompletion()
        {
            habit.ToggleCompletion(date);
            OnPropertyChanged(nameof(IsCompletedToday));
            OnPropertyChanged(nameof(StatusText));
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



