using System;
using Tree;

class Program
{
    static void Main()
    {
        var tree = new QuadTree(4);
        tree.SetAlive(0, 0);
        tree.SetAlive(1, 0);
        tree.SetAlive(2, 0);

        while (true)
        {
            Console.Clear();
            tree.PrintToConsole(10, 10);
            tree.Step();
            Thread.Sleep(500);
        }

    }
}


// y вниз 
// х вправо