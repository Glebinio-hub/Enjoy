
using Avalonia;
using Avalonia.Controls;
using Avalonia.Metadata;
using EnjoyApp.Models;
using EnjoyApp.Services;
using EnjoyApp.ViewModels;
using System;

namespace EnjoyApp.Views;

public partial class MainView : UserControl
{
    private TodayView todayView;
    private TaskStorage taskStorage;
    private TaskService taskService;
    private ScheduleView scheduleView;



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
        TaskDetailsView.DataContext = new TaskDetailsViewModel(task, taskService);
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


    private void OnSubTaskCreatedRequested(object? sender, SubTaskItem subTask)
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
        EditTaskView.DataContext = new EditTaskViewModel(task, taskService);
        EditTaskOverlay.IsVisible = true;
    }

    private void OnCancelEditViewRequested(object? sender, EventArgs e)
    {
        EditTaskOverlay.IsVisible = false;
    }

    private void OnNavigationRequested(object? sender, string page)
    {
        bool showToday = page == "Today";

        todayView.IsVisible = showToday;
        scheduleView.IsVisible = !showToday;
    }


    private void TodayNavigationClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        OnNavigationRequested(this, "Today");
    }

    private void PlanNavigationClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        OnNavigationRequested(this, "Plan");
    }




}
