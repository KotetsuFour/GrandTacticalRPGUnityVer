using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitClass
{
	private string name;
	private Mount mount;
	private int unitType;
	private int[] growthModifiers;
	private int[] proficiencyModifiers;
	private ClassAbility ability;
	private int strengthHeuristicMultiplierVal;
	private int magicHeuristicMultiplierVal;
	private int generalInternalHeuristicVal;


	public UnitClass(string name, Mount mount, int unitType, int[] growthModifiers,
			int[] proficiencyModifiers, int[] statModifiers, UnitClass previous,
			ClassAbility ability)
	{
		this.name = name;
		this.mount = mount;
		this.unitType = unitType;
		this.ability = ability;

		//Calculate heuristics
		//Intended to favor classes with growths corresponding to their proficiencies
		int strNeed = 0;
		int magNeed = 0;
		//For each physical weapon type
		for (int q = 0; q < 6; q++)
		{
			strNeed += proficiencyModifiers[q];
		}
		//For each magic weapon type
		for (int q = 6; q < proficiencyModifiers.Length - 1; q++)
		{
			magNeed += proficiencyModifiers[q];
		}
		magNeed += proficiencyModifiers[9] / 2; //Half for staff
		if (strNeed > magNeed)
		{
			strengthHeuristicMultiplierVal = 2;
			magicHeuristicMultiplierVal = 0;
		}
		else if (magNeed > strNeed)
		{
			strengthHeuristicMultiplierVal = 0;
			magicHeuristicMultiplierVal = 2;
		}
		else
		{ //They're equal
			strengthHeuristicMultiplierVal = 1;
			magicHeuristicMultiplierVal = 1;
		}

		//Heuristic used for balancing class selection
		generalInternalHeuristicVal = 0;
		//Make sure proficiencies are concentrated
		for (int q = 0; q < proficiencyModifiers.Length; q++)
		{
			generalInternalHeuristicVal -= Mathf.Min(10, proficiencyModifiers[q]);
		}
		//Make sure growths are well balanced
		for (int q = 0; q < growthModifiers.Length; q++)
		{
			generalInternalHeuristicVal += Mathf.Min(10, growthModifiers[q]);
		}
		if (ability != ClassAbility.NONE)
		{
			generalInternalHeuristicVal -= 10;
		}
	}

	public string getName()
	{
		return name;
	}

	public int getMountMovement()
	{
		return mount.getMovement();
	}

	public bool mountCanFly()
	{
		if (mount == null)
		{
			return false;
		}
		return mount.canMountFly();
	}

	public Mount getMount()
	{
		return mount;
	}

	public int getMountType()
	{
		if (mount == null)
		{
			return -1;
		}
		return mount.getId();
	}

	public int initializeMountGrowth()
	{
		if (mount == null)
		{
			return 0;
		}
		return mount.getMinGrowth() + RNGStuff.nextInt(mount.getGrowthVariance());
	}

	public int initializeMountHealth()
	{
		if (mount == null)
		{
			return 0;
		}
		return mount.getMinInitialHealth() + RNGStuff.nextInt(mount.getHealthVariance());
	}

	/*
	public int getTier()
	{
		return tier;
	}
	*/

	public int getClassType()
	{
		return unitType;
	}

	public ClassAbility getClassAbility()
	{
		return ability;
	}

	public int[] getGrowthModifiers()
	{
		return growthModifiers;
	}

	public int[] getClassTreeGrowthModifiers()
	{
		return getGrowthModifiers();
	}

	public int[] getProficiencyModifiers()
	{
		return proficiencyModifiers;
	}

	/*
	public int[] getStatModifiers()
	{
		return statModifiers;
	}
	*/

	public int magicHeuristicMultiplier()
	{
		return magicHeuristicMultiplierVal;
	}

	public int strengthHeuristicMultiplier()
	{
		return strengthHeuristicMultiplierVal;
	}

	public int generalInternalHeuristic()
	{
		return generalInternalHeuristicVal;
	}

	public float getMountEvasionBonus()
	{
		if (mount == null)
		{
			return 0;
		}
		return mount.getEvasion();
	}

	public bool canTrainUnitWithMaterials(Human h, int[] mounts)
	{
		//If there are no mounts, there are no constraints
		if (mount == null)
		{
			return true;
		}
		//If the mount isn't there, return false
		if (mounts[mount.getId()] == 0)
		{
			return false;
		}
		//Only female units can ride unicorns
		if (mount == Mount.UNICORN && !(h.getGender()))
		{
			return false;
		}
		return true;
	}

	public enum ClassAbility
    {
		NONE, CONVOY, SAILING, HERDING
    }
}
