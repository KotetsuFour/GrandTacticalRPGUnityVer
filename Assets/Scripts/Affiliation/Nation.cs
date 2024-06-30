using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Nation
{

	public static string[] NATION_TYPES = { "Nation", "Kingdom", "Hegemony", "Empire" };

	public static int MAX_ARMY_SIZE = 500;

	protected string name;
	protected List<CityState> cityStates;
	protected Human leader;
	protected int type;
	protected int nationalLanguage;
	protected Dictionary<Nation, DiplomaticRelation> diplomaticRelations;
	protected List<Unit> army;
	protected List<UnitGroup> unitGroups;
	protected List<HistoricalRecord> history;

	/**
	 * Constructor for player nation
	 * @param name
	 * @param capitalName
	 * @param type
	 * @param language
	 */
	public Nation(string name, string capitalName, int type, int language)
	{
		this.name = name;
		this.cityStates = new List<CityState>();
		//City-State automatically adds itself to the nation
		this.nationalLanguage = language;
		new CityState(capitalName, this);
		this.type = type;
		this.diplomaticRelations = new Dictionary<Nation, DiplomaticRelation>();
		this.diplomaticRelations.Add(this, null);
		this.army = new List<Unit>(50);
		this.unitGroups = new List<UnitGroup>();
		this.history = new List<HistoricalRecord>();
	}

	/**
	 * General constructor, used for AI nations
	 */
	public Nation()
	{
		//		this.nationalLanguage = RNGStuff.nextInt(RNGStuff.numberOfLanguages());
		this.name = RNGStuff.newLocationName(this.nationalLanguage);
		this.cityStates = new List<CityState>();
		//City-State automatically adds itself to the nation
		new CityState(RNGStuff.newLocationName(this.nationalLanguage), this);
		this.leader = Human.completelyRandomHuman(cityStates[0]);
		this.type = RNGStuff.nextInt(NATION_TYPES.Length);
		this.diplomaticRelations = new Dictionary<Nation, DiplomaticRelation>();
		this.diplomaticRelations.Add(this, null);
		this.army = new List<Unit>(50);
		army.Add(this.leader);
		this.unitGroups = new List<UnitGroup>();
		this.history = new List<HistoricalRecord>();
	}

	/**
	 * Constructor for seceeded nations
	 * @param cs
	 */
	protected Nation(CityState cs)
	{
		Nation formerNation = cs.getNation();
		cs.defect(this);
		this.cityStates = new List<CityState>();
		cityStates.Add(cs);
		formerNation.cityStates.Remove(cs);
		this.nationalLanguage = cs.getLanguage();
		this.name = RNGStuff.newLocationName(this.nationalLanguage);
		this.type = RNGStuff.nextInt(NATION_TYPES.Length);
		this.diplomaticRelations = new Dictionary<Nation, DiplomaticRelation>();
		this.diplomaticRelations.Add(this, null);
		this.army = new List<Unit>(50);
		this.unitGroups = new List<UnitGroup>();
		this.history = new List<HistoricalRecord>();
		formerNation.cityStates.Remove(cs);
		Human formerLeader = formerNation.leader;
		//Start at 1, because the capital cannot rebel
		for (int q = 1; q < formerNation.cityStates.Count; q++)
		{
			CityState check = formerNation.cityStates[q];
			int diffsWithRebels = Mathf.Abs(check.getNationalism() - cs.getNationalism())
					+ Mathf.Abs(check.getMilitarism() - cs.getMilitarism())
					+ Mathf.Abs(check.getAltruism() - cs.getAltruism())
					+ Mathf.Abs(check.getFamilism() - cs.getFamilism())
					//More Tolerance, less likely to rebel
					+ check.getTolerance();
			int diffsWithNation = Mathf.Abs(check.getNationalism() - formerLeader.getNationalism())
					+ Mathf.Abs(check.getMilitarism() - formerLeader.getMilitarism())
					+ Mathf.Abs(check.getAltruism() - formerLeader.getAltruism())
					+ Mathf.Abs(check.getFamilism() - formerLeader.getFamilism())
					//More Confidence, more likely to rebel
					+ check.getConfidence();
			if (diffsWithNation > diffsWithRebels)
			{
				cityStates.Add(check);
				formerNation.cityStates.RemoveAt(q);
				q--;
				check.defect(this);
			}
		}
		//First get the humans, so we know if monsters should rebel or not
		for (int q = 0; q < formerNation.army.Count; q++)
		{
			Unit u = formerNation.army[q];
			if (u is Human)
			{
				Human h = (Human)u;
				if (this.cityStates.Contains(h.getHome())
						&& h.getFamilism() + h.getConfidence() > h.getNationalism() + h.getTolerance())
				{
					this.army.Add(h);
					formerNation.army.RemoveAt(q);
					q--;
					h.defect(this);
				}
				else
				{
					h.declareLoyalty();
				}
			}
		}
		//Then remove monsters if their master rebelled
		for (int q = 0; q < formerNation.army.Count; q++)
		{
			Unit u = formerNation.army[q];
			if (u is Monster)
			{
				Monster m = (Monster)u;
				if (m.getAffiliation() == this)
				{
					formerNation.army.RemoveAt(q);
					q--;
					m.defect(this);
				}
			}
		}
		Human lead = null;
		for (int q = 0; q < this.cityStates.Count && lead == null; q++)
		{
			lead = this.cityStates[q].getLeadingNoble();
		}
		if (lead == null)
		{
			for (int q = 0; q < this.cityStates.Count && lead == null; q++)
			{
				lead = this.cityStates[q].getLeadingVeteran();
			}
			if (lead == null)
			{
				lead = Human.completelyRandomHuman(cs);
			}
		}
		this.leader = lead;
		//TODO initialize relations with all nations that formerNation had relations with,
		//and with formerNation of course
		//TODO copy technology tree
		//TODO organize army into groups
		//TODO add secession to nation's history
	}

	public CityState getCapital()
	{
		return cityStates[0];
	}

	public List<CityState> getCityStates()
	{
		return cityStates;
	}

	public string getName()
	{
		return name;
	}

	public string getFullName()
	{
		return "The " + NATION_TYPES[type] + " of " + name;
	}

	public int getNationalLanguage()
	{
		return nationalLanguage;
	}

	public int getPopulation()
	{
		int pop = 0;
		for (int q = 0; q < cityStates.Count; q++)
		{
			pop += cityStates[q].getPopulation();
		}
		return pop;
	}

	public List<Unit> getArmy()
	{
		return army;
	}

	public Human getRuler()
	{
		return leader;
	}
	public void setRuler(Human ruler)
	{
		this.leader = ruler;
	}

	public List<UnitGroup> getUnitGroups()
	{
		return unitGroups;
	}

	/**
	 * Gives the military strength of the nation
	 * @return array with the following indexes:
	 * [0] = physical strength
	 * [1] = magical strength
	 * [2] = accuracy
	 * [3] = avoidance (Basic. that is, torso for units)
	 * [4] = crit rate
	 * [5] = crit avoidance (Basic. that is, torso for units)
	 * [6] = average attack speed
	 * [7] = defense (Basic. that is, torso for units)
	 * [8] = resistance
	 * [9] = head HP
	 * [10] = torso HP
	 */
	public int[] getPower()
	{
		int[] ret = new int[11];
		for (int q = 0; q < army.Count; q++)
		{
			int[] current = army[q].getPower();
			for (int w = 0; w < current.Length; w++)
			{
				ret[w] += current[w];
			}
		}
		ret[6] /= Mathf.Max(1, army.Count); //Make sure attack speed is the average

		//TODO add power of assigned things (ships, and defendable buildings)
		return ret;
	}

	public void addHistoricalRecord(HistoricalRecord record)
	{
		history.Add(record);
	}

	public bool isAtWarWith(Nation n)
	{
		if (n == null)
		{
			return true;
		}
		DiplomaticRelation r = diplomaticRelations[n];
		if (r == null)
		{
			return false;
		}
		return r.getCurrentEvent() is War;
	}

	public bool isAlliedWith(Nation n)
	{
		if (n == this)
		{
			return true;
		}
		DiplomaticRelation r = diplomaticRelations[n];
		if (r == null)
		{
			return false;
		}
		return r.isAlliance();
	}

	public DiplomaticRelation relationshipWith(Nation n)
	{
		DiplomaticRelation dr = diplomaticRelations[n];
		if (dr == null)
		{
			dr = new DiplomaticRelation(this, n);
			diplomaticRelations.Add(n, dr);
			n.diplomaticRelations.Add(this, dr);
		}
		return dr;
	}

	public War getCurrentWarWith(Nation n)
	{
		if (n == this)
		{
			//TODO
		}
		MajorEvent me = diplomaticRelations[n].getCurrentEvent();
		if (me is War)
		{
			return (War)me;
		}
		return null;
	}

	public Nation seceededNation(CityState rebels)
	{
		Nation ret = new Nation(rebels);
		//TODO anything else?
		return ret;
	}

	public List<Report> passMonth(bool yearEnded)
	{
		List<Report> ret = new List<Report>();
		for (int q = 0; q < cityStates.Count; q++)
		{
			ret.AddRange(cityStates[q].passMonth(yearEnded));
		}
		if (yearEnded)
		{
			for (int q = 0; q < army.Count; q++)
			{
				if (army[q] is Human && !((Human)army[q]).incrementAge())
				{
					ret.Add(new OldAgeDeathReport((Human)army[q]));
				}
			}
		}
		return ret;
	}

	public List<Report> passDay(bool yearEnded)
	{
		List<Report> ret = new List<Report>();
		for (int q = 0; q < cityStates.Count; q++)
		{
			ret.AddRange(cityStates[q].passDay(yearEnded));
		}
		if (yearEnded)
		{
			for (int q = 0; q < army.Count; q++)
			{
				if (army[q] is Human && !((Human)army[q]).incrementAge())
				{
					ret.Add(new OldAgeDeathReport((Human)army[q]));
				}
			}
		}
		return ret;
	}

	public List<int[]> getAllItemsInStorage()
	{
		List<int[]> ret = new List<int[]>();
		for (int q = 0; q < cityStates.Count; q++)
		{
			List<int[]> inv = cityStates[q].getAllItemsInStorgage();
			for (int w = 0; w < inv.Count; w++)
			{
				InventoryIndex.moveItemToInventory(ret, inv[w]);
			}
		}

		return ret;
	}

	public int[] getAllMountsForTraining()
	{
		//TODO give all mounts in training facilities
		return null;
	}

	public bool evaluateTradeDeal(int[] desired, int[] offered)
	{
		List<int[]> allItems = getAllItemsInStorage();
		int[] allMounts = getAllMountsForTraining();
		int nationalismAssets = 0;
		int familismAssets = 0;
		int altruismAssets = 0;
		int militarismAssets = 0;
		for (int q = 0; q < allItems.Count; q++)
		{
			Item item = InventoryIndex.getElement(allItems[q]);
			int worth = item.getApproximateWorth() * allItems[q][2];
			if (item is Weapon || item is Armor)
			{
				militarismAssets += worth;
				nationalismAssets += worth / 2;
			}
			else if (item is EdibleCrop)
			{
				familismAssets += worth;
				altruismAssets += worth / 2;
			}
			else if (item is Staff || item is UsableItem)
			{
				altruismAssets += worth;
				familismAssets += worth / 2;
			}
			else if (item is UsableCrop)
			{
				nationalismAssets += worth;
				familismAssets += worth;
			}
			else if (item is Resource)
			{
				nationalismAssets += worth;
			}
		}
		for (int q = 0; q < allMounts.Length; q++)
		{
			militarismAssets += (Mount.values()[q].getWorth() * allMounts[q]);
		}

		//TODO evaluate trade deals

		//TODO based on items held, mounts held, and trade deals established,
		//evaluate the nation's current standing in all pursuits.
		//TODO Evaluate worth of trade
		//TODO Decide if the trade is worth it

		return false;
	}

	public List<Storehouse> getAllStorehousesInNation(Storehouse sender)
	{
		List<Storehouse> ret = new List<Storehouse>();
		for (int q = 0; q < cityStates.Count; q++)
		{
			List<Building> b = cityStates[q].getOtherBuildings();
			for (int w = 0; w < b.Count; w++)
			{
				Building build = b[w];
				if (build is Storehouse && build != sender)
				{
					ret.Add((Storehouse)build);
				}
			}
		}
		return ret;
	}

	public List<TradeCenter> getAllTradeCentersInNation()
	{
		List<TradeCenter> ret = new List<TradeCenter>();
		for (int q = 0; q < cityStates.Count; q++)
		{
			List<Building> b = cityStates[q].getOtherBuildings();
			for (int w = 0; w < b.Count; w++)
			{
				Building build = b[w];
				if (build is TradeCenter)
				{
					ret.Add((TradeCenter)build);
				}
			}
		}
		return ret;
	}

	public void declareWar(Nation n, War.WarCause choice, long date)
	{
		War war = new War(this, n, choice, date);
		DiplomaticRelation dr = diplomaticRelations[n];
		dr.startWar(war);
		HistoricalRecord rec = new HistoricalRecord(war.getName() + " Begins. Cause: " + war.getCause().getDisplayName());
		addHistoricalRecord(rec);
		n.addHistoricalRecord(rec);
		//TODO if this is AI nation, notify AI to reorganize military
		//TODO allow respondent to ask allies for help

		for (int q = 0; q < army.Count; q++)
		{
			army[q].incrementWars();
		}
		for (int q = 0; q < n.army.Count; q++)
		{
			n.army[q].incrementWars();
		}
	}

	public int numCurrentWars()
	{
		int ret = 0;
		foreach (DiplomaticRelation dr in diplomaticRelations.Values)
		{
			if (dr.getCurrentEvent() is War)
			{
				ret++;
			}
		}
		return ret;
	}
}
