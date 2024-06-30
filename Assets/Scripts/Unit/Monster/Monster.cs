using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Monster : Unit
{


	protected Human master;

	protected Monster(int level, UnitClass unitClass, string name, int[] maxHPs, int magic, int skill, int reflex,
			int awareness, int resistance, int movement, int leadership, int[] maxHPGrowths, int magGrowth,
			int sklGrowth, int rfxGrowth, int awrGrowth, int resGrowth, Human master)
			: base(level, unitClass, name, maxHPs, magic, skill, reflex, awareness, resistance, movement, leadership, maxHPGrowths,
				magGrowth, sklGrowth, rfxGrowth, awrGrowth, resGrowth)
	{
		this.master = master;
		// TODO Auto-generated constructor stub
	}


	public override bool gainExperience(int exp)
	{
		//Gain experience only if you're not already max level
		if (level != Unit.MAX_LEVEL)
		{
			exp -= effectiveLevel();
			exp /= 3; //Monsters gain less experience
			exp = Mathf.Max(1, Mathf.Min(Unit.EXPERIENCE_TOWARDS_LEVEL, exp)); //Max 100, Min 1
			experience += exp;
			//If you got enough experience for a level,
			if (experience >= Unit.EXPERIENCE_TOWARDS_LEVEL)
			{
				//Level up
				level++;
				//Adjust experience appropriately
				if (level == MAX_LEVEL)
				{
					experience = 0;
				}
				else
				{
					experience -= EXPERIENCE_TOWARDS_LEVEL;
				}
				//We don't need a level-up animation, considering there are so many units
				//So just adjust the stats silently here
				levelUp();
				return true;
			}
		}
		return false;
	}


	public override int getMorale()
	{
		return 100; //Monsters are always willing to fight
	}


	public override void deathSequence()
	{
		// TODO Auto-generated method stub

	}


	public override Nation getAffiliation()
	{
		if (master.isAlive())
		{
			return master.getAffiliation();
		}
		return null;
	}
	public Human getMaster()
	{
		return master;
	}


	public override void defect(Nation n)
	{
		group.remove(this);
		group = null;
		//Just need to remove from group. The master controls the affiliation
		//No need to add or remove yourself to/from an army. The Nation contructor does this
	}

	/**
	 * Attack speed is just reflex for unequipped monsters
	 * Method is overridden by EquippedMonster
	 * @return
	 */

	public override int attackSpeed()
	{
		return reflex;
	}

	/**
	 * Overridden by EquippedMonster
	 */

	public override int getBaseAccuracy()
	{
		return 0;
	}

	/**
	 * Overridden by EquippedMonster
	 */

	public override int getBaseCrit()
	{
		return 0;
	}

	/**
	 * Unequipped monsters use pure skill as crit
	 * This is overridden by EquippedMonters
	 */

	public override int criticalHitRate()
	{
		return skill;
	}

}
