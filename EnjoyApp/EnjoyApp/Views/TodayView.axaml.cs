using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using EnjoyApp.Services;
using System;
using EnjoyApp.ViewModels;
using EnjoyApp.Models;

namespace EnjoyApp.Views
{
    public partial class TodayView : UserControl
    {
        public event EventHandler? AddTaskRequested;
        public event EventHandler<TaskItem>? TaskCompleted;
        public event EventHandler<TaskItem>? ShowDetailsRequested;
        public TodayView(TaskService taskService)
        {
            InitializeComponent();
            DataContext = new TodayViewModel(taskService);
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
    }
}