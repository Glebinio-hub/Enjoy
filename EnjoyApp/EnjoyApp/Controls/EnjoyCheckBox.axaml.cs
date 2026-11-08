
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
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

        static EnjoyCheckBox()
        {
            IsCheckedProperty.Changed.AddClassHandler<EnjoyCheckBox>(
                (checkBox, args) =>
                {
                    checkBox.UpdateVisualState();
                });
        }

        public EnjoyCheckBox()
        {
            InitializeComponent();
            UpdateVisualState();
        }

        private void CheckBoxButton_Click(
            object? sender,
            RoutedEventArgs e)
        {
            IsChecked = !IsChecked;

            IsCheckedChanged?.Invoke(this, e);

            e.Handled = true;
        }

        private void CheckBoxButton_Tapped(
            object? sender,
            Avalonia.Input.TappedEventArgs e)
        {
            e.Handled = true;
        }

        private void UpdateVisualState()
        {
            var border = this.FindControl<Border>("CheckMarkBox");

            if (border is null)
                return;

            border.Classes.Set("checked", IsChecked);
        }
    }
}
