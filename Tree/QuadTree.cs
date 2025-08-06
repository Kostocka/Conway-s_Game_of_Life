namespace Tree;
class QuadTreeNode
{
    public QuadTreeNode NW, NE, SW, SE;
    public int Level { get; }
    public bool Alive;

    public bool IsLeaf => NW == null && NE == null && SW == null && SE == null;
    public bool IsAlive => IsLeaf && Alive;

    public QuadTreeNode(int level, bool alive = false)
    {
        Level = level;
        Alive = alive;
    }

    public QuadTreeNode(int level, QuadTreeNode nw, QuadTreeNode ne, QuadTreeNode sw, QuadTreeNode se)
    {
        Level = level;
        NW = nw;
        NE = ne;
        SW = sw;
        SE = se;
    }
    
}

class QuadTree
{
    public QuadTreeNode root;
    public QuadTree(int maxLevel){
        root = new QuadTreeNode(maxLevel);
    }

    public void SetAlive(long x, long y, int targetLevel)
    {
        while (!PointInsideRoot(x, y))
        {
            ExpandRoot(x, y);
        }

        long size = 1L << root.Level;
        SetAliveRecursive(root, 0, 0, size, x, y, targetLevel);
    }

    public QuadTreeNode GetNode(QuadTreeNode node, long x0, long y0, long size, long x, long y)
    {
        if (!PointInsideRoot(x, y))
            return null;
            
        if (node.Level == 0)
            return node;

        long half = size / 2;

        if (x < x0 + half)
        {
            if (y < y0 + half)
            {
                if (node.NW == null)
                    node.NW = new QuadTreeNode(node.Level - 1);
                return GetNode(node.NW, x0, y0, half, x, y);
            }
            else
            {
                if (node.SW == null)
                    node.SW = new QuadTreeNode(node.Level - 1);
                return GetNode(node.SW, x0, y0 + half , half, x, y );
            }
        }
        else
        {
            if (y < y0 + half)
            {
                if (node.NE == null)
                    node.NE = new QuadTreeNode(node.Level - 1);
                return GetNode(node.NE, x0 + half, y0, half, x, y);
            }
            else
            {
                if (node.SE == null)
                    node.SE = new QuadTreeNode(node.Level - 1);
                return GetNode(node.SE, x0 + half, y0 + half, half, x, y);
            }
        }
    }

    public void PrintToConsole(long x0, long y0, long width, long height)
    {
        for (long y = y0; y < y0 + height; y++)
        {
            for (long x = x0; x < x0 + width; x++)
            {
                bool alive = GetCellAlive(x, y);
                Console.Write(alive ? 'O' : '.');
            }
            Console.WriteLine();
        }
    }

    private bool GetCellAlive(long x, long y)
    {
        long size = 1L << root.Level;

        if (!PointInsideRoot(x, y))
            return false;

        return GetNode(root, 0, 0, size, x, y).Alive ;
    }



    private bool PointInsideRoot(long x, long y)
    {
        long size = 1L << root.Level;
        return x >= 0 && y >= 0 && x < size && y < size;
    }
    private void ExpandRoot(long x, long y)
    {
        int newLevel = root.Level + 1;
        var newRoot = new QuadTreeNode(newLevel);

        long half = 1L << (newLevel - 1);
        if (x < half)
        {
            if (y < half)
            {
                newRoot.SE = root;
            }
            else
            {
                newRoot.NE = root;
            }
        }
        else
        {
            if (y < half)
            {
                newRoot.SW = root;
            }
            else
            {
                newRoot.NW = root;
            }
        }
        root = newRoot;
    }

    private void SetAliveRecursive(QuadTreeNode node, long x0, long y0, long size, long x, long y, int targetLevel)
    {

        if (node.Level == targetLevel)
        {
            node.Alive = true;
            return;
        }

        long half = size / 2;

        if (x < x0 + half)
        {
            if (y < y0 + half)
            {
                if (node.NW == null)
                    node.NW = new QuadTreeNode(node.Level - 1);
                SetAliveRecursive(node.NW, x0, y0, half, x, y, targetLevel);
            }
            else
            {
                if (node.SW == null)
                    node.SW = new QuadTreeNode(node.Level - 1);
                SetAliveRecursive(node.SW, x0, y0 + half, half, x, y, targetLevel);
            }
        }
        else
        {
            if (y < y0 + half)
            {
                if (node.NE == null)
                    node.NE = new QuadTreeNode(node.Level - 1);
                SetAliveRecursive(node.NE, x0 + half, y0, half, x, y, targetLevel);
            }
            else
            {
                if (node.SE == null)
                    node.SE = new QuadTreeNode(node.Level - 1);
                SetAliveRecursive(node.SE, x0 + half, y0 + half, half, x, y, targetLevel);
            }
        }
    }

}