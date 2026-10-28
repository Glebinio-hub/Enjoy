using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using EnjoyApp.Services;
using EnjoyApp.ViewModels;

namespace EnjoyApp.Views
{
    public partial class ScheduleView : UserControl
    {
        public ScheduleView(TaskService taskService)
        {
            InitializeComponent();
            DataContext = new ScheduleViewModel(taskService);
        }

        private void DateStrip_OnLoaded(
    object? sender,
    RoutedEventArgs e)
        {
            if (sender is ListBox dateStrip &&
                DataContext is ScheduleViewModel vm &&
                vm.SelectedDateItem is not null)
            {
                dateStrip.ScrollIntoView(vm.SelectedDateItem);
            }
        }
    }
}