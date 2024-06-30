public class UsableItem : ManufacturableItem
{
	public UsableItem(string name, int initialUses, int approximateWorth, int weight, int[][] recipe)
		: base (name, initialUses, approximateWorth, weight, recipe)
	{
		// TODO Auto-generated constructor stub
	}

	public override int getGeneralItemId()
	{
		return InventoryIndex.USABLE_ITEM;
	}

	public override string[] getInformationDisplayArray(int[] itemArray)
	{
		// TODO Auto-generated method stub
		return null;
	}
}
