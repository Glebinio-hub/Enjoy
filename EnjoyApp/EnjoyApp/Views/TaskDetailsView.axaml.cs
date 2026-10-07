using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using EnjoyApp.ViewModels;
using System;
using EnjoyApp.Models;

namespace EnjoyApp.Views
{
    public partial class TaskDetailsView : UserControl
    {
        public event EventHandler? CancelRequested;
        public event EventHandler<TaskItem>? DeleteRequested;
        public TaskDetailsView()
        {
            InitializeComponent();
        }

        public void CancelButtonClicked(object? sender, RoutedEventArgs e)
        {
            CancelRequested?.Invoke(this, EventArgs.Empty);
        }

        private void IsCompletedChanged(object? sender, RoutedEventArgs e)
        {
            if (DataContext is TaskDetailsViewModel vm &&
                sender is CheckBox checkBox &&
                checkBox.IsChecked is bool isCompleted)
            {
                vm.SetCompleted(isCompleted);
            }
        }

        public void DeleteButtonClicked(object? sender, RoutedEventArgs e)
        {
            if (DataContext is TaskDetailsViewModel vm && vm.Task is not null)
            {
                DeleteRequested?.Invoke(this, vm.Task);
            }
        }
    }
}