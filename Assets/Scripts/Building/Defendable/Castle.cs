using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Castle : Defendable
{

	private int[] mounts;

	public static int MAXIMUM_ANIMAL_COUNT = 5;

	public static int MAX_TRAINABLE_LEVEL = 15;

	//TODO decide actual values
	public static int[] materialsNeededForConstruction = { };
	public static int MAX_INTEGRITY = 10;
	public static int DURABILITY = 10;
	public static int RESISTANCE = 10;

	public Castle(Human owner, WorldMapTile location)
			: base($"{owner.getName()} Castle", MAX_INTEGRITY, DURABILITY, RESISTANCE, owner, location)
	{
		mounts = new int[Mount.values().Length];
	}
	public Castle(string name, WorldMapTile location)
			: base(name, MAX_INTEGRITY, DURABILITY, RESISTANCE, null, location)
	{
		mounts = new int[Mount.values().Length];
	}


	public override string getType()
	{
		return Building.CASTLE;
	}


	public override void completeDailyAction()
	{
		// TODO Auto-generated method stub

	}


	public override void destroy()
	{
		// TODO Auto-generated method stub

	}


	public override void completeMonthlyAction()
	{
		// TODO Auto-generated method stub

	}

	public void addMount(Mount m, int quantity)
	{
		mounts[m.getId()] += quantity;
	}

	public int[] getMounts()
	{
		return mounts;
	}

	public bool canOutfitOwner()
	{
		return owner != null
				&& getAssignedGroup() != null
				&& getAssignedGroup().containsUnit(owner);
	}

	public void autoAssignClass()
	{
		List<UnitClass> unitClasses = UnitClassIndex.getHumanClasses();
		if (owner.getUnitClass() != null)
		{
			return;
		}
		UnitClass best = unitClasses[0];
		int heuristic = int.MinValue;
		for (int w = 0; w < unitClasses.Count; w++)
		{
			UnitClass uc = unitClasses[w];
			if (uc.canTrainUnitWithMaterials(owner, mounts))
			{
				int check = TrainingFacility.unitEffectivenessInClass(owner, uc);
				if (check > heuristic)
				{
					heuristic = check;
					best = uc;
				}
			}
		}
		assignClass(best);
	}

	public bool assignClass(UnitClass c)
	{
		if (c.canTrainUnitWithMaterials(owner, mounts))
		{
			owner.assignClass(c);
			if (c.getMount() != null)
			{
				mounts[c.getMountType()]--;
			}
			return true;
		}
		return false;
	}

	public bool assignWeapon(int weapon)
	{
		if (owner.receiveNewItem(materials[weapon]))
		{
			materials[weapon][2]--;
			if (materials[weapon][2] == 0)
			{
				materials.RemoveAt(weapon);
			}
			return true;
		}
		return false;
	}
	public void autoAssignWeapon()
	{
		int wepHeur = int.MinValue;
		int idx = -1;
		for (int a = 0; a < materials.Count; a++)
		{
			int[] ie = materials[a];
			int test = owner.getEquipmentHeuristic(ie);
			if (test > wepHeur)
			{
				wepHeur = test;
				idx = a;
			}
		}
		if (idx != -1)
		{
			assignWeapon(idx);
		}
	}

	public bool assignArmor(int armor)
	{
		if (owner.receiveNewArmor(armors[armor]))
		{
			armors[armor][2]--;
			if (armors[armor][2] == 0)
			{
				armors.RemoveAt(armor);
			}
			return true;
		}
		return false;
	}
	public void autoAssignArmor()
	{
		int armHeur = int.MinValue;
		int idx = -1;
		for (int a = 0; a < armors.Count; a++)
		{
			Armor ie = (Armor)InventoryIndex.getElement(armors[a]);
			int test = 0;
			if (owner.getUnitClass() == null || owner.getUnitClass().getMount() == null)
			{
				test -= (Mathf.Max(0, (ie.getWeight() - owner.armStrength())) * 10);
			}
			//Double count head and torso defense
			test += ie.getDefenseFor(0);
			test += ie.getDefenseFor(1);
			int[] defs = ie.getDefenses();
			for (int w = 0; w < defs.Length; w++)
			{
				test += (defs[w] * 10) - owner.getMaximumHPOfBodyPart(w);
			}
			if (test > armHeur)
			{
				armHeur = test;
				idx = a;
			}
		}
		if (idx != -1)
		{
			assignArmor(idx);
		}
	}

	public bool assignStaff(int staff)
	{
		if (owner.receiveNewItem(staves[staff]))
		{
			staves[staff][2]--;
			if (staves[staff][2] == 0)
			{
				staves.RemoveAt(staff);
			}
			return true;
		}
		return false;
	}
	public void autoAssignStaff()
	{
		//TODO
	}

	public string trainOwner()
	{
		if (owner == null)
		{
			return "No owner to train";
		}
		if (getAssignedGroup() == null || !getAssignedGroup().containsUnit(owner))
		{
			return "The owner of the castle is not currently present";
		}
		if (owner.getUnitClass() == null)
		{
			return "Cannot train owner until their class has been assigned via the \"Outfit\" menu";
		}
		if (location.getBattle() != null)
		{
			return "Cannot train while there is a battle occuring here";
		}
		if (owner.getLevel() >= MAX_TRAINABLE_LEVEL)
		{
			return "Cannot train owner above level " + MAX_TRAINABLE_LEVEL;
		}
		int exp = Mathf.Max(1, 50 - owner.effectiveLevel());
		owner.gainExperience(50);
		return owner.getName() + " gained " + exp + " experience!";
	}

}
