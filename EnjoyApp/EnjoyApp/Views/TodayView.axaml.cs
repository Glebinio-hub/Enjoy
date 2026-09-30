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
        public TodayView()
        {
            InitializeComponent();
            var taskService = new TaskService();
            DataContext = new TodayViewModel(taskService);
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
    }
}