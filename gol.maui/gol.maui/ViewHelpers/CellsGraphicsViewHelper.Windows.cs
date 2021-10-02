#if WINDOWS10_0_19041_0

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
                        handler.NativeView.Tapped += (s,e) => cellsGraphicsView.HandleClick(e.GetPosition(null));
                    }
                });
        }
    }
}
#endif