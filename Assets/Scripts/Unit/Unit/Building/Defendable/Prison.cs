using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Prison : Defendable
{

	protected List<Human> prisoners;

	//TODO decide actual values
	public static int[] materialsNeededForConstruction = { };
	public static int MAX_INTEGRITY = 10;
	public static int DURABILITY = 10;
	public static int RESISTANCE = 10;

	public Prison(string name, Human owner, WorldMapTile location)
			: base(name, MAX_INTEGRITY, DURABILITY, RESISTANCE, owner, location)
	{
		// TODO Auto-generated constructor stub
		prisoners = new List<Human>(UnitGroup.CAPACITY);
	}


	public override string getType()
	{
		return Building.PRISON;
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


	public new void defect(Nation n)
	{
		// TODO deal with prisoners as well as owner

	}

	public bool canAcceptPrisonersFromGroup(UnitGroup group)
	{
		//Assume that group has prisoners
		return UnitGroup.CAPACITY >= prisoners.Count + group.getPrisoners().size();
	}

	public void acceptPrisonersFromGroup(UnitGroup group)
	{
		//Assuming that group has prisoners
		UnitGroup captives = group.removePrisoners();
		for (int q = 0; q < captives.size(); q++)
		{
			//Monsters don't surrender, so assume all prisoners are humans
			Human h = (Human)captives.get(q);
			int[][] inv = h.getInventory();
			for (int idx = 0; idx < inv.Length; idx++)
			{
				int[] itemArray = inv[idx];
				//Take all items that can be salvaged
				if (itemArray != null && canReceiveGoods(itemArray))
				{
					Item item = InventoryIndex.getElement(itemArray);
					//Prisons can only receive ManufacturableItems (weapons, armor, and staves)
					//so we can assume that [2] is the item's current uses
					//This kinda feels like it's in danger of bugs though. Not super comfortable with it
					if (itemArray[2] == item.getInitialUses())
					{
						//If the item is in perfect shape, keep it
						receiveGoods(new int[] { itemArray[0], itemArray[1], 1 });
					}
				}
				h.getInventory()[idx] = null;
			}
			if (h.getPassenger() is Convoy)
			{
				int[][] con = ((Convoy)h.getPassenger()).getInventory();
				for (int w = 0; w < con.Length; w++)
				{
					if (con[w] != null && canReceiveGoods(con[w]))
					{
						receiveGoods(con[w]);
						con[w] = null;
					}
				}
			}
			captives.remove(h);
			prisoners.Add(h);
		}
	}

}
