using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using gol.maui.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using static gol.maui.MauiProgram;
using Cell = gol.maui.Models.Cell;

namespace gol.maui.ViewModels
{
    public class MainPageViewModel : BindableObject
    {
        private const string Cancel = "Cancel";
        private Life life;
        private readonly DisplayPromptDelegate _chooseFileName;
        private readonly DisplayActionSheetDelegate _displayActionSheet;

        public MainPageViewModel(DisplayPromptDelegate chooseFileName, DisplayActionSheetDelegate displayActionSheet)
        {
            _chooseFileName = chooseFileName;
            _displayActionSheet = displayActionSheet;
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
            SaveCommand = new Command(async () =>
            {
                var saveFileName = await _chooseFileName("Save game state",
                    "Choose a name for the file to hold the current game state",
                    initialValue: $"SaveGame {DateTime.Now:yyyyy-MM-d HH-mm-ss}.json");
                if (string.IsNullOrWhiteSpace(saveFileName)) { return; }

                var saveFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), saveFileName);

                var cellsString = JsonSerializer.Serialize(Life.Cells);

                await File.WriteAllTextAsync(saveFilePath, cellsString);

            });
            OpenCommand = new Command(async () =>
            {
                // find all previous saved game state files and offer them as action sheet items
                var workingFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var previousSavedGameStateFiles = Directory.EnumerateFiles(workingFolderPath).Where(f => f.ToLowerInvariant().EndsWith(".json"));
                if (!previousSavedGameStateFiles.Any())
                {
                    return;
                }

                var chosenFile = await _displayActionSheet("Choose a previous game state", Cancel, null, previousSavedGameStateFiles.ToArray());
                if (string.IsNullOrWhiteSpace(chosenFile) || chosenFile == Cancel)
                {
                    return;
                }

                Life = new Life(JsonSerializer.Deserialize<Cell[][]>(await File.ReadAllTextAsync(chosenFile)));
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
        public Command SaveCommand { get; set; }
        public Command OpenCommand { get; set; }

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
