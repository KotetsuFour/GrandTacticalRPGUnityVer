public class Demeanor
{

	public static int BROW_BLANK = 0;
	public static int BROW_HAPPY = 1;
	public static int BROW_AFRAID = 2;
	public static int BROW_ANGRY = 3;
	public static int BROW_CONFUSED = 4;

	public static int MOUTH_BLANK = 0;
	public static int MOUTH_HAPPY = 1;
	public static int MOUTH_UPSET = 2;
	public static int MOUTH_SLANT = 3;


	public static Demeanor SERIOUS = new Demeanor("Serious", 0, BROW_CONFUSED, MOUTH_BLANK,
			"Yes, I agree.",
			"I can see that.",
			"Hmm... I disagree.");
	public static Demeanor RELAXED = new Demeanor("Relaxed", 0, BROW_BLANK, MOUTH_SLANT,
			"Yeah, dude...",
			"Oh, cool...",
			"Eh, I dunno...");
	public static Demeanor DETERMINED = new Demeanor("Determined", 0, BROW_ANGRY, MOUTH_HAPPY,
			"For sure!",
			"Ah, I see.",
			"I don't know about that.");
	public static Demeanor ENTHUSIASTIC = new Demeanor("Enthusiastic", 0, BROW_HAPPY, MOUTH_HAPPY,
			"",
			"",
			"");
	public static Demeanor NERVOUS = new Demeanor("Nervous", 0, BROW_AFRAID, MOUTH_HAPPY,
			"Oh, yeah. Heh.",
			"Ah.",
			"Oh...");
	public static Demeanor FRIENDLY = new Demeanor("Friendly", 0, BROW_HAPPY, MOUTH_SLANT,
			"Totally, dude!",
			"Huh, interesting way to think about it!",
			"Eh, we'll have to agree to disagree.");
	public static Demeanor POLITE = new Demeanor("Polite", 0, BROW_BLANK, MOUTH_HAPPY,
			"",
			"",
			"");
	public static Demeanor CURIOUS = new Demeanor("Curious", 0, BROW_CONFUSED, MOUTH_SLANT,
			"Yes, I think so too!",
			"That's an interesting way to think about it.",
			"I'm having trouble understanding...");
	public static Demeanor DISMISSIVE = new Demeanor("Dismissive", 0, BROW_BLANK, MOUTH_BLANK,
			"",
			"",
			"");
	public static Demeanor CHARISMATIC = new Demeanor("Charismatic", 0, BROW_CONFUSED, MOUTH_HAPPY,
			"",
			"",
			"");
	public static Demeanor ASSERTIVE = new Demeanor("Assertive", 0, BROW_ANGRY, MOUTH_SLANT,
			"",
			"",
			"");
	public static Demeanor REFLECTIVE = new Demeanor("Reflective", 0, BROW_HAPPY, MOUTH_BLANK,
			"",
			"",
			"");
	public static Demeanor ABSENT = new Demeanor("Absent", 0, BROW_HAPPY, MOUTH_SLANT,
			"Heh.",
			"Sure.",
			"Meh.");
	public static Demeanor CREEPY = new Demeanor("Creepy", 1, BROW_ANGRY, MOUTH_HAPPY,
			"You get me...",
			"Hehehe.",
			"...");
	public static Demeanor SNOBBISH = new Demeanor("Snobbish", 1, BROW_CONFUSED, MOUTH_UPSET,
			"That's what I keep telling people!",
			"Yeah, I guess.",
			"Incorrect. Try again.");
	public static Demeanor INTIMIDATING = new Demeanor("Intimidating", 1, BROW_ANGRY, MOUTH_UPSET,
			"Indeed.",
			"Hmm...",
			"You... wanna run that by me again?");


	private string displayName;
	private int rarity;
	private int browOrientation;
	private int mouthOrientation;

	private string agree;
	private string indifferent;
	private string disagree;

	//The following are portions of quotes, meant for constructing unique pieces of dialogue

	private string de; //Expression disappointment that loving support partner was killer
	private string ie; //Expression of irony that support partner was killer
	private string ve; //Expression of contempt towards support parter who was killer

	private string re; //Expression of regret for killing support partner
	private string se; //Expression of mild shame for killing support partner
	private string ge; //Expression of gladness after killing support partner

	private string dp; //Dying words to dead parent(s)
	private string ap; //Dying words to alive parent(s)

	private string fw;
	private string nw;
	private string hw;

	private string rr; //Dying words to support partner in romantic relationship
	private string sr; //Dying words to support partner in strong relationship
	private string gr; //Dying words to support partner in good relationship

	private Demeanor(string displayName, int rarity,
			int browOrientation, int mouthOrientation,
			string agree, string indifferent, string disagree)
	{
		this.displayName = displayName;
		this.rarity = rarity;
		this.browOrientation = browOrientation;
		this.mouthOrientation = mouthOrientation;
		this.agree = agree;
		this.indifferent = indifferent;
		this.disagree = disagree;
	}
	public string getDisplayName()
	{
		return displayName;
	}
	public int getRarity()
	{
		return rarity;
	}
	public int getBrowOrientation()
	{
		return browOrientation;
	}
	public int getMouthOrientation()
	{
		return mouthOrientation;
	}

	public string agreementQuote()
	{
		return agree;
	}
	public string indifferenceQuote()
	{
		return indifferent;
	}
	public string disagreementQuote()
	{
		return disagree;
	}

	public string disappointedExpression()
	{
		return de;
	}
	public string ironicExpression()
	{
		return ie;
	}
	public string vengefulExpression()
	{
		return ve;
	}

	public string regretfulExpression()
	{
		return re;
	}
	public string shamefulExpression()
	{
		return se;
	}
	public string gladnessExpression()
	{
		return ge;
	}

	public string aliveParent()
	{
		return ap;
	}
	public string deadParent()
	{
		return dp;
	}

	public string romanticRelationship()
	{
		return rr;
	}
	public string strongRelationship()
	{
		return sr;
	}
	public string goodRelationship()
	{
		return gr;
	}

	public static Demeanor[] values()
    {
		return new Demeanor[] { SERIOUS, RELAXED, DETERMINED, ENTHUSIASTIC, NERVOUS, FRIENDLY,
			POLITE, CURIOUS, DISMISSIVE, CHARISMATIC, ASSERTIVE, REFLECTIVE, ABSENT, CREEPY,
			SNOBBISH, INTIMIDATING
		};
    }

}

