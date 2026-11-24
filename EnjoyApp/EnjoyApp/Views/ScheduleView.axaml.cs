
using Avalonia.Controls;
using Avalonia.Interactivity;
using EnjoyApp.Models;
using EnjoyApp.Services;
using EnjoyApp.ViewModels;
using System.ComponentModel;

namespace EnjoyApp.Views
{
    public partial class ScheduleView : UserControl
    {
        public ScheduleView(TaskService taskService)
        {
            InitializeComponent();

            var vm = new ScheduleViewModel(taskService);

            DataContext = vm;

            vm.PropertyChanged += OnViewModelPropertyChanged;

            PlanCreateTaskView.CancelRequested += (_, _) =>
            {
                if (DataContext is ScheduleViewModel vm)
                    vm.CancelCreateTask();
            };

            PlanCreateTaskView.TaskCreated += (_, task) =>
            {
                if (task is null)
                    return;

                taskService.AddTask(task);

                if (DataContext is ScheduleViewModel vm)
                    vm.CancelCreateTask();
            };

            if (PlanCreateTaskView.DataContext is CreateTaskViewModel createVm)
            {
                createVm.SetTaskService(taskService);
            }
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
            if (DataContext is not ScheduleViewModel scheduleVm)
                return;

            if (PlanCreateTaskView.DataContext is not CreateTaskViewModel createVm)
                return;

            scheduleVm.OpenCreateTask();

            createVm.Reset();
            createVm.TaskDate = scheduleVm.NewTaskDate;

        }
    }
}
