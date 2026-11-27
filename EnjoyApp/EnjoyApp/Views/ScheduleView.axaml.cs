
using Avalonia.Controls;
using Avalonia.Interactivity;
using EnjoyApp.Models;
using EnjoyApp.Services;
using EnjoyApp.ViewModels;
using System;
using System.ComponentModel;

namespace EnjoyApp.Views
{
    public partial class ScheduleView : UserControl
    {
        public event EventHandler<DateOnly>? AddTaskRequested;
        public ScheduleView(TaskService taskService)
        {
            InitializeComponent();

            var vm = new ScheduleViewModel(taskService);

            DataContext = vm;

            vm.PropertyChanged += OnViewModelPropertyChanged;

        }

        public ScheduleView()
            : this(new TaskService(new TaskStorage()))
        {
        }

        private void DateStrip_OnLoaded(
            object? sender,
            RoutedEventArgs e)
        {
            ScrollToSelectedDate();
        }

        private void OnViewModelPropertyChanged(
            object? sender,
            PropertyChangedEventArgs e)
        {
            if (e.PropertyName ==
                nameof(ScheduleViewModel.SelectedDateItem))
            {
                Avalonia.Threading.Dispatcher.UIThread.Post(
                    ScrollToSelectedDate,
                    Avalonia.Threading.DispatcherPriority.Loaded);
            }
        }

        private void ScrollToSelectedDate()
        {
            if (DataContext is ScheduleViewModel vm &&
                vm.SelectedDateItem is not null)
            {
                DateStrip.ScrollIntoView(vm.SelectedDateItem);
            }
        }


        private void OpenCalendar_Click(
            object? sender, RoutedEventArgs e)
        {
            if (DataContext is ScheduleViewModel vm)
                vm.OpenCalendar();
        }

        private void CloseCalendar_Click(
            object? sender, RoutedEventArgs e)
        {
            if (DataContext is ScheduleViewModel vm)
                vm.CloseCalendar();
        }

        private void PreviousCalendarMonth_Click(
            object? sender, RoutedEventArgs e)
        {
            if (DataContext is ScheduleViewModel vm)
                vm.ChangeCalendarMonth(-1);
        }

        private void NextCalendarMonth_Click(
            object? sender, RoutedEventArgs e)
        {
            if (DataContext is ScheduleViewModel vm)
                vm.ChangeCalendarMonth(1);
        }

        private void PreviousYear_Click(
            object? sender, RoutedEventArgs e)
        {
            if (DataContext is ScheduleViewModel vm)
                vm.ChangeCalendarMonth(-12);
        }

        private void NextYear_Click(
            object? sender, RoutedEventArgs e)
        {
            if (DataContext is ScheduleViewModel vm)
                vm.ChangeCalendarMonth(12);
        }

        private void CalendarDay_Click(
            object? sender, RoutedEventArgs e)
        {
            if (DataContext is not ScheduleViewModel vm)
                return;

            if (sender is Button button &&
                button.DataContext is ScheduleCalendarDay day &&
                day.Date.HasValue)
            {
                vm.SelectCalendarDate(day.Date.Value);
            }
        }
        private void AddTask_Click(object? sender, RoutedEventArgs e)
        {

            if (DataContext is not ScheduleViewModel vm)
                return;

            DateOnly selectedDate = DateOnly.FromDateTime(
                vm.SelectedDate?.Date ?? DateTime.Today);

            AddTaskRequested?.Invoke(this, selectedDate);
        }



        private void DeleteTask_Click(object? sender, RoutedEventArgs e)
        {
            if (DataContext is not ScheduleViewModel vm)
                return;

            if (sender is Button button &&
                button.DataContext is TaskItem task)
            {
                vm.DeleteTask(task);
            }
        }
    }
}
