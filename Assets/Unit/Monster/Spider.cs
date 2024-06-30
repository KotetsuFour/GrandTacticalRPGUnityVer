using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Spider : Monster
{

	public static int LEG1 = 2;
	public static int LEG2 = 3;
	public static int LEG3 = 4;
	public static int LEG4 = 5;
	public static int LEG5 = 6;
	public static int LEG6 = 7;
	public static int LEG7 = 8;
	public static int LEG8 = 9;
	public static int EYE1 = 10;
	public static int EYE2 = 11;
	public static int EYE3 = 12;
	public static int EYE4 = 13;
	public static int MOUTH = 14;

	public static string[] BODY_PARTS_STRINGS = {
	"Head", "Torso", "Leg 1",
			"Leg 2", "Leg 3", "Leg 4", "Leg 5", "Leg 6", "Leg 7", "Leg 8",
			"Eye 1", "Eye 2", "Eye 3", "Eye 4", "Mouth"};

	protected Spider(int level, UnitClass unitClass, string name, int[] maxHPs, int magic, int skill, int reflex,
			int awareness, int resistance, int movement, int leadership, int[] maxHPGrowths, int magGrowth,
			int sklGrowth, int rfxGrowth, int awrGrowth, int resGrowth, Human master)
			: base(level, unitClass, name, maxHPs, magic, skill, reflex, awareness, resistance, movement, leadership, maxHPGrowths,
				magGrowth, sklGrowth, rfxGrowth, awrGrowth, resGrowth, master)
	{
		// TODO Auto-generated constructor stub
	}

	private float percentageLegsHP()
	{
		int totalCurrentLegsHP = bodyPartsCurrentHP[LEG1] + bodyPartsCurrentHP[LEG2]
				+ bodyPartsCurrentHP[LEG3] + bodyPartsCurrentHP[LEG4] + bodyPartsCurrentHP[LEG5]
				+ bodyPartsCurrentHP[LEG6] + bodyPartsCurrentHP[LEG7] + bodyPartsCurrentHP[LEG8];
		int totalMaximumLegsHP = bodyPartsMaximumHP[LEG1] + bodyPartsMaximumHP[LEG2]
				+ bodyPartsMaximumHP[LEG3] + bodyPartsMaximumHP[LEG4] + bodyPartsMaximumHP[LEG5]
				+ bodyPartsMaximumHP[LEG6] + bodyPartsMaximumHP[LEG7] + bodyPartsMaximumHP[LEG8];
		return (float)((0.0 + totalCurrentLegsHP) / (0.0 + totalMaximumLegsHP));
	}

	private float percentageEyesHP()
	{
		int totalCurrentEyesHP = bodyPartsCurrentHP[EYE1] + bodyPartsCurrentHP[EYE2]
				+ bodyPartsCurrentHP[EYE3] + bodyPartsCurrentHP[EYE4];
		int totalMaximumEyesHP = bodyPartsMaximumHP[EYE1] + bodyPartsMaximumHP[EYE2]
				+ bodyPartsMaximumHP[EYE3] + bodyPartsMaximumHP[EYE4];
		return (float)((0.0 + totalCurrentEyesHP) / (0.0 + totalMaximumEyesHP));
	}


	public override bool isUsingMagic()
	{
		return false;
	}


	public override int getMovement()
	{
		return Mathf.Max(0, Mathf.RoundToInt(percentageLegsHP() * movement));
	}


	public override bool canCarryUnit()
	{
		//Can always carry units
		return true;
	}


	public override bool canBeCarried()
	{
		//Can never be carried
		return false;
	}


	public override bool canFly()
	{
		//Spiders can never fly. That would be an abomination
		return false;
	}


	public override int attackStrength()
	{
		return bodyPartsCurrentHP[MOUTH] / 3;
	}


	public override int defense(bool isMagicAttack, int bodyPart)
	{
		if (isMagicAttack)
		{
			return resistance;
		}
		return effectiveLevel();
	}


	public override int accuracy()
	{
		//Skill takes into account eyes, but not legs
		int effectiveSkill = Mathf.RoundToInt(percentageEyesHP() * skill);
		//Awareness takes into account legs, because spiders use the hair-like things on
		//their legs to sense
		int effectiveAwareness = Mathf.RoundToInt(percentageLegsHP() * awareness);
		return (effectiveSkill * 2) + effectiveAwareness;
	}


	public override int avoidance(int bodyPart)
	{
		//Effective awareness takes legs instead of eyes into account, because of spiders'
		//hair-like sensor things
		int effectiveAwareness = Mathf.RoundToInt((percentageLegsHP() * awareness));

		int avoidance = (attackSpeed() * 2) + effectiveAwareness;
		if (group != null && this != group.getLeader())
		{ //Leader cannot give themselves a bonus
			avoidance += group.getLeadershipBonus(this);
		}
		return avoidance;
	}


	public override int criticalHitAvoid()
	{
		return Mathf.RoundToInt(awareness * percentageLegsHP());
	}


	public override int[] getRanges()
	{
		int[] ret = { 0, 1, -1, -1 };
		return ret;
	}


	public override string[] getBodyPartsNames()
	{
		return BODY_PARTS_STRINGS;
	}

}
