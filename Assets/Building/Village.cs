using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Village : Building
{


	protected double populationInThousands;
	protected List<Human> veteranResidents;
	protected CityState cityState;

	//TODO decide actual values
	public static int[] materialsNeededForConstruction = { };
	public static int MAX_INTEGRITY = 10;
	public static int DURABILITY = 10;
	public static int RESISTANCE = 10;
	public static int MAX_VETERANS = 5;

	public Village(string name, CityState cityState)
			: base(name, MAX_INTEGRITY, DURABILITY, RESISTANCE, Human.completelyRandomHuman(cityState))
	{
		veteranResidents = new List<Human>(5);
		//Between 1 and 10 thousand residents will arrive
		this.populationInThousands = RNGStuff.nextInt(9) + 1;
		this.cityState = cityState;
	}

	public Village(CityState cityState)
			: this(RNGStuff.randomName(cityState.getLanguage()), cityState)
	{
	}

	public override string getType()
	{
		return Building.VILLAGE;
	}

	public int getPopulation()
	{
		return Mathf.RoundToInt((float)populationInThousands);
	}

	public CityState getCityState()
	{
		return cityState;
	}

	public List<TrainingFacility> recruitDestinations()
	{
		List<TrainingFacility> ret = new List<TrainingFacility>();
		for (int q = 0; q < cityState.getOtherBuildings().Count; q++)
		{
			if (cityState.getOtherBuildings()[q] is TrainingFacility)
			{
				ret.Add((TrainingFacility)cityState.getOtherBuildings()[q]);
			}
		}
		return ret;
	}

	public override void completeDailyAction()
	{
		int feed = 0;
		int build = 0;
		int bonusFeed = 0;
		for (int q = 0; q < materials.Count; q++)
		{
			int[] itemArray = materials[q];
			if (itemArray[0] == InventoryIndex.USABLECROP)
			{
				//TODO get enough wood to fix damage
			}
			else if (itemArray[0] == InventoryIndex.EDIBLECROP)
			{
				int needed = Mathf.min(itemArray[2], (int)Mathf.round(populationInThousands * 200) - feed);
				itemArray[2] -= needed;
				feed += needed;
				bonusFeed += itemArray[2];
			}
		}

		//TODO only affect population if there's a major food shortage

		//		double percentFed = (feed + 0.0) / (populationInThousands * 200);
		//		populationInThousands *= percentFed;
		//		
		//		//TODO possibly rebalance
		//		double percentGrowth = Mathf.min((0.0 + bonusFeed) / (populationInThousands * 200), 0.01);
		//		populationInThousands *= (1 + percentGrowth);
	}

	public override void destroy()
	{
		// TODO Auto-generated method stub

	}

	public void rebuild(int[] materials)
	{
		//TODO
	}

	public Human recruitUnit()
	{
		return cityState.recruitUnit();
	}

	public override void completeMonthlyAction()
	{
		int feed = 0;
		int build = 0;
		int bonusFeed = 0;
		for (int q = 0; q < materials.Count; q++)
		{
			int[] itemArray = materials[q];
			if (itemArray[0] == InventoryIndex.USABLECROP)
			{
				//TODO get enough wood to fix damage
			}
			else if (itemArray[0] == InventoryIndex.EDIBLECROP)
			{
				int needed = Mathf.min(itemArray[2], (int)Mathf.round(populationInThousands * 6000) - feed);
				itemArray[2] -= needed;
				feed += needed;
				bonusFeed += itemArray[2];
			}
		}
		double percentFed = Mathf.max(0.8, (feed + 0.0) / (populationInThousands * 6000));
		populationInThousands *= percentFed;

		//TODO possibly rebalance
		double percentGrowth = Mathf.min((0.0 + bonusFeed) / (populationInThousands * 6000), 0.01);
		populationInThousands *= (1 + percentGrowth);
	}

	public override bool canReceiveGoods(int[] goods)
	{
		int type = goods[0];
		return type == InventoryIndex.EDIBLECROP
				|| type == InventoryIndex.USABLECROP;
	}

	public new void defect(Nation n)
	{
		this.owner.setAffiliation(n);
		for (int q = 0; q < veteranResidents.Count; q++)
		{
			Human h = veteranResidents[q];
			h.defect(n);
			if (h.getAffiliation() != null)
			{
				//If the unit failed to defect, they will try to flee from this city-state
				if (!(h.retire()))
				{
					//If they can't flee, they are forced to defect anyway
					h.setAffiliation(n);
				}
			}
		}
	}

	public List<Human> getVeterans()
	{
		return veteranResidents;
	}

	public bool addVeteran(Human h)
	{
		if (veteranResidents.Count >= MAX_VETERANS)
		{
			return false;
		}
		veteranResidents.Add(h);
		return true;
	}

	public new void restockInventory()
	{
		List<Building> b = getCityState().getOtherBuildings();
		//TODO stop  traversing if the need for food is met
		for (int q = 0; q < b.Count; q++)
		{
			if (b[q] is Storehouse)
			{
				Storehouse s = (Storehouse)b[q];
				List<int[]> store = s.getMaterials();
				//TODO get as much food and stuff as you need from the storehouse
			}
		}
	}

}
