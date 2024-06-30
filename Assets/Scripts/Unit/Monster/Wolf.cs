using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Wolf : Monster
{

	public static int LEG1 = 2;
	public static int LEG2 = 3;
	public static int LEG3 = 4;
	public static int LEG4 = 5;
	public static int RIGHT_EYE = 6;
	public static int LEFT_EYE = 7;
	public static int MOUTH = 8;

	public static string[] BODY_PARTS_STRINGS = {
	"Head", "Torso", "Leg 1",
			"Leg 2", "Leg 3", "Leg 4", "Right Eye", "Left Eye", "Mouth"};

	protected Wolf(int level, UnitClass unitClass, string name, int[] maxHPs, int magic, int skill, int reflex,
			int awareness, int resistance, int movement, int leadership, int[] maxHPGrowths, int magGrowth,
			int sklGrowth, int rfxGrowth, int awrGrowth, int resGrowth, Human master)
			: base(level, unitClass, name, maxHPs, magic, skill, reflex, awareness, resistance, movement, leadership, maxHPGrowths,
				magGrowth, sklGrowth, rfxGrowth, awrGrowth, resGrowth, master)
	{
		// TODO Auto-generated constructor stub
	}

	private float percentageEyesHP()
	{
		int totalEyesCurrentHP = bodyPartsCurrentHP[RIGHT_EYE] + bodyPartsCurrentHP[LEFT_EYE];
		int totalEyesMaximumHP = bodyPartsMaximumHP[RIGHT_EYE] + bodyPartsMaximumHP[LEFT_EYE];
		return (float)((0.0 + totalEyesCurrentHP) / (0.0 + totalEyesMaximumHP));
	}

	private float percentageLegsHP()
	{
		int totalCurrentLegsHP = bodyPartsCurrentHP[LEG1] + bodyPartsCurrentHP[LEG2]
				+ bodyPartsCurrentHP[LEG3] + bodyPartsCurrentHP[LEG4];
		int totalMaximumLegsHP = bodyPartsMaximumHP[LEG1] + bodyPartsMaximumHP[LEG2]
				+ bodyPartsMaximumHP[LEG3] + bodyPartsMaximumHP[LEG4];
		return (float)((0.0 + totalCurrentLegsHP) / (0.0 + totalMaximumLegsHP));
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
		//Can always carry units (think Loth Wolves)
		return true;
	}


	public override bool canBeCarried()
	{
		//Can never be carried by another unit (again, think Loth Wolves)
		return false;
	}


	public override bool canFly()
	{
		//Wolves can never fly. I refuse
		return false;
	}


	public override int attackStrength()
	{
		return bodyPartsCurrentHP[MOUTH] / 2;
	}


	public override int defense(bool isMagicAttack, int bodyPart)
	{
		if (isMagicAttack)
		{
			return resistance;
		}
		return 0;
	}


	public override int accuracy()
	{
		int effectiveSkill = Mathf.RoundToInt((percentageEyesHP() * skill));

		int maxHeadHP = Mathf.Max(1, bodyPartsMaximumHP[HEAD]);
		double percentageHeadHP = (0.0 + bodyPartsMaximumHP[HEAD]) / maxHeadHP;
		int effectiveAwareness = Mathf.RoundToInt((float)(percentageHeadHP * awareness));

		int accuracy = (effectiveSkill * 2) + effectiveAwareness;

		if (group != null && this != group.getLeader())
		{ //Leader cannot give themselves a bonus
			accuracy += group.getLeadershipBonus(this);
		}
		return accuracy;
	}


	public override int avoidance(int bodyPart)
	{
		int effectiveAwareness = Mathf.RoundToInt((percentageEyesHP() * awareness));

		int avoidance = (attackSpeed() * 2) + effectiveAwareness;
		if (group != null && this != group.getLeader())
		{ //Leader cannot give themselves a bonus
			avoidance += group.getLeadershipBonus(this);
		}
		if (bodyPart == 0)
		{
			avoidance += 15;
		}
		else if (bodyPart == LEG1 || bodyPart == LEG2
			  || bodyPart == LEG3 || bodyPart == LEG4)
		{
			avoidance += 35;
		}
		else if (bodyPart == RIGHT_EYE || bodyPart == LEFT_EYE)
		{
			avoidance += 70;
		}
		return avoidance;
	}


	public override int criticalHitAvoid()
	{
		return Mathf.RoundToInt(awareness * percentageEyesHP());
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
