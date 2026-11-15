using EnjoyApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EnjoyApp.Services
{
    public class HabitService
    {
        private readonly SemaphoreSlim saveLock = new(1, 1);
        public ObservableCollection<HabitItem> Habits { get; } = new();

        private readonly HabitStorage habitStorage;

        public HabitService(HabitStorage habitStorage)
        {
            this.habitStorage = habitStorage;
        }

        public async Task AddHabitAsync(HabitItem habit)
        {
            if (Habits.Any(h => h.Id == habit.Id))
            {
                return;
            }
            else
            {
                Habits.Add(habit);
            }

            await SaveAsync();
        }

        public async Task<bool> RemoveHabitAsync(Guid id)
        {
            var habit = GetHabit(id);

            if (habit == null)
                return false;

            Habits.Remove(habit);

            await SaveAsync();

            return true;
        }

        public async Task SaveAsync()
        {
            await saveLock.WaitAsync();

            try
            {
                var data = Habits.Select(habit => new HabitData
                {
                    Id = habit.Id,
                    Name = habit.Name,
                    Description = habit.Description,
                    CreatedAt = habit.CreatedAt,
                    CompletedDates = habit.GetCompletedDates()
                }).ToList();

                await habitStorage.SaveAsync(data);
            }
            finally
            {
                saveLock.Release();
            }
        }


        public async Task LoadAsync()
        {
            var data = await habitStorage.LoadAsync();

            var loadedHabits = new List<HabitItem>();

            foreach (var item in data)
            {
                var habit = new HabitItem
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    CreatedAt = item.CreatedAt
                };

                habit.RestoreCompletedDates(item.CompletedDates);

                loadedHabits.Add(habit);
            }

            Habits.Clear();

            foreach (var habit in loadedHabits)
            {
                Habits.Add(habit);
            }
        }


        public async Task ToggleHabitCompletion(Guid habitId, DateOnly date)
        {
            var habit = GetHabit(habitId);
            if (habit != null)
            {
                habit.ToggleCompletion(date);
                await SaveAsync();
            }
            else
            {
                return;
            }
        }

        public HabitItem? GetHabit(Guid id)
        {
            return Habits.FirstOrDefault(h => h.Id == id);
        }
    }
}
