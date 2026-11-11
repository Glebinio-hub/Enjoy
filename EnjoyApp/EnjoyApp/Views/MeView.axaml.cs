using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using EnjoyApp.ViewModels;

namespace EnjoyApp.Views
{
    public partial class MeView : UserControl
    {
        public MeView()
        {
            InitializeComponent();

            ReadingHabitCard.DataContext = new HabitCardViewModel
            {
                Name = "Reading",
            };
        }
    }
}