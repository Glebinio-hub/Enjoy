using System;
using System.Collections.Generic;
using Avalonia;
using System.Text;
using Avalonia.Controls;
using Avalonia.Media;
namespace EnjoyApp.Controls
{
    public class ProgressRing : Control
    {
        public static readonly StyledProperty<double> ProgressProperty =
     AvaloniaProperty.Register<ProgressRing, double>(nameof(Progress));

        public double Progress
        {
            get => GetValue(ProgressProperty);
            set => SetValue(ProgressProperty, value);
        }

        static ProgressRing()
        {
            ProgressProperty.Changed.AddClassHandler<ProgressRing>((ring, _) => ring.InvalidateVisual());
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            var center = new Point(
                Bounds.Width / 2,
                Bounds.Height / 2
            );

            var radius = Math.Min(
                Bounds.Width,
                Bounds.Height
            ) / 2 - 10;

            var pen = new Pen(
                Brushes.SlateGray,
                12
            );

            double angle = Progress / 100.0 * 360.0;

            double startAngle = -90;
            double endAngle = startAngle + angle;

            double startRadians = startAngle * Math.PI / 180;
            double endRadians = endAngle * Math.PI / 180;

            var startPoint = new Point(
                center.X + radius * Math.Cos(startRadians),
                center.Y + radius * Math.Sin(startRadians)
            );

            var endPoint = new Point(
                center.X + radius * Math.Cos(endRadians),
                center.Y + radius * Math.Sin(endRadians)
            );

            var geometry = new StreamGeometry();

            using (var ctx = geometry.Open())
            {
                ctx.BeginFigure(startPoint, false);

                ctx.ArcTo(
                    endPoint,
                    new Size(radius, radius),
                    0,
                    angle > 180,
                    SweepDirection.Clockwise
                );
            }

            context.DrawGeometry(
                null,
                pen,
                geometry
            );
        }

    }
}

