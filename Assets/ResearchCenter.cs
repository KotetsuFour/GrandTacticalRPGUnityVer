using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ResearchCenter : Building
{

	//TODO decide actual values
public static int[] materialsNeededForConstruction = { };
public static int MAX_INTEGRITY = 10;
public static int DURABILITY = 10;
public static int RESISTANCE = 10;

public ResearchCenter(string name, Human owner)
		: base(name, MAX_INTEGRITY, DURABILITY, RESISTANCE, owner)
{
	// TODO Auto-generated constructor stub
}

	public override string getType()
{
	return Building.RESEARCH_CENTER;
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

	public new void defect(Nation n)
{
	// TODO maybe deal with technology tree as well as owner

}

	public new List<int[]> getStorehouseNeeds()
{
	// TODO Auto-generated method stub
	return null;
}

}
