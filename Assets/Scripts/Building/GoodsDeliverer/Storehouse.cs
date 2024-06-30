using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Storehouse : GoodsDeliverer
{

	protected List<int[]> weaponStorage;
	protected List<int[]> armorStorage;
	protected List<int[]> staffStorage;

	//TODO decide actual values
	public static int[] materialsNeededForConstruction = { };
	public static int MAX_INTEGRITY = 10;
	public static int DURABILITY = 10;
	public static int RESISTANCE = 10;

	public Storehouse(string name, Human owner, WorldMapTile location)
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
		return Building.STOREHOUSE;
	}


	public override bool deliverGoods(Building recipient)
	{
		//Never give all goods to a single building
		return false;
	}

	public bool deliverGoods(Building recipient, List<int[]> order)
	{
		//TODO give order to building
		return false;
	}


	public override bool giveGoods(Unit recipient)
	{
		//Never give all goods to one person
		return false;
	}

	public bool giveGoods(Unit recipient, List<int[]> order)
	{
		//TODO give order to recipient
		return false;
	}


	public override void completeDailyAction()
	{
		List<Building> b = location.getOwner().getOtherBuildings();
		List<Village> v = location.getOwner().getResidentialAreas();
		//Castles don't matter here
		for (int q = 0; q < b.Count; q++)
		{
			provideNeedsOfBuilding(b[q]);
		}
		for (int q = 0; q < v.Count; q++)
		{
			provideNeedsOfBuilding(v[q]);
		}
	}

	public void provideNeedsOfBuilding(Building build)
	{
		List<int[]> needs = build.getStorehouseNeeds();
		if (needs != null)
		{
			for (int w = 0; w < needs.Count; w++)
			{
				int[] need = needs[w];
				//TODO materials should probably be a map or something so that
				//this is more efficient. We'll see how it runs first though
				Item i = InventoryIndex.getElement(need);
				List<int[]> check = null;
				if (i is Weapon)
				{
					check = weaponStorage;
				}
				else if (i is Armor)
				{
					check = armorStorage;
				}
				else if (i is Staff)
				{
					check = staffStorage;
				}
				else
				{
					check = materials;
				}
				for (int e = 0; e < check.Count; e++)
				{
					if (InventoryIndex.elementsAreEqual(need, check[e]))
					{
						int numToSend = Mathf.Min(need[2], check[e][2]);
						int[] delivery = new int[] { need[0], need[1], numToSend };
						check[e][2] -= numToSend;
						build.receiveGoods(delivery);
						break;
					}
				}
			}
		}
	}


	public override void destroy()
	{
		// TODO Auto-generated method stub

	}


	public override void completeMonthlyAction()
	{
		//Exact same as monthly action
		completeDailyAction();
	}


	public new bool receiveGoods(int[] goods)
	{
		Item i = InventoryIndex.getElement(goods);
		List<int[]> check = null;
		if (i is Weapon)
		{
			check = weaponStorage;
		}
		else if (i is Armor)
		{
			check = armorStorage;
		}
		else if (i is Staff)
		{
			check = staffStorage;
		}
		else
		{
			check = materials;
		}
		for (int q = 0; q < check.Count; q++)
		{
			int[] m = check[q];
			if (InventoryIndex.elementsAreEqual(m, goods))
			{
				m[2] += goods[2];
				return true;
			}
		}
		check.Add((int[])goods.Clone());
		return true;
	}


	public override bool canReceiveGoods(int[] goods)
	{
		return true;
	}


	public new List<Building> possibleRecipients()
	{
		// TODO Auto-generated method stub
		return null;
	}

	public void submitRequest(int[] need)
	{
		// TODO Auto-generated method stub
	}

}

