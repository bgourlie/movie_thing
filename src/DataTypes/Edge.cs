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

		protected bool Equals(Edge other)
		{
			return MovieId == other.MovieId;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Edge) obj);
		}

		public override int GetHashCode()
		{
			return MovieId;
		}

		public static bool operator ==(Edge left, Edge right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Edge left, Edge right)
		{
			return !Equals(left, right);
		}
	}
}