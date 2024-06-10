using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace MonoGame.Tests.Framework
{
    public class PointTest
    {
        [Test]
        public void Deconstruct()
        {
            Point point = new Point(int.MinValue, int.MaxValue);


            point.Deconstruct(out int x, out int y);

            Assert.AreEqual(x, point.X);
            Assert.AreEqual(y, point.Y);
        }
    }
}
