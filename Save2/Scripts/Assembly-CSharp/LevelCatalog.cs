using System;

// Runtime numbers are stable save/asset slots, not menu positions or release IDs.
public static class LevelCatalog
{
	public sealed class Level
	{
		public int WorldNumber { get; private set; }
		public int SourceNumber { get; private set; }
		public string ResourceRoot { get; private set; }
		public string Title { get; private set; }
		public int MaxGems { get; private set; }
		public bool IsExpansion { get { return WorldNumber >= 9; } }
		// Donor IDs, not menu positions. Keep the explicit catalogue entries below
		// compatible with export_expansion_maps.py's constructor parser.
		public int MaxCrowns
		{
			get
			{
				return IsExpansion && ((SourceNumber >= 16 && SourceNumber <= 30) || SourceNumber == 32 ||
					(SourceNumber >= 37 && SourceNumber <= 40) || SourceNumber == 42) ? 3 : 0;
			}
		}

		public Level(int worldNumber, int sourceNumber, string title, int maxGems, string resourceRoot = null)
		{
			WorldNumber = worldNumber;
			SourceNumber = sourceNumber;
			ResourceRoot = resourceRoot ?? (sourceNumber <= 20 ? "Classic1471" : sourceNumber <= 25 ? "Classic1561" : "Classic1782");
			Title = title;
			MaxGems = maxGems;
		}
	}

	public const int Count = 10;

	private static readonly Level[] levels =
	{
		new Level(1, 1, "Massif", 20),
		new Level(2, 2, "Sky", 20),
		new Level(3, 3, "Forest", 20),
		new Level(4, 4, "Desert", 20),
		new Level(5, 5, "Volcano", 20),
		new Level(6, 6, "City", 20),
		new Level(7, 7, "Sci-Tech", 20),
		new Level(8, 8, "Tetris", 20),
		new Level(9, 9, "E-labyrinth", 20),
		new Level(10, 10, "Castle in the Sky", 20)
	};

	// https://rolling-sky.fandom.com/wiki/Level (revision 148880), easy to hard.
	// Use our classic variants: Cloud B, Massif B, Illusion B, Forest B,
	// Sunny A, Hoop Dream A (not the 2026 extended B), and Monody A.
	private static readonly int[] menuOrder =
	{
		1, 2, 3, 4, 5, 6, 7, 8, 9, 10, // The Legendary 10
	};

	public static Level Get(int worldNumber)
	{
		if (worldNumber < 1 || worldNumber > Count)
			throw new ArgumentOutOfRangeException("worldNumber");
		return levels[worldNumber - 1];
	}

	public static Level AtMenuIndex(int index)
	{
		return Get(menuOrder[index]);
	}

	public static int MenuIndexOf(int worldNumber)
	{
		Get(worldNumber);
		return Array.IndexOf(menuOrder, worldNumber);
	}

	public static int NextWorld(int worldNumber)
	{
		return AtMenuIndex((MenuIndexOf(worldNumber) + 1) % Count).WorldNumber;
	}
}
