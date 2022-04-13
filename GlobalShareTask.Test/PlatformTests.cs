using GlobalShareTask.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace GlobalShareTask.Test
{
    [TestClass]
    public class PlatformTests
    {
        [TestMethod]
        public void Constructor_NullLandingArea_ExceptionThrown()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new Platform(null, new Area(new(5, 5), 100, 100), 1));
        }

        [TestMethod]
        public void Constructor_NullPlatformArea_ExceptionThrown()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new Platform(new Area(new(0, 0), 100, 100), null, 1));
        }

        [TestMethod]
        public void Constructor_PlatformAreaExceedLandingAreaHorizontally_ExceptionThrown()
        {
            Assert.ThrowsException<PlatformException>(() => new Platform(new Area(new(0, 0), 100, 100), new Area(new(5, 5), 100, 10), 1));
        }

        [TestMethod]
        public void Constructor_PlatformAreaExceedLandingAreaVertically_ExceptionThrown()
        {
            Assert.ThrowsException<PlatformException>(() => new Platform(new Area(new(0, 0), 100, 100), new Area(new(5, 5), 10, 100), 1));
        }

        [TestMethod]
        public void GetLandingStatus_NullPosition_ExceptionThrown()
        {
            Platform platform = new Platform(new Area(new(0, 0), 100, 100), new Area(new(5, 5), 10, 10), 1);
            Assert.ThrowsException<ArgumentNullException>(() => platform.GetLandingStatus(null));
        }

        [TestMethod]
        public void GetLandingStatus_ValidPosition_OkForLanding()
        {
            Platform platform = new Platform(new Area(new(0, 0), 100, 100), new Area(new(5, 5), 10, 10), 1);
            Assert.AreEqual(LandingStatus.OkForLanding, platform.GetLandingStatus(new Point(5, 5)));
        }

        [TestMethod]
        public void GetLandingStatus_InvalidPosition_OutOfPlatform()
        {
            Platform platform = new Platform(new Area(new(0, 0), 100, 100), new Area(new(5, 5), 10, 10), 1);
            Assert.AreEqual(LandingStatus.OutOfPlatform, platform.GetLandingStatus(new Point(20, 20)));
        }

        [TestMethod]
        public void GetLandingStatus_PreviouslyChecked_Clash()
        {
            Platform platform = new Platform(new Area(new(0, 0), 100, 100), new Area(new(5, 5), 10, 10), 1);
            Assert.AreEqual(LandingStatus.OkForLanding, platform.GetLandingStatus(new Point(5, 5)));
            Assert.AreEqual(LandingStatus.Clash, platform.GetLandingStatus(new Point(5, 5)));
        }

        [TestMethod]
        public void GetLandingStatus_NextToPreviouslyChecked_Clash()
        {
            Platform platform = new Platform(new Area(new(0, 0), 100, 100), new Area(new(5, 5), 10, 10), 1);
            Assert.AreEqual(LandingStatus.OkForLanding, platform.GetLandingStatus(new Point(5, 5)));
            Assert.AreEqual(LandingStatus.Clash, platform.GetLandingStatus(new Point(6, 6)));
        }

        [TestMethod]
        public async Task GetLandingStatus_TwoConcurrentRequestsOnSamePostion_CorrectResponseExpected()
        {
            Platform platform = new Platform(new Area(new(0, 0), 100, 100), new Area(new(5, 5), 10, 10), 1);

            Task<LandingStatus> task1 = Task.Factory.StartNew(() => platform.GetLandingStatus(new Point(5, 5)));
            Task<LandingStatus> task2 = Task.Factory.StartNew(() => platform.GetLandingStatus(new Point(5, 5)));

            LandingStatus[] results =await Task.WhenAll(task1, task2);

            Assert.AreEqual(2, results.Length);
            Assert.IsTrue(results.Count(s => s == LandingStatus.OkForLanding) == 1);
            Assert.IsTrue(results.Count(s => s == LandingStatus.Clash) == 1);
        }
    }
}
