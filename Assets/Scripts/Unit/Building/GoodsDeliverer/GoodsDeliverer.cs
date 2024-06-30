using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class GoodsDeliverer : Building
{


	protected Building customer;
protected WorldMapTile location;
protected bool continuousDelivery;

public GoodsDeliverer(string name, int maxStructuralIntegrity, int durability,
		int resistance, Human owner, WorldMapTile location)
		: base(name, maxStructuralIntegrity, durability, resistance, owner)
{
	this.location = location;
	this.continuousDelivery = false;
}

public abstract bool deliverGoods(Building recipient);

public abstract bool giveGoods(Unit recipient);

/**
 * Returns the amount of products producible using the current allotted resources
 * Returns -1 by default for buildings that do not need resources to make products
 * @return amount producible, or -1 if there is no assignment
 */
public int amountProducibleWithResources()
{
	return -1;
}

/**
 * Gives all of the buildings that this GoodsDeliverer is able to send its products to
 * 
 * By default, this includes all buildings within its own city that can receive its goods
 * 
 * @return all buildings this building can deliver its goods to
 */
public List<Building> possibleRecipients()
{
	List<Building> ret = new List<Building>();
	List<Building> b = location.getOwner().getOtherBuildings();
	for (int q = 0; q < b.Count; q++)
	{
		Building check = b[q];
		for (int w = 0; w < materials.Count; w++)
		{
			if (check != this && check.canReceiveGoods(materials[w]))
			{
				ret.Add(check);
				break;
			}
		}
	}
	return ret;
}

public List<Building> possibleRecipientsOfItem(Item item)
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
	return ret;
}

public WorldMapTile.WorldMapTileType getTerrainType()
{
	return location.getType();
}

public bool isDeliveringContinuously()
{
	return continuousDelivery;
}

public Building getCustomer()
{
	return customer;
}

public abstract void autoGiveAssignment();
}
