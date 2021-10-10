using gol.maui.Views;
using Microsoft.Maui.Graphics.Native;

namespace gol.maui
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