# Conway's Game of Life

**Conway’s Game of Life** is a famous **cellular automaton** where each cell lives or dies based on simple rules, creating fascinating patterns. 
For more information, see the [Wikipedia article](https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life).


This implementation uses a **QuadTree backend** for an infinite grid and provides an **interactive GUI** built with [AvaloniaUI](https://avaloniaui.net/).

## 🔹 What is this game about?

 The Game of Life is played on an **infinite two-dimensional grid** consisting of square cells. Each cell can be either **alive** or **dead**. The game progresses in distinct stages called **generations**. At each generation, the state of each cell is updated simultaneously based on the data of its neighbors, following a set of rules. 

Although the rules are simple, the game can exhibit "non-standard behavior" by creating stable structures, oscillators, and patterns that move across the grid ("spaceships"). This is what makes it interesting.

---

## 🧩 Rules

For each grid cell:

1. **Underpopulation:** Any live cell with fewer than two live neighbors dies. 
2. **Survival:** Any living cell with two or three living neighbors survives. 
3. **Overpopulation:** Any living cell with more than three living neighbors dies. 
4. **Reproduction:** Any dead cell that has exactly three living neighbors comes back to life.

## 🔹 Game Features
- Infinite grid representation using a **QuadTree**
- Interactive drawing of alive/dead cells with the mouse
- **Zoom** in/out and **drag** the field with smooth panning
- Simulation can run in real-time or step-by-step
- **etc.**

## 🎮 Controls

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
- **Clear** - clearing the field and the initial field
- **Settings** - you can change colors and slider borders

## ⚙️ Project Info

## **Dependencies:**
The script will automatically pack and pull everything and you don't need to download anything, but still
- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download) or higher
- AvaloniaUI packages (automatically restored):
  - Avalonia (11.3.3)
  - Avalonia.Desktop (11.3.3)
  - Avalonia.ReactiveUI (11.3.3)
  - Avalonia.Themes.Fluent (11.3.3) 
  - Microsoft.NET.ILLink.Tasks (8.0.17)
  - System.Text.Json   (9.0.8)


### 🚀Build & Run

In the project root, there is a folder called **Executable Files**
You can run the game by double-clicking it

If you are on MacOS, you should download the **MacOS Silicon** folder from **Executable Files** and you will see a regular file for your operating system

For Windows, you will immediately see an exe file

OR
For developers
If you are on MacOS you are lucky that too because I made a script that automates and you just have to write in the root this

```bash
./publish.sh