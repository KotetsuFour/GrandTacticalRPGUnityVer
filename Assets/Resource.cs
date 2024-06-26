public class Resource : Item
{

	private int strength;

	private int durability;

	private float rarity;

	private WorldMapTileType[] whereToFind;

	public Resource(string name, int approximateWorth, int weight, int strength, int durability,
			float rarity, WorldMapTileType[] whereToFind)
			: base (name, 0, approximateWorth, weight)
	{
		// TODO Auto-generated constructor stub
		this.strength = strength;
		this.durability = durability;
		this.rarity = rarity;
		this.whereToFind = whereToFind;
	}

		public override int getGeneralItemId()
	{
		return InventoryIndex.RESOURCE;
	}

	/**
	 * Gives the factor for daily chance of finding the resource
	 * @return
	 */
	public float getRarity()
	{
		return rarity;
	}

	/**
	 * Gives specific environments where this can be found, or null if it can be found anywhere
	 * @return
	 */
	public WorldMapTileType[] getPlacesToFind()
	{
		return whereToFind;
	}

	public bool canBeFoundHere(WorldMapTileType type)
	{
		if (whereToFind == null)
		{
			return true;
		}
		for (int q = 0; q < whereToFind.Length; q++)
		{
			if (type == whereToFind[q])
			{
				return true;
			}
		}
		return false;
	}

	public int getStrength()
	{
		return strength;
	}

	public int getDurability()
	{
		return durability;
	}

	public override string[] getInformationDisplayArray(int[] itemArray)
	{
		// TODO Auto-generated method stub
		return null;
	}
}
