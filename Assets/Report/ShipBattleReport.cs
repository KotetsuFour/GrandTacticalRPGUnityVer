public class ShipBattleReport : Report
{

	private int[] details;
	private int atkLevelsGained;
	private bool dfdGainedLevel;
	private string initialDeathNotification;
	private string shipDeathNotification;
	private Human deadSpeaker;
	private Human deadSpeakerSupportPartner;
	private string deathQuote;
	private string despairQuote;
	private bool battleShouldEnd;

	public ShipBattleReport(int[] details)
	{
		this.details = details;
	}

	public void setAtkGainedLevel(bool atkGainedLevel)
	{
		if (atkGainedLevel)
		{
			atkLevelsGained++;
		}
	}

	public void setDfdGainedLevel(bool dfdGainedLevel)
	{
		this.dfdGainedLevel = dfdGainedLevel;
	}

	public void setInitialDeathNotification(string notification)
	{
		this.initialDeathNotification = notification;
	}
	public Human getDeadSpeaker()
	{
		return deadSpeaker;
	}

	public void setDeadSpeaker(Human deadSpeaker)
	{
		this.deadSpeaker = deadSpeaker;
	}

	public Human getDeadSpeakerSupportPartner()
	{
		return deadSpeakerSupportPartner;
	}

	public void setDeadSpeakerSupportPartner(Human deadSpeakerSupportPartner)
	{
		this.deadSpeakerSupportPartner = deadSpeakerSupportPartner;
	}
	public string getDeathQuote()
	{
		return deathQuote;
	}

	public void setDeathQuote(string deathQuote)
	{
		this.deathQuote = deathQuote;
	}

	public string getDespairQuote()
	{
		return despairQuote;
	}

	public void setDespairQuote(string despairQuote)
	{
		this.despairQuote = despairQuote;
	}

	public bool shouldEndBattle()
	{
		return battleShouldEnd;
	}

	public void setShouldEndBattle(bool shouldEndBattle)
	{
		this.battleShouldEnd = shouldEndBattle;
	}

	public void setShipDeathNotification(string notification)
	{
		// TODO Auto-generated method stub

	}


}
