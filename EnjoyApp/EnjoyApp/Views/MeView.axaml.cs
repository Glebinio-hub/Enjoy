using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using EnjoyApp.Services;
using EnjoyApp.ViewModels;
using System;

namespace EnjoyApp.Views
{
    public partial class MeView : UserControl
    {
        public MeView(HabitService habitService)
        {
            InitializeComponent();


            DataContext = new MeViewModel(habitService);

        }



        private void AddHabit_Click(
        object? sender,
        RoutedEventArgs e)
        {
            if (DataContext is MeViewModel vm)
            {
                vm.OpenCreateHabit();
            }
        }

        private void CancelHabit_Click(
            object? sender,
            RoutedEventArgs e)
        {
            if (DataContext is MeViewModel vm)
            {
                vm.CancelCreateHabit();
            }
        }

        private async void CreateHabit_Click(
            object? sender,
            RoutedEventArgs e)
        {
            if (DataContext is not MeViewModel vm)
                return;

            try
            {
                await vm.CreateHabitAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private void HabitCard_DeleteRequested(
    object? sender,
    Guid habitId)
        {
            if (DataContext is MeViewModel vm)
            {
                vm.OpenDeleteHabit(habitId);
            }
        }

        private void CancelDeleteHabit_Click(
            object? sender,
            RoutedEventArgs e)
        {
            if (DataContext is MeViewModel vm)
            {
                vm.CancelDeleteHabit();
            }
        }

        private void HabitCard_OpenRequested(object? sender, Guid habitId)
        {
            if (DataContext is MeViewModel vm)
            {
                vm.OpenHabitDetails(habitId);
            }
        }

        private void CloseHabitDetails_Click(
            object? sender,
            RoutedEventArgs e)
        {
            if (DataContext is MeViewModel vm)
            {
                vm.CloseHabitDetails();
            }
        }

        private async void ConfirmDeleteHabit_Click(
            object? sender,
            RoutedEventArgs e)
        {
            if (DataContext is MeViewModel vm)
            {
                try
                {
                    await vm.ConfirmDeleteHabitAsync();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
        }
    }
}