
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;

namespace EnjoyApp.Controls
{
    public partial class EnjoyCheckBox : UserControl
    {
        public static readonly StyledProperty<bool> IsCheckedProperty =
            AvaloniaProperty.Register<EnjoyCheckBox, bool>(
                nameof(IsChecked),
                defaultValue: false,
                defaultBindingMode: BindingMode.TwoWay);
        public event EventHandler<RoutedEventArgs>? IsCheckedChanged;

        public bool IsChecked
        {
            get => GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

        public EnjoyCheckBox()
        {
            InitializeComponent();

            IsCheckedProperty.Changed.AddClassHandler<EnjoyCheckBox>(
     (checkBox, args) =>
     {
         checkBox.UpdateVisualState();
         checkBox.IsCheckedChanged?.Invoke(
             checkBox,
             new RoutedEventArgs());
     });
        }

        private void CheckBoxButton_Click(
            object? sender,
            RoutedEventArgs e)
        {
            IsChecked = !IsChecked;
        }

        private void UpdateVisualState()
        {
            var border = this.FindControl<Border>("CheckMarkBox");

            if (border is null)
                return;

            border.Background = IsChecked
                ? new SolidColorBrush(Color.Parse("#702F40"))
                : new SolidColorBrush(Color.Parse("#F7F2E9"));
        }
    }
}
