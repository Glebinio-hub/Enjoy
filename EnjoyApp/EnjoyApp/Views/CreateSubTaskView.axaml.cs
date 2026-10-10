using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using EnjoyApp.Models;
using EnjoyApp.ViewModels;

namespace EnjoyApp.Views
{
    public partial class CreateSubTaskView : UserControl
    {
        public event EventHandler? CancelSTRequested;
        public event EventHandler<SubTaskItem>? SubTaskCreated;
        public CreateSubTaskView()
        {
            InitializeComponent();
            if (DataContext is CreateSubTaskViewModel vm)
            {
                vm.SubTaskCreated += (_, subtask) =>
                {
                    SubTaskCreated?.Invoke(this, subtask);
                };
            }
        }

        private void CancelButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            CancelSTRequested?.Invoke(this, EventArgs.Empty);
        }

    }
}