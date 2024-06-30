using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Ranch : GoodsDeliverer
{

	protected int products;
	protected int assignedAnimal;
	protected int assignedAmount;

	//TODO decide actual values
	public static int[] materialsNeededForConstruction = { };
	public static int MAX_INTEGRITY = 10;
	public static int DURABILITY = 10;
	public static int RESISTANCE = 10;

	public Ranch(string name, Human owner, WorldMapTile location)
			: base(name, MAX_INTEGRITY, DURABILITY, RESISTANCE, owner, location)
	{
		products = 0;
		assignedAnimal = -1;
		assignedAmount = -1;
		autoGiveAssignment();
	}


	public override void autoGiveAssignment()
	{
		Mount[] ms = Mount.values();
		int type = 0;
		double heur = -1;
		for (int q = 0; q < ms.Length; q++)
		{
			double check = ms[q].getRearingFactor(location.getType());
			if (check > heur)
			{
				type = q;
				heur = check;
			}
		}
		List<Building> b = location.getOwner().getOtherBuildings();
		Building dest = null;
		for (int q = 0; q < b.Count; q++)
		{
			Building cur = b[q];
			if (cur is TrainingFacility && cur != customer)
			{
				if (customer == null || customer is Castle
									|| (customer is TradeCenter/**TODO AND TradeCenter does not need animals*/)
									|| (customer is TrainingFacility && ((TrainingFacility)customer).getMounts()[type] < 20))
				{
					dest = cur;
				}
			}
			else if (cur is TradeCenter && cur != customer)
			{
				if (customer == null || customer is Castle
									|| (customer is TradeCenter/**TODO AND TradeCenter does not need animals*/)
									|| (customer is TrainingFacility /**TODO AND cur does not need animals*/))
				{
					dest = cur;
				}
			}
		}
		giveAssignment(true, type, 0, dest);
	}


	public override string getType()
	{
		return Building.RANCH;
	}


	public override bool deliverGoods(Building recipient)
	{
		if (recipient is TrainingFacility)
		{
			TrainingFacility tf = (TrainingFacility)recipient;
			int amount = Mathf.Min(products, TrainingFacility.MAXIMUM_ANIMAL_COUNT - tf.getMounts()[assignedAnimal]);
			tf.getMounts()[assignedAnimal] += amount;
			assignedAmount -= amount;
			products -= amount;
			return true;
		}
		if (recipient is Castle)
		{
			Castle c = (Castle)recipient;
			int amount = Mathf.Min(products, Castle.MAXIMUM_ANIMAL_COUNT - c.getMounts()[assignedAnimal]);
			c.getMounts()[assignedAnimal] += amount;
			assignedAmount -= amount;
			products -= amount;
			return true;
		}
		if (recipient is TradeCenter)
		{
			TradeCenter tc = (TradeCenter)recipient;
			int amount = Mathf.Min(products, TradeCenter.MAXIMUM_ANIMAL_COUNT - tc.getMounts()[assignedAnimal]);
			tc.getMounts()[assignedAnimal] += amount;
			assignedAmount -= amount;
			products -= amount;
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
		//TODO maybe rebalance
		if (products < assignedAmount)
		{
			Mount m = Mount.values()[assignedAnimal];
			float growthFactor = m.getRearingFactor(location.getType());
			growthFactor *= (percentageHealth() / 30);
			if (RNGStuff.random0To99() < growthFactor * 100)
			{
				products++;
			}
		}

		if (continuousDelivery || (assignedAnimal == products && customer != null))
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
		//TODO maybe rebalance
		if (products < assignedAmount)
		{
			Mount m = Mount.values()[assignedAnimal];
			float growthFactor = m.getRearingFactor(location.getType());
			growthFactor *= percentageHealth();
			products += (int)Mathf.Min(assignedAmount - products, (growthFactor * 20));
		}

		if (continuousDelivery || (assignedAnimal == products && customer != null))
		{
			deliverGoods(customer);
		}
	}


	public override bool canReceiveGoods(int[] goods)
	{
		return false;
	}


	public new int amountProducibleWithResources()
	{
		//TODO
		return -1;
	}


	public new List<Building> possibleRecipients()
	{
		List<Building> ret = new List<Building>();
		List<Building> b = location.getOwner().getOtherBuildings();
		for (int q = 0; q < b.Count; q++)
		{
			Building check = b[q];
			if (check is TradeCenter || check is TrainingFacility)
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
		//This building doesn't produce items, and all buildings that accept any animals
		//accept all animals, so this method does the same as possibleRecipients
		return possibleRecipients();
	}

	public int[] getAssignment()
	{
		if (assignedAnimal < 0)
		{
			return null;
		}
		return new int[] { assignedAnimal, assignedAmount };
	}

	public void giveAssignment(int type, int amount)
	{
		this.assignedAnimal = type;
		this.assignedAmount = amount;
	}

	public void giveAssignment(bool continuous, int type, int amount, Building destination)
	{
		this.continuousDelivery = continuous;
		this.assignedAnimal = type;
		this.assignedAmount = amount;
		this.customer = destination;
	}

	public int getNumAnimals()
	{
		return products;
	}


	public new List<int[]> getStorehouseNeeds()
	{
		// TODO Auto-generated method stub
		return null;
	}

}
