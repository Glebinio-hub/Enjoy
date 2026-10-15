using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using EnjoyApp.Models;
using EnjoyApp.ViewModels;
using EnjoyApp.Views;
using System;

namespace EnjoyApp.Views
{
    public partial class EditTaskView : UserControl
    {
        public event EventHandler<TaskItem> EditSaveRequested;
        public event EventHandler CancelEditSaveRequested;

        public event EventHandler EditSubTasksRequested;

        public EditTaskView()
        {
            InitializeComponent();
        }

        private void SaveButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (DataContext is EditTaskViewModel vm)
            {
                EditSaveRequested?.Invoke(this, vm.Task);
            }
        }

        private void CancelButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            CancelEditSaveRequested?.Invoke(this, EventArgs.Empty);
            
        }

        private void EditSubtaskButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            EditSubTasksRequested?.Invoke(this, EventArgs.Empty);

        }


    }
}