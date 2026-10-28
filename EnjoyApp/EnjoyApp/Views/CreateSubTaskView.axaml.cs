using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using EnjoyApp.Models;
using EnjoyApp.ViewModels;
using System;

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

        private void StartTimeButtonClick(object? sender, RoutedEventArgs e)
        {
            if (DataContext is CreateSubTaskViewModel vm)
                vm.OpenTimePicker(true);
        }

        private void EndTimeButtonClick(object? sender, RoutedEventArgs e)
        {
            if (DataContext is CreateSubTaskViewModel vm)
                vm.OpenTimePicker(false);
        }

        private void TimePickerCancelRequested(object? sender, EventArgs e)
        {
            if (DataContext is CreateSubTaskViewModel vm)
                vm.CancelTimePicker();


        }

        private void TimePickerDoneRequested(object? sender, EventArgs e)
        {
            if (DataContext is CreateSubTaskViewModel vm)
                vm.ConfirmTimePicker();
        }
    }
}