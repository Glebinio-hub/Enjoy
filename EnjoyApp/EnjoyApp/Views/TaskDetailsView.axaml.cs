using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using EnjoyApp.ViewModels;
using System;

namespace EnjoyApp.Views
{
    public partial class TaskDetailsView : UserControl
    {
        public event EventHandler? CancelRequested;
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
    }
}