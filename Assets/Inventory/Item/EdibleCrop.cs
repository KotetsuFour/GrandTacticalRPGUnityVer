public class EdibleCrop : Item
{

	public EdibleCrop(string name, int initialUses, int approximateWorth, int weight)
		: base (name, initialUses, approximateWorth, weight)
	{
		// TODO Auto-generated constructor stub
	}

	public override int getGeneralItemId()
	{
		return InventoryIndex.EDIBLECROP;
	}

	public override string[] getInformationDisplayArray(int[] itemArray)
	{
		// TODO Auto-generated method stub
		return null;
	}

}
