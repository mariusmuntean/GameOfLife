using System;
using System.Text.Json.Serialization;

namespace gol.blazorwasm.Models
{
    public class Cell
    {
        /// <summary>
        /// For the deserializer.
        /// </summary>
        /// <param name="currentState"></param>
        /// <param name="nextState"></param>
        [JsonConstructorAttribute]
        public Cell(CellState currentState, CellState nextState)
        {
            CurrentState = currentState;
            NextState = nextState;
        }

        public Cell(CellState currentCellState = CellState.Dead)
        {
        }

        public CellState CurrentState { get; private set; } = CellState.Dead;
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