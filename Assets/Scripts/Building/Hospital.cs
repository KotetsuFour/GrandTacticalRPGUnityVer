using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Hospital : Building
{

	protected List<Human> patients;
	protected int healingPower;
	protected int curingPower;

	public static int HEALING_NEEDED = 1000;
	public static int CURING_NEEDED = 100;


	//TODO decide actual values
	public static int[] materialsNeededForConstruction = { };
	public static int MAX_INTEGRITY = 10;
	public static int DURABILITY = 10;
	public static int RESISTANCE = 10;

	public Hospital(string name, Human owner)
			: base(name, MAX_INTEGRITY, DURABILITY, RESISTANCE, owner)
	{
		patients = new List<Human>();
		// TODO Auto-generated constructor stub
	}

	public override string getType()
	{
		return Building.HOSPITAL;
	}

	public override void completeDailyAction()
	{
		restockInventory();
		for (int q = 0; q < patients.Count && healingPower > 0; q++)
		{
			Human h = patients[q];
			int[] hps = h.getBodyPartsCurrentHP();
			for (int w = 0; w < hps.Length && healingPower > 0; w++)
			{
				h.getStatusEffects()[Staff.INJURY] = 0;
				if (h.getCurrentHPOfBodyPart(w) < h.getMaximumHPOfBodyPart(w))
				{
					h.heal(w, 2);
					healingPower -= 2;
				}
			}
		}
		cureEffects();
	}

	public override void completeMonthlyAction()
	{
		restockInventory();
		for (int q = 0; q < patients.Count && healingPower > 0; q++)
		{
			Human h = patients[q];
			int[] hps = h.getBodyPartsCurrentHP();
			for (int w = 0; w < hps.Length && healingPower > 0; w++)
			{
				int toHeal = Mathf.Min(h.getMaximumHPOfBodyPart(w) - h.getCurrentHPOfBodyPart(w),
						healingPower);
				h.heal(w, toHeal);
				healingPower -= toHeal;
			}
		}
		cureEffects();
	}

	public new void restockInventory()
	{
		if ((healingPower < HEALING_NEEDED || curingPower < CURING_NEEDED) && owner != null)
		{
			List<Building> b = owner.getHome().getOtherBuildings();
			for (int q = 0; q < b.Count; q++)
			{
				if (b[q] is Storehouse)
				{
					Storehouse s = (Storehouse)b[q];
					List<int[]> store = s.getMaterials();
					for (int w = 0; w < store.Count; w++)
					{
						int[] itemArray = store[w];
						if (itemArray[0] == InventoryIndex.USABLE_ITEM)
						{
							UsableItem item = (UsableItem)InventoryIndex.getElement(itemArray);
							//TODO find out how many you need to get healingPower and curingPower
							// up to acceptable values and take that many
						}
						else if (itemArray[0] == InventoryIndex.USABLECROP)
						{
							UsableCrop item = (UsableCrop)InventoryIndex.getElement(itemArray);
							//TODO find out how many you need to get healingPower and curingPower
							// up to acceptable values and take that many
						}
						else if (itemArray[0] == InventoryIndex.SUPPORT_STAFF)
						{
							SupportStaff item = (SupportStaff)InventoryIndex.getElement(itemArray);
							//TODO find out how many you need to get healingPower and curingPower
							// up to acceptable values and take that many
						}
					}
				}
			}
		}
	}

	private void cureEffects()
	{
		for (int q = 0; q < patients.Count && curingPower > 0; q++)
		{
			Human h = patients[q];
			int[] status = h.getStatusEffects();
			if (curingPower > 0 && status[Staff.BERSERK] > 0)
			{
				int cure = Mathf.Min(curingPower, status[Staff.BERSERK]);
				status[Staff.BERSERK] -= cure;
				//TODO take off berserk list
				curingPower -= cure;
			}
			if (curingPower > 0 && status[Staff.POISON] > 0)
			{
				int cure = Mathf.Min(curingPower, status[Staff.POISON]);
				status[Staff.POISON] -= cure;
				//TODO take off poison list
				curingPower -= cure;
			}
			if (curingPower > 0 && status[Staff.SLEEP] > 0)
			{
				int cure = Mathf.Min(curingPower, status[Staff.SLEEP]);
				status[Staff.SLEEP] -= cure;
				//TODO take off sleep list
				curingPower -= cure;
			}
		}
	}

	public override void destroy()
	{
		// TODO Auto-generated method stub
	}

	public override bool canReceiveGoods(int[] goods)
	{
		return goods[0] == InventoryIndex.USABLE_ITEM
				|| (goods[0] == InventoryIndex.USABLECROP && !((UsableCrop)InventoryIndex.getElement(goods)).isUsedInBuilding())
				|| goods[0] == InventoryIndex.SUPPORT_STAFF;
	}

	public new bool receiveGoods(int[] goods)
	{
		if (!canReceiveGoods(goods))
		{
			return false;
		}
		if (goods[0] == InventoryIndex.USABLE_ITEM)
		{
			//TODO if it's an antidote, add the strength to curing power
			//TODO if it's a vulnerary, add the strength to healing power
		}
		else if (goods[0] == InventoryIndex.SUPPORT_STAFF)
		{
			//TODO if it's a healing staff, add the strength to healing power
			//TODO if it's a restoring staff, add the strength to curing power
		}
		else if (goods[0] == InventoryIndex.USABLECROP)
		{
			//TODO add the strength to healing power
		}
		return true;
	}

	public new void defect(Nation n)
	{
		// TODO deal with patients as well as owner

	}

	public new List<int[]> getStorehouseNeeds()
	{
		// TODO Auto-generated method stub
		return null;
	}

}
