#if MACCATALYST

using gol.maui.Views;
using Microsoft.Maui.Graphics.Native;

namespace gol.maui.ViewHelpers
{
    public static class CellsGraphicsViewHelper
    {
        public static void AddClickHandler()
        {
            Microsoft.Maui.Handlers.GraphicsViewHandler.ViewMapper.Add("ClickFoo", (handler, view) =>
                {
                    if (view is CellsGraphicsView cellsGraphicsView)
                    {
                        UIKit.UITapGestureRecognizer gestureRecognizer = new(
                            g =>
                            {
                                var clickLocation = g.LocationInView(g.View).AsPointF();
                                cellsGraphicsView.HandleClick(clickLocation);
                            }
                            );
                        handler.NativeView.AddGestureRecognizer(gestureRecognizer);
                    }
                });
        }
    }
}
#endif

#if IOS

using gol.maui.Views;
using Microsoft.Maui.Graphics.Native;

namespace gol.maui.ViewHelpers
{
    public static class CellsGraphicsViewHelper
    {
        public static void AddClickHandler()
        {
            Microsoft.Maui.Handlers.GraphicsViewHandler.ViewMapper.Add("ClickFoo", (handler, view) =>
            {
                if (view is CellsGraphicsView cellsGraphicsView)
                {
                    UIKit.UITapGestureRecognizer gestureRecognizer = new(
                        g =>
                        {
                            var clickLocation = g.LocationInView(g.View).AsPointF();
                            cellsGraphicsView.HandleClick(clickLocation);
                        }
                        );
                    handler.NativeView.AddGestureRecognizer(gestureRecognizer);
                }
            });
        }
    }
}
#endif

#if ANDROID

using gol.maui.Views;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Native;

namespace gol.maui.ViewHelpers
{
    public static class CellsGraphicsViewHelper
    {
        public static void AddClickHandler()
        {
            Microsoft.Maui.Handlers.GraphicsViewHandler.ViewMapper.Add("ClickFoo", (handler, view) =>
                {
                    if (view is CellsGraphicsView cellsGraphicsView)
                    {
                        handler.NativeView.Touch += (s,e) => cellsGraphicsView.HandleClick(new PointF(e.Event.RawX, e.Event.RawY));
                    }
                });
        }
    }
}
#endif