using UnityEngine;
using System;
public abstract class EquippedMonster : Monster, Equippable
{

	protected int[][] inventory;

	protected int[] armor;

	protected int[] proficiency;

	public static int RIGHT_ARM = 2;
	public static int LEFT_ARM = 3;

	protected EquippedMonster(int level, UnitClass unitClass, string name, int[] maxHPs, int magic, int skill, int reflex,
			int awareness, int resistance, int movement, int leadership, int[] maxHPGrowths, int magGrowth,
			int sklGrowth, int rfxGrowth, int awrGrowth, int resGrowth, Human master)
			: base(level, unitClass, name, maxHPs, magic, skill, reflex, awareness, resistance, movement, leadership, maxHPGrowths,
				magGrowth, sklGrowth, rfxGrowth, awrGrowth, resGrowth, master)
	{
		// TODO Auto-generated constructor stub
	}

	public Item getEquippedItem()
	{
		//TODO remove try-catch after creating index;
		try
		{
			return InventoryIndex.getElement(inventory[0]);
		}
		catch (Exception e)
		{
			Debug.Log(e);
			return null;
		}
	}


	public override bool isUsingMagic()
	{
		Item i = getEquippedItem();
		return i is Weapon && ((Weapon)i).isMagic();
	}


	public void useWeapon(bool hit)
	{
		// TODO Auto-generated method stub
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


	public int[][] getInventory()
	{
		return inventory;
	}


	public override int attackStrength()
	{
		Item i = getEquippedItem();
		if (i is Weapon)
		{
			Weapon w = (Weapon)i;
			if (w.isMagic())
			{
				return magic + w.getMight();
			}
			return armStrength() + w.getMight();
		}
		return armStrength();
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


	public new abstract int getBaseAccuracy();


	public override int getBaseCrit()
	{
		int crit = skill;
		return crit;
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


	public new int criticalHitRate()
	{
		int crit = getBaseCrit();
		Item i = getEquippedItem();
		if (i is Weapon)
		{
			Weapon w = (Weapon)i;
			crit += w.getCrit();
			crit += Mathf.Min(10, proficiency[w.getProficiencyIndex()] - w.getProficiencyRequirement());
		}
		return crit;
	}


	public override int criticalHitAvoid()
	{
		int avoid = awareness;
		return avoid;
	}


	public bool canUseBallista()
	{
		return armStrength() > 10;
	}


	public bool canUse(StationaryWeapon weapon)
	{
		if (weapon.isMagic())
		{
			return canUseMagicTurrets();
		}
		return canUseBallista();
	}


	public bool canUseMagicTurrets()
	{
		return magic > 0;
	}


	public Armor getArmor()
	{
		// TODO Auto-generated method stub
		return null;
	}


	public void destroyArmor()
	{
		// TODO Auto-generated method stub

	}


	public string getArmorName()
	{
		// TODO Auto-generated method stub
		return null;
	}


	public string getWeaponName()
	{
		// TODO Auto-generated method stub
		return null;
	}


	public HandheldWeapon getEquippedWeapon()
	{
		// TODO Auto-generated method stub
		return null;
	}


	public void autoEquip()
	{
		// TODO Auto-generated method stub

	}


	public int getEquipmentHeuristic(int[] item)
	{
		// TODO Auto-generated method stub
		return 0;
	}


	public void equip(int idx)
	{
		// TODO Auto-generated method stub

	}


	public bool receiveNewArmor(int[] armor)
	{
		// TODO Auto-generated method stub
		return false;
	}


	public bool receiveNewItem(int[] item)
	{
		// TODO Auto-generated method stub
		return false;
	}


	public int proficiencyWith(int type)
	{
		return proficiency[type];
	}

}
