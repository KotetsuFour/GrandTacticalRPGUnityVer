using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TrainingFacility : Defendable
{

	private int[] mounts;
	private List<Human> trainees; //Just humans, as monsters cannot be trained

	public static int MAX_TRAINABLE_LEVEL = 10;

	/**This value, when gained every day for 30 days, will bring a level 0 tier 1 unit up
	 * to level 10, with only 1 extra experience point (taking into account reduced exp
	 * gain for higher level units)
	 */
	public static int OPTIMAL_TRAINING_PER_TURN = 38;
	public static int MAXIMUM_ANIMAL_COUNT = 50;

	//TODO decide actual values
	public static int[] materialsNeededForConstruction = { };
	public static int MAX_INTEGRITY = 10;
	public static int DURABILITY = 10;
	public static int RESISTANCE = 10;

	public TrainingFacility(string name, Human owner, WorldMapTile location)
			: base(name, MAX_INTEGRITY, DURABILITY, RESISTANCE, owner, location)
	{
		mounts = new int[Mount.values().Length];
		trainees = new List<Human>();
	}


	public override string getType()
	{
		return Building.TRAINING_FACILITY;
	}


	public override void completeDailyAction()
	{

		//To relieve the player of tedious responsibility, if classes are not assigned
		//immediately after recruitment, then assume the player doesn't care and auto-assign
		autoAssignClasses();
		autoAssignAllWeapons();
		autoAssignAllArmors();
		autoAssignAllStaves();

		for (int q = 0; q < trainees.Count; q++)
		{
			Human h = trainees[q];
			if (h.getUnitClass() == null)
			{
				continue;
			}
			if (h.getLevel() < MAX_TRAINABLE_LEVEL)
			{
				//Doesn't really matter, but this means that higher tier units will
				//gain less experience (tier 3 and 4 units will hardly gain any experience)
				h.gainExperience(OPTIMAL_TRAINING_PER_TURN);
			}
		}
	}

	/**
	 * Overridden for optimization's sake. The superclass's method would also
	 * function correctly, but slower, probably
	 */

	public override void completeMonthlyAction()
	{

		//To relieve the player of tedious responsibility, if classes are not assigned
		//immediately after recruitment, then assume the player doesn't care and auto-assign
		autoAssignClasses();
		autoAssignAllWeapons();
		autoAssignAllArmors();
		autoAssignAllStaves();

		for (int q = 0; q < trainees.Count; q++)
		{
			Human h = trainees[q];
			if (h.getUnitClass() == null)
			{
				continue;
			}
			//For units above tier 1, this is much faster than the daily action method
			//I don't really mind the difference, though
			while (h.getLevel() < MAX_TRAINABLE_LEVEL)
			{
				h.finishLevel();
			}
		}
	}


	public override void destroy()
	{
		// TODO Auto-generated method stub
	}

	public bool addUnit(Human u)
	{
		if (trainees.Count >= UnitGroup.CAPACITY || u.getAffiliation().getArmy().Count >= Nation.MAX_ARMY_SIZE)
		{
			return false;
		}
		if (u.getGroup() != null)
		{
			u.getGroup().remove(u);
		}
		if (trainees.Count == 0)
		{
			new UnitGroup(u); //group automatically adds itself to the nation
		}
		else
		{
			trainees[0].getGroup().add(u);
		}
		u.getAffiliation().getArmy().Add(u);
		trainees.Add(u);
		return true;
	}

	public List<Human> getTrainees()
	{
		return trainees;
	}

	public void addMount(Mount m, int quantity)
	{
		mounts[m.getId()] += quantity;
	}

	public int[] getMounts()
	{
		return mounts;
	}

	public bool assignWeapon(int unit, int weapon)
	{
		if (trainees[unit].receiveNewItem(materials[weapon]))
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

	public bool assignArmor(int unit, int armor)
	{
		if (trainees[unit].receiveNewArmor(armors[armor]))
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

	public bool assignStaff(int unit, int staff)
	{
		if (trainees[unit].receiveNewItem(staves[staff]))
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

	public bool readyToGraduate()
	{
		for (int q = 0; q < trainees.Count; q++)
		{
			if (trainees[q].getUnitClass() == null
					|| trainees[q].getLevel() < MAX_TRAINABLE_LEVEL)
			{
				return false;
			}
		}
		return true;
	}

	public bool graduateUnits(WorldMap map)
	{
		if (trainees.Count == 0)
		{
			return false;
		}
		if (location.isVacant())
		{
			UnitGroup group = trainees[0].getGroup();
			group.autoAssignLeader();
			location.sendHere(group);
			trainees = new List<Human>(UnitGroup.CAPACITY);
			return true;
		}
		return false;
	}

	/**
	 * This method exists only for testing purposes
	 * @return graduated unit group
	 */
	public UnitGroup graduateUnitsForTesting()
	{
		UnitGroup group = new UnitGroup(trainees);
		group.autoAssignLeader();
		trainees = new List<Human>(UnitGroup.CAPACITY);
		return group;
	}

	public void autoAssignClasses()
	{
		for (int q = 0; q < trainees.Count; q++)
		{
			autoAssignClass(q);
		}
	}

	public void autoAssignClass(int unit)
	{
		List<UnitClass> unitClasses = UnitClassIndex.getHumanClasses();
		Human h = trainees[unit];
		if (h.getUnitClass() != null)
		{
			return;
		}
		UnitClass best = unitClasses[0];
		int heuristic = int.MinValue;
		for (int w = 0; w < unitClasses.Count; w++)
		{
			UnitClass uc = unitClasses[w];
			if (uc.canTrainUnitWithMaterials(h, mounts))
			{
				int check = unitEffectivenessInClass(h, uc);
				if (check > heuristic)
				{
					heuristic = check;
					best = uc;
				}
			}
		}
		assignClass(unit, best);
	}

	public bool assignClass(int unit, UnitClass c)
	{
		if (c.canTrainUnitWithMaterials(trainees[unit], mounts))
		{
			trainees[unit].assignClass(c);
			if (c.getMount() != null)
			{
				mounts[c.getMountType()]--;
			}
			return true;
		}
		return false;
	}

	public void autoAssignSupportPartners()
	{
		//Just assign randomly
		//TODO maybe strategically assign partners just to make sure the player
		//doesn't have that advantage over the AI

		for (int q = 0; q < trainees.Count; q++)
		{
			Human searching = trainees[q];
			if (searching.canAssignSupportPartner())
			{
				for (int w = q + 1; w < trainees.Count; w++)
				{
					Human found = trainees[w];
					if (found.canAssignSupportPartner())
					{
						searching.assignSupportPartner(found);
						break;
					}
				}
			}
		}
	}

	public void autoAssignAllWeapons()
	{
		for (int q = 0; q < trainees.Count; q++)
		{
			autoAssignWeapon(q);
		}
	}

	public void autoAssignWeapon(int unit)
	{
		Human h = trainees[unit];
		//If the unit already has a weapon that they're somewhat proficient in, don't do this
		HandheldWeapon eq = h.getEquippedWeapon();
		if (eq != null && h.proficiencyWith(eq.getProficiencyIndex()) > 0)
		{
			return;
		}
		int wepHeur = int.MinValue;
		if (eq != null)
		{
			wepHeur = h.getEquipmentHeuristic(h.getInventory()[0]);
		}
		int idx = -1;
		for (int a = 0; a < materials.Count; a++)
		{
			int[] ie = materials[a];
			int test = h.getEquipmentHeuristic(ie);
			if (test > wepHeur)
			{
				wepHeur = test;
				idx = a;
			}
		}
		if (idx != -1)
		{
			assignWeapon(unit, idx);
			h.autoEquip();
		}
	}

	public void autoAssignAllArmors()
	{
		for (int q = 0; q < trainees.Count; q++)
		{
			autoAssignArmor(q);
		}
	}

	public void autoAssignArmor(int unit)
	{
		Human h = trainees[unit];
		if (h.getArmor() != null)
		{
			return;
		}
		int armHeur = int.MinValue;
		int idx = -1;
		for (int a = 0; a < armors.Count; a++)
		{
			Armor ie = (Armor)InventoryIndex.getElement(armors[a]);
			int test = 0;
			if (h.getUnitClass() == null || h.getUnitClass().getMount() == null)
			{
				test -= (Mathf.Max(0, (ie.getWeight() - h.armStrength())) * 10);
			}
			//Double count head and torso defense
			test += ie.getDefenseFor(0);
			test += ie.getDefenseFor(1);
			int[] defs = ie.getDefenses();
			for (int w = 0; w < defs.Length; w++)
			{
				test += (defs[w] * 10) - h.getMaximumHPOfBodyPart(w);
			}
			if (test > armHeur)
			{
				armHeur = test;
				idx = a;
			}
		}
		if (idx != -1)
		{
			assignArmor(unit, idx);
		}
	}

	public void autoAssignAllStaves()
	{
		for (int q = 0; q < trainees.Count; q++)
		{
			autoAssignStaff(q);
		}
	}

	public void autoAssignStaff(int unit)
	{
		Human h = trainees[unit];
		if (h.proficiencyWith(Weapon.STAFF) == 0)
		{
			return;
		}
		//TODO
	}

	public static int unitEffectivenessInClass(Human h, UnitClass uc)
	{
		int ret = uc.generalInternalHeuristic();

		//The commented code below and the one commented line in the following if block
		// Are supposed to make the heuristic decently influenced by the resources available
		// However, this is probably not a helpful approach. It is better to make an impartial
		// decision about the unit's best class, then order weapons according to what was picked
		//		for (int q = 0; q < materials.Count; q++) {
		//			int[] mat = materials[q];
		//			HandheldWeapon w = (HandheldWeapon)InventoryIndex.getElement(mat);
		//			if (uc.getProficiencyModifiers()[w.getProficiencyIndex()] >= w.getProficiencyRequirement()) {
		//				ret += (mat[2] * 10);
		//			}
		//		}
		if (uc.getMount() != null)
		{
			//			ret += (mounts[uc.getMountType()] * 10);
			ret += (int)(uc.getMountMovement() + (uc.getMountEvasionBonus() * 5));
		}

		int[] parts = h.getBodyPartsMaximumHPGrowth();
		int[] gMods = uc.getGrowthModifiers();
		ret += (gMods[0] + (parts[Human.HEAD] / 2)) - Mathf.Abs(gMods[0] - (parts[Human.HEAD] / 2));
		ret += (gMods[1] + (parts[Human.TORSO] / 2)) - Mathf.Abs(gMods[1] - (parts[Human.TORSO] / 2));
		ret += ((gMods[2] + (parts[Human.RIGHT_ARM] / 2)) - Mathf.Abs(gMods[2] - (parts[Human.RIGHT_ARM] / 2)));
		ret += (gMods[2] + (parts[Human.LEFT_ARM] / 2)) - Mathf.Abs(gMods[2] - (parts[Human.LEFT_ARM] / 2)); //Don't multiply strength again. That's too much
		ret += (gMods[3] + (parts[Human.RIGHT_LEG] / 2)) - Mathf.Abs(gMods[3] - (parts[Human.RIGHT_LEG] / 2));
		ret += (gMods[3] + (parts[Human.LEFT_LEG] / 2)) - Mathf.Abs(gMods[3] - (parts[Human.LEFT_LEG] / 2));

		ret += ((gMods[4] + h.getMagicGrowth()) - Mathf.Abs(gMods[4] - h.getMagicGrowth())) * uc.magicHeuristicMultiplier();
		ret += (gMods[5] + h.getSkillGrowth()) - Mathf.Abs(gMods[5] - h.getSkillGrowth());
		ret += (gMods[6] + h.getReflexGrowth()) - Mathf.Abs(gMods[6] - h.getReflexGrowth());
		ret += (gMods[7] + h.getAwarenessGrowth()) - Mathf.Abs(gMods[7] - h.getSkillGrowth());
		ret += (gMods[8] + h.getResistanceGrowth()) - Mathf.Abs(gMods[8] - h.getResistanceGrowth());

		return ret;
	}


	public new void defect(Nation n)
	{
		// TODO deal with trainees as well as owner

	}

}
