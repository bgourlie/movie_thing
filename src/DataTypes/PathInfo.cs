namespace DataTypes
{
    public class PathInfo
    {
        public readonly Edge Edge;
        public readonly int Node;
        public readonly int ParentNode;

        public PathInfo(int node, int parentNode, Edge edge)
        {
            Node = node;
            Edge = edge;
            ParentNode = parentNode;
        }
    }
}