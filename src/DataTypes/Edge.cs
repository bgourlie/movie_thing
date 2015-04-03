namespace DataTypes
{
	public class Edge
	{
		public readonly int MovieId;
		public readonly byte Distance;

		public Edge(int movieId, byte distance)
		{
			MovieId = movieId;
			Distance = distance;
		}

		public override string ToString()
		{
			return string.Format("Id: {0} Distance: {1}", MovieId, Distance);
		}
	}
}