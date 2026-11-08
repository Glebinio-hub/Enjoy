using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using Avalonia.Controls;
using Avalonia.Interactivity;
using EnjoyApp.ViewModels;

namespace EnjoyApp.Views
{
    public partial class HabitCard : UserControl
    {
        public HabitCard()
        {
            InitializeComponent();
        }

        private void EnjoyCheckBox_CheckedChanged(object? sender, RoutedEventArgs e)
        {
            if (DataContext is HabitCardViewModel vm)
            {
                vm.ToggleCompletion();
            }
        }
    }
}