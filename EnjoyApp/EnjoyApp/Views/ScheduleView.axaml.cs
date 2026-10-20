using Avalonia;
using Avalonia.Controls;
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
    }
}