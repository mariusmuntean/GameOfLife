using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;

namespace gol.maui.Extensions
{
    static class GraphicsViewExtensions
    {
        public static void Invalidate(this GraphicsView graphicView)
        {
#if WINDOWS10_0_18362_0_OR_GREATER
            if (graphicView?.Handler is GraphicsViewHandler gvh)
            {
                gvh.NativeView.Invalidate();
            }

#elif IOS
            if (graphicView?.Handler is GraphicsViewHandler gvh)
            {
                gvh.NativeView.InvalidateDrawable();
            }

#elif ANDROID
            if (graphicView?.Handler is GraphicsViewHandler gvh)
            {
                gvh.NativeView.Invalidate();
            }
#endif
        }
    }
}
