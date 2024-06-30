using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Gorgon : EquippedMonster
{


	public static int RIGHT_EYE = 4;
	public static int LEFT_EYE = 5;
	public static int TAIL = 6;

	public static string[] BODY_PARTS_STRINGS = {
	"Head", "Torso", "Right Arm",
			"Left Arm", "Right Eye", "Left Eye", "Tail"};

	protected Gorgon(int level, UnitClass unitClass, string name, int[] maxHPs, int magic, int skill, int reflex,
			int awareness, int resistance, int movement, int leadership, int[] maxHPGrowths, int magGrowth,
			int sklGrowth, int rfxGrowth, int awrGrowth, int resGrowth, Human master)
			: base(level, unitClass, name, maxHPs, magic, skill, reflex, awareness, resistance, movement, leadership, maxHPGrowths,
				magGrowth, sklGrowth, rfxGrowth, awrGrowth, resGrowth, master)
	{
		// TODO Auto-generated constructor stub
	}


	public override int getMovement()
	{
		//If no mount, Movement is the percentage of the unit's tail's health
		float percentMove =
				(float)((0.0 + bodyPartsCurrentHP[TAIL]) / (0.0 + bodyPartsMaximumHP[TAIL]));
		return Mathf.Max(0, Mathf.RoundToInt(percentMove * movement));
	}


	public override bool canCarryUnit()
	{
		//Can never carry unit
		return false;
	}


	public override bool canBeCarried()
	{
		//Can always be carried
		return true;
	}


	public override int attackSpeed()
	{
		int encumberment = 0;
		Armor a = getArmor();
		if (a != null)
		{
			encumberment += a.getWeight() - armStrength();
		}
		Item i = InventoryIndex.getElement(inventory[0]);
		if (i is Weapon)
		{
			encumberment += ((Weapon)i).getWeight();
		}
		encumberment = Mathf.Max(0, encumberment);
		return Mathf.Max(0, reflex - encumberment);
	}


	public override bool canFly()
	{
		//Can never fly
		return false;
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

		if (group != null && this != group.getLeader())
		{ //Leader cannot give themselves a bonus
			accuracy += group.getLeadershipBonus(this);
		}
		return accuracy;
	}


	public override int avoidance(int bodyPart)
	{
		int currentEyesHP = bodyPartsCurrentHP[RIGHT_EYE] + bodyPartsCurrentHP[LEFT_EYE];
		int maxEyesHP = Mathf.Max(1, bodyPartsMaximumHP[RIGHT_EYE] + bodyPartsMaximumHP[LEFT_EYE]);
		double percentageEyesHP = (0.0 + currentEyesHP) / maxEyesHP;
		int effectiveAwareness = Mathf.RoundToInt((float)(percentageEyesHP * awareness));

		int avoidance = (attackSpeed() * 2) + effectiveAwareness;
		if (group != null && this != group.getLeader())
		{ //Leader cannot give themselves a bonus
			avoidance += group.getLeadershipBonus(this);
		}
		if (bodyPart == 0)
		{
			avoidance += 15;
		}
		else if (bodyPart == RIGHT_ARM || bodyPart == LEFT_ARM)
		{
			avoidance += 30;
		}
		else if (bodyPart == RIGHT_EYE || bodyPart == LEFT_EYE)
		{
			avoidance += 70;
		}
		else if (bodyPart == TAIL)
		{
			avoidance += 10;
		}
		return avoidance;
	}


	public override string[] getBodyPartsNames()
	{
		return BODY_PARTS_STRINGS;
	}

}
