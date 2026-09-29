using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using EnjoyApp.Services;
using System;
using EnjoyApp.ViewModels;

namespace EnjoyApp.Views
{
    public partial class TodayView : UserControl
    {
        public event EventHandler? AddTaskRequested;
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
    }
}