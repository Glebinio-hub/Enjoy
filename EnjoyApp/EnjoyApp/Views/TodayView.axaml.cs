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
                if (vm.CurrentTask is not null)
                {
                    TaskCompleted?.Invoke(this, vm.CurrentTask);
                }

            }
        }

        private void ShowDetailsView(object? sender, Avalonia.Input.TappedEventArgs e)
        {
            if (DataContext is TodayViewModel vm)
            {
                if (vm.CurrentTask is not null)
                {
                    ShowDetailsRequested?.Invoke(this, vm.CurrentTask);
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
            if (e.PropertyName == nameof(TodayViewModel.HasCurrentTask) ||
                e.PropertyName == nameof(TodayViewModel.CurrentTask))
            {
                if (DataContext is TodayViewModel vm)
                {
                    _ = SwitchCardsAsync(vm.HasCurrentTask);
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

        private async Task SwitchCardsAsync(bool hasCurrentTask)
        {
            if (_isAnimating)
                return;

            var activeCard = hasCurrentTask ? RelaxCard : TaskCard;
            var nextCard = hasCurrentTask ? TaskCard : RelaxCard;

            if (nextCard.IsVisible)
                return;

            _isAnimating = true;

            try
            {
                nextCard.IsVisible = true;
                nextCard.IsHitTestVisible = false;
                nextCard.Opacity = 0;
                nextCard.RenderTransform = new Avalonia.Media.TranslateTransform(0, 8);

                activeCard.IsHitTestVisible = false;
                activeCard.Opacity = 0;
                activeCard.RenderTransform = new Avalonia.Media.TranslateTransform(0, -8);

                await Task.Delay(300);

                activeCard.IsVisible = false;
                activeCard.RenderTransform = new Avalonia.Media.TranslateTransform(0, 0);

                nextCard.Opacity = 1;
                nextCard.RenderTransform = new Avalonia.Media.TranslateTransform(0, 0);

                await Task.Delay(300);

                nextCard.IsHitTestVisible = true;
            }
            finally
            {
                _isAnimating = false;
            }
        }
    }
}