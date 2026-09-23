
using Avalonia;
using Avalonia.Controls;
using System;

namespace EnjoyApp.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        TodayView.AddTaskRequested += OnAddTaskRequested;
        CreateTaskView.CancelRequested += OnCancelRequested;

    }

    private void OnAddTaskRequested(object? sender, EventArgs e)
    {
        CreateTaskOverlay.IsVisible = true;
    }

    private void OnCancelRequested(object? sender, EventArgs e)
    {
        CreateTaskOverlay.IsVisible = false;
    }


}
