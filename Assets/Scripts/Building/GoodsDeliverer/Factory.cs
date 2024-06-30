using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Factory : GoodsDeliverer
{

	protected int[] products;
	protected int[] assignment;
	protected List<int[]> neededIngredients;

	//TODO decide actual values
	public static int[] materialsNeededForConstruction = { };
	public static int MAX_INTEGRITY = 10;
	public static int DURABILITY = 10;
	public static int RESISTANCE = 10;

	public Factory(string name, Human owner, WorldMapTile location)
			: base(name, MAX_INTEGRITY, DURABILITY, RESISTANCE, owner, location)
	{
		products = new int[3];
		//assignment and neededIngredients are initially null
	}

	public override void autoGiveAssignment()
	{
		// TODO Auto-generated method stub
	}

	public override string getType()
	{
		return Building.FACTORY;
	}

	public override bool deliverGoods(Building b)
	{
		if (b.receiveGoods(products))
		{
			assignment[2] -= products[2];
			products[2] = 0;
			if (!continuousDelivery)
			{
				if (assignment[2] == 0)
				{
					assignment = null;
					neededIngredients = null;
					products = new int[3];
				}
			}
			return true;
		}
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
		int amountToMake = Mathf.Min(amountProducibleWithResources(), 10);
		if (amountToMake <= 0)
		{
			return;
		}
		amountToMake = Mathf.RoundToInt(amountToMake * percentageHealth());
		if (!continuousDelivery)
		{
			amountToMake = Mathf.Min(amountToMake, assignment[2] - products[2]);
		}
		if (products == null)
		{
			products = new int[] { assignment[0], assignment[1], amountToMake };
		}
		else
		{
			products[2] += amountToMake;
		}

		if (continuousDelivery || products[2] == assignment[2])
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
		restockInventory();
		int amountToMake = Mathf.Min(amountProducibleWithResources(), 300);
		if (amountToMake <= 0)
		{
			return;
		}
		amountToMake = Mathf.RoundToInt(amountToMake * percentageHealth());
		if (!continuousDelivery)
		{
			amountToMake = Mathf.Min(amountToMake, assignment[2] - products[2]);
		}
		if (products == null)
		{
			products = new int[] { assignment[0], assignment[1], amountToMake };
		}
		else
		{
			products[2] += amountToMake;
		}

		if (continuousDelivery || products[2] == assignment[2])
		{
			deliverGoods(customer);
		}
	}

	public override bool canReceiveGoods(int[] goods)
	{
		int type = goods[0];
		return type == InventoryIndex.RESOURCE
				|| type == InventoryIndex.USABLECROP;
	}

	public new bool receiveGoods(int[] goods)
	{
		if (!canReceiveGoods(goods))
		{
			return false;
		}
		//Deal with needed ingredients first
		if (neededIngredients != null)
		{
			for (int q = 0; q < neededIngredients.Count; q++)
			{
				int[] need = neededIngredients[q];
				if (InventoryIndex.elementsAreEqual(goods, need))
				{
					need[2] -= goods[2];
					if (need[2] <= 0)
					{
						neededIngredients.RemoveAt(q);
						if (neededIngredients.Count == 0)
						{
							neededIngredients = null;
						}
					}
					break;
				}
			}
		}
		return base.receiveGoods((int[])goods.Clone());
	}

	public new int amountProducibleWithResources()
	{
		if (assignment == null)
		{
			return -1;
		}
		Item item = InventoryIndex.getElement(assignment);
		int[][] recipe = ((ManufacturableItem)item).getRecipe();
		int amountToMake = int.MaxValue;
		for (int q = 0; q < recipe.Length; q++)
		{
			int[] need = recipe[q];
			bool exists = false;
			for (int w = 0; w < materials.Count; w++)
			{
				int[] check = materials[w];
				if (InventoryIndex.elementsAreEqual(check, need))
				{
					amountToMake = Mathf.Min(amountToMake, check[2] / need[2]);
					exists = true;
					break;
				}
			}
			if (!exists)
			{
				amountToMake = 0;
				break;
			}
		}
		return amountToMake;
	}

	public void giveAssignment(int[] a)
	{
		this.assignment = a;
		products[0] = assignment[0];
		products[1] = assignment[1];
		products[2] = 0;

		neededIngredients = new List<int[]>();

		Item item = InventoryIndex.getElement(a);
		int[][] recipe = ((ManufacturableItem)item).getRecipe();
		for (int q = 0; q < recipe.Length; q++)
		{
			int[] ingredient = recipe[q];
			for (int w = 0; w < materials.Count; w++)
			{
				int[] avail = materials[w];
				if (InventoryIndex.elementsAreEqual(ingredient, avail))
				{
					int needed = (ingredient[2] * assignment[2]) - avail[2];
					if (needed > 0)
					{
						neededIngredients.Add(new int[] { ingredient[0], ingredient[1], needed });
					}
					break;
				}
			}
		}
	}

	public void giveAssignment(bool continuous, int[] a, Building destination)
	{
		this.continuousDelivery = continuous;
		this.assignment = a;
		this.customer = destination;
		products[0] = assignment[0];
		products[1] = assignment[1];
		products[2] = 0;

		neededIngredients = new List<int[]>();

		Item item = InventoryIndex.getElement(a);
		int[][] recipe = ((ManufacturableItem)item).getRecipe();
		for (int q = 0; q < recipe.Length; q++)
		{
			int[] ingredient = recipe[q];
			for (int w = 0; w < materials.Count; w++)
			{
				int[] avail = materials[w];
				if (InventoryIndex.elementsAreEqual(ingredient, avail))
				{
					int needed = (ingredient[2] * assignment[2]) - avail[2];
					if (needed > 0)
					{
						neededIngredients.Add(new int[] { ingredient[0], ingredient[1], needed });
					}
					break;
				}
			}
		}
	}

	public int[] getAssignment()
	{
		return assignment;
	}

	public List<Item> possibleProducts()
	{
		List<Item> ret = new List<Item>();

		//Can produce non-magic handheld weapons
		List<Item> wep = InventoryIndex.index[InventoryIndex.HANDHELD_WEAPON];
		for (int q = 0; q < wep.Count; q++)
		{
			Weapon w = (Weapon)wep[q];
			if (!w.isMagic())
			{
				ret.Add(w);
			}
		}

		//Can produce non-magic stationary weapons
		List<Item> stn = InventoryIndex.index[InventoryIndex.STATIONARY_WEAPON];
		for (int q = 0; q < stn.Count; q++)
		{
			Weapon w = (Weapon)stn[q];
			if (!w.isMagic())
			{
				ret.Add(w);
			}
		}

		//Can produce armor
		List<Item> arm = InventoryIndex.index[InventoryIndex.ARMOR];
		for (int q = 0; q < arm.Count; q++)
		{
			Armor w = (Armor)arm[q];
			ret.Add(w);
		}

		//Can produce usable items such as medicines
		List<Item> use = InventoryIndex.index[InventoryIndex.USABLE_ITEM];
		for (int q = 0; q < use.Count; q++)
		{
			UsableItem w = (UsableItem)use[q];
			ret.Add(w);
		}

		return ret;
	}

	public int[] getProducts()
	{
		return products;
	}

	public new List<Building> possibleRecipients()
	{
		List<Building> ret = new List<Building>();
		List<Building> b = location.getOwner().getOtherBuildings();
		for (int q = 0; q < b.Count; q++)
		{
			Building check = b[q];
			if (check != this && check.canReceiveGoods(products))
			{
				ret.Add(check);
			}
		}
		List<Castle> c = location.getOwner().getNobleResidences();
		for (int q = 0; q < c.Count; q++)
		{
			ret.Add(c[q]);
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
			}
		}
		List<Castle> c = location.getOwner().getNobleResidences();
		for (int q = 0; q < c.Count; q++)
		{
			ret.Add(c[q]);
		}
		return ret;
	}

	public new List<int[]> getStorehouseNeeds()
	{
		return neededIngredients;
	}
}
