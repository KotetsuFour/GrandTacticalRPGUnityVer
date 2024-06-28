public class Coliseum : Building
{

	//TODO decide actual values
	public static int[] materialsNeededForConstruction = { };
	public static int MAX_INTEGRITY = 10;
	public static int DURABILITY = 10;
	public static int RESISTANCE = 10;

	public Coliseum(string name, Human owner)
			: base(name, MAX_INTEGRITY, DURABILITY, RESISTANCE, owner)
	{
		// TODO Auto-generated constructor stub
	}

	public override string getType()
	{
		return Building.COLISEUM;
	}

	public override void completeDailyAction()
	{
		// TODO Auto-generated method stub
	}

	public override void completeMonthlyAction()
	{
		// TODO Auto-generated method stub
	}

	public override void destroy()
	{
		// TODO Auto-generated method stub

	}

	public override bool canReceiveGoods(int[] goods)
	{
		return false;
	}

}
