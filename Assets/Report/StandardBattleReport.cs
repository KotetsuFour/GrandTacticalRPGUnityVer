public class StandardBattleReport : Report
{

	private int[] details;
	private string notification;
	private Human deadSpeaker;
	private Human deadSpeakerSupportPartner;
	private string deathQuote;
	private string despairQuote;
	private bool battleShouldEnd;
	private bool atkGainedLevel;
	private bool dfdGainedLevel;

	public StandardBattleReport(int[] details)
	{
		this.details = details;
	}

	public bool shouldEndBattle()
	{
		return battleShouldEnd;
	}

	public void setShouldEndBattle(bool shouldEndBattle)
	{
		this.battleShouldEnd = shouldEndBattle;
	}

	public int[] getDetails()
	{
		return details;
	}

	public string getNotification()
	{
		return notification;
	}

	public void setNotification(string notification)
	{
		this.notification = notification;
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

	public bool isAtkGainedLevel()
	{
		return atkGainedLevel;
	}

	public void setAtkGainedLevel(bool atkGainedLevel)
	{
		this.atkGainedLevel = atkGainedLevel;
	}

	public bool isDfdGainedLevel()
	{
		return dfdGainedLevel;
	}

	public void setDfdGainedLevel(bool dfdGainedLevel)
	{
		this.dfdGainedLevel = dfdGainedLevel;
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
}
