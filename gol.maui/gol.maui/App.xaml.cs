using gol.maui.Views;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using Application = Microsoft.Maui.Controls.Application;

namespace gol.maui
{
    public partial class App : Application
    {
        public App(MainPage mainPage)
        {
            InitializeComponent();

#if WINDOWS10_0_19041_0
            Microsoft.Maui.Handlers.ViewHandler.ViewMapper.Add("x", (handler, view) =>
            {
                if (view is SimpleLife2)
                {
                    handler.NativeView.Tapped += (s, e) => Console.WriteLine(e.GetPosition(null));
                }
            });
#endif

            MainPage = mainPage;
        }
    }
}
