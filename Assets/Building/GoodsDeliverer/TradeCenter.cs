using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TradeCenter : GoodsDeliverer
{

	private int[] mounts;
private int dayCounter;

/**
 * Assignment mostly functions as normal
 * 
 * When assigned to transport mounts, [0] is negative mount ID - 1, and [1] is quantity
 */
private int[] assignment;
private TradeCenter assignedTarget;

public static int MAXIMUM_ANIMAL_COUNT = 500;

//TODO decide actual values
public static int[] materialsNeededForConstruction = { };
public static int MAX_INTEGRITY = 10;
public static int DURABILITY = 10;
public static int RESISTANCE = 10;

public TradeCenter(string name, Human owner, WorldMapTile location)
		: base (name, MAX_INTEGRITY, DURABILITY, RESISTANCE, owner, location)
{
	this.mounts = new int[Mount.values().Length];
	this.dayCounter = 0;
	//assignment and assignedTradeCenter are initially false
}


	public override void autoGiveAssignment()
{
	// TODO Auto-generated method stub
}


	public override string getType()
{
	return Building.TRADE_CENTER;
}

public int[] getMounts()
{
	return mounts;
}


	public override bool deliverGoods(Building recipient)
{
	//TODO
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
	dayCounter++;
	if (dayCounter == 30)
	{
		completeMonthlyAction();
	}
}


	public override void completeMonthlyAction()
{
	restockInventory();
	dayCounter = 0;
	if (isSendingGoods())
	{
		//If assigned to transport mounts, then [0] is negative
		if (assignment[0] < 0)
		{
			int mountToGive = (assignment[0] + 1) * -1;
			int given = Mathf.Min(assignment[1], mounts[mountToGive]);
			int owed = assignment[1] - mounts[mountToGive];
			mounts[mountToGive] -= given;

			assignedTarget.getMounts()[mountToGive] += given;
			if (owed > 0)
			{
				//TODO recipient becomes upset
				assignment[1] += owed;
			}
		}
		else
		{
			for (int q = 0; q < materials.Count; q++)
			{
				int[] item = materials[q];
				if (InventoryIndex.elementsAreEqual(item, assignment))
				{
					int given = Mathf.Min(item[2], assignment[2]);
					int owed = assignment[2] - item[2];
					item[2] -= given;

					assignedTarget.receiveGoods(new int[] { item[0], item[1], given });
					if (owed > 0)
					{
						//TODO recipient becomes upset
						assignment[2] += owed;
					}
				}
			}
		}
	}
}


	public override void destroy()
{
	// TODO Auto-generated method stub

}


	public override bool canReceiveGoods(int[] goods)
{
	return true;
}


	public new void defect(Nation n)
{
	// TODO maybe deal with trade relations as well as owner
}

public bool isSendingGoods()
{
	return assignment != null && assignedTarget != null;
}


	public new List<Building> possibleRecipients()
{
	// TODO Auto-generated method stub
	return null;
}


	public new List<int[]> getStorehouseNeeds()
{
	if (isSendingGoods())
	{
		List<int[]> needs = new List<int[]>();
		//Always ask for items in assignment
		needs.Add(assignment);
	}
	return null;
}

}
