using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DiplomaticRelation
{

	private Nation nation1;
	private Nation nation2;
	private int relationshipStrength;
	private int wars;
	private int sports;
	private int festivals;
	private List<int[]> tradeDeals;
	private MajorEvent currentEvent;

	public DiplomaticRelation(Nation nation1, Nation nation2)
	{
		this.nation1 = nation1;
		this.nation2 = nation2;
		this.tradeDeals = new List<int[]>();
	}

	public MajorEvent getCurrentEvent()
	{
		return currentEvent;
	}

	public bool isAlliance()
	{
		return currentEvent is WarAlliance;
	}

	public Nation getNation1()
	{
		return nation1;
	}

	public Nation getNation2()
	{
		return nation2;
	}

	public int getRelationshipStrength()
	{
		return relationshipStrength;
	}

	public int getWarsCount()
	{
		return wars;
	}

	public int getSportsCount()
	{
		return sports;
	}

	public int getFestivalsCount()
	{
		return festivals;
	}

	public List<int[]> getTradeDeals()
	{
		return tradeDeals;
	}

	public string getRelationshipStrengthDisplay()
	{
		string sb = "";
		if (relationshipStrength >= 90)
		{
			sb += ("Faithful Alliance");
		}
		else if (relationshipStrength >= 70)
		{
			sb += ("Very Strong");
		}
		else if (relationshipStrength >= 50)
		{
			sb += ("Strong");
		}
		else if (relationshipStrength >= 30)
		{
			sb += ("Good");
		}
		else if (relationshipStrength >= 10)
		{
			sb += ("Decent");
		}
		else if (relationshipStrength >= 0)
		{
			sb += ("Somewhat Positive");
		}
		else if (relationshipStrength >= -10)
		{
			sb += ("Somewhat Negative");
		}
		else if (relationshipStrength >= -30)
		{
			sb += ("Poor");
		}
		else if (relationshipStrength >= -50)
		{
			sb += ("Bad");
		}
		else if (relationshipStrength >= -70)
		{
			sb += ("Contemptuous");
		}
		else if (relationshipStrength >= -90)
		{
			sb += ("Very Contemptuous");
		}
		else
		{
			sb += ("Deep Mutual Hatred");
		}
		sb += (" (" + relationshipStrength + ")");

		return sb;
	}

	public void cancelTradeDeal(int idx)
	{
		tradeDeals.RemoveAt(idx);
		//TODO maybe affect relationship
	}

	public void startWar(War war)
	{
		tradeDeals = new List<int[]>();
		//TODO probably go through trade centers and stop shipping

		this.currentEvent = war;
		//TODO affect relationship

		this.wars++;
	}
}
