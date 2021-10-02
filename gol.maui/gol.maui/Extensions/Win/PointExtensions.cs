#if WINDOWS10_0_19041_0

using Microsoft.Maui.Graphics;

namespace gol.maui.Extensions.Win
{
    public static class PointExtensions
    {
        public static PointF AsPointF(this Windows.Foundation.Point point)
        {
            return new PointF(((float)point.X), (float)point.Y);
        }
    }
}

#endif