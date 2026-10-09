using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;

namespace EnjoyApp.Views
{
    public partial class CreateSubTaskView : UserControl
    {
        public event EventHandler? CancelSTRequested;
        public CreateSubTaskView()
        {
            InitializeComponent();
        }

        private void CancelButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            CancelSTRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}