using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class War : MajorEvent
{

	private Nation initiator;
	private Nation responder;
	private WarCause warCause;
	private int battles;
	private int initiatorRecruitDeaths;
	private int responderRecruitDeaths;
	private int initiatorMonsterDeaths;
	private int responderMonsterDeaths;
	private int initiatorCloneDeaths;
	private int responderCloneDeaths;
	private int initiatorCivilianDeaths;
	private int responderCivilianDeaths;

	public War(Nation initiator, Nation responder, WarCause cause, long date)
			: base(date)
	{
		this.initiator = initiator;
		this.responder = responder;
		this.warCause = cause;
		this.name = initiator.getName() + "-" + responder.getName() + " War";
	}

	public void incrementKills(Unit unit)
	{
		if (unit.getAffiliation() == initiator)
		{
			if (unit is Monster)
			{
				this.initiatorMonsterDeaths++;
			}
			else if (unit is Clone)
			{
				this.initiatorCloneDeaths++;
			}
			else
			{
				this.initiatorRecruitDeaths++;
			}
		}
		else if (unit.getAffiliation() == responder)
		{
			if (unit is Monster)
			{
				this.responderMonsterDeaths++;
			}
			else if (unit is Clone)
			{
				this.responderCloneDeaths++;
			}
			else
			{
				this.responderRecruitDeaths++;
			}
		}
		else
		{
			throw new Exception("The unit given is not a part of this war");
		}
	}

	public void registerCivilianDeaths(Nation n, int num)
	{
		if (n == initiator)
		{
			initiatorCivilianDeaths += num;
		}
		else if (n == responder)
		{
			responderCivilianDeaths += num;
		}
		else
		{
			throw new Exception("The nation given is not a part of this war");
		}
	}

	public void incrementBattles()
	{
		battles++;
	}

	public WarCause getCause()
	{
		return warCause;
	}

	public void rename(string name)
	{
		this.name = name;
	}

	public string getDescription()
	{
		// TODO Auto-generated method stub
		return null;
	}

	public Nation getInitiator()
	{
		return this.initiator;
	}

	public Nation getResponder()
	{
		return this.responder;
	}

	public int getInitiatorRecruitDeaths()
	{
		return this.initiatorRecruitDeaths;
	}

	public int getInitiatorCivilianDeaths()
	{
		return this.initiatorCivilianDeaths;
	}

	public int getInitiatorCloneDeaths()
	{
		return this.initiatorCloneDeaths;
	}

	public int getInitiatorMonsterDeaths()
	{
		return this.initiatorMonsterDeaths;
	}

	public int getResponderRecruitDeaths()
	{
		return this.responderRecruitDeaths;
	}

	public int getResponderCivilianDeaths()
	{
		return this.responderCivilianDeaths;
	}

	public int getResponderCloneDeaths()
	{
		return this.responderCloneDeaths;
	}

	public int getResponderMonsterDeaths()
	{
		return this.responderMonsterDeaths;
	}

	public class WarCause
	{
		public static WarCause CANCEL = new WarCause("Cancel", true);
		public static WarCause TRADE_REFUSAL = new WarCause("Target refused peaceful diplomacy", true);
		public static WarCause WEALTH_ENVY = new WarCause("To steal wealth from target", true);
		public static WarCause EXPANSION_BLOCKING = new WarCause("Target is blocking expansion", true);
		public static WarCause ALLIANCE_LOYALTY = new WarCause("Joining war on behalf of an ally", false);
		public static WarCause SIMPLE_CONQUEST = new WarCause("Pursuing goal of conquest", true);
		public static WarCause SIMPLE_HATRED = new WarCause("Desire to destroy the target", true);


		private string displayName;
		private bool initiatorOption;

		WarCause(string displayName, bool initiatorOption)
		{
			this.displayName = displayName;
			this.initiatorOption = initiatorOption;
		}

		public string getDisplayName()
		{
			return this.displayName;
		}

		public bool isInitiatorOption()
		{
			return this.initiatorOption;
		}


		public string tostring()
		{
			return getDisplayName();
		}

		public static WarCause[] getAllInitiatorOptions()
		{
			List<WarCause> list = new List<WarCause>(values().Length);
			for (int q = 0; q < values().Length; q++)
			{
				if (values()[q].isInitiatorOption())
				{
					list.Add(values()[q]);
				}
			}
			WarCause[] ret = new WarCause[list.Count];
			for (int q = 0; q < list.Count; q++)
			{
				ret[q] = list[q];
			}
			return ret;
		}

		public static WarCause[] values()
        {
			return new WarCause[] { CANCEL, TRADE_REFUSAL, WEALTH_ENVY, EXPANSION_BLOCKING, ALLIANCE_LOYALTY,
			SIMPLE_CONQUEST, SIMPLE_HATRED };
        }
	}

}
