using Application = Microsoft.Maui.Controls.Application;

namespace gol.maui
{
    public partial class App : Application
    {
        public App(MainPage mainPage)
        {
            InitializeComponent();

            MainPage = mainPage;
        }
    }
}
