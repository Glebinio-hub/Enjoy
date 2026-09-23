using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;

namespace EnjoyApp.Views
{
    public partial class CreateTaskView : UserControl
    {
        public event EventHandler? CancelRequested;
        public CreateTaskView()
        {
            InitializeComponent();
        }

        private void CancelButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            CancelRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}