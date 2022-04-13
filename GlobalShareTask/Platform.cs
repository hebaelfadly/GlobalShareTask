using GlobalShareTask.Contracts;
using GlobalShareTask.Models;
using System;

namespace GlobalShareTask
{
    public class Platform : IPlatform
    {
        private readonly Area landingArea;
        private readonly Area platformArea;
        private readonly uint separationPoints;
        private Area reservedArea;

        private Object thisLock = new Object();

        public Platform(Area landingArea, Area platformArea, uint separationPoints)
        {
            this.landingArea = landingArea ?? throw new ArgumentNullException(nameof(landingArea));
            this.platformArea = platformArea ?? throw new ArgumentNullException(nameof(platformArea));
            this.separationPoints = separationPoints;
            Validate(landingArea, platformArea);
        }

        public LandingStatus GetLandingStatus(Point point)
        {
            lock (thisLock)
            {
                _ = point ?? throw new ArgumentNullException(nameof(point));

                if (!platformArea.Contains(point))
                    return LandingStatus.OutOfPlatform;

                if (reservedArea != null && reservedArea.Contains(point))
                    return LandingStatus.Clash;

                SetReservedArea(point);

                return LandingStatus.OkForLanding;
            }
        }

        private static void Validate(Area landingArea, Area platformArea)
        {
            if (platformArea.EndPoint.X > landingArea.EndPoint.X)
                throw new PlatformException($"Platform width should not exceed {landingArea.Width - platformArea.StartPoint.X}");

            if (platformArea.EndPoint.Y > landingArea.EndPoint.Y)
                throw new PlatformException($"Platform hieght should not exceed {landingArea.Height - platformArea.StartPoint.Y}");
        }

        private void SetReservedArea(Point position)
        {
            Point reservedAreaStartPoint = new Point(position.X - separationPoints, position.Y - separationPoints);
            uint reservedAreaWidth = 2 * separationPoints + 1;
            reservedArea = new Area(reservedAreaStartPoint, reservedAreaWidth, reservedAreaWidth);
        }
    }
}