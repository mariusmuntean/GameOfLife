using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Graphics;
using System;
using System.Runtime.CompilerServices;

namespace gol.maui.Views
{
    public class SimpleLife : ContentView
    {
        private RelativeLayout _relativeLayout;

        public SimpleLife()
        {
            InitContent();

            VerticalOptions = LayoutOptions.FillAndExpand;
            HorizontalOptions = LayoutOptions.FillAndExpand;
        }

        private void InitContent()
        {
            _relativeLayout = new RelativeLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            Content = _relativeLayout;
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
            if (Cells is not null)
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

            // Use DataTriggers to toggle the BoxView color to red/black when the cell's current state is Alive/Dead
            DataTrigger aliveStateTrigger = GetColorTrigger(currentCell, Models.CellState.Alive, Colors.Red);
            DataTrigger deadStateTrigger = GetColorTrigger(currentCell, Models.CellState.Dead, Colors.Black);
            boxView.Triggers.Add(aliveStateTrigger);
            boxView.Triggers.Add(deadStateTrigger);

            // When the BoxView is clicked, invoke the ClickedCommand command
            var clickGestureRecognizer = new TapGestureRecognizer();
            clickGestureRecognizer.Tapped += (s, e) =>
            {
                this.ClickedCommand?.Execute(currentCell);
            };
            boxView.GestureRecognizers.Add(clickGestureRecognizer);

            return boxView;
        }

        private static DataTrigger GetColorTrigger(Models.Cell currentCell, Models.CellState cellStateToTriggerOn, Color backgroundColor)
        {
            DataTrigger trigger = new DataTrigger(typeof(BoxView));
            trigger.Binding = new Binding()
            {
                Source = currentCell,
                Path = nameof(Models.Cell.CurrentState)
            };
            trigger.Value = cellStateToTriggerOn;
            trigger.Setters.Add(new Setter
            {
                Property = BoxView.ColorProperty,
                Value = backgroundColor
            });
            return trigger;
        }
    }
}