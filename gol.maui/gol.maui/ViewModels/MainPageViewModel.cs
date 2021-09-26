using gol.maui.Models;
using Microsoft.Maui.Controls;

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

            CellClickedCommand = new Command<Models.Cell>(cell => cell.Toggle(), cell => cell is not null);
            TickCommand = new Command(() => Life?.Tick(), () => Life is not null);
        }

        public Command<Models.Cell> CellClickedCommand { get; set; }
        public Command TickCommand { get; set; }

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
