using EnjoyApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace EnjoyApp.Services
{
    public class HabitService
    {
        public ObservableCollection<HabitItem> Habits { get; } = new();

        public void AddHabit(HabitItem habit)
        {
            if (Habits.Any(h => h.Id == habit.Id))
            {
                return;
            }
            else
            {
                Habits.Add(habit);
            }
        }

        public bool RemoveHabit(Guid id)
        {
            var habit = GetHabit(id);

            if (habit == null)
                return false;

            return Habits.Remove(habit);
        }

        public HabitItem? GetHabit(Guid id)
        {
            return Habits.FirstOrDefault(h => h.Id == id);
        }
    }
}
