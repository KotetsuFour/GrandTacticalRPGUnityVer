public class TileStaff : Staff
{

	public TileStaff(string name, int initialUses, int approximateWorth, int weight,
			int range, int[][] recipe)
		: base (name, initialUses, approximateWorth, weight, range, recipe)
	{
		// TODO Auto-generated constructor stub
	}

	public void effect(BattlegroundTile target)
	{
		//TODO
	}
		public override int getGeneralItemId()
	{
		return InventoryIndex.TILE_STAFF;
	}
}
