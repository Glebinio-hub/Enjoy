using EnjoyApp.Models;
using EnjoyApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace EnjoyApp.ViewModels
{
    public class MeViewModel
    {

        private readonly HabitService habitService;

        public MeViewModel(HabitService habitService)
        {
            this.habitService = habitService;
        }

        public ObservableCollection<HabitItem> Habits => habitService.Habits;
    }
}
