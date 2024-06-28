using System.Collections.Generic;
public class Port : Building
{

	//TODO decide actual values
	public static int[] materialsNeededForConstruction = { };
	public static int MAX_INTEGRITY = 10;
	public static int DURABILITY = 10;
	public static int RESISTANCE = 10;
	public static int MAX_NUM_SHIPS = 4;

	private List<Ship> ships;

	public Port(string name, Human owner)
			: base(name, MAX_INTEGRITY, DURABILITY, RESISTANCE, owner)
	{
		ships = new List<Ship>(MAX_NUM_SHIPS);
		// TODO Auto-generated constructor stub
	}

	public override string getType()
	{
		return Building.PORT;
	}

	public bool isFull()
	{
		return ships.Count >= MAX_NUM_SHIPS;
	}

	public override void completeDailyAction()
	{
		restockInventory();
		// TODO Auto-generated method stub
	}

	public override void completeMonthlyAction()
	{
		restockInventory();
		// TODO Auto-generated method stub
	}

	public override void destroy()
	{
		// TODO Auto-generated method stub

	}

	public override bool canReceiveGoods(int[] goods)
	{
		// TODO Auto-generated method stub
		return false;
	}

	public new List<int[]> getStorehouseNeeds()
	{
		// TODO Auto-generated method stub
		return null;
	}

}
