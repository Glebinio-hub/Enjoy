
using Avalonia;
using Avalonia.Controls;
using EnjoyApp.Models;
using EnjoyApp.Services;
using EnjoyApp.ViewModels;
using System;
using System.ComponentModel;

namespace EnjoyApp.Views;

public partial class MainView : UserControl
{
    private TodayView todayView;
    private TaskStorage taskStorage;
    private TaskService taskService;
    private ScheduleView scheduleView;

    private EditTaskViewModel? activeEditTimePickerVm;

    public MainView()
    {
        InitializeComponent();

        taskStorage = new TaskStorage();
        taskService = new TaskService(taskStorage);

        todayView = new TodayView(taskService);
        scheduleView = new ScheduleView(taskService);

        scheduleView.IsVisible = false;

        todayView.AddTaskRequested += OnAddTaskRequested;
        CreateTaskView.CancelRequested += OnCancelRequested;
        CreateTaskView.TaskCreated += OnTaskCreated;
        todayView.TaskCompleted += OnTaskCompleted;
        todayView.ShowDetailsRequested += OnShowDetailsRequested;

        TaskDetailsView.CancelRequested += OnCancelDetViewRequested;
        TaskDetailsView.DeleteRequested += OnDeleteRequested;

        CreateTaskView.AddSubtaskRequested += OnAddSubtaskRequested;
        CreateSubTaskView.CancelSTRequested += OnCancelSTRRequested;
        CreateSubTaskView.SubTaskCreated += OnSubTaskCreatedRequested;

        TaskDetailsView.EditRequested += OnEditRequested;

        EditTaskView.EditSaveRequested += OnEditSaveRequested;
        EditTaskView.CancelEditSaveRequested += OnCancelEditViewRequested;
        EditTaskView.EditSubTasksRequested += OnAddSubtaskRequested;

        ContentGrid.Children.Add(todayView);
        ContentGrid.Children.Add(scheduleView);

        scheduleView.IsVisible = false;

        todayView.NavigationRequested += OnNavigationRequested;

        if (CreateTaskView.DataContext is CreateTaskViewModel createVm)
        {
            createVm.PropertyChanged += CreateTimePickerPropertyChanged;
            TimePicker.DataContext = createVm;
        }


        TimePicker.CancelRequested += (_, _) => CancelActiveTimePicker();
        TimePicker.DoneRequested += (_, _) => ConfirmActiveTimePicker();

        TimePickerOverlay.IsVisible = false;
    }

    private void CreateTimePickerPropertyChanged(
        object? sender,
        PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(CreateTaskViewModel.IsTimePickerOpen))
            return;

        if (sender is not CreateTaskViewModel vm)
            return;

        if (vm.IsTimePickerOpen)
        {
            TimePicker.DataContext = vm;
            TimePickerOverlay.IsVisible = true;
            TimePicker.CenterSelectedTime();
        }
        else if (ReferenceEquals(TimePicker.DataContext, vm))
        {
            TimePickerOverlay.IsVisible = false;
        }
    }

    private void EditTimePickerPropertyChanged(
        object? sender,
        PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(EditTaskViewModel.IsTimePickerOpen))
            return;

        if (sender is not EditTaskViewModel vm)
            return;

        if (vm.IsTimePickerOpen)
        {
            TimePicker.DataContext = vm;
            TimePickerOverlay.IsVisible = true;
            TimePicker.CenterSelectedTime();
        }
        else if (ReferenceEquals(TimePicker.DataContext, vm))
        {
            TimePickerOverlay.IsVisible = false;
        }
    }

    private void CancelActiveTimePicker()
    {
        switch (TimePicker.DataContext)
        {
            case CreateTaskViewModel createVm:
                createVm.CancelTimePicker();
                break;

            case EditTaskViewModel editVm:
                editVm.CancelTimePicker();
                break;
        }
    }

    private void ConfirmActiveTimePicker()
    {
        switch (TimePicker.DataContext)
        {
            case CreateTaskViewModel createVm:
                createVm.ConfirmTimePicker();
                break;

            case EditTaskViewModel editVm:
                editVm.ConfirmTimePicker();
                break;
        }
    }

    private void OnAddTaskRequested(object? sender, EventArgs e)
    {
        if (CreateTaskView.DataContext is CreateTaskViewModel vm)
        {
            vm.Reset();
        }

        CreateTaskOverlay.IsVisible = true;
    }

    private void OnCancelRequested(object? sender, EventArgs e)
    {
        if (CreateTaskView.DataContext is CreateTaskViewModel vm)
        {
            vm.Reset();
        }

        CreateTaskOverlay.IsVisible = false;
    }

    private void OnTaskCreated(object? sender, TaskItem task)
    {
        if (todayView.DataContext is TodayViewModel viewModel)
        {
            viewModel.AddTask(task);

            if (CreateTaskView.DataContext is CreateTaskViewModel vm)
            {
                vm.Reset();
            }

            CreateTaskOverlay.IsVisible = false;
        }
    }

    private void OnTaskCompleted(object? sender, TaskItem task)
    {
        if (todayView.DataContext is TodayViewModel viewModel)
        {
            viewModel.CompleteTask(task);
        }
    }

    private void OnShowDetailsRequested(object? sender, TaskItem task)
    {
        TaskDetailsView.DataContext =
            new TaskDetailsViewModel(task, taskService);

        TaskDetailsOverlay.IsVisible = true;
    }

    public void OnCancelDetViewRequested(object? sender, EventArgs e)
    {
        TaskDetailsOverlay.IsVisible = false;

        if (todayView.DataContext is TodayViewModel viewModel)
        {
            viewModel.RefreshTasks();
        }
    }

    public void OnDeleteRequested(object? sender, TaskItem task)
    {
        TaskDetailsOverlay.IsVisible = false;

        if (todayView.DataContext is TodayViewModel vm)
        {
            vm.DeleteTask(task);
        }
    }

    private void OnAddSubtaskRequested(object? sender, EventArgs e)
    {
        CreateSubTaskOverlay.IsVisible = true;
    }

    private void OnCancelSTRRequested(object? sender, EventArgs e)
    {
        if (CreateSubTaskView.DataContext is CreateSubTaskViewModel vm)
        {
            vm.Reset();
        }

        CreateSubTaskOverlay.IsVisible = false;
    }

    private void OnSubTaskCreatedRequested(
        object? sender,
        SubTaskItem subTask)
    {
        if (EditTaskOverlay.IsVisible &&
            EditTaskView.DataContext is EditTaskViewModel editVm)
        {
            editVm.SubTasks.Add(subTask);
            CreateSubTaskOverlay.IsVisible = false;
        }
        else if (CreateTaskView.DataContext is CreateTaskViewModel createVm)
        {
            createVm.SubTasks.Add(subTask);
            CreateSubTaskOverlay.IsVisible = false;
        }

        if (CreateSubTaskView.DataContext is CreateSubTaskViewModel subTaskVm)
        {
            subTaskVm.Reset();
        }
    }

    private void OnEditSaveRequested(object? sender, TaskItem task)
    {
        if (EditTaskView.DataContext is EditTaskViewModel vm)
        {
            vm.EditTask();

            if (string.IsNullOrWhiteSpace(vm.ValidationMessage))
            {
                vm.CancelTimePicker();
                EditTaskOverlay.IsVisible = false;

                if (todayView.DataContext is TodayViewModel tvm)
                {
                    tvm.RefreshTasks();
                }
            }
        }
    }

    private void OnEditRequested(object? sender, TaskItem task)
    {
        if (activeEditTimePickerVm is not null)
        {
            activeEditTimePickerVm.PropertyChanged -=
                EditTimePickerPropertyChanged;

            activeEditTimePickerVm.CancelTimePicker();
        }

        var editVm = new EditTaskViewModel(task, taskService);

        EditTaskView.DataContext = editVm;

        editVm.PropertyChanged += EditTimePickerPropertyChanged;

        activeEditTimePickerVm = editVm;

        EditTaskOverlay.IsVisible = true;
    }

    private void OnCancelEditViewRequested(object? sender, EventArgs e)
    {
        if (EditTaskView.DataContext is EditTaskViewModel vm)
        {
            vm.CancelTimePicker();
        }

        EditTaskOverlay.IsVisible = false;
    }

    private void OnNavigationRequested(object? sender, string page)
    {
        bool showToday = page == "Today";

        todayView.IsVisible = showToday;
        scheduleView.IsVisible = !showToday;

        TodayTabButton.Classes.Set("active", showToday);
        PlanTabButton.Classes.Set("active", !showToday);
    }

    private void TodayNavigationClick(
        object? sender,
        Avalonia.Interactivity.RoutedEventArgs e)
    {
        OnNavigationRequested(this, "Today");
    }

    private void PlanNavigationClick(
        object? sender,
        Avalonia.Interactivity.RoutedEventArgs e)
    {
        OnNavigationRequested(this, "Plan");
    }

    private void NavigationGrid_SizeChanged(
        object? sender,
        SizeChangedEventArgs e)
    {
        double buttonWidth = Math.Max(0, e.NewSize.Width / 4 - 20);

        TodayTabButton.Width = buttonWidth;
        PlanTabButton.Width = buttonWidth;
        AiTabButton.Width = buttonWidth;
        MeTabButton.Width = buttonWidth;
    }
}
