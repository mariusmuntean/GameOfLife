using Microsoft.Maui.Graphics;

namespace gol.maui
{
    public static class PointExtensions
    {
        public static PointF AsPointF(this Windows.Foundation.Point point)
        {
            return new PointF((float)point.X, (float)point.Y);
        }
    }
}