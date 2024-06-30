public class StationaryStaff : Staff
{

	public StationaryStaff(string name, int initialUses, int approximateWorth, int weight,
			int range, int[][] recipe)
		: base (name, initialUses, approximateWorth, weight, range, recipe)
	{
		// TODO Auto-generated constructor stub
	}

	public void effect()
	{
		//TODO
	}

		public override int getGeneralItemId()
	{
		return InventoryIndex.STATIONARY_STAFF;
	}
}
