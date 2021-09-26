using gol.maui.Models;
using Microsoft.Maui.Controls;
using System;

namespace gol.maui.ViewModels
{
    public class MainPageViewModel : BindableObject
    {
        private Life life;

        public MainPageViewModel()
        {
            Life = new Life(30, 40);

            // Glider
            Life.Toggle(2, 2);
            Life.Toggle(3, 2);
            Life.Toggle(4, 2);
            Life.Toggle(4, 1);
            Life.Toggle(3, 0);

            Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            {
                Life.Toggle(11, 11);
                return false;
            });
        }

        public Life Life
        {
            get => life;
            set
            {
                life = value;
                OnPropertyChanged();
            }
        }

    }
}
