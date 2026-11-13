using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using EnjoyApp.Services;
using EnjoyApp.ViewModels;

namespace EnjoyApp.Views
{
    public partial class MeView : UserControl
    {
        public MeView(HabitService habitService)
        {
            InitializeComponent();


            DataContext = new MeViewModel(habitService);

        }
    }
}