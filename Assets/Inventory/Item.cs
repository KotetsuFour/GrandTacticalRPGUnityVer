public abstract class Item
{

	protected string name;
	protected int initialUses;
	protected int approximateWorth;
	protected int specificItemId;
	protected int weight;

	public Item(string name, int initialUses, int approximateWorth, int weight)
	{
		this.name = name;
		this.initialUses = initialUses;
		this.approximateWorth = approximateWorth;
		this.weight = weight;
	}

	public string getName()
	{
		return name;
	}
	public int getInitialUses()
	{
		return initialUses;
	}
	public int getApproximateWorth()
	{
		return approximateWorth;
	}
	public int getSpecificItemId()
	{
		return specificItemId;
	}
	public int getWeight()
	{
		return weight;
	}
	public void setSpecificItemId(int id)
	{
		specificItemId = id;
	}

	public abstract int getGeneralItemId();

	public string toString()
	{
		return getName();
	}

	public abstract string[] getInformationDisplayArray(int[] itemArray);
}
