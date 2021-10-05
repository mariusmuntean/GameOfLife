using System;
using System.Text.Json.Serialization;
using Microsoft.Maui.Controls;

namespace gol.maui.Models
{
    public class Cell : BindableObject
    {
        private CellState currentState = CellState.Dead;

        /// <summary>
        /// For the deserializer.
        /// </summary>
        /// <param name="currentState"></param>
        /// <param name="nextState"></param>
        [JsonConstructor]
        public Cell(CellState currentState, CellState nextState)
        {
            CurrentState = currentState;
            NextState = nextState;
        }

        public Cell(CellState currentCellState = CellState.Dead)
        {
        }

        public CellState CurrentState
        {
            get => currentState;
            private set
            {
                if (currentState == value)
                {
                    return;
                }

                currentState = value;
                OnPropertyChanged();
            }
        }

        public CellState NextState { get; set; } = CellState.Dead;

        public void Tick()
        {
            CurrentState = NextState;
            NextState = CellState.Dead;
        }

        public void Toggle() => CurrentState = CurrentState switch
        {
            CellState.Alive => CellState.Dead,
            CellState.Dead => CellState.Alive,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}