using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Building
{

	protected string name;
	protected int structuralIntegrity;
	protected int maxStructuralIntegrity;
	protected int durability;
	protected int resistance;
	protected Human owner;
	protected List<int[]> materials;

	public static string COLISEUM = "Coliseum";
	public static string HOSPITAL = "Hospital";
	public static string PORT = "Port";
	public static string RESEARCH_CENTER = "Research Center";
	public static string SHIPYARD = "Shipyard";
	public static string VILLAGE = "Village";
	public static string WARP_PAD = "Warp Pad";
	public static string BARRACKS = "Barracks";
	public static string CASTLE = "Castle";
	public static string FORTRESS = "Fortress";
	public static string PRISON = "Prison";
	public static string TRAINING_FACILITY = "Training Facility";
	public static string FACTORY = "Factory";
	public static string FARM = "Farm";
	public static string MAGIC_PROCESSING_FACILITY = "Magic Processing Facility";
	public static string MINING_FACILITY = "Mining Facility";
	public static string RANCH = "Ranch";
	public static string STOREHOUSE = "Storehouse";
	public static string TRADE_CENTER = "Trade Center";
	
	public Building(string name, int maxStructuralIntegrity, int durability, int resistance,
			Human owner)
	{
		this.name = name;
		this.maxStructuralIntegrity = maxStructuralIntegrity;
		this.structuralIntegrity = this.maxStructuralIntegrity;
		this.durability = durability;
		this.resistance = resistance;
		this.owner = owner;
		this.materials = new List<int[]>();
	}

	public string getName()
	{
		return name;
	}

	public string getNameAndType()
	{
		return $"{name} ({GetType()})";
	}

	public abstract string getType();

	public Human getOwner()
	{
		return owner;
	}

	public void removeOwner(CityState cs)
	{
		owner = Human.completelyRandomHuman(cs);
	}

	public void defect(Nation n)
	{
		owner.defect(n);
	}

	public bool takeDamage(bool isMagicAttack, int damage)
	{
		if (isMagicAttack)
		{
			damage -= getResistance();
		}
		else
		{
			damage -= getDurability();
		}
		if (damage > 0)
		{
			structuralIntegrity -= damage;
			return structuralIntegrity > 0;
		}
		return true;
	}

	public void rename(string newName)
	{
		this.name = newName;
	}

	public abstract void completeDailyAction();

	public abstract void completeMonthlyAction();

	public int getCurrentHP()
	{
		return this.structuralIntegrity;
	}

	public int getMaximumHP()
	{
		return this.maxStructuralIntegrity;
	}

	public abstract void destroy();

	public bool receiveGoods(int[] goods)
	{
		if (!canReceiveGoods(goods))
		{
			return false;
		}
		for (int q = 0; q < materials.Count; q++)
		{
			int[] m = materials[q];
			if (InventoryIndex.elementsAreEqual(m, goods))
			{
				m[2] += goods[2];
				return true;
			}
		}
		materials.Add((int[])goods.Clone());
		return true;
	}
	public abstract bool canReceiveGoods(int[] goods);

	public List<int[]> getMaterials()
	{
		return materials;
	}

	public int getDurability()
	{
		return durability;
	}
	public int getResistance()
	{
		return resistance;
	}

	public float percentageHealth()
	{
		return (float)(0.0 + getCurrentHP()) / getMaximumHP();
	}

	public string tostring()
	{
		return getNameAndType();
	}

	/**
	 * Gives a list of materials that the building requires from a storehouse
	 * in order to do its tasks
	 * 
	 * By default, this returns null to indicate that no materials are needed
	 * @return
	 */
	public List<int[]> getStorehouseNeeds()
	{
		return null;
	}

	/**
	 * Get needs from storehouse
	 * By default, this looks at storehouse needs and meets them as stated
	 * 
	 * Note that some buildings will have more abstract needs
	 *  and thus will override this method
	 */
	public void restockInventory()
	{
		List<int[]> needs = getStorehouseNeeds();
		if (needs != null && owner != null)
		{
			List<Building> b = owner.getHome().getOtherBuildings();
			for (int q = 0; q < b.Count && needs.Count != 0; q++)
			{
				if (b[q] is Storehouse) {
				Storehouse s = (Storehouse)b[q];
				List<int[]> store = s.getMaterials();
				//TODO if you cannot find the item in any storehouses, submit a request to
				// ONE of them to get the item from another storehouse in the nation
				for (int w = 0; w < needs.Count; w++)
				{
					for (int e = 0; e < store.Count; e++)
					{
						//Once you find the item in the inventory
						if (InventoryIndex.elementsAreEqual(needs[w], store[e]))
						{
							int[] itemArray = needs[w];

							//Make array for transferring materials
							int[] transfer = { itemArray[0], itemArray[1], Mathf.Min(itemArray[2], store[e][2]) };

							//Remove from needs and store the materials that are being transfered
							store[e][2] -= transfer[2];
							itemArray[2] -= transfer[2];

							//If the need has been completely met, remove this from the list
							if (itemArray[2] == 0)
							{
								needs.RemoveAt(w);
								w--;
							}
							break;
						}
					}
				}
			}
		}
	}
}
}
