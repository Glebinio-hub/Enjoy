using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using EnjoyApp.Models;
using EnjoyApp.ViewModels;
using System;
using System.Diagnostics;

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

        private void StartTimeButtonClick(object? sender, RoutedEventArgs e)
        {
            if (DataContext is CreateTaskViewModel vm)
                vm.OpenTimePicker(true);
        }

        private void EndTimeButtonClick(object? sender, RoutedEventArgs e)
        {
            if (DataContext is CreateTaskViewModel vm)
                vm.OpenTimePicker(false);
        }

        private void TimePickerCancelRequested(object? sender, EventArgs e)
        {
            if (DataContext is CreateTaskViewModel vm)
                vm.CancelTimePicker();

            Debug.WriteLine("CreateTaskView received CANCEL");

        }

        private void TimePickerDoneRequested(object? sender, EventArgs e)
        {
            if (DataContext is CreateTaskViewModel vm)
                vm.ConfirmTimePicker();
        }

    }
}