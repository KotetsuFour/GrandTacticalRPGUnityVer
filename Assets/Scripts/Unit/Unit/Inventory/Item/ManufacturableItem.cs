public abstract class ManufacturableItem : Item
{

	protected int[][] recipe;

	public ManufacturableItem(string name, int initialUses, int approximateWorth, int weight, int[][] recipe)
		: base (name, initialUses, approximateWorth, weight)
	{
		this.recipe = recipe;
	}


	public int[][] getRecipe()
	{
		return recipe;
	}
}
