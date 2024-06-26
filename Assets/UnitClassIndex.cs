using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitClassIndex
{

	public static List<List<UnitClass>> index;

	public static int HUMAN = 0;

	public static void initialize()
	{
		index = new List<List<UnitClass>>();
		index.Add(new List<UnitClass>());
		//TODO add other class types
		generateStandardHumanClasses();
	}

	public static void addClass(UnitClass uc)
	{
		index[uc.getClassType()].Add(uc);
	}

	public static UnitClass getUnitClass(int type, int idx)
	{
		return index[type][idx];
	}

	public static List<UnitClass> getHumanClasses()
	{
		return getClassesForUnitType(HUMAN);
	}

	public static List<UnitClass> getClassesForUnitType(int type)
	{
		return index[type];
	}

	public static UnitClass getClassByName(int type, string name)
	{
		List<UnitClass> possible = index[type];
		for (int q = 0; q < possible.Count; q++)
		{
			if (possible[q].getName() == name)
			{
				return possible[q];
			}
		}
		return null;
	}

	/**
	 * Used to initially add the standard classes to the index
	 * Used mainly for testing
	 */
	public static void generateStandardHumanClasses()
	{
		//TODO rebalance stats
		int[] trnGMods = { 20, 20, 20, 5, 0, 5, 15, 30, 0 };
		int[] trnPMods = { 20, 0, 0, 0, 20, 0, 0, 0, 0, 0 };
		UnitClass transport = new UnitClass("Transport", Mount.HORSE, 0, trnGMods, trnPMods, null, null, UnitClass.ClassAbility.CONVOY);
		addClass(transport);

		int[] salGMods = { 5, 30, 30, 10, 0, 30, 10, 10, 10 };
		int[] salPMods = { 20, 0, 0, 0, 0, 20, 0, 0, 0, 0 };
		UnitClass sailor = new UnitClass("Sailor", null, 0, salGMods, salPMods, null, null, UnitClass.ClassAbility.SAILING);
		addClass(sailor);

		int[] shpGMods = { 15, 10, 20, 15, 20, 20, 10, 20, 5 };
		int[] shpPMods = { 0, 0, 0, 20, 0, 0, 0, 0, 0, 20 };
		UnitClass shepherd = new UnitClass("Shepherd", null, 0, shpGMods, shpPMods, null, null, UnitClass.ClassAbility.HERDING);
		addClass(shepherd);

		int[] cavGMods = { 20, 30, 30, 10, 5, 20, 20, 20, 5 };
		int[] cavPMods = { 20, 20, 0, 0, 0, 0, 0, 0, 0, 0 };
		UnitClass cavalier = new UnitClass("Cavalier", Mount.HORSE, 0, cavGMods, cavPMods, null, null, UnitClass.ClassAbility.NONE);
		addClass(cavalier);

		int[] pegGMods = { 20, 10, 25, 10, 0, 20, 35, 20, 20 };
		int[] pegPMods = { 10, 30, 0, 0, 0, 0, 0, 0, 0, 0 };
		UnitClass pegasusKnight = new UnitClass("Pegasus Knight", Mount.PEGASUS, 0, pegGMods, pegPMods, null, null, UnitClass.ClassAbility.NONE);
		addClass(pegasusKnight);

		int[] wyvGMods = { 20, 30, 30, 20, 0, 20, 30, 10, 0 };
		int[] wyvPMods = { 0, 30, 10, 0, 0, 0, 0, 0, 0, 0 };
		UnitClass wyvernKnight = new UnitClass("Wyvern Knight", Mount.WYVERN, 0, wyvGMods, wyvPMods, null, null, UnitClass.ClassAbility.NONE);
		addClass(wyvernKnight);

		int[] grfGMods = { 10, 15, 25, 20, 10, 20, 20, 30, 10 };
		int[] grfPMods = { 0, 10, 30, 0, 0, 0, 0, 0, 0, 0 };
		UnitClass griffinKnight = new UnitClass("Griffin Knight", Mount.GRIFFIN, 0, grfGMods, grfPMods, null, null, UnitClass.ClassAbility.NONE);
		addClass(griffinKnight);

		int[] uniGMods = { 10, 10, 10, 10, 30, 10, 20, 30, 30 };
		int[] uniPMods = { 0, 10, 0, 0, 0, 0, 0, 0, 0, 30 };
		UnitClass unicornKnight = new UnitClass("Unicorn Knight", Mount.UNICORN, 0, uniGMods, uniPMods, null, null, UnitClass.ClassAbility.NONE);
		addClass(unicornKnight);

		int[] arcGMods = { 25, 20, 25, 20, 0, 35, 15, 20, 0 };
		int[] arcPMods = { 0, 0, 0, 40, 0, 0, 0, 0, 0, 0 };
		UnitClass archer = new UnitClass("Archer", null, 0, arcGMods, arcPMods, null, null, UnitClass.ClassAbility.NONE);
		addClass(archer);

		int[] armGMods = { 30, 30, 40, 30, 0, 30, 10, 10, 0 };
		int[] armPMods = { 0, 40, 0, 0, 0, 0, 0, 0, 0, 0 };
		UnitClass armorKnight = new UnitClass("Armor Knight", null, 0, armGMods, armPMods, null, null, UnitClass.ClassAbility.NONE);
		addClass(armorKnight);

		int[] swdGMods = { 20, 20, 10, 10, 10, 30, 30, 30, 20 };
		int[] swdPMods = { 40, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
		UnitClass swordFighter = new UnitClass("Sword Fighter", null, 0, swdGMods, swdPMods, null, null, UnitClass.ClassAbility.NONE);
		addClass(swordFighter);

		int[] axeGMods = { 20, 25, 30, 20, 0, 25, 25, 25, 10 };
		int[] axePMods = { 0, 0, 40, 0, 0, 0, 0, 0, 0, 0 };
		UnitClass axeFighter = new UnitClass("Axe Fighter", null, 0, axeGMods, axePMods, null, null, UnitClass.ClassAbility.NONE);
		addClass(axeFighter);

		int[] eleGMods = { 20, 10, 20, 10, 30, 20, 30, 20, 20 };
		int[] elePMods = { 0, 0, 0, 0, 0, 0, 40, 0, 0, 0 };
		UnitClass elementalMage = new UnitClass("Elemental Mage", null, 0, eleGMods, elePMods, null, null, UnitClass.ClassAbility.NONE);
		addClass(elementalMage);

		int[] litGMods = { 20, 10, 10, 10, 20, 30, 20, 30, 30 };
		int[] litPMods = { 0, 0, 0, 0, 0, 0, 0, 40, 0, 0 };
		UnitClass lightMage = new UnitClass("Light Mage", null, 0, litGMods, litPMods, null, null, UnitClass.ClassAbility.NONE);
		addClass(lightMage);

		int[] drkGMods = { 20, 20, 10, 10, 30, 15, 20, 20, 35 };
		int[] drkPMods = { 0, 0, 0, 0, 0, 0, 0, 0, 40, 0 };
		UnitClass darkMage = new UnitClass("Dark Mage", null, 0, drkGMods, drkPMods, null, null, UnitClass.ClassAbility.NONE);
		addClass(darkMage);

		//TODO add the rest
	}
}
