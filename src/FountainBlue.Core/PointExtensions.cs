using System.Drawing;

namespace FountainBlue.Core
{
    public static class PointExtensions
    {
        /// <summary>
        ///     Computes a position relative to the specified area.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="area">The area.</param>
        /// <returns>The location.</returns>
        public static Point RelativeTo(this Point point, Rectangle area)
        {
            return new Point(point.X + area.X, point.Y + area.Y);
        }
    }
}