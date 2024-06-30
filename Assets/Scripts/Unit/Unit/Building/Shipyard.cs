using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Shipyard : Building
{

	//TODO decide actual values
public static int[] materialsNeededForConstruction = { };
public static int MAX_INTEGRITY = 10;
public static int DURABILITY = 10;
public static int RESISTANCE = 10;

public Shipyard(string name, Human owner)
		: base (name, MAX_INTEGRITY, DURABILITY, RESISTANCE, owner)
{
	// TODO Auto-generated constructor stub
}

	public override string getType()
{
	return Building.SHIPYARD;
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
	int type = goods[0];
	return type == InventoryIndex.RESOURCE
			|| type == InventoryIndex.STATIONARY_WEAPON
			|| (type == InventoryIndex.USABLECROP && ((UsableCrop)InventoryIndex.getElement(goods)).isUsedInBuilding());
}

	public new List<int[]> getStorehouseNeeds()
{
	// TODO Auto-generated method stub
	return null;
}

}
