using gol.maui.Views;

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
						handler.NativeView.Tapped += (s, e) =>
						{
							cellsGraphicsView.HandleClick(e.GetPosition(handler.NativeView).AsPointF());
						};
					}
				});
		}
	}
}