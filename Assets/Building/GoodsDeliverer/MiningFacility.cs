using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MiningFacility : GoodsDeliverer
{

	protected bool[] assignment;

	//TODO decide actual values
	public static int[] materialsNeededForConstruction = { };
	public static int MAX_INTEGRITY = 10;
	public static int DURABILITY = 10;
	public static int RESISTANCE = 10;

	public MiningFacility(string name, Human owner, WorldMapTile location)
			: base(name, MAX_INTEGRITY, DURABILITY, RESISTANCE, owner, location)
	{
		this.location = location;
		int numResources = InventoryIndex.index[InventoryIndex.RESOURCE].Count;
		for (int q = 0; q < numResources; q++)
		{
			materials.Add(new int[] { InventoryIndex.RESOURCE, q, 0 });
		}
		assignment = new bool[numResources];
		autoGiveAssignment();
	}


	public override void autoGiveAssignment()
	{
		for (int q = 0; q < assignment.Length; q++)
		{
			assignment[q] = ((Resource)InventoryIndex.getElement(materials[q])).canBeFoundHere(location.getType());
		}
		this.continuousDelivery = true;
	}


	public override string getType()
	{
		return Building.MINING_FACILITY;
	}


	public override bool deliverGoods(Building recipient)
	{
		bool ret = false;
		for (int q = 0; q < materials.Count; q++)
		{
			if (materials[q][2] > 0 && recipient.receiveGoods((int[])materials[q].Clone()))
			{
				materials[q][2] = 0;
				ret = true;
			}
		}
		return ret;
	}


	public override bool giveGoods(Unit recipient)
	{
		// TODO Auto-generated method stub
		return false;
	}


	public override void completeDailyAction()
	{
		//As incentive to mine different things at different facilities,
		//the more different things you mine at a single facility, the
		//harder it is to mine any individual thing
		float div = 0;
		for (int q = 0; q < assignment.Length; q++)
		{
			if (assignment[q])
			{
				div++;
			}
		}
		if (div == 0) div = 1;

		float minability = (location.getType().getMinability() * 10) / div; //Since minability is 0-10,
																			//Multiplying by 10 makes it a percentage
		minability *= (0.0f + getCurrentHP()) / getMaximumHP(); //Affected by building's percentage HP

		for (int q = 0; q < materials.Count; q++)
		{
			if (assignment[q])
			{
				Resource r = (Resource)InventoryIndex.getElement(new int[] { InventoryIndex.RESOURCE, q });
				if (!r.canBeFoundHere(location.getType()))
				{
					continue;
				}
				materials[q][2] += Mathf.RoundToInt(minability * r.getRarity());
			}
		}

		if (customer != null)
		{
			deliverGoods(customer);
		}
	}


	public override void destroy()
	{
		// TODO Auto-generated method stub

	}


	public override void completeMonthlyAction()
	{
		//As incentive to mine different things at different facilities,
		//the more different things you mine at a single facility, the
		//harder it is to mine any individual thing
		float div = 0;
		for (int q = 0; q < assignment.Length; q++)
		{
			if (assignment[q])
			{
				div++;
			}
		}
		if (div == 0) div = 1;

		//Calculate health only once to be used multiple times
		float percentHealth = percentageHealth();

		//Deal with common minable materials first
		float minability = (location.getType().getMinability() * 10) / div; //Since minability is 0-10,
																			//Multiplying by 10 makes it a percentage
		minability *= percentHealth; //Affected by building's percentage HP

		for (int q = 0; q < materials.Count; q++)
		{
			if (assignment[q])
			{
				Resource r = (Resource)InventoryIndex.getElement(new int[] { InventoryIndex.RESOURCE, q });
				if (!r.canBeFoundHere(location.getType()))
				{
					continue;
				}
				materials[q][2] += Mathf.RoundToInt(minability * r.getRarity() * 30);
			}
		}

		if (customer != null)
		{
			deliverGoods(customer);
		}
	}

	public bool isProducing(int idx)
	{
		return assignment[idx];
	}

	public void toggleProduction(int idx)
	{
		assignment[idx] = !assignment[idx];
	}

	public void giveAssignment(Building customer)
	{
		this.customer = customer;
	}


	public override bool canReceiveGoods(int[] goods)
	{
		return false;
	}
}
