using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using EnjoyApp.Services;

namespace EnjoyApp.Views
{
    public partial class ScheduleView : UserControl
    {
        public ScheduleView(TaskService taskService)
        {
            InitializeComponent();
        }
    }
}