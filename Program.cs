using Tree;

class Program
{
    static void Main()
    {
        var tree = new QuadTree(3); // поле 8x8
        tree.SetAlive(1, 2, 0);
        tree.SetAlive(2, 2, 0);
        tree.SetAlive(3, 2, 0);

        tree.PrintToConsole(0, 0, 8, 8);

    }
}

// y вниз 
// х вправо