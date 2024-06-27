public abstract class Staff : ManufacturableItem
{

	//minRange should always be 1
	private int staffMaxRange;

	public static int INJURY = 0;
	public static int HEALING = 1;
	public static int POISON = 2;
	public static int SLEEP = 3;
	public static int BERSERK = 4;


	public Staff(string name, int initialUses, int approximateWorth, int weight, int range, int[][] recipe)
		: base (name, initialUses, approximateWorth, weight, recipe)
	{
		// TODO Auto-generated constructor stub
	}

	public int maxRange()
	{
		return staffMaxRange;
	}

	public override string[] getInformationDisplayArray(int[] itemArray)
	{
		// TODO Auto-generated method stub
		return null;
	}
}
