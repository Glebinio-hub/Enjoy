
using Avalonia;
using Avalonia.Controls;
using EnjoyApp.Models;
using EnjoyApp.ViewModels;
using System;

namespace EnjoyApp.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        TodayView.AddTaskRequested += OnAddTaskRequested;
        CreateTaskView.CancelRequested += OnCancelRequested;
        CreateTaskView.TaskCreated += OnTaskCreated;

    }

    private void OnAddTaskRequested(object? sender, EventArgs e)
    {
        CreateTaskOverlay.IsVisible = true;
    }

    private void OnCancelRequested(object? sender, EventArgs e)
    {
        CreateTaskOverlay.IsVisible = false;
    }

    private void OnTaskCreated(object? sender, TaskItem task)
    {
        if (TodayView.DataContext is TodayViewModel viewModel)
        {
            viewModel.AddTask(task);
            CreateTaskOverlay.IsVisible = false;
        }
    }

}
