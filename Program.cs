using System;
using Tree;

class Program
{
    static void Main()
    {
        var tree = new QuadTree(3); // поле 8x8

        // Блинкер (осциллятор) по центру 8x8
        tree.SetAlive(1, 2);
        tree.SetAlive(2, 2);
        tree.SetAlive(3, 2);

        Console.WriteLine("Печать 8x8 начиная с (0,0):");
        tree.PrintToConsole(60, 60);
        Console.WriteLine();

        Console.WriteLine("Добавим точку далеко справа-внизу:");
        tree.SetAlive(50, 50); // вне текущего root, проверка ExpandRoot
        tree.PrintToConsole(150, 150);
        Console.WriteLine();

        Console.WriteLine("Добавим точку далеко вверх-слева (отрицательная область):");
        tree.SetAlive(-5, -3);
        tree.PrintToConsole(80, 80);
        Console.WriteLine();

        Console.WriteLine("Отключим точку (2,2):");
        tree.UnsetAlive(2, 2);
        tree.PrintToConsole(80, 80);
    }
}


// y вниз 
// х вправо