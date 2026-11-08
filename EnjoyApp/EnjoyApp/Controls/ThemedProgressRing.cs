using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace EnjoyApp.Controls;

/// <summary>
/// Полный контур + акцентная дуга. Progress от 0 до 100.
/// Не заменяет существующий ProgressRing, чтобы не затронуть другие экраны.
/// </summary>
public class ThemedProgressRing : Control
{
    public static readonly StyledProperty<double> ProgressProperty =
        AvaloniaProperty.Register<ThemedProgressRing, double>(nameof(Progress));

    public static readonly StyledProperty<IBrush?> TrackBrushProperty =
        AvaloniaProperty.Register<ThemedProgressRing, IBrush?>(nameof(TrackBrush));

    public static readonly StyledProperty<IBrush?> AccentBrushProperty =
        AvaloniaProperty.Register<ThemedProgressRing, IBrush?>(nameof(AccentBrush));

    static ThemedProgressRing()
    {
        AffectsRender<ThemedProgressRing>(ProgressProperty, TrackBrushProperty, AccentBrushProperty);
    }

    public double Progress
    {
        get => GetValue(ProgressProperty);
        set => SetValue(ProgressProperty, value);
    }

    public IBrush? TrackBrush
    {
        get => GetValue(TrackBrushProperty);
        set => SetValue(TrackBrushProperty, value);
    }

    public IBrush? AccentBrush
    {
        get => GetValue(AccentBrushProperty);
        set => SetValue(AccentBrushProperty, value);
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);
        const double thickness = 10;
        double radius = Math.Max(0, Math.Min(Bounds.Width, Bounds.Height) / 2 - thickness / 2);
        if (radius <= 0) return;

        var center = new Point(Bounds.Width / 2, Bounds.Height / 2);
        if (TrackBrush is not null)
            context.DrawEllipse(null, new Pen(TrackBrush, thickness), center, radius, radius);

        double progress = Math.Clamp(Progress, 0, 100);
        if (progress <= 0 || AccentBrush is null) return;
        var accentPen = new Pen(AccentBrush, thickness, lineCap: PenLineCap.Round);
        if (progress >= 100)
        {
            context.DrawEllipse(null, accentPen, center, radius, radius);
            return;
        }

        double endAngle = -Math.PI / 2 + (progress / 100) * Math.PI * 2;
        double x1 = center.X;
        double y1 = center.Y - radius;
        double x2 = center.X + radius * Math.Cos(endAngle);
        double y2 = center.Y + radius * Math.Sin(endAngle);
        int largeArc = progress > 50 ? 1 : 0;
        string F(double value) => value.ToString("0.######", CultureInfo.InvariantCulture);
        string path = $"M {F(x1)},{F(y1)} A {F(radius)},{F(radius)} 0 {largeArc} 1 {F(x2)},{F(y2)}";
        context.DrawGeometry(null, accentPen, StreamGeometry.Parse(path));
    }
}
