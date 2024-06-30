public class HistoricalRecord
{

	private long date;
	private string description;

	public static string[] MONTH_NAMES = {"Space Moon", "Light Moon", "Earthen Moon",
			"Time's Moon", "Reality Moon", "Dark Moon",
			"Fortune Moon", "War Moon", "Peace Moon",
			"Bonding Moon", "Life Moon", "Death Moon"};


	public static HistoricalRecord standardBattleDeath(string notification)
	{
		return new HistoricalRecord(notification);
		//TODO this method probably isn't necessary. You can just instantiate the
		//HistoricalRecord directly. I was thinking the process would be more complicated
		//than it actually needs to be
	}
	public HistoricalRecord(string notification)
	{
		this.description = notification;
		this.date = GeneralGameplayManager.getDaysSinceGameStart();
	}
	public string getDateAsString()
	{
		return getTimeAsString(date);
	}

	public static string getTimeAsString(long dayOfGame)
    {
		long year = dayOfGame / 360;
		dayOfGame %= 360;
		long month = (dayOfGame / 12) + 1;
		dayOfGame %= 12;
		long dayOfYear = dayOfGame + 1;
		//TODO make cooler names for the months and the calendar system (___ moon, 2nd year of the ___ calendar)
		return $"Day {dayOfYear} of Month {MONTH_NAMES[month]}, in Year {year}";
	}
}
