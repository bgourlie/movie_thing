namespace MoviesPreprocessor
{
	class MovieEntry
	{
		public readonly int Id;
		public readonly string Title;
		public readonly string[] Cast;
		public readonly ushort Year;

		public MovieEntry(int id, string title, ushort year, string[] cast)
		{
			Id = id;
			Title = title;
			Year = year;
			Cast = cast;
		}

	    protected bool Equals(MovieEntry other)
	    {
	        return Id == other.Id;
	    }

	    public override bool Equals(object obj)
	    {
	        if (ReferenceEquals(null, obj)) return false;
	        if (ReferenceEquals(this, obj)) return true;
	        if (obj.GetType() != this.GetType()) return false;
	        return Equals((MovieEntry) obj);
	    }

	    public override int GetHashCode()
	    {
	        return Id;
	    }

	    public static bool operator ==(MovieEntry left, MovieEntry right)
	    {
	        return Equals(left, right);
	    }

	    public static bool operator !=(MovieEntry left, MovieEntry right)
	    {
	        return !Equals(left, right);
	    }

	    public override string ToString()
		{
			return string.Format("{0} [{1}] starring {2}", Title, Year, string.Join(", ", Cast));
		}
	}
}