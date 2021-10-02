using gol.maui.Models;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;
using System;
using System.Runtime.CompilerServices;

namespace gol.maui.Views
{
    public class SimpleLife2 : ContentView
    {
        GraphicsView _cellGraphics;

        public SimpleLife2()
        {
            InitContent();

            VerticalOptions = LayoutOptions.FillAndExpand;
            HorizontalOptions = LayoutOptions.FillAndExpand;
        }

        private void InitContent()
        {
            // ToDo: migrate from RelativeLayout to GraphicsView + custom handler for determining the exact click location

            _cellGraphics = new CellsGraphicsView(Cells, (a) =>
            {
                Console.WriteLine(a);
            });

            Content = _cellGraphics;


            if (Cells is not null)
            {
                var rows = Cells.GetLength(0);
                var cols = Cells[0].GetLength(0);

                for (var row = 0; row < rows; row++)
                {
                    for (var col = 0; col < cols; col++)
                    {
                        var currentCell = Cells[row][col];

                        DataTrigger trigger = new DataTrigger(typeof(SimpleLife2));
                        trigger.Binding = new Binding()
                        {
                            Source = currentCell,
                            Path = nameof(Models.Cell.CurrentState)
                        };
                        trigger.Value = CellState.Alive;
                        trigger.EnterActions.Add(new CC(() =>
                        {
                            (_cellGraphics.Handler as GraphicsViewHandler)?.NativeView.UpdateDrawable(_cellGraphics);
                        }));

                        this.Triggers.Add(trigger);
                    }
                }
            }

        }

        class CC : TriggerAction<VisualElement>
        {
            private readonly Action action;

            public CC(Action action)
            {
                this.action = action;
            }
            protected override void Invoke(VisualElement sender)
            {
                action?.Invoke();
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

        public Command<Models.Cell> ClickedCommand
        {
            get => (Command<Models.Cell>)GetValue(ClickedCommandProperty);
            set => SetValue(ClickedCommandProperty, value);
        }

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
            InitContent();
        }
    }
}