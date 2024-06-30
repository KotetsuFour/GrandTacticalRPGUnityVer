public class UsableCrop : Item
{

	protected bool usedInBuilding;

	public UsableCrop(string name, int initialUses, int approximateWorth, int weight,
			bool usedInBuilding)
			: base (name, initialUses, approximateWorth, weight)
	{
		this.usedInBuilding = usedInBuilding;
	}

	public override int getGeneralItemId()
	{
		return InventoryIndex.USABLECROP;
	}

	public bool isUsedInBuilding()
	{
		return usedInBuilding;
	}

	public override string[] getInformationDisplayArray(int[] itemArray)
	{
		// TODO Auto-generated method stub
		return null;
	}
}
