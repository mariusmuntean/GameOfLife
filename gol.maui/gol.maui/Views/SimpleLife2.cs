using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Runtime.CompilerServices;

namespace gol.maui.Views
{
    public class SimpleLife2 : ContentView
    {
        public SimpleLife2()
        {
            InitContent();

            VerticalOptions = LayoutOptions.FillAndExpand;
            HorizontalOptions = LayoutOptions.FillAndExpand;
        }

        private void InitContent()
        {
            // ToDo: migrate from RelativeLayout to GraphicsView + custom handler for determining the exact click location

            GraphicsView graphicsView = new GraphicsView
            {
                Drawable = new c(Cells),
            };

            Content = graphicsView;
        }

        class c : IDrawable
        {
            public c(Models.Cell[][] cells)
            {
                Cells = cells;
            }

            public Models.Cell[][] Cells { get; }

            public void Draw(ICanvas canvas, RectangleF dirtyRect)
            {
                if (Cells is not null)
                {
                    var rows = Cells.GetLength(0);
                    var cols = Cells[0].GetLength(0);

                    var (CellEdgeLength, CellEdgeLengthWithSpacing) = GetCellEdgeLengths(dirtyRect, rows, cols);

                    for (var row = 0; row < rows; row++)
                    {
                        for (var col = 0; col < cols; col++)
                        {
                            var localRow = row;
                            var localCol = col;
                            var currentCell = Cells[row][col];

                            canvas.FillColor = currentCell.CurrentState switch
                            {
                                Models.CellState.Dead => Colors.Black,
                                Models.CellState.Alive => Colors.Red,
                            };
                            canvas.FillRectangle(
                                col * CellEdgeLengthWithSpacing,
                                row * CellEdgeLengthWithSpacing,
                                CellEdgeLength,
                                CellEdgeLength
                                );

                        }
                    }
                }
            }

            private (float CellEdgeLength, float CellEdgeLengthWithSpacing) GetCellEdgeLengths(RectangleF parent, int rows, int cols)
            {
                var horizontalCellEdgeLengthWithSpacing = parent.Width / cols;
                var verticalCellEdgeLengthWithSpacing = parent.Height / rows;

                var cellEdgeLengthWithSpacing = Math.Min(horizontalCellEdgeLengthWithSpacing, verticalCellEdgeLengthWithSpacing);
                return (0.9f * cellEdgeLengthWithSpacing, cellEdgeLengthWithSpacing);
            }
        }

        public static BindableProperty CellsProperty = BindableProperty.Create(
            nameof(Cells),
            typeof(Models.Cell[][]),
            typeof(SimpleLife),
            null
            );

        public Models.Cell[][] Cells
        {
            get => (Models.Cell[][])GetValue(CellsProperty);
            set => SetValue(CellsProperty, value);
        }

        public static BindableProperty ClickedCommandProperty = BindableProperty.Create(
            nameof(ClickedCommand),
            typeof(Command<Models.Cell>),
            typeof(SimpleLife),
            null
            );

        public Command<Models.Cell> ClickedCommand { get => (Command<Models.Cell>)GetValue(ClickedCommandProperty); set => SetValue(ClickedCommandProperty, value); }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(Cells))
            {
                OnCellsChanged();
            }
        }

        private void OnCellsChanged()
        {
            RedrawChildren();
        }

        private void RedrawChildren()
        {
            InitContent();
        }
    }
}