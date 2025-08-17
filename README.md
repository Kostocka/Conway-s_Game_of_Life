# Conway's Game of Life

This project is a **Conway’s Game of Life** implementation with a **QuadTree backend** and an **GUI** built with [AvaloniaUI](https://avaloniaui.net/)


## Features
- Infinite grid representation using a **QuadTree**
- Interactive drawing of alive/dead cells with the mouse
- **Zoom** in/out and **drag** the field with smooth panning
- Simulation can run in real-time or step-by-step
- **etc.**

## Controls

- **Left click** – set a cell alive
- **Right click** – clear a cell
- **Ctrl + Left drag** – move the grid
- **Mouse wheel** – zoom in/out, cursor stays on the same cell

## UI Buttons

- **Step** – simulation by one generation
- **Start** – run the simulation continuously
  - Green when idle (ready to start)
  - Gray when running
- **Pause** – stop the simulation
  - Red while running
- **Restart** – reset to the initial state (generation 0) defined before the first run

## Getting Started
- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download) or higher
- AvaloniaUI packages (automatically restored):
  - Avalonia (11.3.3)
  - Avalonia.Desktop (11.3.3)
  - Avalonia.ReactiveUI (11.3.3)
  - Avalonia.Themes.Fluent (11.3.3)

### Build & Run
```bash
git clone https://github.com/Kostocka/Conway-s_Game_of_Life
cd Conway-s_Game_of_Life
dotnet run