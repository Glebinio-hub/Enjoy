using EnjoyApp.Models;
using EnjoyApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace EnjoyApp.ViewModels
{
    public class MeViewModel
    {

        private readonly HabitService habitService;

        public MeViewModel(HabitService habitService)
        {
            this.habitService = habitService;

            foreach (var habit in habitService.Habits)
            {
                HabitCards.Add(new HabitCardViewModel(habit, DateOnly.FromDateTime(DateTime.Today)));
            }
            habitService.Habits.CollectionChanged += OnHabitsChanged;
        }

        public ObservableCollection<HabitItem> Habits => habitService.Habits;
        public ObservableCollection<HabitCardViewModel> HabitCards
        { get; } = new();


        private void OnHabitsChanged(
    object? sender,
    NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null)
                    {
                        foreach (var item in e.NewItems)
                        {
                            if (item is HabitItem habit)
                            {
                                HabitCards.Add(
                                    new HabitCardViewModel(
                                        habit,
                                        DateOnly.FromDateTime(DateTime.Today)
                                    )
                                );
                            }
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null)
                    {
                        foreach (var item in e.OldItems)
                        {
                            if (item is HabitItem habit)
                            {
                                var card = HabitCards.FirstOrDefault(
                                    c => c.HabitId == habit.Id
                                );

                                if (card != null)
                                {
                                    HabitCards.Remove(card);
                                }
                            }
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    HabitCards.Clear();
                    break;
            }
        }
    }
}
