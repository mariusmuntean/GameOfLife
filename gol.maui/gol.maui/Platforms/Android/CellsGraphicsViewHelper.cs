using Android.App;
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
                        handler.NativeView.Touch += (s, e) =>
                        {
                            if (e.Event.Action == Android.Views.MotionEventActions.Up)
                            {
                                var dispM = Application.Context.Resources.DisplayMetrics;
                                var scaledX = e.Event.GetX() / dispM.Density;
                                var scaledY = e.Event.GetY() / dispM.Density;
                                cellsGraphicsView.HandleClick(new PointF(scaledX, scaledY));
                            }
                        };
                    }
                });
        }
    }
}