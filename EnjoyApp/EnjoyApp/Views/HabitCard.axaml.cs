using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using EnjoyApp.ViewModels;
using System;

namespace EnjoyApp.Views
{
    public partial class HabitCard : UserControl
    {
        public event EventHandler<Guid>? DeleteRequested;
        public HabitCard()
        {
            InitializeComponent();
        }

        private async void EnjoyCheckBox_CheckedChanged(object? sender, RoutedEventArgs e)
        {

            if (DataContext is not HabitCardViewModel vm)
                return;

            try
            {
                await vm.ToggleCompletionAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
        


        private void DeleteHabit_Click(object? sender, RoutedEventArgs e)
        {
            if (DataContext is HabitCardViewModel vm)
            {
                DeleteRequested?.Invoke(this, vm.HabitId);
            }
        }

    }
}