public class BattlegroundTile
{

	private BFTileOccupant occupant;
	private Unit unit;
	private BattlegroundTileType type;

	public BattlegroundTile(BattlegroundTileType type)
	{
		this.type = type;
	}

	public BattlegroundTileType getType()
	{
		return type;
	}

	public int avoidanceBonus()
	{
		return type.getAvoidanceBonus();
	}

	public int moveCostOnFoot()
	{
		return type.getMoveCostOnFoot();
	}

	public int moveCostInAir()
	{
		return type.getMoveCostInAir();
	}

	public int getMoveCost(Unit u)
	{
		if (u.canFly())
		{
			return moveCostInAir();
		}
		return moveCostOnFoot();
	}

	public void placeUnit(Unit u)
	{
		this.unit = u;
	}

	public void placeOccupant(BFTileOccupant bfTileOccupant)
	{
		if (bfTileOccupant is Unit)
		{
			placeUnit((Unit)bfTileOccupant);
		}
		else if (bfTileOccupant != null)
		{
			occupant = bfTileOccupant;
		}

	}

	public Unit getUnit()
	{
		return unit;
	}
	public void removeUnit()
	{
		unit = null;
	}
	public BFTileOccupant getInanimateObjectOccupant()
	{
		return occupant;
	}

	public void removeInanimateObjectOccupant()
	{
		occupant = null;
	}

	public bool isVacant()
	{
		return unit == null;
	}

	public class BattlegroundTileType
	{
		public static BattlegroundTileType GRASS = new BattlegroundTileType("Grass", 1, 1, 0);
		public static BattlegroundTileType SAND = new BattlegroundTileType("Sand", 2, 1, 5);
		public static BattlegroundTileType TREE = new BattlegroundTileType("Tree", 2, 1, 20);
		public static BattlegroundTileType THICKET = new BattlegroundTileType("Thicket", 6, 1, 40);
		public static BattlegroundTileType MOUNTAIN = new BattlegroundTileType("Mountain", 4, 1, 30);
		public static BattlegroundTileType PEAK = new BattlegroundTileType("Peak", 4, 1, 40);
		public static BattlegroundTileType HOUSE = new BattlegroundTileType("House", 1, 1, 10);
		public static BattlegroundTileType HOUSE_DOOR = new BattlegroundTileType("Door", 1, 1, 10); //Part of a house that can be interacted with
		public static BattlegroundTileType RUBBLE = new BattlegroundTileType("Rubble", 2, 1, 0);
		public static BattlegroundTileType PILLAR = new BattlegroundTileType("Pillar", 2, 6, 20);
		public static BattlegroundTileType GATE = new BattlegroundTileType("Gate", 1, 1, 20);
		public static BattlegroundTileType WALL = new BattlegroundTileType("Wall", int.MaxValue, 1, 0);
		public static BattlegroundTileType SHALLOW_WATER = new BattlegroundTileType("Shallow Water", 3, 1, 10);
		public static BattlegroundTileType DEEP_WATER = new BattlegroundTileType("Deep Water", int.MaxValue, 1, 10);
		public static BattlegroundTileType CAVE = new BattlegroundTileType("Cave", 1, 6, 10);
		public static BattlegroundTileType MAGMA = new BattlegroundTileType("Molten Rock", 3, 3, 10);
		public static BattlegroundTileType FLOOR = new BattlegroundTileType("Floor", 1, 5, 0);
		public static BattlegroundTileType THRONE = new BattlegroundTileType("Throne", 1, 5, 30);
		public static BattlegroundTileType CHEST = new BattlegroundTileType("Chest", 1, 5, 0);
		public static BattlegroundTileType WETLAND = new BattlegroundTileType("Wetland", 2, 1, 5);
		public static BattlegroundTileType WASTELAND = new BattlegroundTileType("Wasteland", 1, 1, 0);
		public static BattlegroundTileType ROAD = new BattlegroundTileType("Road", 1, 1, 0);
		public static BattlegroundTileType WARP_TILE = new BattlegroundTileType("Warp Tile", 2, 3, 0);
		public static BattlegroundTileType DECK = new BattlegroundTileType("Deck", 1, 1, 0);
		public static BattlegroundTileType DOCK = new BattlegroundTileType("Dock", 1, 1, 5);
		public static BattlegroundTileType SNOW = new BattlegroundTileType("Snow", 2, 1, 5);
		//		STRUCTURE("")
		private string name;
		private int moveCostOnFoot;
		private int moveCostInAir;
		private int avoidanceBonus;
		private BattlegroundTileType(string name, int moveCostOnFoot, int moveCostInAir,
				int avoidanceBonus)
		{
			this.name = name;
			this.moveCostOnFoot = moveCostOnFoot;
			this.moveCostInAir = moveCostInAir;
			this.avoidanceBonus = avoidanceBonus;
		}
		public string getName()
		{
			return name;
		}
		public int getMoveCostOnFoot()
		{
			return moveCostOnFoot;
		}
		public int getMoveCostInAir()
		{
			return moveCostInAir;
		}
		public int getAvoidanceBonus()
		{
			return avoidanceBonus;
		}
	}
}
