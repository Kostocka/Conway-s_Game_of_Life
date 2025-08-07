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