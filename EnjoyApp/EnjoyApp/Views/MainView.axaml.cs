
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
    public MainView()
    {
        InitializeComponent();
        taskStorage = new TaskStorage();
        taskService = new TaskService(taskStorage);
        todayView = new TodayView(taskService);
        todayView.AddTaskRequested += OnAddTaskRequested;
        CreateTaskView.CancelRequested += OnCancelRequested;
        CreateTaskView.TaskCreated += OnTaskCreated;
        todayView.TaskCompleted += OnTaskCompleted;
        todayView.ShowDetailsRequested += OnShowDetailsRequested;
        TaskDetailsView.CancelRequested += OnCancelDetViewRequested;
        TaskDetailsView.DeleteRequested += OnDeleteRequested;
        RootGrid.Children.Insert(0, todayView);


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
        CreateTaskOverlay.IsVisible = false;
    }

    private void OnTaskCreated(object? sender, TaskItem task)
    {
        if (todayView.DataContext is TodayViewModel viewModel)
        {
            viewModel.AddTask(task);
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



}
