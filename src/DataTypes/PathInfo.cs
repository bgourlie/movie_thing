namespace DataTypes
{
    public class PathInfo
    {
        public readonly Edge Edge;
        public readonly int ParentNode;

        public PathInfo(int parentNode, Edge edge)
        {
            Edge = edge;
            ParentNode = parentNode;
        }
    }
}