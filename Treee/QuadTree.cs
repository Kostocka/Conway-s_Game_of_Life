namespace Treee;

class QuadTree
{
    public QuadTreeNode root;
    
    private long originX = 0;
    private long originY = 0;
    
    public QuadTree(int maxLevel)
    {
        root = new QuadTreeNode(maxLevel);
    }

    public void SetAlive(long x, long y)
    {
        while (!PointInsideRoot(x, y))
        {
            ExpandRoot(x, y);
        }

        long size = 1L << root.Level;
        SetAliveRecursive(root, originX, originY, size, x, y);
    }

    public QuadTreeNode GetNode(QuadTreeNode node, long x0, long y0, long size, long x, long y)
    {
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
                return GetNode(node.SW, x0, y0 + half, half, x, y);
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
    
    public void UnsetAlive(long x, long y)
    {
        QuadTreeNode temp = GetNode(root, originX, originY, 1L << root.Level, x, y);
        temp.Alive = false;
    }

    public void PrintToConsole(long width, long height)
    {
        long x0 = -width / 2;
        long y0 = -height / 2;


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

    public void Step()
    {
        var candidates = new HashSet<(long x, long y)>();
        var currentAlive = new HashSet<(long x, long y)>();

        CollectAliveCells(root, originX, originY, 1L << root.Level, currentAlive);

        foreach (var (x, y) in currentAlive)
        {
            for (long dx = -1; dx <= 1; dx++)
                for (long dy = -1; dy <= 1; dy++)
                    candidates.Add((x + dx, y + dy));
        }

        var nextTree = new QuadTree(root.Level);

        foreach (var (x, y) in candidates)
        {
            int count = CountAliveNeighbors(x, y);
            bool isAlive = GetCellAlive(x, y);
            if (isAlive && (count == 2 || count == 3))
            {
                nextTree.SetAlive(x, y);
            }
            else if (!isAlive && count == 3)
            {
                nextTree.SetAlive(x, y);
            }
        }

        this.root = nextTree.root;
        this.originX = nextTree.originX;
        this.originY = nextTree.originY;

    }

    private int CountAliveNeighbors(long x, long y)
    {
        int count = 0;
        for (long dx = -1; dx <= 1; dx++)
        {
            for (long dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;
                if (GetCellAlive(x + dx, y + dy))
                    count++;
            }
        }

        return count;
    }

    private void CollectAliveCells(QuadTreeNode node, long x0, long y0, long size, HashSet<(long, long)> result)
    {
        if (node == null)
            return;

        if (node.Level == 0)
        {
            if (node.Alive)
                result.Add((x0, y0));
            return;
        }

        long half = size / 2;

        CollectAliveCells(node.NW, x0, y0, half, result);
        CollectAliveCells(node.NE, x0 + half, y0, half, result);
        CollectAliveCells(node.SW, x0, y0 + half, half, result);
        CollectAliveCells(node.SE, x0 + half, y0 + half, half, result);
    }


    private QuadTreeNode? TryGetLeaf(QuadTreeNode node, long x0, long y0, long size, long x, long y)
    {
        if (node == null)
            return null;

        if (node.Level == 0)
            return node;

        long half = size / 2;

        if (x < x0 + half)
        {
            if (y < y0 + half)
            {
                return node.NW == null ? null : TryGetLeaf(node.NW, x0, y0, half, x, y);
            }
            else
            {
                return node.SW == null ? null : TryGetLeaf(node.SW, x0, y0 + half, half, x, y);
            }
        }
        else
        {
            if (y < y0 + half)
            {
                return node.NE == null ? null : TryGetLeaf(node.NE, x0 + half, y0, half, x, y);
            }
            else
            {
                return node.SE == null ? null : TryGetLeaf(node.SE, x0 + half, y0 + half, half, x, y);
            }
        }
    }


    private void CollapseDeadBranches(QuadTreeNode node)
    {
        if (node == null || node.IsLeaf)
            return;

        CollapseDeadBranches(node.NW);
        CollapseDeadBranches(node.NE);
        CollapseDeadBranches(node.SW);
        CollapseDeadBranches(node.SE);

        if (AllChildrenDeadLeaves(node))
        {
            node.NW = node.NE = node.SW = node.SE = null;
            node.Alive = false;
        }
    }

    private bool AllChildrenDeadLeaves(QuadTreeNode node)
    {
        return node.NW != null && node.NE != null && node.SW != null && node.SE != null &&
            node.NW.IsLeaf && !node.NW.Alive &&
            node.NE.IsLeaf && !node.NE.Alive &&
            node.SW.IsLeaf && !node.SW.Alive &&
            node.SE.IsLeaf && !node.SE.Alive;
    }

    public bool GetCellAlive(long x, long y)
    {
        long size = 1L << root.Level;

        if (!PointInsideRoot(x, y))
            return false;

        var leaf = TryGetLeaf(root, originX, originY, size, x, y);
        return leaf != null && leaf.Alive;
    }


    private bool PointInsideRoot(long x, long y)
    {
        long size = 1L << root.Level;
        return x >= originX && y >= originY && x < originX + size && y < originY + size;
    }

    private void ExpandRoot(long x, long y)
    {
        int newLevel = root.Level + 1;
        var newRoot = new QuadTreeNode(newLevel);

        long half = 1L << (newLevel - 1);

        if (x < originX + half)
        {
            if (y < originY + half)
            {
                newRoot.SE = root;
                originX -= half;
                originY -= half;
            }
            else
            {
                newRoot.NE = root;
                originX -= half;
            }
        }
        else
        {
            if (y < originY + half)
            {
                newRoot.SW = root;
                originY -= half;
            }
            else
            {
                newRoot.NW = root;
            }
        }

        root = newRoot;
    }



    private void SetAliveRecursive(QuadTreeNode node, long x0, long y0, long size, long x, long y)
    {

        if (node.Level == 0)
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
                SetAliveRecursive(node.NW, x0, y0, half, x, y);
            }
            else
            {
                if (node.SW == null)
                    node.SW = new QuadTreeNode(node.Level - 1);
                SetAliveRecursive(node.SW, x0, y0 + half, half, x, y);
            }
        }
        else
        {
            if (y < y0 + half)
            {
                if (node.NE == null)
                    node.NE = new QuadTreeNode(node.Level - 1);
                SetAliveRecursive(node.NE, x0 + half, y0, half, x, y);
            }
            else
            {
                if (node.SE == null)
                    node.SE = new QuadTreeNode(node.Level - 1);
                SetAliveRecursive(node.SE, x0 + half, y0 + half, half, x, y);
            }
        }
    }

}