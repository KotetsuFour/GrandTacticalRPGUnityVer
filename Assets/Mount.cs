public class Mount
{
	public static Mount HORSE = new Mount(0, 9, 5,
				new WorldMapTile.WorldMapTileType[] { WorldMapTile.WorldMapTileType.PLAIN },
				0.8, 30, 31, 20, 61, false, "Horse",
				"Common beasts of burden. They are relatively simple to raise and tame.");
	public static Mount UNICORN = new Mount(1, 9, 10,
			new WorldMapTile.WorldMapTileType[] { WorldMapTile.WorldMapTileType.DENSE_FOREST },
				0.2, 20, 41, 0, 51, false, "Unicorn",
			"Horses with horns that grant a strong magical affinity. They can only be tamed by women.");
	public static Mount GRIFFIN = new Mount(2, 8, 10,
			new WorldMapTile.WorldMapTileType[] { WorldMapTile.WorldMapTileType.FOREST, WorldMapTile.WorldMapTileType.MOUNTAIN },
			0.7, 30, 31, 30, 51, true, "Griffin",
			"Products of the hybridization of lions and eagles. They are agreeable, yet ferocious.");
	public static Mount WYVERN = new Mount(3, 10, 10,
			new WorldMapTile.WorldMapTileType[] { WorldMapTile.WorldMapTileType.DESERT, WorldMapTile.WorldMapTileType.MOUNTAIN, WorldMapTile.WorldMapTileType.SNOWY_MOUNTAIN },
			0.5, 45, 51, 0, 31, true, "Wyvern",
			"Often considered a lesser species of dragon. They are easy to raise and difficult to tame.");
	public static Mount PEGASUS = new Mount(4, 8, 15,
			new WorldMapTile.WorldMapTileType[] { WorldMapTile.WorldMapTileType.MOUNTAIN, WorldMapTile.WorldMapTileType.SNOWY_MOUNTAIN },
			0.4, 20, 41, 40, 61, true, "Pegasus",
			"Horses with large, bird-like wings. Their wings keep them balanced as they magically gallop on thin air.");
	public static Mount ALICORN = new Mount(5, 10, 20,
				new WorldMapTile.WorldMapTileType[] { WorldMapTile.WorldMapTileType.SNOWY_MOUNTAIN },
				0, 30, 41, 30, 61, true, "Alicorn",
				"Hybrids of unicorns and pegasi. They combine the inherent strengths of both species.");

	private int id;
	private int movement;
	private int evasion;
	private double rearingSimplicity;
	private WorldMapTile.WorldMapTileType[] favoredClimates;
	private int minInitHealth;
	private int healthVariance;
	private int minGrowth;
	private int growthVariance;
	private bool canFly;
	private string displayName;
	private string description;

	public static Mount[] values()
	{
		return new Mount[] { HORSE, UNICORN, GRIFFIN, WYVERN, PEGASUS, ALICORN };
	}
	private Mount(int id, int movement, int evasion, WorldMapTile.WorldMapTileType[] favoredClimates,
			double rearingSimplicity, int minInitHealth, int healthVariance,
			int minGrowth, int growthVariance, bool canFly, string displayName,
			string description)
	{
		this.id = id;
		this.movement = movement;
		this.evasion = evasion;
		this.favoredClimates = favoredClimates;
		this.rearingSimplicity = rearingSimplicity;
		this.minInitHealth = minInitHealth;
		this.healthVariance = healthVariance;
		this.minGrowth = minGrowth;
		this.growthVariance = growthVariance;
		this.canFly = canFly;
		this.displayName = displayName;
		this.description = description;
	}

	public int getId()
	{
		return id;
	}

	public int getMovement()
	{
		return movement;
	}

	public int getEvasion()
	{
		return evasion;
	}

	public float getRearingFactor(WorldMapTile.WorldMapTileType type)
	{
		for (int q = 0; q < favoredClimates.Length; q++)
		{
			if (favoredClimates[q] == type)
			{
				return 1;
			}
		}
		return (float)(rearingSimplicity * type.getProliferability());
	}

	public WorldMapTile.WorldMapTileType[] favoredClimate()
	{
		return favoredClimates;
	}

	public int getMinInitialHealth()
	{
		return minInitHealth;
	}

	public int getHealthVariance()
	{
		return healthVariance;
	}

	public int getMinGrowth()
	{
		return minGrowth;
	}

	public int getGrowthVariance()
	{
		return growthVariance;
	}
	public bool canMountFly()
	{
		return canFly;
	}

	public string getDisplayName()
	{
		return displayName;
	}

	public string getDescription()
	{
		return description;
	}

	public int getWorth()
	{
		// TODO Auto-generated method stub
		return 0;
	}
}
