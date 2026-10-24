using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Avalonia.VisualTree;
using System;
using System.Diagnostics;
using System.Linq;

namespace EnjoyApp.Controls
{

    public partial class EnjoyTimePicker : UserControl
    {
        private ScrollViewer? _hoursScrollViewer;
        private ScrollViewer? _minutesScrollViewer;

        private readonly DispatcherTimer _hoursSnapTimer = new()
        {
            Interval = TimeSpan.FromMilliseconds(180)
        };

        private readonly DispatcherTimer _minutesSnapTimer = new()
        {
            Interval = TimeSpan.FromMilliseconds(180)
        };

        public static readonly StyledProperty<TimeOnly> SelectedTimeProperty =
   AvaloniaProperty.Register<EnjoyTimePicker, TimeOnly>(
       nameof(SelectedTime),
       defaultValue: new TimeOnly(9, 0),
       defaultBindingMode: BindingMode.TwoWay);

        private bool _isUpdatingSelection;

        private bool _isUpdatingFromScroll;

        public TimeOnly SelectedTime
        {
            get => GetValue(SelectedTimeProperty);
            set => SetValue(SelectedTimeProperty, value);
        }

        public event EventHandler? CancelRequested;
        public event EventHandler? DoneRequested;

        private void CancelButtonClick(object? sender, RoutedEventArgs e)
        {
            CancelRequested?.Invoke(this, EventArgs.Empty);
        }

        private void DoneButtonClick(object? sender, RoutedEventArgs e)
        {
            DoneRequested?.Invoke(this, EventArgs.Empty);
        }
        static EnjoyTimePicker()
        {
            SelectedTimeProperty.Changed.AddClassHandler<EnjoyTimePicker>(
                (picker, _) => picker.UpdateWheelSelection());
        }


        public EnjoyTimePicker()
        {
            InitializeComponent();

            _hoursSnapTimer.Tick += (_, _) =>
            {
                _hoursSnapTimer.Stop();
                SnapWheel(HoursList, _hoursScrollViewer);
            };

            _minutesSnapTimer.Tick += (_, _) =>
            {
                _minutesSnapTimer.Stop();
                SnapWheel(MinutesList, _minutesScrollViewer);
            };

            HoursList.ItemsSource = Enumerable.Range(0, 24)
                .Select(hour => hour.ToString("D2"))
                .ToList();

            MinutesList.ItemsSource = Enumerable.Range(0, 60)
                .Select(minute => minute.ToString("D2"))
                .ToList();

            HoursList.SelectionChanged += TimeSelectionChanged;
            MinutesList.SelectionChanged += TimeSelectionChanged;

            UpdateWheelSelection();
            AttachedToVisualTree += EnjoyTimePicker_AttachedToVisualTree;
            DetachedFromVisualTree += EnjoyTimePicker_DetachedFromVisualTree;
        }


        private void TimeSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (_isUpdatingSelection)
                return;

            if (HoursList.SelectedItem is not string hour ||
                MinutesList.SelectedItem is not string minute)
                return;

            SelectedTime = new TimeOnly(
                int.Parse(hour),
                int.Parse(minute));
        }

        private void EnjoyTimePicker_AttachedToVisualTree(
    object? sender,
    VisualTreeAttachmentEventArgs e)
        {
            Dispatcher.UIThread.Post(() =>
            {
                HoursList.ApplyTemplate();
                MinutesList.ApplyTemplate();

                _hoursScrollViewer = HoursList
                    .GetVisualDescendants()
                    .OfType<ScrollViewer>()
                    .FirstOrDefault();

                _minutesScrollViewer = MinutesList
                    .GetVisualDescendants()
                    .OfType<ScrollViewer>()
                    .FirstOrDefault();

                Debug.WriteLine($"Hours ScrollViewer: {_hoursScrollViewer is not null}");
                Debug.WriteLine($"Minutes ScrollViewer: {_minutesScrollViewer is not null}");

                if (_hoursScrollViewer is not null)
                    _hoursScrollViewer.PropertyChanged += HoursScrollChanged;

                if (_minutesScrollViewer is not null)
                    _minutesScrollViewer.PropertyChanged += MinutesScrollChanged;

                CenterSelectedTime();

            }, DispatcherPriority.Loaded);
        }

        private void HoursScrollChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property != ScrollViewer.OffsetProperty || _isUpdatingSelection)
                return;

            UpdateSelectionFromScroll(_hoursScrollViewer, HoursList, 23);
            _hoursSnapTimer.Stop();
            _hoursSnapTimer.Start();
        }

        private void MinutesScrollChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property != ScrollViewer.OffsetProperty || _isUpdatingSelection)
                return;

            UpdateSelectionFromScroll(_minutesScrollViewer, MinutesList, 59);
            _minutesSnapTimer.Stop();
            _minutesSnapTimer.Start();
        }

        private void UpdateSelectionFromScroll(
    ScrollViewer? scrollViewer,
    ListBox listBox,
    int maxValue)
        {
            if (scrollViewer is null)
                return;

            const double itemHeight = 52;

            int index = (int)Math.Round(
                scrollViewer.Offset.Y / itemHeight,
                MidpointRounding.AwayFromZero);

            index = Math.Clamp(index, 0, maxValue);

            string value = index.ToString("D2");

            if (listBox.SelectedItem as string != value)
            {
                _isUpdatingFromScroll = true;

                try
                {
                    listBox.SelectedItem = value;
                }
                finally
                {
                    _isUpdatingFromScroll = false;
                }
            }
        }

        private void UpdateWheelSelection()
        {
            if (_isUpdatingSelection || _isUpdatingFromScroll)
                return;

            _isUpdatingSelection = true;

            try
            {
                HoursList.SelectedItem = SelectedTime.Hour.ToString("D2");
                MinutesList.SelectedItem = SelectedTime.Minute.ToString("D2");
                CenterSelectedTime();
            }
            finally
            {
                _isUpdatingSelection = false;
            }
        }

        public void CenterSelectedTime()
        {
            Dispatcher.UIThread.Post(() =>
            {
                if (_hoursScrollViewer is null || _minutesScrollViewer is null)
                    return;

                _isUpdatingSelection = true;

                try
                {
                    CenterWheel(HoursList, _hoursScrollViewer);
                    CenterWheel(MinutesList, _minutesScrollViewer);
                }
                finally
                {
                    _isUpdatingSelection = false;
                }
            }, DispatcherPriority.Loaded);
        }

        private void CenterWheel(ListBox listBox, ScrollViewer? scrollViewer)
        {
            if (scrollViewer is null || listBox.SelectedItem is null)
                return;

            listBox.UpdateLayout();

            var container = listBox.ContainerFromItem(listBox.SelectedItem);

            if (container is null)
                return;

            var position = container.TranslatePoint(
                new Avalonia.Point(0, 0),
                scrollViewer);

            if (position is null)
                return;

            double itemCenter = position.Value.Y + container.Bounds.Height / 2;
            double viewportCenter = scrollViewer.Bounds.Height / 2;

            double newOffset = scrollViewer.Offset.Y + itemCenter - viewportCenter;

            newOffset = Math.Clamp(
                newOffset,
                0,
                Math.Max(0, scrollViewer.Extent.Height - scrollViewer.Viewport.Height));

            scrollViewer.Offset = new Avalonia.Vector(
                scrollViewer.Offset.X,
                newOffset);
        }

        private void EnjoyTimePicker_DetachedFromVisualTree(
    object? sender,
    VisualTreeAttachmentEventArgs e)

        {
            _hoursSnapTimer.Stop();
            _minutesSnapTimer.Stop();

            if (_hoursScrollViewer is not null)
                _hoursScrollViewer.PropertyChanged -= HoursScrollChanged;

            if (_minutesScrollViewer is not null)
                _minutesScrollViewer.PropertyChanged -= MinutesScrollChanged;

            _hoursScrollViewer = null;
            _minutesScrollViewer = null;
        }

        private void SnapWheel(ListBox listBox, ScrollViewer? scrollViewer)
        {
            if (scrollViewer is null || listBox.SelectedItem is null)
                return;

            _isUpdatingSelection = true;

            try
            {
                CenterWheel(listBox, scrollViewer);
            }
            finally
            {
                _isUpdatingSelection = false;
            }
        }


    }
}