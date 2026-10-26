using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using EnjoyApp.Models;
using EnjoyApp.ViewModels;
using EnjoyApp.Views;
using System;
using System.Diagnostics;

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

        private void StartTimeButtonClick(object? sender, RoutedEventArgs e)
        {
            if (DataContext is EditTaskViewModel vm)
                vm.OpenTimePicker(true);
        }

        private void EndTimeButtonClick(object? sender, RoutedEventArgs e)
        {
            if (DataContext is EditTaskViewModel vm)
                vm.OpenTimePicker(false);
        }

        private void TimePickerCancelRequested(object? sender, EventArgs e)
        {
            if (DataContext is EditTaskViewModel vm)
                vm.CancelTimePicker();


        }

        private void TimePickerDoneRequested(object? sender, EventArgs e)
        {
            if (DataContext is EditTaskViewModel vm)
                vm.ConfirmTimePicker();
        }


    }
}