using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Human : Unit, Equippable
{
	public class Interest
	{
		public static Interest GARDENING = new Interest("Gardening");
		public static Interest HORSEBACK_RIDING = new Interest("Horseback Riding");
		public static Interest STUDYING = new Interest("Studying");
		public static Interest COOKING = new Interest("Cooking");
		public static Interest HUNTING = new Interest("Hunting");
		public static Interest COLLECTING = new Interest("Collecting");
		public static Interest PAINTING = new Interest("Painting");
		public static Interest WRITING = new Interest("Writing");
		public static Interest READING = new Interest("Reading");
		public static Interest PLAYING_MUSIC = new Interest("Playing Music");
		public static Interest WRITING_MUSIC = new Interest("Writing Music");
		public static Interest SCIENTIFIC_DISCOVERY = new Interest("Scientific Discovery");
		public static Interest ANIMALS = new Interest("Animals");
		public static Interest STRATEGY_GAMES = new Interest("Strategy Games");
		public static Interest ADVENTURE = new Interest("Adventure");
		public static Interest CARPENTRY = new Interest("Carpentry");
		public static Interest PRACTICAL_JOKES = new Interest("Practical Jokes");
		public static Interest KNITTING = new Interest("Knitting");
		public static Interest SWIMMING = new Interest("Swimming");
		public static Interest PERFORMING = new Interest("Performing");
		public static Interest SCULPTING = new Interest("Sculpting");
		public static Interest SPORTS = new Interest("Sports");
		public static Interest HIKING = new Interest("Hiking");
		public static Interest TRAVELING = new Interest("Traveling");

	private string displayName;
	private Interest(string displayName)
	{
		this.displayName = displayName;
	}

	public string getDisplayName()
	{
		return displayName;
	}
		public static Interest[] values()
        {
			return new Interest[] { GARDENING, HORSEBACK_RIDING, STUDYING, COOKING, HUNTING, COLLECTING,
			PAINTING, WRITING, READING, PLAYING_MUSIC, WRITING_MUSIC, SCIENTIFIC_DISCOVERY, ANIMALS,
			STRATEGY_GAMES, ADVENTURE, CARPENTRY, PRACTICAL_JOKES, KNITTING, SWIMMING, PERFORMING,
			SCULPTING, SPORTS, HIKING, TRAVELING };
        }
}

	public class CombatTrait
	{
		public static CombatTrait ACCURACY = new CombatTrait("Accuracy", 5);
		public static CombatTrait AVOIDANCE = new CombatTrait("Avoidance", 5);
		public static CombatTrait CRITRATE = new CombatTrait("Critical Hit Rate", 5);
		public static CombatTrait CRITAVOID = new CombatTrait("Security", 5);
		public static CombatTrait ATTACKPOWER = new CombatTrait("Attack Power", 5);

		private string displayName;
	private int supportDividend;
	private CombatTrait(string displayName, int supportDividend)
	{
		this.displayName = displayName;
		this.supportDividend = supportDividend;
	}
	public string getDisplayName()
	{
		return displayName;
	}
	public int getSupportDividend()
	{
		return supportDividend;
	}
		public static CombatTrait[] values()
        {
			return new CombatTrait[] { ACCURACY, AVOIDANCE, CRITRATE, CRITAVOID, ATTACKPOWER };
        }
}



public static int RIGHT_ARM = 2;
	public static int LEFT_ARM = 3;
	public static int RIGHT_LEG = 4;
	public static int LEFT_LEG = 5;
	public static int RIGHT_EYE = 6;
	public static int LEFT_EYE = 7;
	public static int MOUNT = 8;

	public static string[] BODY_PARTS_STRINGS = {"Head", "Torso", "Right Arm",
			"Left Arm", "Right Leg", "Left Leg", "Right Eye", "Left Eye", "Mount"};

	protected int age;
	protected bool humanIsMortal;
	protected bool gender; //True for female, false for male
	protected int nationalism;
	protected int militarism;
	protected int altruism;
	protected int familism;
	protected int confidence;
	protected int tolerance;
	protected Interest[] interests;
	protected Interest[] disinterests;
	protected Demeanor demeanor;
	protected CombatTrait valuedTrait;
	protected int morale; //Integer from 0-100
	protected int[] appearance;
	protected int[][] inventory;
	protected int[] armor;
	/**
	 * proficiency indexes are...
	 * 0 for sword
	 * 1 for lance
	 * 2 for axe
	 * 3 for bow
	 * 4 for knife
	 * 5 for ballista
	 * 6 for anima
	 * 7 for light
	 * 8 for dark
	 * 9 for staff
	 */
	protected int[] proficiency;
	protected CityState home;
	protected Nation affiliation;
	protected Human supportPartner;
	protected bool isParent;
	protected int relationshipWithPlayer;
	protected int relationshipWithSupportPartner;

	public Human(string name, bool gender, int[] maxHPs, int magic, int skill, int reflex,
			int awareness, int resistance, int movement, int leadership, int[] maxHPGrowths,
			int magGrowth, int sklGrowth, int rfxGrowth, int awrGrowth, int resGrowth,
			int age, int[] personalValues, int[] appearance, Interest[] interests,
			Interest[] disinterests, Demeanor demeanor, CombatTrait valuedTrait, CityState home)
		: base(0, null, //Start out at level 0, with no class
				name, maxHPs, magic, skill, reflex,
				awareness, resistance, movement, leadership, maxHPGrowths,
				magGrowth, sklGrowth, rfxGrowth, awrGrowth, resGrowth)
	{
		//Specifically human values
		this.gender = gender;
		this.age = age;
		this.humanIsMortal = true; //No one is born immortal
		this.nationalism = personalValues[0];
		this.militarism = personalValues[1];
		this.altruism = personalValues[2];
		this.familism = personalValues[3];
		this.confidence = personalValues[4];
		this.tolerance = personalValues[5];
		this.appearance = appearance;
		this.interests = interests;
		this.disinterests = disinterests;
		this.demeanor = demeanor;
		this.valuedTrait = valuedTrait;
		//TODO initialize morale
		this.inventory = new int[3][];
		this.proficiency = new int[10];
		this.home = home;
		this.affiliation = home.getNation();
		this.relationshipWithPlayer = 0;
		this.relationshipWithSupportPartner = 0;
	}

	public void assignSupportPartner(Human partner)
	{
		if (supportPartner != null || partner.supportPartner != null
				|| isParent || partner.isParent)
		{
			throw new Exception("Cannot assign a new support partner");
		}
		this.supportPartner = partner;
		partner.supportPartner = this;
		calculateInitialSupportRelationship();
		partner.calculateInitialSupportRelationship();
	}

	private void calculateInitialSupportRelationship()
	{
		Human p = supportPartner;
		int love = 0;
		int hate = 0;
		for (int q = 0; q < interests.Length; q++)
		{
			for (int w = 0; w < p.interests.Length; w++)
			{
				if (interests[q] == p.interests[w])
				{
					love += 10;
				}
			}
		}
		for (int q = 0; q < disinterests.Length; q++)
		{
			for (int w = 0; w < p.interests.Length; w++)
			{
				if (disinterests[q] == p.interests[w])
				{
					hate += 10;
				}
			}
		}
		relationshipWithSupportPartner = love - hate;
		int natDiff = Mathf.Abs(nationalism - p.getNationalism());
		if (natDiff <= 10)
		{
			relationshipWithSupportPartner += 10;
		}
		else
		{
			relationshipWithSupportPartner -= natDiff / tolerance;
		}
		int milDiff = Mathf.Abs(militarism - p.getMilitarism());
		if (milDiff <= 10)
		{
			relationshipWithSupportPartner += 10;
		}
		else
		{
			relationshipWithSupportPartner -= milDiff / tolerance;
		}
		int altDiff = Mathf.Abs(altruism - p.getAltruism());
		if (altDiff <= 10)
		{
			relationshipWithSupportPartner += 10;
		}
		else
		{
			relationshipWithSupportPartner -= altDiff / tolerance;
		}
		int famDiff = Mathf.Abs(familism - p.getFamilism());
		if (famDiff <= 10)
		{
			relationshipWithSupportPartner += 10;
		}
		else
		{
			relationshipWithSupportPartner -= famDiff / tolerance;
		}
		if (p.getDemeanor().getRarity() > 0)
		{
			//Rare demeanors are generally less likeable.
			//Maybe I'll change this so the Demeanor enum specifies likeability
			relationshipWithSupportPartner -= 10;
		}
	}
	public void alterSupportRelationship(int amount)
	{
		//I've decided that relationship can be as positive or negative as possible
		//It doesn't have to be bounded by -100 and 100
		relationshipWithSupportPartner += amount;
	}
	public int getRelationshipWithSupportPartner()
	{
		return relationshipWithSupportPartner;
	}
	public int getRelationshipWithPlayer()
	{
		return relationshipWithPlayer;
	}
	/**
	 * This is used to determine if this unit is married to their support partner and
	 * is able to reproduce.
	 * @return
	 */
	public bool mayReproduce()
	{

		//Must have a support partner to potentially be their spouse
		if (supportPartner == null)
		{
			return false;
		}
		Human p = supportPartner;

		//Must not already have a child. Only allowing one child ensures that there are
		//no uncles, aunts, cousins, etc. for someone to marry. Also, not allowing parents
		//to re-marry prevents them from marrying their children, grandchildren, etc.
		if (isParent || p.isParent)
		{
			return false;
		}

		//Let's not have any significant age gaps.
		//Sure they're all adults, but I just don't feel like it
		//Also, since Humans can be instantiated at max age 30, it makes
		//sense that you'd be at least 50 by the time we meet your adult kid
		//The lore logic falls apart if you consider a couple that enlisted at
		//30, retired by 50, and introduced a kid at 50 who enlisted at 30
		//(that'd mean the child was born ten years before his parents met)
		//But whatever. We never explicitly say they didn't know each other before
		//enlisting. Maybe they did and had a kid before marriage
		if (Mathf.Abs(age - p.age) > 10 || Mathf.Min(age, p.age) < 50)
		{
			return false;
		}

		//Must be married to support partner
		return marriedToSupportPartner();
	}

	public bool marriedToSupportPartner()
	{
		//Obviously, no support partner means no romantic relationship
		if (supportPartner == null)
		{
			return false;
		}

		//Must be opposite genders
		if (gender == supportPartner.getGender())
		{
			return false;
		}

		//Must be mutually in love (which might as well be a marriage, since support
		//partners are already exclusively together for life)
		return relationshipWithSupportPartner >= 90 && supportPartner.relationshipWithSupportPartner >= 90;
	}

	/**
	 * Assuming this is a legal action, this person and their support partner
	 * have a child
	 * @return
	 */
	public Offspring reproduce()
	{
		isParent = true;
		supportPartner.isParent = true;
		return Offspring.getChild(this, supportPartner);
	}

	public bool canAssignSupportPartner()
	{
		return supportPartner == null && !(isParent);
	}

	public void assignClass(UnitClass unitClass)
	{
		this.unitClass = unitClass;
		int[] gMods = unitClass.getGrowthModifiers();
		bodyPartsMaximumHPGrowth[HEAD] += gMods[0]; //gMods[0] is for the head
		bodyPartsMaximumHPGrowth[TORSO] += gMods[1]; //gMods[1] is for the torso
		bodyPartsMaximumHPGrowth[RIGHT_ARM] += gMods[2]; //gMods[2] is for the arms
		bodyPartsMaximumHPGrowth[LEFT_ARM] += gMods[2];
		bodyPartsMaximumHPGrowth[RIGHT_LEG] += gMods[3]; //gMods[3] is for the legs
		bodyPartsMaximumHPGrowth[LEFT_LEG] += gMods[3];
		//		bodyPartsMaximumHPGrowth[RIGHT_EYE] += 0;        Eyes do not change
		//		bodyPartsMaximumHPGrowth[LEFT_EYE] += 0;
		bodyPartsMaximumHPGrowth[MOUNT] = unitClass.initializeMountGrowth();
		bodyPartsMaximumHP[MOUNT] = unitClass.initializeMountHealth();
		bodyPartsCurrentHP[MOUNT] = bodyPartsMaximumHP[MOUNT]; //Mount starts at full health

		//The rest of the growths are as follows
		magicGrowth += gMods[4];
		skillGrowth += gMods[5];
		reflexGrowth += gMods[6];
		awarenessGrowth += gMods[7];
		resistanceGrowth += gMods[8];

		for (int q = 0; q < proficiency.Length; q++)
		{
			proficiency[q] += unitClass.getProficiencyModifiers()[q];
		}
		/*
		if (unitClass.getTier() > 1)
		{
			int[] sMods = unitClass.getStatModifiers();
			bodyPartsMaximumHP[HEAD] += sMods[0]; //sMods[0] is for the head
			bodyPartsMaximumHP[TORSO] += sMods[1]; //sMods[1] is for the torso
			bodyPartsMaximumHP[RIGHT_ARM] += sMods[2]; //sMods[2] is for the arms
			bodyPartsMaximumHP[LEFT_ARM] += sMods[2];
			bodyPartsMaximumHP[RIGHT_LEG] += sMods[3]; //sMods[3] is for the legs
			bodyPartsMaximumHP[LEFT_LEG] += sMods[3];
			//			bodyPartsMaximumHP[RIGHT_EYE] += 0;        Eyes do not change
			//			bodyPartsMaximumHP[LEFT_EYE] += 0;
			bodyPartsMaximumHP[MOUNT] = unitClass.initializeMountGrowth(); //Mount is upgraded
			bodyPartsMaximumHP[MOUNT] = unitClass.initializeMountHealth();
			bodyPartsCurrentHP[MOUNT] = bodyPartsMaximumHP[MOUNT]; //New mount gets full health
																   //The rest is as follows
			magic += sMods[4];
			skill += sMods[5];
			reflex += sMods[6];
			awareness += sMods[7];
			resistance += sMods[8];
			movement += sMods[9];
			leadership += sMods[10];
		}
		*/
	}

	public int getBaseMovement()
	{
		return movement;
	}

	public override int getMovement()
	{
		//If unit has a mount, Movement is the percentage of the mount's health
		if (bodyPartsCurrentHP[MOUNT] > 0)
		{
			float mountPercentMove = (float)((0.0 + bodyPartsCurrentHP[MOUNT]) / (0.0 + bodyPartsMaximumHP[MOUNT]));
			return Mathf.Max(0, Mathf.RoundToInt(mountPercentMove * unitClass.getMountMovement()));
		}
		//If no mount, Movement is the percentage of the unit's legs' health
		float percentMove =
				(float)((0.0 + bodyPartsCurrentHP[RIGHT_LEG] + bodyPartsCurrentHP[LEFT_LEG]) / (0.0 + bodyPartsMaximumHP[RIGHT_LEG] + bodyPartsMaximumHP[LEFT_LEG]));
		return Mathf.Max(0, Mathf.RoundToInt(percentMove * movement));
	}

	public override bool canCarryUnit()
	{
		//Can carry a unit if the unit has a mount and the mount is alive
		return unitClass != null && bodyPartsCurrentHP[MOUNT] > 0;
	}

	public override bool canBeCarried()
	{
		//Can be carried if there is no mount or their mount is dead (i.e. when canCarryUnit is false)
		return !canCarryUnit();
	}

	public override int attackSpeed()
	{
		Item i = InventoryIndex.getElement(inventory[0]);
		int encumberment = 0;
		if (bodyPartsCurrentHP[MOUNT] <= 0)
		{ //Mounted units aren't encumbered by armor
			Armor a = getArmor();
			if (a != null)
			{
				encumberment += a.getWeight() - armStrength();
			}
		}
		if (i is Weapon)
		{
			Weapon w = (Weapon)i;
			encumberment += w.getWeight() - armStrength();
		}
		encumberment = Mathf.Max(0, encumberment);
		return Mathf.Max(0, reflex - encumberment);
	}

	public override bool canFly()
	{
		//Can fly if they have a flying mount and the mount is alive
		return unitClass != null &&
				unitClass.mountCanFly() && bodyPartsCurrentHP[MOUNT] > 0;
	}

	public override int attackStrength()
	{
		Item i = getEquippedItem();
		if (i is Weapon)
		{
			Weapon w = (Weapon)i;
			if (w.isMagic())
			{
				return magic + w.getMight() + getSupportRNGBonus(CombatTrait.ATTACKPOWER);
			}
			return armStrength() + w.getMight() + getSupportRNGBonus(CombatTrait.ATTACKPOWER);
		}
		return armStrength() + getSupportRNGBonus(CombatTrait.ATTACKPOWER);
	}

	public int armStrength()
	{
		//Average arms HP / 3 (Just divide by 6 to account for the / 2 and the / 3. Don't change it, future me!)
		return Mathf.RoundToInt((float)((0.0 + bodyPartsCurrentHP[RIGHT_ARM] + bodyPartsCurrentHP[LEFT_ARM]) / 6));
	}

	public override int defense(bool isMagicAttack, int bodyPart)
	{
		if (isMagicAttack)
		{
			return resistance;
		}
		Armor a = getArmor();
		if (a == null)
		{
			return 0;
		}
		return a.getDefenseFor(bodyPart);
	}

	public override int getBaseAccuracy()
	{
		int currentArmsHP = bodyPartsCurrentHP[RIGHT_ARM] + bodyPartsCurrentHP[LEFT_ARM];
		int maxArmsHP = Mathf.Max(1, bodyPartsMaximumHP[RIGHT_ARM] + bodyPartsMaximumHP[LEFT_ARM]);
		double percentageArmsHP = (0.0 + currentArmsHP) / maxArmsHP;
		int currentEyesHP = bodyPartsCurrentHP[RIGHT_EYE] + bodyPartsCurrentHP[LEFT_EYE];
		int maxEyesHP = Mathf.Max(1, bodyPartsMaximumHP[RIGHT_EYE] + bodyPartsMaximumHP[LEFT_EYE]);
		double percentageEyesHP = (0.0 + currentEyesHP) / maxEyesHP;

		int effectiveSkill = Mathf.RoundToInt((float)(percentageArmsHP * percentageEyesHP * skill));

		int maxHeadHP = Mathf.Max(1, bodyPartsMaximumHP[HEAD]);
		double percentageHeadHP = (0.0 + bodyPartsMaximumHP[HEAD]) / maxHeadHP;
		int effectiveAwareness = Mathf.RoundToInt((float)(percentageHeadHP * awareness));

		int accuracy = (effectiveSkill * 2) + effectiveAwareness;

		accuracy += getSupportRNGBonus(CombatTrait.ACCURACY);
		if (group != null && this != group.getLeader())
		{ //Leader cannot give themselves a bonus
			accuracy += group.getLeadershipBonus(this);
		}
		return accuracy;
	}

	public override int accuracy()
	{
		int accuracy = getBaseAccuracy();

		Item i = getEquippedItem();
		if (i is Weapon)
		{
			Weapon w = (Weapon)i;
			accuracy += w.getHit();
			accuracy += Mathf.Min(10, proficiency[w.getProficiencyIndex()] - w.getProficiencyRequirement());
		}
		//TODO manage weapon triangle advantage in the external battle manager
		return accuracy;
	}

	public override int avoidance(int bodyPart)
	{
		int currentEyesHP = bodyPartsCurrentHP[RIGHT_EYE] + bodyPartsCurrentHP[LEFT_EYE];
		int maxEyesHP = Mathf.Max(1, bodyPartsMaximumHP[RIGHT_EYE] + bodyPartsMaximumHP[LEFT_EYE]);
		double percentageEyesHP = (0.0 + currentEyesHP) / maxEyesHP;
		int effectiveAwareness = Mathf.RoundToInt((float)(percentageEyesHP * awareness));

		int effectiveMountEvasion = 0;
		if (unitClass != null && unitClass.getMount() != null)
		{
			int maxMountHP = Mathf.Max(1, bodyPartsCurrentHP[MOUNT]);
			double percentageMountHP = (0.0 + bodyPartsCurrentHP[MOUNT]) / maxMountHP;
			effectiveMountEvasion = Mathf.RoundToInt((float)(percentageMountHP * unitClass.getMountEvasionBonus()));
		}

		int avoidance = (attackSpeed() * 2) + effectiveAwareness + effectiveMountEvasion;
		if (group != null && this != group.getLeader())
		{ //Leader cannot give themselves a bonus
			avoidance += group.getLeadershipBonus(this);
		}
		avoidance += getSupportRNGBonus(CombatTrait.AVOIDANCE);
		if (bodyPart == 0)
		{
			avoidance += 15;
		}
		else if (bodyPart == RIGHT_ARM || bodyPart == LEFT_ARM)
		{
			avoidance += 30;
		}
		else if (bodyPart == RIGHT_LEG || bodyPart == LEFT_LEG)
		{
			avoidance += 45;
		}
		else if (bodyPart == RIGHT_EYE || bodyPart == LEFT_EYE)
		{
			avoidance += 70;
		}
		return avoidance;
	}

	private int getSupportRNGBonus(CombatTrait ct)
	{
		if (supportPartner != null &&
				supportPartner.getGroup() != null
				&& getGroup().getBattle() != null
				&& getGroup().getBattle() == supportPartner.getGroup().getBattle()
				&& supportPartner.getValuedTrait() == ct)
		{
			return Mathf.Max(-20, Mathf.Min(20, relationshipWithSupportPartner / ct.getSupportDividend()));
		}
		return 0;
	}

	public override int getBaseCrit()
	{
		int crit = skill + getSupportRNGBonus(CombatTrait.CRITRATE);
		return crit;
	}

	public override int criticalHitRate()
	{
		int crit = getBaseCrit();
		Item i = getEquippedItem();
		if (i is Weapon)
		{
			Weapon w = (Weapon)i;
			crit += w.getCrit();
			crit += Mathf.Min(10, proficiency[w.getProficiencyIndex()] - w.getProficiencyRequirement());
		}
		return Mathf.Max(0, crit);
	}

	public override int criticalHitAvoid()
	{
		int avoid = awareness + getSupportRNGBonus(CombatTrait.CRITAVOID);
		return avoid;
	}

	public override bool isUsingMagic()
	{
		Item i = getEquippedItem();
		return i is Weapon && ((Weapon)i).isMagic();
	}

	public void useWeapon(bool hit)
	{
		HandheldWeapon w = getEquippedWeapon();
		if (w != null)
		{
			proficiency[w.getProficiencyIndex()]++;
			if (hit || w.usesDurabilityWithoutHitting())
			{
				inventory[0][2]--; //Equipped item loses durability
								   //TODO add breaking function somewhere else, so we can tell what weapon
								   //killed a person even if it breaks. Also, remember to remove this part
								   //in EquippedMonster
								   //				if (inventory[0][2] == 0) {
								   //					inventory[0] = null;
								   //					autoEquip();
								   //				}
			}
		}
	}

	public override int[] getRanges()
	{
		int wepIdx = 0;
		int wepRng = 1;
		int stfIdx = -1;
		int stfRng = -1;
		for (int q = 0; q < inventory.Length; q++)
		{
			Item i = InventoryIndex.getElement(inventory[q]);
			if (i is HandheldWeapon)
			{
				int check = ((HandheldWeapon)i).maxRange();
				if (inventory[wepIdx] == null || check > wepRng)
				{
					wepRng = check;
					wepIdx = q;
				}
			}
			else if (i is Staff)
			{
				int check = ((Staff)i).maxRange();
				if (check > stfRng)
				{
					stfRng = check;
					stfIdx = q;
				}
			}
		}
		int[] ret = { wepIdx, wepRng, stfIdx, stfRng };
		return ret;
	}

	public void incrementSupportRelationship()
	{
		if (supportPartner != null && getGroup() == supportPartner.getGroup())
		{
			alterSupportRelationship(1);
			supportPartner.alterSupportRelationship(1);
		}
	}

	public override bool gainExperience(int exp)
	{
		//Gain experience only if you're not already max level
		if (level != Unit.MAX_LEVEL)
		{
			exp -= effectiveLevel();
			exp = Mathf.Max(1, Mathf.Min(Unit.EXPERIENCE_TOWARDS_LEVEL, exp)); //Max 100, Min 1
			experience += exp;
			//If you got enough experience for a level,
			if (experience >= Unit.EXPERIENCE_TOWARDS_LEVEL)
			{
				//Level up
				//We don't need a level-up animation, considering there are so many units
				//So just adjust the stats silently here
				levelUp();
				//Adjust experience appropriately
				if (level == MAX_LEVEL)
				{
					experience = 0;
				}
				else
				{
					experience -= EXPERIENCE_TOWARDS_LEVEL;
				}
				return true;
			}
		}
		return false;
	}

	/**
	 * Used by training facilities for leveling up units
	 */
	public void finishLevel()
	{
		if (level < Unit.MAX_LEVEL)
		{
			experience = 0;
			levelUp();
		}
	}

	public void fullHeal()
	{
		for (int q = 0; q < bodyPartsCurrentHP.Length; q++)
		{
			bodyPartsCurrentHP[q] = bodyPartsMaximumHP[q];
		}
	}

	public int getAge()
	{
		return age;
	}

	public bool isMortal()
	{
		return humanIsMortal;
	}

	public void toggleMortality()
	{
		humanIsMortal = !(humanIsMortal);
	}

	/**
	 * Increment age if mortal and perform aging processes.
	 * @return true if the unit is still alive, false if they died
	 */
	public bool incrementAge()
	{
		if (humanIsMortal)
		{
			age++;
			if (RNGStuff.random0To99() < age - 40)
			{
				//Reduce growths of all body parts except eyes and mount
				bodyPartsMaximumHPGrowth[HEAD] = Mathf.Max(0, bodyPartsMaximumHPGrowth[HEAD] - 5);
				bodyPartsMaximumHPGrowth[TORSO] = Mathf.Max(0, bodyPartsMaximumHPGrowth[1] - 5);
				bodyPartsMaximumHPGrowth[RIGHT_ARM] = Mathf.Max(0, bodyPartsMaximumHPGrowth[RIGHT_ARM] - 5);
				bodyPartsMaximumHPGrowth[LEFT_ARM] = Mathf.Max(0, bodyPartsMaximumHPGrowth[LEFT_ARM] - 5);
				bodyPartsMaximumHPGrowth[RIGHT_LEG] = Mathf.Max(0, bodyPartsMaximumHPGrowth[RIGHT_LEG] - 5);
				bodyPartsMaximumHPGrowth[LEFT_LEG] = Mathf.Max(0, bodyPartsMaximumHPGrowth[LEFT_LEG] - 5);
				//Reduce growths of physical attributes
				skillGrowth = Mathf.Max(0, skillGrowth - 5);
				reflexGrowth = Mathf.Max(0, reflexGrowth - 5);
				awarenessGrowth = Mathf.Max(0, awarenessGrowth - 5);
			}
			if (RNGStuff.random0To99() < (age - 50) / 2)
			{
				//TODO die
				return false;
			}
		}
		return true;
	}

	public override void deathSequence()
	{
		affiliation.getArmy().Remove(this);
		if (affiliation.getRuler() == this)
		{
			affiliation.setRuler(null);
		}
		group.remove(this);
		Human sp = supportPartner;
		supportPartner = null;
		if (sp != null)
		{
			sp.supportPartner = null;
		}
		if (sp != null)
		{
			int moraleLoss = Mathf.Min(sp.getMorale(),
					sp.relationshipWithSupportPartner - (sp.getMilitarism() / 5));
			moraleLoss = Mathf.Max(moraleLoss, 0);
			sp.morale -= moraleLoss;
		}
		home.mournSoldierDeath();
	}

	/**
	 * Determines if the unit can use classes with unicorns
	 * @return
	 */
	public bool getGender()
	{
		return gender;
	}

	public bool canUseBallista()
	{
		return armStrength() > 10;
	}

	public bool canUseMagicTurrets()
	{
		return magic > 0;
	}

	public bool canUse(StationaryWeapon weapon)
	{
		if (weapon.isMagic())
		{
			return canUseMagicTurrets();
		}
		return canUseBallista();
	}

	public Item getEquippedItem()
	{
		return InventoryIndex.getElement(inventory[0]);
	}

	public String getWeaponName()
	{
		HandheldWeapon w = getEquippedWeapon();
		if (w == null)
		{
			return "None";
		}
		return w.getName();
	}

	public String getArmorName()
	{
		Armor a = getArmor();
		if (a == null)
		{
			return "None";
		}
		return a.getName();
	}

	public int[][] getInventory()
	{
		return inventory;
	}

	public Armor getArmor()
	{
		Item i = InventoryIndex.getElement(armor);
		if (i is Armor)
		{
			return (Armor)i;
		}
		return null;
	}

	public void destroyArmor()
	{
		armor = null;
	}

	public HandheldWeapon getEquippedWeapon()
	{
		Item i = InventoryIndex.getElement(inventory[0]);
		if (i is HandheldWeapon)
		{
			return (HandheldWeapon)i;
		}
		return null;
	}

	public void autoEquip()
	{
		int idx = 0;
		int h = getEquipmentHeuristic(inventory[idx]);
		if (getEquipmentHeuristic(inventory[1]) > h)
		{
			idx = 1;
		}
		if (getEquipmentHeuristic(inventory[2]) > h)
		{
			idx = 2;
		}
		equip(idx);
	}

	public int getEquipmentHeuristic(int[] item)
	{
		Item i = InventoryIndex.getElement(item);
		if (i is HandheldWeapon)
		{
			int h = 0;
			HandheldWeapon w = (HandheldWeapon)i;
			int profBonus = proficiency[w.getProficiencyIndex()] - w.getProficiencyRequirement();
			h += Mathf.Min(100, (profBonus * 10));
			h += w.getHit();
			h += w.getMight();
			if (w.isMagic())
			{
				h += getMagic();
			}
			else
			{
				h += armStrength();
			}
			return h;
		}
		else if (i is Armor)
		{
			//TODO
		}
		else if (i is Staff)
		{
			//TODO
		}
		return int.MinValue;
	}

	public void equip(int idx)
	{
		int[] temp = inventory[0];
		inventory[0] = inventory[idx];
		inventory[idx] = temp;
	}

	public bool receiveNewItem(int[] item)
	{
		for (int q = 0; q < inventory.Length; q++)
		{
			if (inventory[q] == null)
			{
				inventory[q] = InventoryIndex.newInstanceOfItem(item);
				return true;
			}
		}
		return false;
	}

	public bool receiveNewArmor(int[] arm)
	{
		if (armor == null)
		{
			armor = InventoryIndex.newInstanceOfItem(arm);
			return true;
		}
		return false;
	}

	public int proficiencyWith(int type)
	{
		return proficiency[type];
	}

	public new string getDisplayName()
	{
		return name + " of " + home.getName();
	}
	public int getNationalism()
	{
		return nationalism;
	}
	public int getMilitarism()
	{
		return militarism;
	}
	public int getAltruism()
	{
		return altruism;
	}
	public int getFamilism()
	{
		return familism;
	}
	public int getConfidence()
	{
		return confidence;
	}
	public int getTolerance()
	{
		return tolerance;
	}
	public string getInterest1()
	{
		return interests[0].getDisplayName();
	}
	public string getInterest2()
	{
		return interests[1].getDisplayName();
	}
	public string getInterest3()
	{
		return interests[2].getDisplayName();
	}
	public string getDisinterest1()
	{
		return disinterests[0].getDisplayName();
	}
	public string getDisinterest2()
	{
		return disinterests[1].getDisplayName();
	}
	public string getDisinterest3()
	{
		return disinterests[2].getDisplayName();
	}
	public Demeanor getDemeanor()
	{
		return demeanor;
	}
	public CombatTrait getValuedTrait()
	{
		return valuedTrait;
	}
	public string getDemeanorAsString()
	{
		return demeanor.getDisplayName();
	}
	public string getValuedTraitAsString()
	{
		return valuedTrait.getDisplayName();
	}
	public int[] getAppearance()
	{
		return appearance;
	}
	public Color getHairColor()
	{
		return RNGStuff.HAIR_COLORS_IN_USE.colorAtIndex(appearance[10]);
	}
	public Color getSkinColor()
	{
		return new Color((0.0f + appearance[11]) / 255,
				(0.0f + appearance[12]) / 255,
				(0.0f + appearance[13]) / 255,
				1);
	}
	public Color getEyeColor()
	{
		return RNGStuff.EYE_COLORS_IN_USE.colorAtIndex(appearance[14]);
	}
	public Human getSupportPartner()
	{
		return supportPartner;
	}
	public string getSupportPartnerName()
	{
		if (supportPartner == null)
		{
			return "None";
		}
		return supportPartner.getName();
	}
	public string getGenderAsString()
	{
		if (gender)
		{
			return "Female";
		}
		return "Male";
	}

	public override int getMorale()
	{
		return morale;
	}

	public CityState getHome()
	{
		return home;
	}

	public override void defect(Nation n)
	{
		group.remove(this);
		group = null;
		affiliation = n;
		//You are as loyal to your new nation as you are to your family, which
		//encouraged you to rebel
		militarism = familism;
		if (home.getNation() != n)
		{
			home = n.getCapital();
		}
		//No need to add or remove yourself to/from an army. The Nation constructor does this
	}

	/**
	 * Meant for owners of buildings only, not for playable units
	 * @param n
	 */
	public void setAffiliation(Nation n)
	{
		this.affiliation = n;
	}

	public void declareLoyalty()
	{
		this.home = affiliation.getCapital();
	}

	public override Nation getAffiliation()
	{
		return affiliation;
	}

	public bool retire()
	{
		if (!humanIsMortal)
		{
			return false;
			//Immortal units will not be allowed to retire
		}
		if (home.getNation() != affiliation)
		{
			home = affiliation.getCapital();
		}
		int res = RNGStuff.nextInt(home.getResidentialAreas().Count);
		int initRes = res;
		while (!(home.getResidentialAreas()[res].addVeteran(this)))
		{
			res++;
			if (res == home.getResidentialAreas().Count)
			{
				res = 0;
			}
			if (res == initRes)
			{
				//All residential areas are full, so the unit cannot be retired
				return false;
			}
		}
		if (group != null)
		{
			group.remove(this);
			group = null;
		}
		affiliation.getArmy().Remove(this);
		return true;
	}

	public static Human completelyRandomHuman(CityState home)
	{
		int[] personalValues = new int[6];
		Interest[] interests = new Interest[3];
		Interest[] disinterests = new Interest[3];
		generatePersonality(personalValues, interests, disinterests, home.getValues());
		Demeanor[] types = Demeanor.values();
		Demeanor demeanor = types[RNGStuff.nextInt(types.Length)];
		if (demeanor.getRarity() > 0)
		{
			demeanor = types[RNGStuff.nextInt(types.Length)];
		}
		CombatTrait[] traits = CombatTrait.values();
		CombatTrait valued = traits[RNGStuff.nextInt(traits.Length)];
		int[] appearance = generateAppearance();
		int[] maxHPs = generateMaxHPs();
		bool gend = RNGStuff.nextBoolean();
		//Stats ranges based on Thracia 776
		int mag = RNGStuff.nextInt(12);
		int skl = RNGStuff.nextInt(13);
		int rfx = RNGStuff.nextInt(12);
		int awr = RNGStuff.nextInt(13);
		int res = RNGStuff.nextInt(9);
		int mov = RNGStuff.nextInt(3) + 5;
		int ldr = RNGStuff.nextInt(6);
		if (ldr > 3 && RNGStuff.nextBoolean())
		{
			ldr = RNGStuff.nextInt(2); //Can't have too many good leaders
		}
		int[] maxHPsGrowths = generateMaxHPGrowths();
		int magGrowth = RNGStuff.nextInt(31); //Growth rate before class modifiers is between 0 and 60
		int sklGrowth = RNGStuff.nextInt(51);
		int rfxGrowth = RNGStuff.nextInt(51);
		int awrGrowth = RNGStuff.nextInt(51);
		int resGrowth = RNGStuff.nextInt(11);
		string genName = RNGStuff.randomName(home.getLanguage());
		int age = RNGStuff.nextInt(13) + 18;
		return new Human(genName, gend, maxHPs, mag, skl, rfx, awr, res, mov, ldr,
				maxHPsGrowths, magGrowth, sklGrowth, rfxGrowth, awrGrowth, resGrowth,
				age, personalValues, appearance, interests, disinterests, demeanor,
				valued, home);
	}

	protected static void generatePersonality(int[] hVals, Interest[] interests,
			Interest[] disinterests)
	{
		hVals[0] = RNGStuff.random0To100(); //Nationalism
		hVals[1] = RNGStuff.random0To100(); //Militarism
		hVals[2] = RNGStuff.random0To100(); //Altruism
		hVals[3] = RNGStuff.random0To100(); //Familism
		hVals[4] = RNGStuff.random0To100(); //Confidence
		hVals[5] = RNGStuff.random0To100(); //Tolerance
		Interest[] choices = Interest.values();
		int i1 = RNGStuff.nextInt(choices.Length);
		int i2 = RNGStuff.nextInt(choices.Length);
		if (i1 == i2)
		{
			i2++;
			if (i2 == choices.Length)
			{
				i2 = 0;
			}
		}
		int i3 = RNGStuff.nextInt(choices.Length);
		while (i2 == i3 || i1 == i3)
		{
			i3++;
			if (i3 == choices.Length)
			{
				i3 = 0;
			}
		}
		int d1 = RNGStuff.nextInt(choices.Length);
		int d2 = RNGStuff.nextInt(choices.Length);
		int d3 = RNGStuff.nextInt(choices.Length);
		while (d1 == i1 || d1 == i2 || d1 == i3)
		{
			d1++;
			if (d1 == choices.Length)
			{
				d1 = 0;
			}
		}
		while (d2 == i1 || d2 == i2 || d2 == i3 || d2 == d1)
		{
			d2++;
			if (d2 == choices.Length)
			{
				d2 = 0;
			}
		}
		while (d3 == i1 || d3 == i2 || d3 == i3 || d3 == d1 || d3 == d2)
		{
			d3++;
			if (d3 == choices.Length)
			{
				d3 = 0;
			}
		}
		interests[0] = choices[i1];
		interests[1] = choices[i2];
		interests[2] = choices[i3];
		disinterests[0] = choices[d1];
		disinterests[1] = choices[d2];
		disinterests[2] = choices[d3];
	}

	/**
	 * Set the values of the interests, disinterests, and personal values arrays (the latter
	 * influenced by the city-state values) and return the demeanor
	 * @param hVals
	 * @param interests
	 * @param disinterests
	 * @param csValues
	 * @return
	 */
	protected static void generatePersonality(int[] hVals, Interest[] interests,
			Interest[] disinterests, int[] csValues)
	{
		//Generate personality as normal
		generatePersonality(hVals, interests, disinterests);
		//Average personal values with city-state's values
		hVals[0] = (hVals[0] + csValues[0]) / 2;
		hVals[1] = (hVals[1] + csValues[1]) / 2;
		hVals[2] = (hVals[2] + csValues[2]) / 2;
		hVals[3] = (hVals[3] + csValues[3]) / 2;
		hVals[4] = (hVals[4] + csValues[4]) / 2;
		hVals[5] = (hVals[5] + csValues[5]) / 2;
	}

	/**
	 * For humans, body part indexes are
	 * 0 for head
	 * 1 for torso
	 * 2 for arm1
	 * 3 for arm2
	 * 4 for leg1
	 * 5 for leg2
	 * 6 for eye1
	 * 7 for eye2
	 * 8 for mount
	 * @return
	 */
	protected static int[] generateMaxHPs()
	{
		int[] mhps = new int[9];
		mhps[HEAD] = RNGStuff.nextInt(11) + 10; //Head HP is 10-20
		mhps[TORSO] = RNGStuff.nextInt(17) + 14; //Torso HP is 14-30
		mhps[RIGHT_ARM] = RNGStuff.nextInt(11) + 10; //Arms HPs are 10-20
		mhps[LEFT_ARM] = mhps[RIGHT_ARM];
		mhps[RIGHT_LEG] = RNGStuff.nextInt(16) + 10; //Legs HPs are 10-25
		mhps[LEFT_LEG] = mhps[RIGHT_LEG];
		mhps[RIGHT_EYE] = RNGStuff.nextInt(6) + 5; //Eyes HPs are 5-10
		mhps[LEFT_EYE] = mhps[RIGHT_EYE];
		mhps[MOUNT] = 0; //Mount's HP is always 0, since no one starts with a mount
		return mhps;
	}

	/**
	 * For humans, body part indexes are
	 * 0 for head
	 * 1 for torso
	 * 2 for arm1
	 * 3 for arm2
	 * 4 for leg1
	 * 5 for leg2
	 * 6 for eye1
	 * 7 for eye2
	 * 8 for mount
	 * @return
	 */
	protected static int[] generateMaxHPGrowths()
	{
		int[] growths = new int[9];
		growths[HEAD] = RNGStuff.nextInt(51) + 20; //Head
		growths[TORSO] = RNGStuff.nextInt(61) + 30; //Torso
		growths[RIGHT_ARM] = RNGStuff.nextInt(71) + 10; //Arm1
		growths[LEFT_ARM] = growths[RIGHT_ARM];           //Arm2 = Arm1
		growths[RIGHT_LEG] = RNGStuff.nextInt(71) + 20; //Leg1
		growths[LEFT_LEG] = growths[RIGHT_LEG];           //Leg2 = Leg1
		growths[RIGHT_EYE] = 0; //Eyes always have a growth rate of 0
		growths[LEFT_EYE] = 0;
		growths[MOUNT] = 0; //Mount's growth rate will be determined if/when the unit gets a mount
		return growths;
	}

	protected static int[] generateAppearance()
	{
		int[] app = new int[15];
		//TODO make sure numbers are all right
		app[0] = RNGStuff.nextInt(15); //Face shape
		app[1] = RNGStuff.nextInt(2); //Lips
		app[2] = RNGStuff.nextInt(15); //Nose shape
		app[3] = RNGStuff.nextInt(7); //Ear shape
		app[4] = RNGStuff.nextInt(15); //Eye shape
		app[5] = RNGStuff.nextInt(7); //Iris appearance (Unused)
		app[6] = RNGStuff.nextInt(3); //Eyebrows
		app[7] = RNGStuff.nextInt(15); //Hairstyle
		if (RNGStuff.nextBoolean())
		{
			app[8] = RNGStuff.nextInt(12); //Mustache style
			app[9] = RNGStuff.nextInt(12); //Beard style
		}
		else
		{
			app[8] = 0; //No mustache
			app[9] = 0; //No beard
		}
		app[10] = RNGStuff.getRandomHairColor(); //Hair color
		Color skinColor = RNGStuff.SKIN_COLORS_IN_USE.colorAtIndex(RNGStuff.getRandomSkinColor()); //Skin color
		app[11] = (int)Mathf.Round(skinColor.r * 255);
		app[12] = (int)Mathf.Round(skinColor.g * 255);
		app[13] = (int)Mathf.Round(skinColor.b * 255);
		app[14] = RNGStuff.getRandomEyeColor(); //Eye color
		return app;
	}

	/**
	 * For testing purposes only
	 * @return
	 */
	public string showStats()
	{
		string sb = $"Name: {getDisplayName()}, Gender: {getGenderAsString()}, Age: {getAge()}\n";
		sb += $"Militarism: {getMilitarism()}, Altruism: {getAltruism()}, Familism: {getFamilism()}, Nationalism: {getNationalism()}\n";
		sb += $"Confidence: {getConfidence()}, Tolerance: {getTolerance()}, Interests: {getInterest1()}, {getInterest2()}, {getInterest3()}\n";
		sb += $"Demeanor: {getDemeanorAsString()}, Valued Trait: {getValuedTraitAsString()}, Disinterests: {getDisinterest1()}, {getDisinterest2()}, {getDisinterest3()}\n";
		int[] c = bodyPartsCurrentHP;
		int[] m = bodyPartsMaximumHP;
		int[] g = bodyPartsMaximumHPGrowth;
		sb += $"Head: {c[HEAD]}/{m[HEAD]} ({g[HEAD]}%), Torso: {c[TORSO]}/{m[TORSO]} ({g[TORSO]}%), Arm1: {c[RIGHT_ARM]}/{m[RIGHT_ARM]} ({g[RIGHT_ARM]}%), Arm2: {c[LEFT_ARM]}/{m[LEFT_ARM]} ({g[LEFT_ARM]}%)\n"
			+ $"Leg1: {c[RIGHT_LEG]}/{m[RIGHT_LEG]}] ({g[RIGHT_LEG]}%), Leg2: {c[LEFT_LEG]}/{m[LEFT_LEG]} ({g[LEFT_LEG]}%), Eye1: {c[RIGHT_EYE]}/{m[RIGHT_EYE]} ({g[RIGHT_EYE]}%), Eye2: {c[LEFT_EYE]}/{m[LEFT_EYE]} ({g[LEFT_EYE]}%), Mount: {c[MOUNT]}/{m[MOUNT]} ({g[MOUNT]}%)\n";
		sb += $"Magic: {magic} ({magicGrowth}%), Skill: {skill} ({skillGrowth}%), Reflex: {reflex} ({reflexGrowth}%), Awareness: {awareness} ({awarenessGrowth}%)\n"
				+ $"Resistance: {resistance} ({resistanceGrowth}%), Movement: {movement}, Leadership: {leadership}\n";
		sb += $"Class: {getUnitClassName()}, Level: {getLevel()}, EXP: {getExperience()}, Weapon: {getWeaponName()}, Armor: {getArmorName()}\n";
		sb += $"Support Partner: {getSupportPartnerName()} {relationshipWithSupportPartner}\n";
		sb += $"ATK: {attackStrength()}, ACC: {accuracy()}, AVO(Torso): {avoidance(TORSO)}, CRT: {criticalHitRate()}, CRTAVO(Torso): {criticalHitAvoid()}\n";

		return sb;
	}

	public override string[] getBodyPartsNames()
	{
		return BODY_PARTS_STRINGS;
	}

}
