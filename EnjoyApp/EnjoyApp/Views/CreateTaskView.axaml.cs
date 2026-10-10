using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using EnjoyApp.Models;
using System;
using EnjoyApp.ViewModels;

namespace EnjoyApp.Views
{
    public partial class CreateTaskView : UserControl
    {
        public event EventHandler? CancelRequested;
        public event EventHandler<TaskItem?> TaskCreated;
        public event EventHandler? AddSubtaskRequested;
        public CreateTaskView()
        {
            InitializeComponent();
            if (DataContext is CreateTaskViewModel vm)
            {
                vm.TaskCreated += (_, task) =>
                {
                    TaskCreated?.Invoke(this, task);
                };
            }

        }

        private void CancelButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            CancelRequested?.Invoke(this, EventArgs.Empty);
        }

        private void AddSubtaskButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            AddSubtaskRequested?.Invoke(this, EventArgs.Empty);
        }

    }
}