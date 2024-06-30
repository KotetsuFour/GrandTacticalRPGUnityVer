public class SupportStaff : Staff
{

	public SupportStaff(string name, int initialUses, int approximateWorth, int weight,
			int range, int[][] recipe)
		: base (name, initialUses, approximateWorth, weight, range, recipe)
	{
		// TODO Auto-generated constructor stub
	}

	public void effect(GameUnit u)
	{
		//TODO
	}

		public override int getGeneralItemId()
	{
		return InventoryIndex.SUPPORT_STAFF;
	}
}
