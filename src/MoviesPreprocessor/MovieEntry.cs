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

		public override string ToString()
		{
			return string.Format("{0} [{1}] starring {2}", Title, Year, string.Join(", ", Cast));
		}
	}
}