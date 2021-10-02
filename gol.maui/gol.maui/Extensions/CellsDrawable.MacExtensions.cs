#if __MACCATALYST__

using System;
using gol.maui.Views;
using Microsoft.Maui.Graphics.Native;

namespace gol.maui.Extensions
{
    public static class CellsDrawableMacExtensions
    {
        public static void AddClickHandler(this CellsGraphicsView cellsDrawable)
        {
            Microsoft.Maui.Handlers.GraphicsViewHandler.ViewMapper.Add("x", (handler, view) =>
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