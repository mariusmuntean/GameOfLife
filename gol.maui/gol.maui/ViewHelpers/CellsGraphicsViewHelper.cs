using System;
using gol.maui.Views;

namespace gol.maui.ViewHelpers
{
    public static class CellsGraphicsViewHelper
    {
        public static void AddClickHandler()
        {
            Console.WriteLine($"If you see this in the console then there's no click handler for the {nameof(CellsGraphicsView)} on this platform.");
        }
    }
}