using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using Avalonia.Interactivity;
using System.Threading.Tasks;
using Avalonia;
using System.Linq;

namespace Treee;

public partial class MainWindow : Window
{
    private QuadTree tree = new QuadTree(4);

    private QuadTree initialTree;

    private DispatcherTimer timer;

    private double zoom = 1.0;
    private double offsetX = 0;
    private double offsetY = 0;

    private bool isDragging = false;
    private Point lastDragPoint;

    public MainWindow()
    {
        InitializeComponent();

        Opened += (_, __) => DrawTree();

        StepButton.Click += (_, __) => { tree.Step(); DrawTree(); };
        PauseButton.Click += (_, __) => { timer.Stop(); UpdateButtons(); };
        PlayButton.Click += (_, __) =>
        {
            if (initialTree == null)
                initialTree = tree.Clone();
            timer.Start();
            UpdateButtons();
        };
        RestartButton.Click += (_, __) =>
        {
            if (initialTree != null)
            {
                tree = initialTree.Clone();
                timer.Stop();
                UpdateButtons();
                DrawTree();
            }
        };

        SpeedSlider.PropertyChanged += (_, e) =>
        {
            if (e.Property == Slider.ValueProperty)
            {
                var ms = (int)SpeedSlider.Value;
                timer.Interval = TimeSpan.FromMilliseconds(ms);
            }
        };

        ClearButton.Click += (_, __) => { timer.Stop(); ClearTree(); DrawTree(); UpdateButtons(); };
        SettingsButton.Click += async (_, __) =>
        {
            timer.Stop();

            var settingsWindow = new SettingsWindow
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            await settingsWindow.ShowDialog(this);

            UpdateButtons();
        };


        GameCanvas.PointerWheelChanged += OnPointerWheelChanged;
        GameCanvas.PointerMoved += OnPointerMoved;
        GameCanvas.PointerReleased += OnPointerReleased;

        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromMilliseconds(500);
        timer.Tick += (_, __) =>
        {
            if (!tree.Step())
            {
                timer.Stop();
                UpdateButtons();
            }
            DrawTree();
        };
        GameCanvas.PointerPressed += OnPointerPressed;
        UpdateButtons();
    }

    private void DrawTree()
    {
        GameCanvas.Children.Clear();

        int cellSize = 10;

        double canvasWidth = GameCanvas.Bounds.Width;
        double canvasHeight = GameCanvas.Bounds.Height;

        int startX = (int)(offsetX - canvasWidth / (2 * cellSize * zoom));
        int endX = (int)(offsetX + canvasWidth / (2 * cellSize * zoom));
        int startY = (int)(offsetY - canvasHeight / (2 * cellSize * zoom));
        int endY = (int)(offsetY + canvasHeight / (2 * cellSize * zoom));

        for (int x = startX; x <= endX; x++)
        {
            var line = new Line
            {
                StartPoint = new Point((x - offsetX) * cellSize * zoom + canvasWidth / 2, 0),
                EndPoint = new Point((x - offsetX) * cellSize * zoom + canvasWidth / 2, canvasHeight),
                Stroke = Brushes.Gray,
                StrokeThickness = 0.5
            };
            GameCanvas.Children.Add(line);
        }

        for (int y = startY; y <= endY; y++)
        {
            var line = new Line
            {
                StartPoint = new Point(0, (y - offsetY) * cellSize * zoom + canvasHeight / 2),
                EndPoint = new Point(canvasWidth, (y - offsetY) * cellSize * zoom + canvasHeight / 2),
                Stroke = Brushes.Gray,
                StrokeThickness = 0.5
            };
            GameCanvas.Children.Add(line);
        }

        for (int y = startY; y <= endY; y++)
        {
            for (int x = startX; x <= endX; x++)
            {
                if (tree.GetCellAlive(x, y))
                {
                    var rect = new Rectangle
                    {
                        Width = cellSize * zoom,
                        Height = cellSize * zoom,
                        Fill = Brushes.Green
                    };
                    Canvas.SetLeft(rect, (x - offsetX) * cellSize * zoom + canvasWidth / 2);
                    Canvas.SetTop(rect, (y - offsetY) * cellSize * zoom + canvasHeight / 2);
                    GameCanvas.Children.Add(rect);
                }
            }
        }
    }

    private void OnPointerPressed(object sender, PointerPressedEventArgs e)
    {
        var point = e.GetPosition(GameCanvas);

        if (e.KeyModifiers.HasFlag(KeyModifiers.Control) && e.GetCurrentPoint(GameCanvas).Properties.IsLeftButtonPressed)
        {
            isDragging = true;
            lastDragPoint = point;
            return;
        }

        if (!e.KeyModifiers.HasFlag(KeyModifiers.Control))
        {
            SetCellAtPoint(point, e);
        }
    }

    private void OnPointerMoved(object sender, PointerEventArgs e)
    {
        var point = e.GetPosition(GameCanvas);
        int cellSize = 10;

        if (isDragging)
        {
            double dx = (point.X - lastDragPoint.X) / (cellSize * zoom);
            double dy = (point.Y - lastDragPoint.Y) / (cellSize * zoom);

            offsetX -= dx;
            offsetY -= dy;

            lastDragPoint = point;
            DrawTree();
        }
        else if (!e.KeyModifiers.HasFlag(KeyModifiers.Control))
        {
            var props = e.GetCurrentPoint(GameCanvas).Properties;

            if (props.IsLeftButtonPressed || props.IsRightButtonPressed)
            {
                SetCellAtPoint(point, e);
            }
        }
    }

    private void SetCellAtPoint(Point point, PointerEventArgs e)
    {
        int cellSize = 10;
        double canvasWidth = GameCanvas.Bounds.Width;
        double canvasHeight = GameCanvas.Bounds.Height;

        double gridX = (point.X - canvasWidth / 2) / (cellSize * zoom) + offsetX;
        double gridY = (point.Y - canvasHeight / 2) / (cellSize * zoom) + offsetY;

        int x = (int)Math.Floor(gridX);
        int y = (int)Math.Floor(gridY);

        var props = e.GetCurrentPoint(GameCanvas).Properties;
        if (props.IsLeftButtonPressed)
            tree.SetAlive(x, y);
        else if (props.IsRightButtonPressed)
            tree.UnsetAlive(x, y);

        DrawTree();
    }


    private void OnPointerReleased(object sender, PointerReleasedEventArgs e)
    {
        isDragging = false;
    }


    private void OnPointerWheelChanged(object sender, PointerWheelEventArgs e)
    {
        var pos = e.GetPosition(GameCanvas);
        double oldZoom = zoom;
        zoom += e.Delta.Y > 0 ? 0.1 : -0.1;
        zoom = Math.Clamp(zoom, 0.2, 5.0);

        offsetX += (pos.X - GameCanvas.Bounds.Width / 2) / (10 * oldZoom) - (pos.X - GameCanvas.Bounds.Width / 2) / (10 * zoom);
        offsetY += (pos.Y - GameCanvas.Bounds.Height / 2) / (10 * oldZoom) - (pos.Y - GameCanvas.Bounds.Height / 2) / (10 * zoom);

        DrawTree();
    }

    private void UpdateButtons()
    {
        if (timer.IsEnabled)
        {
            PlayButton.Background = Brushes.Gray;
            PauseButton.Background = Brushes.Red;
        }
        else
        {
            PlayButton.Background = Brushes.Green;
            PauseButton.Background = Brushes.Gray;
        }
    }

    public void ClearTree()
    {
        initialTree = null;
        tree = new QuadTree(4); 
    }


}

