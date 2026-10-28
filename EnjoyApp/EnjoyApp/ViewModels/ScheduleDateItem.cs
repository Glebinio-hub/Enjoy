using System;
using System.ComponentModel;
using System.Globalization;

namespace EnjoyApp.ViewModels
{
    public class ScheduleDateItem : INotifyPropertyChanged
    {
        private bool _isSelected;

        public event PropertyChangedEventHandler? PropertyChanged;

        public DateTimeOffset Date { get; }

        public string DayLabel =>
            Date.ToString(
                "ddd",
                CultureInfo.GetCultureInfo("en-US")
            ).ToUpperInvariant();

        public string DayNumber =>
            Date.Day.ToString(CultureInfo.InvariantCulture);

        public bool IsToday =>
            Date.Date == DateTimeOffset.Now.Date;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value)
                    return;

                _isSelected = value;

                PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs(nameof(IsSelected))
                );
            }
        }

        public ScheduleDateItem(DateTimeOffset date)
        {
            Date = date;
        }
    }
}