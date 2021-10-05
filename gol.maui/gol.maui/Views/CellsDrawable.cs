using System;
using gol.maui.Models;
using Microsoft.Maui.Graphics;

namespace gol.maui.Views
{
    class CellsDrawable : IDrawable
    {
        RectangleF? _lastDrawnArea;
        (float cellEdgeLength, float cellEdgeLengthWithSpacing)? _lastCellEdgeLengthts;

        public CellsDrawable(Models.Cell[][] cells)
        {
            Cells = cells;
        }

        public Models.Cell[][] Cells { get; }

        public void Draw(ICanvas canvas, RectangleF dirtyRect)
        {
            _lastDrawnArea = null;
            _lastCellEdgeLengthts = null;

            if (Cells is not null)
            {
                var rows = Cells.GetLength(0);
                var cols = Cells[0].GetLength(0);

                _lastCellEdgeLengthts = GetCellEdgeLengths(dirtyRect, rows, cols);
                var (cellEdgeLength, cellEdgeLengthWithSpacing) = _lastCellEdgeLengthts.Value;

                for (var row = 0; row < rows; row++)
                {
                    for (var col = 0; col < cols; col++)
                    {
                        var currentCell = Cells[row][col];

                        canvas.FillColor = currentCell.CurrentState switch
                        {
                            CellState.Dead => Colors.Black,
                            CellState.Alive => Colors.Red,
                            _ => Colors.Fuchsia
                        };

                        // Determine the rectangle to fill for the current cell
                        var currentCellRect = new RectangleF(col * cellEdgeLengthWithSpacing, row * cellEdgeLengthWithSpacing, cellEdgeLength, cellEdgeLength);
                        canvas.FillRectangle(currentCellRect);

                        // Enlarge the drawn area with the current cell rectangle
                        _lastDrawnArea = _lastDrawnArea switch
                        {
                            null => currentCellRect,
                            _ => _lastDrawnArea.Value.Union(currentCellRect)
                        };
                    }
                }
            }
        }

        public (int row, int column)? CheckClickLocation(PointF clickLocation)
        {
            if (Cells is null || _lastDrawnArea is null || _lastCellEdgeLengthts is null)
            {
                return null;
            }

            if (!_lastDrawnArea.Value.Contains(clickLocation))
            {
                return null;
            }

            // The rectangle might not be anchored at (0,0), so I'll translate it to the origin to make the computations simpler
            var xDiff = _lastDrawnArea.Value.Left;
            var yDiff = _lastDrawnArea.Value.Top;

            var lastDrawnAreaFromOrigin = _lastDrawnArea.Value.Offset(-xDiff, -yDiff);

            // I'll translate the clickLocation point as well
            var clickLocationAtOrigin = clickLocation.Offset(-xDiff, -yDiff);

            // Let's consider the cell edge with the spacing such that even clicking between cells will assign the click to a cell
            var col = (int)Math.Truncate(clickLocationAtOrigin.X / _lastCellEdgeLengthts.Value.cellEdgeLengthWithSpacing);
            var row = (int)Math.Truncate(clickLocationAtOrigin.Y / _lastCellEdgeLengthts.Value.cellEdgeLengthWithSpacing);

            return (row, col);
        }

        private static (float CellEdgeLength, float CellEdgeLengthWithSpacing) GetCellEdgeLengths(RectangleF parent, int rows, int cols)
        {
            var horizontalCellEdgeLengthWithSpacing = parent.Width / cols;
            var verticalCellEdgeLengthWithSpacing = parent.Height / rows;

            var cellEdgeLengthWithSpacing = Math.Min(horizontalCellEdgeLengthWithSpacing, verticalCellEdgeLengthWithSpacing);
            return (0.9f * cellEdgeLengthWithSpacing, cellEdgeLengthWithSpacing);
        }
    }
}