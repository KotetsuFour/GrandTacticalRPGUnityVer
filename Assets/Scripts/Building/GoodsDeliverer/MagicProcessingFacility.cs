using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MagicProcessingFacility : GoodsDeliverer
{

	protected int[] products;
	protected int[] assignment;

	//TODO decide actual values
	public static int[] materialsNeededForConstruction = { };
	public static int MAX_INTEGRITY = 10;
	public static int DURABILITY = 10;
	public static int RESISTANCE = 10;

	public MagicProcessingFacility(string name, Human owner, WorldMapTile location)
		: base(name, MAX_INTEGRITY, DURABILITY, RESISTANCE, owner, location)
	{
		// TODO Auto-generated constructor stub
	}


	public override void autoGiveAssignment()
	{
		// TODO Auto-generated method stub
	}


	public override string getType()
	{
		return Building.MINING_FACILITY;
	}


	public override bool deliverGoods(Building recipient)
	{
		// TODO Auto-generated method stub
		return false;
	}


	public override bool giveGoods(Unit recipient)
	{
		// TODO Auto-generated method stub
		return false;
	}


	public override void completeDailyAction()
	{
		restockInventory();
		// TODO Auto-generated method stub
	}


	public override void destroy()
	{
		// TODO Auto-generated method stub

	}


	public override void completeMonthlyAction()
	{
		restockInventory();
		// TODO Auto-generated method stub
	}


	public override bool canReceiveGoods(int[] goods)
	{
		int type = goods[0];
		return type == InventoryIndex.RESOURCE
				|| type == InventoryIndex.USABLECROP
				|| type == InventoryIndex.HANDHELD_WEAPON;
	}


	public new int amountProducibleWithResources()
	{
		//TODO
		return -1;
	}


	public new List<int[]> getStorehouseNeeds()
	{
		// TODO Auto-generated method stub
		return null;
	}
}
