using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CityState
{

	protected string name;
	protected List<Village> residentialAreas;
	protected List<Castle> nobleResidences;
	//Storehouses are what provide resources for villages
	protected List<Building> otherBuildings;
	protected Nation nation;
	protected int[] values;
	protected int language;
	protected int count;
	public static int MAX_SIZE = 16;
	public static int NATIONALISM = 0;
	public static int MILITARISM = 1;
	public static int ALTRUISM = 2;
	public static int FAMILISM = 3;
	public static int CONFIDENCE = 4;
	public static int TOLERANCE = 5;

	public CityState(string name, Nation nation)
		: this(nation)
	{
		this.name = name;
	}

	public CityState(Nation nation)
	{
		this.residentialAreas = new List<Village>(8);
		this.nobleResidences = new List<Castle>(2);
		//Storehouses are what provide resources for villages
		this.otherBuildings = new List<Building>(6);
		this.nation = nation;
		nation.getCityStates().Add(this);
		this.language = nation.getNationalLanguage();
		this.name = RNGStuff.newLocationName(language);
		this.values = new int[6];
		for (int q = 0; q < values.Length; q++)
		{
			this.values[q] = RNGStuff.random0To100();
		}
	}

	public CityState(int language, Nation nation)
		: this(nation)
	{
		this.language = language;
		this.name = RNGStuff.newLocationName(language);
	}

	/**
	 * Maybe only used for testing
	 * @param nation
	 * @param values
	 */
	public CityState(Nation nation, int[] values)
		: this(nation)
	{
		this.values = values;
		//		this.language = RNGStuff.nextInt(RNGStuff.numberOfLanguages());
		this.name = RNGStuff.newLocationName(language);
	}

	public int getPopulation()
	{
		int pop = 0;
		for (int q = 0; q < residentialAreas.Count; q++)
		{
			pop += residentialAreas[q].getPopulation();
		}
		return pop;
	}

	public int getLanguage()
	{
		return language;
	}

	public List<Village> getResidentialAreas()
	{
		return residentialAreas;
	}

	public List<Castle> getNobleResidences()
	{
		return nobleResidences;
	}

	public List<Building> getOtherBuildings()
	{
		return otherBuildings;
	}

	public int getNationApproval()
	{
		return getNationalism() + getTolerance();
	}

	public Human getLeadingNoble()
	{
		for (int q = 0; q < nobleResidences.Count; q++)
		{
			Human h = nobleResidences[q].getOwner();
			if (h != null)
			{
				return h;
			}
		}
		return null;
	}

	public Human getLeadingVeteran()
	{
		Human ret = null;
		for (int q = 0; q < residentialAreas.Count; q++)
		{
			List<Human> vets = residentialAreas[q].getVeterans();
			for (int w = 0; w < vets.Count; w++)
			{
				Human check = vets[w];
				if (ret == null
						|| (getMilitarism() > 50 && ret.getBattles() < check.getBattles())
						|| ret.getLeadership() < check.getLeadership())
				{
					ret = check;
				}
			}
		}
		return ret;
	}

	public int getNumBuildings()
	{
		return residentialAreas.Count + nobleResidences.Count + otherBuildings.Count;
	}

	public int[] getValues()
	{
		return values;
	}

	public Nation getNation()
	{
		return nation;
	}

	public Human recruitUnit()
	{
		Human ret = Human.completelyRandomHuman(this);
		int wars = nation.numCurrentWars();
		for (int q = 0; q < wars; q++)
		{
			ret.incrementWars();
		}
		int detriment = getMilitarism() + getTolerance();
		if (detriment < 0)
		{ //Assume Tolerance is always > 0
			values[NATIONALISM] = Mathf.Max(-100, getNationalism() + (getMilitarism() / 10));
		}
		return ret;
	}

	public bool wantsSecession()
	{
		Human lead = getLeadingNoble();
		//Veteran opinions don't have an affect
		if (lead == null)
		{
			return getNationApproval() < 0;
		}
		Human rule = nation.getRuler();
		int ret = getNationApproval();
		int diffs = Mathf.Abs(rule.getNationalism() - lead.getNationalism())
				+ Mathf.Abs(rule.getAltruism() - lead.getAltruism())
				+ Mathf.Abs(rule.getFamilism() - lead.getFamilism())
				+ Mathf.Abs(rule.getMilitarism() - lead.getMilitarism())
				+ lead.getConfidence() - lead.getTolerance();
		return ret - diffs < 0;
	}

	public void defect(Nation n)
	{
		nation = n;
		values[NATIONALISM] = values[FAMILISM];
		for (int q = 0; q < residentialAreas.Count; q++)
		{
			Village b = residentialAreas[q];
			b.defect(n);
		}
		for (int q = 0; q < nobleResidences.Count; q++)
		{
			Castle b = nobleResidences[q];
			b.defect(n);
		}
		for (int q = 0; q < otherBuildings.Count; q++)
		{
			Building b = otherBuildings[q];
			b.defect(n);
		}
	}

	public int getNationalism()
	{
		return values[0];
	}
	public int getMilitarism()
	{
		return values[1];
	}
	public int getAltruism()
	{
		return values[2];
	}
	public int getFamilism()
	{
		return values[3];
	}
	public int getConfidence()
	{
		return values[4];
	}
	public int getTolerance()
	{
		return values[5];
	}

	public string getName()
	{
		return name;
	}

	/**
	 * Lose militarism due to a soldier from here dying in battle
	 */
	public void mournSoldierDeath()
	{
		values[MILITARISM] = Mathf.Max(-100, values[MILITARISM] - 1);
	}

	public void addBuilding(Building b)
	{
		if (b is Village)
		{
			residentialAreas.Add((Village)b);
		}
		else if (b is Castle)
		{
			nobleResidences.Add((Castle)b);
		}
		else
		{
			otherBuildings.Add(b);
		}
	}

	public void incrementSize()
	{
		if (count == MAX_SIZE)
		{
			throw new Exception("Cannot expand this city-state further");
		}
		count++;
	}

	public void decrementSize()
	{
		if (count == 0)
		{
			throw new Exception("Cannot lose more territory");
		}
		count--;
	}

	public int size()
	{
		return count;
	}

	public bool canExpand()
	{
		return count < MAX_SIZE;
	}

	public List<Report> passMonth(bool yearEnded)
	{
		List<Report> ret = new List<Report>();
		for (int q = 0; q < residentialAreas.Count; q++)
		{
			Village b = residentialAreas[q];
			b.completeMonthlyAction();
		}
		for (int q = 0; q < nobleResidences.Count; q++)
		{
			Castle b = nobleResidences[q];
			b.completeMonthlyAction();
		}
		for (int q = 0; q < otherBuildings.Count; q++)
		{
			Building b = otherBuildings[q];
			b.completeMonthlyAction();
		}
		if (yearEnded)
		{
			for (int q = 0; q < residentialAreas.Count; q++)
			{
				Village b = residentialAreas[q];
				if (!(b.getOwner().incrementAge()))
				{
					if (b.getOwner().isImportant())
					{
						ret.Add(new OldAgeDeathReport(b.getOwner()));
					}
					b.removeOwner(this);
				}
				for (int w = 0; w < b.getVeterans().Count; w++)
				{
					Human vet = b.getVeterans()[w];
					if (!(vet.incrementAge()))
					{
						if (vet.isImportant())
						{
							ret.Add(new OldAgeDeathReport(vet));
						}
						b.getVeterans().RemoveAt(w);
						w--;
					}
				}
			}
			for (int q = 0; q < nobleResidences.Count; q++)
			{
				Castle b = nobleResidences[q];
				b.completeMonthlyAction();
				if (!(b.getOwner().incrementAge()))
				{
					if (b.getOwner().isImportant())
					{
						ret.Add(new OldAgeDeathReport(b.getOwner()));
					}
					b.removeOwner(this);
				}
			}
			for (int q = 0; q < otherBuildings.Count; q++)
			{
				Building b = otherBuildings[q];
				b.completeMonthlyAction();
				if (!(b.getOwner().incrementAge()))
				{
					if (b.getOwner().isImportant())
					{
						ret.Add(new OldAgeDeathReport(b.getOwner()));
					}
					b.removeOwner(this);
				}
			}
		}
		return ret;
	}
	public List<Report> passDay(bool yearEnded)
	{
		List<Report> ret = new List<Report>();
		for (int q = 0; q < residentialAreas.Count; q++)
		{
			Village b = residentialAreas[q];
			b.completeDailyAction();
		}
		for (int q = 0; q < nobleResidences.Count; q++)
		{
			Castle b = nobleResidences[q];
			b.completeDailyAction();
		}
		for (int q = 0; q < otherBuildings.Count; q++)
		{
			Building b = otherBuildings[q];
			b.completeDailyAction();
		}
		if (yearEnded)
		{
			for (int q = 0; q < residentialAreas.Count; q++)
			{
				Village b = residentialAreas[q];
				if (!(b.getOwner().incrementAge()))
				{
					if (b.getOwner().isImportant())
					{
						ret.Add(new OldAgeDeathReport(b.getOwner()));
					}
					b.removeOwner(this);
				}
				for (int w = 0; w < b.getVeterans().Count; w++)
				{
					Human vet = b.getVeterans()[w];
					if (!(vet.incrementAge()))
					{
						if (vet.isImportant())
						{
							ret.Add(new OldAgeDeathReport(vet));
						}
						b.getVeterans().RemoveAt(w);
						w--;
					}
				}
			}
			for (int q = 0; q < nobleResidences.Count; q++)
			{
				Castle b = nobleResidences[q];
				b.completeMonthlyAction();
				if (!(b.getOwner().incrementAge()))
				{
					if (b.getOwner().isImportant())
					{
						ret.Add(new OldAgeDeathReport(b.getOwner()));
					}
					b.removeOwner(this);
				}
			}
			for (int q = 0; q < otherBuildings.Count; q++)
			{
				Building b = otherBuildings[q];
				b.completeMonthlyAction();
				if (!(b.getOwner().incrementAge()))
				{
					if (b.getOwner().isImportant())
					{
						ret.Add(new OldAgeDeathReport(b.getOwner()));
					}
					b.removeOwner(this);
				}
			}
		}
		return ret;
	}

	public List<int[]> getAllItemsInStorgage()
	{
		List<int[]> ret = new List<int[]>();

		for (int q = 0; q < otherBuildings.Count; q++)
		{
			if (otherBuildings[q] is Storehouse)
			{
				List<int[]> mat = ((Storehouse)otherBuildings[q]).getMaterials();
				for (int w = 0; w < mat.Count; w++)
				{
					InventoryIndex.moveItemToInventory(ret, (int[])mat[w].Clone());
				}
			}
		}

		return ret;
	}
}
