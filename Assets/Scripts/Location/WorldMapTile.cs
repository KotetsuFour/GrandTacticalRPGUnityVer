using UnityEngine;
public class WorldMapTile
{

	private WorldMapTileType type;
	private Building building;
	private BattleGround battle;
	private CityState owner;
	private int magicPotency;
	private int magicType;
	private WMTileOccupant groupPresent;

	public WorldMapTile(WorldMapTileType type, int magicPotency, int magicType)
	{
		this.type = type;
		this.magicPotency = magicPotency;
		this.magicType = magicType;
	}

	public bool isVacant()
	{
		return groupPresent == null;
	}

	public void sendHere(UnitGroup group)
	{
		if (group.getLocation() != null)
		{
			group.getLocation().removeGroupOrShip();
		}
		this.groupPresent = group;
		group.sendTo(this);
	}

	public int getMoveCost(UnitGroup group)
	{
		if (group.canFly())
		{
			return 1;
		}
		return type.moveCostOnFoot();
	}

	public int getMoveCost(Ship group)
	{
		if (type == WorldMapTileType.DEEP_WATER)
		{
			return 1;
		}
		else if (type == WorldMapTileType.SHALLOW_WATER)
		{
			return 2;
		}
		return int.MaxValue;
	}

	public void removeGroupOrShip()
	{
		this.groupPresent = null;
	}

	public WMTileOccupant getGroupPresent()
	{
		return groupPresent;
	}

	public Building getBuilding()
	{
		return building;
	}
	public void setBuilding(Building b)
	{
		this.building = b;
	}

	public CityState getOwner()
	{
		return owner;
	}
	public void setOwner(CityState owner)
	{
		this.owner = owner;
	}
	public string getMagicTypeAsString()
	{
		if (magicType == 0)
		{
			return "Earth";
		}
		if (magicType == 1)
		{
			return "Light";
		}
		if (magicType == 2)
		{
			return "Dark";
		}
		return "None";
	}
	public int getMagicPotency()
	{
		return magicPotency;
	}

	public void setBattle(BattleGround battle)
	{
		this.battle = battle;
		if (groupPresent is UnitGroup) {
			removeGroupOrShip();
		}
	}

	public BattleGround getBattle()
	{
		return battle;
	}

	public Nation getAffiliation()
	{
		if (owner == null)
		{
			return null;
		}
		return owner.getNation();
	}

	public WorldMapTileType getType()
	{
		return type;
	}

	public class WorldMapTileType
	{
		public static WorldMapTileType PLAIN = new WorldMapTileType("Plain", 1, 1, 6, new Color(0.5f, 1, 0), 0.5f);
		public static WorldMapTileType DESERT = new WorldMapTileType("Desert", 2, 0.2f, 7, new Color(1, 1, 0), 0.5f);
		public static WorldMapTileType FOREST = new WorldMapTileType("Forest", 2, 7, 6, new Color(0.03f, 0.8f, 0), 0.5f);
		public static WorldMapTileType DENSE_FOREST = new WorldMapTileType("Dense Forest", 5, 0.5f, 4, new Color(0.01f, 0.3f, 0), 0.5f);
		public static WorldMapTileType MOUNTAIN = new WorldMapTileType("Mountain", 4, 5, 10, new Color(0.45f, 0.4f, 0.5f), 2);
		public static WorldMapTileType SHALLOW_WATER = new WorldMapTileType("Shallow Water", 5, 0.3f, 1, new Color(0, 0.9f, 1), 0);
		public static WorldMapTileType DEEP_WATER = new WorldMapTileType("Deep Water", int.MaxValue, 0, 0, new Color(0, 0, 1), 0);
		public static WorldMapTileType SNOWY_PLAIN = new WorldMapTileType("Snowy Plain", 1, 0.3f, 5, new Color(1, 1, 1), 0.5f);
		public static WorldMapTileType SNOWY_MOUNTAIN = new WorldMapTileType("Snowy Mountain", 4, 0.3f, 8, new Color(0.7f, 0.7f, 0.7f), 2);
		public static WorldMapTileType SWAMP = new WorldMapTileType("Swamp", 4, 0.5f, 4, new Color(0.3f, 0.4f, 0.3f), 0.5f);
		public static WorldMapTileType WASTELAND = new WorldMapTileType("Wasteland", 1, 0, 5, new Color(0.48f, 0.37f, 0), 0.5f);
		public static WorldMapTileType GLACIER = new WorldMapTileType("Glacier", 2, 0.2f, 2, new Color(0.7f, 1, 1), 0.25f);


		private string name;
		private int moveCost;
		private float proliferability;
		private int minability;
		private Color displayColor;
		private float height;
		private WorldMapTileType(string name, int moveCost, float proliferability,
				int minability, Color displayColor, float height)
		{
			this.name = name;
			this.moveCost = moveCost;
			//It is easier to grow certain animals and plants depending on the climate
			//0 means nothing can be grown. Otherwise...
			//Multiple of 2 means temperate, multiple of 3 is cold, 5 is hot, 7 is dry,
			//11 is wet, 13 is elevated, 17 is dark
			this.proliferability = proliferability;
			this.minability = minability;
			this.displayColor = displayColor;
			this.height = height;
		}

		public string getName()
		{
			return name;
		}

		public int moveCostOnFoot()
		{
			return moveCost;
		}

		public int getMinability()
		{
			return minability;
		}
		public float getProliferability()
		{
			return proliferability;
		}
		public Color getDisplayColor()
		{
			return displayColor;
		}
		public float getHeight()
        {
			return height;
        }
	}

}
