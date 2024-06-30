public class Fortress : Defendable
{

	//TODO decide actual values
	public static int[] materialsNeededForConstruction = { };
	public static int MAX_INTEGRITY = 10;
	public static int DURABILITY = 10;
	public static int RESISTANCE = 10;

	public Fortress(string name, Human owner, WorldMapTile location)
			: base(name, MAX_INTEGRITY, DURABILITY, RESISTANCE, owner, location)
	{
		// TODO Auto-generated constructor stub
	}


	public override string getType()
	{
		return Building.FORTRESS;
	}


	public override void completeDailyAction()
	{
		// TODO Auto-generated method stub

	}


	public override void destroy()
	{
		// TODO Auto-generated method stub

	}


	public override void completeMonthlyAction()
	{
		// TODO Auto-generated method stub

	}


	public override bool canReceiveGoods(int[] goods)
	{
		// TODO Auto-generated method stub
		return false;
	}

}
