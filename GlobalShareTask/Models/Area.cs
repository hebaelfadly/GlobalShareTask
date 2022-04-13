using System;

namespace GlobalShareTask
{
    public class Area
    {
        public Point StartPoint { get; }
        public Point EndPoint { get; }
        public uint Width { get; }
        public uint Height { get; }

        public Area(Point startPoint, uint width, uint height)
        {
            StartPoint = startPoint ?? throw new ArgumentNullException(nameof(startPoint));
            if (width == 0)
                throw new PlatformException("Width should be greater than zero");
            if (height == 0)
                throw new PlatformException("Height should be greater than zero");

            Width = width;
            Height = height;
            EndPoint = new(width + startPoint.X, height + startPoint.Y);
        }

        public bool Contains(Point point)
        {
            return !(point.X < StartPoint.X
                || point.X > EndPoint.X
                || point.Y < StartPoint.Y
                || point.Y > EndPoint.Y);
        }
    }

    public record Point(uint X, uint Y);
}