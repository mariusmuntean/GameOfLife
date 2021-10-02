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
            AddGlider();

            CellClickedCommand = new Command<Models.Cell>(cell => cell.Toggle(), cell => cell is not null);
            TickCommand = new Command(() => Life?.Tick(), () => Life is not null);
            ClearCommand = new Command(() =>
            {
                Life = new Life(Life?.Cells.GetLength(0) ?? 10, Life?.Cells[0]?.GetLength(0) ?? 10);
                AddGlider();
            });
        }

        private void AddGlider()
        {
            Life.Toggle(2, 2);
            Life.Toggle(3, 2);
            Life.Toggle(4, 2);
            Life.Toggle(4, 1);
            Life.Toggle(3, 0);
        }

        public Command<Models.Cell> CellClickedCommand { get; set; }
        public Command TickCommand { get; set; }
        public Command ClearCommand { get; set; }

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
