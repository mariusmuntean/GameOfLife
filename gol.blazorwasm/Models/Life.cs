using System;
using System.Collections.Generic;
using System.Linq;

namespace gol.blazorwasm.Models
{
    using CellsChanged = Action<Cell[][]>;

    public class Life
    {
        private readonly Cell[][] _cells;
        private readonly int _rows;
        private readonly int _cols;

        private readonly CellsChanged _onNewGeneration;

        public Life(int rows, int cols, CellsChanged onNewGeneration = null)
        {
            if (rows <= 0 || cols <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rows) + " " + nameof(cols), "the rows and columns cannot be 0 or less");
            }

            _rows = rows;
            _cols = cols;
            _cells = new Cell[rows][];
            for (var row = 0; row < rows; row++)
            {
                _cells[row] ??= new Cell[cols];
                for (var col = 0; col < cols; col++)
                {
                    _cells[row][col] = new Cell(CellState.Dead);
                }
            }

            _onNewGeneration = onNewGeneration;
        }

        public Life(Cell[][] initialCells, CellsChanged onNewGeneration = null)
        {
            var newRows = initialCells.GetLength(0);
            var newCols = initialCells.GetLength(0);

            if (newRows <= 0 || newCols <= 0)
            {
                throw new ArgumentOutOfRangeException("one or both dimensions of the provided 2d array is 0");
            }

            _cells = initialCells;
            _rows = newRows;
            _cols = newCols;
            _onNewGeneration = onNewGeneration;
        }

        public Cell[][] Cells => _cells;

        public void Tick()
        {
            // Compute the next state for foreach cell
            for (var row = 0; row < _rows; row++)
            {
                for (var col = 0; col < _cols; col++)
                {
                    var currentCell = _cells[row][col];
                    var cellNeighbors = GetCellNeighbors(row, col);
                    var liveCellNeighbors = cellNeighbors.Count(cell => cell.CurrentState == CellState.Alive);

                    // Rule source - https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life#Rules
                    if (currentCell.CurrentState == CellState.Alive && (liveCellNeighbors == 2 || liveCellNeighbors == 3))
                    {
                        currentCell.NextState = CellState.Alive;
                    }
                    else if (currentCell.CurrentState == CellState.Dead && liveCellNeighbors == 3)
                    {
                        currentCell.NextState = CellState.Alive;
                    }
                    else
                    {
                        currentCell.NextState = CellState.Dead;
                    }
                }
            }

            // Switch to the next state for each cell
            for (var row = 0; row < _rows; row++)
            {
                for (var col = 0; col < _cols; col++)
                {
                    var currentCell = _cells[row][col];
                    currentCell.Tick();
                }
            }

            _onNewGeneration?.Invoke(_cells);
        }

        public void Toggle(int row, int col)
        {
            if (row < 0 || row >= _rows)
            {
                throw new ArgumentOutOfRangeException(nameof(row));
            }

            if (col < 0 || col >= _cols)
            {
                throw new ArgumentOutOfRangeException(nameof(col));
            }

            _cells[row][col].Toggle();
            _onNewGeneration?.Invoke(_cells);
        }

        private List<Cell> GetCellNeighbors(int row, int col)
        {
            var neighbors = new List<Cell>(8);

            for (int rowOffset = -1; rowOffset <= 1; rowOffset++)
            {
                for (int colOffset = -1; colOffset <= 1; colOffset++)
                {
                    if (rowOffset == 0 && colOffset == 0)
                    {
                        // Skip self
                        continue;
                    }

                    var neighborRow = row + rowOffset;
                    var neighborCol = col + colOffset;
                    if (neighborRow < 0 || neighborRow >= _rows)
                    {
                        continue;
                    }

                    if (neighborCol < 0 || neighborCol >= _cols)
                    {
                        continue;
                    }

                    neighbors.Add(_cells[neighborRow][neighborCol]);
                }
            }

            return neighbors;
        }
    }
}