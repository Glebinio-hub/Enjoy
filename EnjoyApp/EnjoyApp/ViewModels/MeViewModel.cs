using Avalonia;
using Avalonia.Controls;
using EnjoyApp.Models;
using EnjoyApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnjoyApp.ViewModels
{
    public class MeViewModel : INotifyPropertyChanged
    {

        private readonly HabitService habitService;
        private Guid? selectedHabitId;
        private string newHabitName = string.Empty;

        public string NewHabitName
        {
            get { return newHabitName; }
            set
            {
                if (newHabitName != value)
                {
                    newHabitName = value;
                    OnPropertyChanged(nameof(NewHabitName));
                }

                if (!string.IsNullOrWhiteSpace(value))
                {
                    HabitValidationMessage = string.Empty;
                }
            }
        }

        private string newHabitDescription = string.Empty;

        public string NewHabitDescription
        {
            get { return newHabitDescription; }
            set
            {
                if (newHabitDescription != value)
                {
                    newHabitDescription = value;
                    OnPropertyChanged(nameof(NewHabitDescription));
                }
            }
        }

        private bool isCreateHabitOpen;

        public bool IsCreateHabitOpen
        {
            get { return isCreateHabitOpen; }
            set
            {
                if (isCreateHabitOpen != value)
                {
                    isCreateHabitOpen = value;
                    OnPropertyChanged(nameof(IsCreateHabitOpen));
                }
            }
        }

        private bool isDeleteHabitOpen;

        public bool IsDeleteHabitOpen
        {
            get { return isDeleteHabitOpen; }
            set
            {
                if (isDeleteHabitOpen != value)
                {
                    isDeleteHabitOpen = value;
                    OnPropertyChanged(nameof(IsDeleteHabitOpen));
                }

            }
        }


        private string habitValidationMessage = string.Empty;

        public string HabitValidationMessage
        {
            get => habitValidationMessage;
            set
            {
                if (habitValidationMessage != value)
                {
                    habitValidationMessage = value;
                    OnPropertyChanged(nameof(HabitValidationMessage));
                    OnPropertyChanged(nameof(HasHabitValidationError));
                }
            }
        }

        public bool HasHabitValidationError =>
            !string.IsNullOrEmpty(HabitValidationMessage);


        public MeViewModel(HabitService habitService)
        {
            this.habitService = habitService;

            foreach (var habit in habitService.Habits)
            {
                HabitCards.Add(new HabitCardViewModel(habit, DateOnly.FromDateTime(DateTime.Today), habitService));
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
                                        DateOnly.FromDateTime(DateTime.Today),
                                        habitService
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

        public async Task CreateHabitAsync()
        {
            if (string.IsNullOrWhiteSpace(NewHabitName))
            {
                HabitValidationMessage = "Please enter a habit name.";
                return;

            }

            HabitValidationMessage = string.Empty;

            var newHabit = new HabitItem
            {
                Name = NewHabitName.Trim(),
                Description = NewHabitDescription.Trim()
            };

            await habitService.AddHabitAsync(newHabit);

            NewHabitName = string.Empty;
            NewHabitDescription = string.Empty;

            IsCreateHabitOpen = false;
        }

        public void OpenCreateHabit()
        {
            IsCreateHabitOpen = true;
        }

        public void CancelCreateHabit()
        {
            HabitValidationMessage = string.Empty;
            NewHabitName = string.Empty;
            NewHabitDescription = string.Empty;
            IsCreateHabitOpen = false;
        }

        public void OpenDeleteHabit(Guid habitId)
        {
            if (habitService.GetHabit(habitId) == null)
                return;
            selectedHabitId = habitId;
            IsDeleteHabitOpen = true;
        }

        public void CancelDeleteHabit()
        {
            selectedHabitId = null;
            IsDeleteHabitOpen = false;
        }

        public async Task ConfirmDeleteHabitAsync()
        {
            if (selectedHabitId == null)
                return;

            bool removed = await habitService.RemoveHabitAsync(
                selectedHabitId.Value);

            if (!removed)
                return;

            selectedHabitId = null;
            IsDeleteHabitOpen = false;
        }
        public async Task<bool> DeleteHabit(Guid habitId)
        {
            return await habitService.RemoveHabitAsync(habitId);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
