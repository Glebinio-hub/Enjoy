using EnjoyApp.Models;
using EnjoyApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace EnjoyApp.ViewModels
{
    public class HabitCardViewModel : INotifyPropertyChanged
    {

        private readonly HabitItem habit;
        private readonly DateOnly date;
        private readonly HabitService habitService;
        public Guid HabitId => habit.Id;
        public HabitCardViewModel(HabitItem habit, DateOnly date, HabitService habitService)
        {
            this.habit = habit;
            this.date = date;
            this.habitService = habitService;
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


        public async Task ToggleCompletionAsync()
        {
            await habitService.ToggleHabitCompletion(HabitId, date);
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



