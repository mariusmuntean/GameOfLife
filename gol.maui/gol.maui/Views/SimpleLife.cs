using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Graphics;
using System;
using System.Runtime.CompilerServices;

namespace gol.maui.Views
{
    public class SimpleLife : ContentView
    {
        private readonly RelativeLayout _relativeLayout;

        public SimpleLife()
        {
            _relativeLayout = new RelativeLayout();
            _relativeLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
            _relativeLayout.VerticalOptions = LayoutOptions.FillAndExpand;

            Content = _relativeLayout;
            VerticalOptions = LayoutOptions.FillAndExpand;
            HorizontalOptions = LayoutOptions.FillAndExpand;
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
            if (Cells is null)
            {
                _relativeLayout.Children.Clear();
            }
            else
            {
                var rows = Cells.GetLength(0);
                var cols = Cells[0].GetLength(0);

                for (var row = 0; row < rows; row++)
                {
                    for (var col = 0; col < cols; col++)
                    {
                        var localRow = row;
                        var localCol = col;
                        var currentCell = Cells[row][col];
                        _relativeLayout.Children.Add(
                            GetViewForCell(currentCell),
                            Constraint.RelativeToParent(parent => GetCellX(parent, rows, cols, localCol)),
                            Constraint.RelativeToParent(parent => GetCellY(parent, rows, cols, localRow)),
                            Constraint.RelativeToParent(parent => GetCellEdgeLengths(parent, rows, cols).CellEdgeLength),
                            Constraint.RelativeToParent(parent => GetCellEdgeLengths(parent, rows, cols).CellEdgeLength)
                            );
                    }
                }
            }
        }

        private double GetCellX(RelativeLayout parent, int rows, int cols, int localCol)
        {
            var (_, CellEdgeLengthWithSpacing) = GetCellEdgeLengths(parent, rows, cols);
            return localCol * CellEdgeLengthWithSpacing;
        }

        private double GetCellY(RelativeLayout parent, int rows, int cols, int localRow)
        {
            var (_, CellEdgeLengthWithSpacing) = GetCellEdgeLengths(parent, rows, cols);
            return localRow * CellEdgeLengthWithSpacing;
        }

        private (double CellEdgeLength, double CellEdgeLengthWithSpacing) GetCellEdgeLengths(RelativeLayout parent, int rows, int cols)
        {
            var horizontalCellEdgeLengthWithSpacing = parent.Width / cols;
            var verticalCellEdgeLengthWithSpacing = parent.Height / rows;

            var cellEdgeLengthWithSpacing = Math.Min(horizontalCellEdgeLengthWithSpacing, verticalCellEdgeLengthWithSpacing);
            return (0.9f * cellEdgeLengthWithSpacing, cellEdgeLengthWithSpacing);
        }

        private View GetViewForCell(Models.Cell currentCell)
        {
            BoxView boxView = new BoxView()
            {
                Color = currentCell.CurrentState switch
                {
                    Models.CellState.Dead => Colors.Black,
                    Models.CellState.Alive => Colors.Red,
                    _ => Colors.White
                }
            };

            // Use a DataTrigger to toggle the BoxView color to red when the cell's current state is Alive
            DataTrigger trigger = new DataTrigger(typeof(BoxView));
            trigger.Binding = new Binding()
            {
                Source = currentCell,
                Path = nameof(Models.Cell.CurrentState)
            };
            trigger.Value = Models.CellState.Alive;
            trigger.Setters.Add(new Setter
            {
                Property = BoxView.ColorProperty,
                Value = Colors.Red
            });

            boxView.Triggers.Add(trigger);

            // When the BoxView is clicked, invoke the ClickedCommand command
            var clickGestureRecognizer = new TapGestureRecognizer();
            clickGestureRecognizer.Tapped += (s, e) =>
            {
                this.ClickedCommand?.Execute(currentCell);
            };
            boxView.GestureRecognizers.Add(clickGestureRecognizer);

            return boxView;
        }
    }
}