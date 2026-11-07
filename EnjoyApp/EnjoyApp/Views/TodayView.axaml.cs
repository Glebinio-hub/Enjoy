using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using EnjoyApp.Services;
using System;
using EnjoyApp.ViewModels;
using EnjoyApp.Models;
using System.ComponentModel;
using System.Threading.Tasks;

namespace EnjoyApp.Views
{
    public partial class TodayView : UserControl
    {
        private bool _isAnimating;
        public event EventHandler? AddTaskRequested;
        public event EventHandler<TaskItem>? TaskCompleted;
        public event EventHandler<TaskItem>? ShowDetailsRequested;
        public event EventHandler<string>? NavigationRequested;
        public TodayView(TaskService taskService)
        {
            InitializeComponent();
            DataContext = new TodayViewModel(taskService);
            if (DataContext is TodayViewModel vm)
            {
                vm.PropertyChanged += OnViewModelPropertyChanged;
                UpdateCardsImmediately(vm.HasCurrentTask);
            }
        }

        public TodayView() : this(new TaskService(new TaskStorage()))
        {
        }

        private void AddTaskButtonClick(object? sender, RoutedEventArgs e)
        {
            AddTaskRequested?.Invoke(this, EventArgs.Empty);
        }

        private void IsTaskCompleted(object? sender, RoutedEventArgs e)
        {
            if (DataContext is TodayViewModel vm)
            {
                if (vm.DisplayedTask is not null)
                {
                    TaskCompleted?.Invoke(this, vm.DisplayedTask);
                }

            }
        }

        private void ShowDetailsView(object? sender, Avalonia.Input.TappedEventArgs e)
        {
            if (_isAnimating)
                return;

            if (DataContext is TodayViewModel vm)
            {
                if (vm.DisplayedTask is not null)
                {
                    ShowDetailsRequested?.Invoke(this, vm.DisplayedTask);
                }
            }
        }

        private void NavigationButtonClick(object? sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string page)
            {
                NavigationRequested?.Invoke(this, page);
            }
        }

        private void OnViewModelPropertyChanged(
    object? sender,
    PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TodayViewModel.CurrentTask))
            {
                if (DataContext is TodayViewModel vm)
                {
                    if (!ReferenceEquals(vm.DisplayedTask, vm.CurrentTask))
                    {
                        _ = SwitchCardsAsync();
                    }
                }
            }
        }

        private void UpdateCardsImmediately(bool hasCurrentTask)
        {
            TaskCard.IsVisible = hasCurrentTask;
            RelaxCard.IsVisible = !hasCurrentTask;

            TaskCard.Opacity = 1;
            RelaxCard.Opacity = 1;

            TaskCard.IsHitTestVisible = hasCurrentTask;
            RelaxCard.IsHitTestVisible = !hasCurrentTask;
        }



        private async Task SwitchCardsAsync()
        {
            if (_isAnimating)
                return;

            if (DataContext is not TodayViewModel vm)
                return;

            _isAnimating = true;

            TaskCard.IsHitTestVisible = false;
            RelaxCard.IsHitTestVisible = false;

            try
            {
                while (!ReferenceEquals(vm.DisplayedTask, vm.CurrentTask))
                {
                    var oldCard = vm.DisplayedTask is null
                        ? RelaxCard
                        : TaskCard;

                    oldCard.Opacity = 0;
                    oldCard.RenderTransform =
                        new Avalonia.Media.TranslateTransform(0, -8);

                    await Task.Delay(300);

                    oldCard.IsVisible = false;
                    oldCard.RenderTransform =
                        new Avalonia.Media.TranslateTransform(0, 0);

                    vm.UpdateDisplayedTask();

                    var newCard = vm.DisplayedTask is null
                        ? RelaxCard
                        : TaskCard;

                    newCard.IsVisible = true;
                    newCard.Opacity = 0;
                    newCard.RenderTransform =
                        new Avalonia.Media.TranslateTransform(0, 8);

                    await Task.Delay(20);

                    newCard.Opacity = 1;
                    newCard.RenderTransform =
                        new Avalonia.Media.TranslateTransform(0, 0);

                    await Task.Delay(300);
                }
            }
            finally
            {
                TaskCard.IsHitTestVisible = vm.DisplayedTask is not null;
                RelaxCard.IsHitTestVisible = vm.DisplayedTask is null;

                _isAnimating = false;
            }
        }
    }
}