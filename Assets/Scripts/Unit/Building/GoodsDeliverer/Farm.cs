using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Farm : GoodsDeliverer
{

	protected int[] assignment;

	//TODO decide actual values
	public static int[] materialsNeededForConstruction = { };
	public static int MAX_INTEGRITY = 10;
	public static int DURABILITY = 10;
	public static int RESISTANCE = 10;

	public Farm(string name, Human owner, WorldMapTile location)
			: base(name, MAX_INTEGRITY, DURABILITY, RESISTANCE, owner, location)
	{
		autoGiveAssignment();
	}


	public override void autoGiveAssignment()
	{
		// TODO Auto-generated method stub
	}


	public override string getType()
	{
		return Building.FARM;
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
		if (assignment == null)
		{
			return;
		}
		// TODO probably rebalance
		float increaseBy = Mathf.Min(Mathf.RoundToInt(location.getType().getProliferability() * 100f), assignment[2]);
		increaseBy *= percentageHealth();
		bool accountedFor = false;
		int idxOfProduct = -1;
		for (int q = 0; q < materials.Count; q++)
		{
			int[] prod = materials[q];
			if (prod[0] == assignment[0] && prod[1] == assignment[1])
			{
				int amount = Mathf.RoundToInt(increaseBy);
				prod[2] += amount;
				assignment[2] -= amount;
				accountedFor = true;
				idxOfProduct = q;
				break;
			}
		}
		if (!accountedFor)
		{
			idxOfProduct = materials.Count;
			materials.Add(new int[] { assignment[0], assignment[1], Mathf.RoundToInt(increaseBy) });
			assignment[2] -= Mathf.RoundToInt(increaseBy);
		}

		//TODO after updating UI, remove conditional for whether customer is null
		if (continuousDelivery || (customer != null && materials[idxOfProduct][2] == assignment[2]))
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
		if (assignment == null)
		{
			return;
		}
		// TODO probably rebalance
		float increaseBy = Mathf.Min(Mathf.Round(location.getType().getProliferability() * 3000), assignment[2]);
		increaseBy *= (0.0f + getCurrentHP()) / getMaximumHP();
		bool accountedFor = false;
		int idxOfProduct = -1;
		for (int q = 0; q < materials.Count; q++)
		{
			int[] prod = materials[q];
			if (prod[0] == assignment[0] && prod[1] == assignment[1])
			{
				int amount = Mathf.RoundToInt(increaseBy);
				prod[2] += amount;
				assignment[2] -= amount;
				accountedFor = true;
				idxOfProduct = q;
				break;
			}
		}
		if (!accountedFor)
		{
			idxOfProduct = materials.Count;
			materials.Add(new int[] { assignment[0], assignment[1], Mathf.RoundToInt(increaseBy) });
			assignment[2] -= Mathf.RoundToInt(increaseBy);
		}

		//TODO after updating UI, remove conditional for whether customer is null
		if (continuousDelivery || (customer != null && materials[idxOfProduct][2] == assignment[2]))
		{
			deliverGoods(customer);
		}
	}


	public override bool canReceiveGoods(int[] goods)
	{
		return false;
	}

	public void giveAssignment(int[] a)
	{
		this.assignment = a;
	}

	public void giveAssignment(bool continuous, int[] a, Building destination)
	{
		this.continuousDelivery = continuous;
		this.assignment = a;
		this.customer = destination;
	}

	public int[] getAssignment()
	{
		return assignment;
	}

	public List<Item> possibleProducts()
	{
		List<Item> ret = new List<Item>();

		List<Item> use = InventoryIndex.index[InventoryIndex.USABLECROP];
		for (int q = 0; q < use.Count; q++)
		{
			ret.Add(use[q]);
		}

		List<Item> edi = InventoryIndex.index[InventoryIndex.EDIBLECROP];
		for (int q = 0; q < edi.Count; q++)
		{
			ret.Add(edi[q]);
		}

		return ret;
	}

	public new List<Building> possibleRecipients()
	{
		List<Building> ret = new List<Building>();
		List<Building> b = location.getOwner().getOtherBuildings();
		for (int q = 0; q < b.Count; q++)
		{
			Building check = b[q];
			for (int w = 0; w < materials.Count; w++)
			{
				if (check.canReceiveGoods(materials[w]))
				{
					ret.Add(check);
					break;
				}
			}
		}
		List<Village> v = location.getOwner().getResidentialAreas();
		for (int q = 0; q < v.Count; q++)
		{
			Building check = v[q];
			for (int w = 0; w < materials.Count; w++)
			{
				if (check.canReceiveGoods(materials[w]))
				{
					ret.Add(check);
					break;
				}
			}
		}

		return ret;
	}


	public new List<Building> possibleRecipientsOfItem(Item item)
	{
		List<Building> ret = new List<Building>();
		List<Building> b = location.getOwner().getOtherBuildings();
		int[] itemArray = new int[] { item.getGeneralItemId(), item.getSpecificItemId() };
		for (int q = 0; q < b.Count; q++)
		{
			Building check = b[q];
			if (check != this && check.canReceiveGoods(itemArray))
			{
				ret.Add(check);
				break;
			}
		}
		List<Village> v = location.getOwner().getResidentialAreas();
		for (int q = 0; q < v.Count; q++)
		{
			Building check = v[q];
			if (check.canReceiveGoods(itemArray))
			{
				ret.Add(check);
				break;
			}
		}

		return ret;
	}
}
