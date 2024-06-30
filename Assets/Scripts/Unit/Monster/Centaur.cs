using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Centaur : EquippedMonster
{

	public static int HORSE_BODY = 4;
	public static int RIGHT_EYE = 5;
	public static int LEFT_EYE = 6;

	public static string[] BODY_PARTS_STRINGS = {
	"Head", "Torso", "Right Arm",
			"Left Arm", "Horse Body", "Right Eye", "Left Eye"};

	protected Centaur(int level, UnitClass unitClass, string name, int[] maxHPs, int magic, int skill, int reflex,
			int awareness, int resistance, int movement, int leadership, int[] maxHPGrowths, int magGrowth,
			int sklGrowth, int rfxGrowth, int awrGrowth, int resGrowth, Human master)
			: base(level, unitClass, name, maxHPs, magic, skill, reflex, awareness, resistance, movement, leadership, maxHPGrowths,
				magGrowth, sklGrowth, rfxGrowth, awrGrowth, resGrowth, master)
	{
		// TODO Auto-generated constructor stub
	}


	public override int getMovement()
	{
		//Movement is affected by the percentage health of the unit's horse body
		float percentMove = (float)((0.0 + bodyPartsCurrentHP[HORSE_BODY]) / (0.0 + bodyPartsMaximumHP[HORSE_BODY]));
		return Mathf.Max(0, Mathf.RoundToInt(percentMove * movement));
	}


	public override bool canCarryUnit()
	{
		//Can always carry another unit
		return true;
	}


	public override bool canBeCarried()
	{
		//Can never be carried by another unit
		return false;
	}


	public override int attackSpeed()
	{
		Item i = InventoryIndex.getElement(inventory[0]);
		int encumberment = 0;
		//Centaurs aren't encumbered by armor
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

		int maxHorseHP = Mathf.Max(1, bodyPartsCurrentHP[HORSE_BODY]);
		double percentageHorseHP = (0.0 + bodyPartsCurrentHP[HORSE_BODY]) / maxHorseHP;
		int effectiveHorseEvasion = Mathf.RoundToInt((float)(percentageHorseHP * Mount.HORSE.getEvasion()));

		int avoidance = (attackSpeed() * 2) + effectiveAwareness + effectiveHorseEvasion;
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
		return avoidance;
	}


	public override string[] getBodyPartsNames()
	{
		return BODY_PARTS_STRINGS;
	}

}
