using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;

namespace EnjoyApp.Views
{
    public partial class TodayView : UserControl
    {
        public event EventHandler? AddTaskRequested;
        public TodayView()
        {
            InitializeComponent();
        }
        private void AddTaskButtonClick(object? sender, RoutedEventArgs e)
        {
            AddTaskRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}