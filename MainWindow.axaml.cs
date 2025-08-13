using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Threading;
using System;

namespace Treee
{
    public partial class MainWindow : Window
    {
        private QuadTree tree = new QuadTree(4);
        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();

   tree.SetAlive(0, 0);
    tree.SetAlive(0, 1);
    tree.SetAlive(1, 0);
    tree.SetAlive(-1, 0);
    tree.SetAlive(0, -1); 

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += (_, __) =>
            {
                tree.Step();
                DrawTree();
            };
            timer.Start();

            GameCanvas.PointerPressed += OnPointerPressed;
        }

        private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            var point = e.GetPosition(GameCanvas);
            int x = (int)point.X / 10;
            int y = (int)point.Y / 10;
            tree.SetAlive(x, y);
            DrawTree();
        }

        private void DrawTree()
        {
            GameCanvas.Children.Clear();
            long width = 80;
            long height = 60;

            for (long y = -height / 2; y < height / 2; y++)
            {
                for (long x = -width / 2; x < width / 2; x++)
                {
                    if (tree.GetCellAlive(x, y))
                    {
                        var rect = new Rectangle
                        {
                            Width = 10,
                            Height = 10,
                            Fill = Brushes.Green
                        };
                        Canvas.SetLeft(rect, (x + width / 2) * 10);
                        Canvas.SetTop(rect, (y + height / 2) * 10);
                        GameCanvas.Children.Add(rect);
                    }
                }
            }
        }
    }
}
