using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GlobalShareTask.Test.ModelsTests
{
    [TestClass]
    public class AreaTests
    {
        [TestMethod]
        public void Constructor_NullStartingPoint_ExceptionThrown()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new Area(null, 10, 10));
        }

        [TestMethod]
        public void Constructor_ZeroWidth_ExceptionThrown()
        {
            Assert.ThrowsException<PlatformException>(() => new Area(new Point(0,0), 0, 10));
        }

        [TestMethod]
        public void Constructor_ZeroHeight_ExceptionThrown()
        {
            Assert.ThrowsException<PlatformException>(() => new Area(new Point(0, 0), 10, 0));
        }

        [TestMethod]
        public void Contains_PointInsideArea_True()
        {
            Area area = new Area(new Point(0, 0), 10, 10);
            Assert.IsTrue(area.Contains(new Point(5, 5)));
        }

        [TestMethod]
        public void Contains_PointOutsideArea_True()
        {
            Area area = new Area(new Point(0, 0), 10, 10);
            Assert.IsFalse(area.Contains(new Point(15,15)));
        }
    }
}
