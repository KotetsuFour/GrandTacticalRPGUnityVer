using UnityEngine;
public class Clone : Human
{
	public static int IDEAL_CLONING_PROFICIENCY = 300;

	private Clone(string name, bool gender, int[] maxHPs, int magic, int skill, int reflex, int awareness, int resistance,
			int movement, int leadership, int[] maxHPGrowths, int magGrowth, int sklGrowth, int rfxGrowth,
			int awrGrowth, int resGrowth, int age, int[] personality, int[] appearance, Interest[] interests,
			Interest[] disinterests, Demeanor demeanor, CombatTrait valuedTrait, CityState home)
			: base(name, gender, maxHPs, magic, skill, reflex, awareness, resistance, movement, leadership, maxHPGrowths,
				magGrowth, sklGrowth, rfxGrowth, awrGrowth, resGrowth, age, personality, appearance,
				interests, disinterests, demeanor, valuedTrait, home)
	{
	}

	/**
	 * Creates and returns a new human clone
	 * @param x is the human reference (a reference made from the human, not the actual human)
	 * @param y is the unit that is performing the cloning process
	 * @param loc is the city-state where the cloning is taking place
	 * @return
	 */
	public static Clone cloneOfXProducedByY(ArtificialHumanTemplate x, Equippable y, CityState loc)
	{
		float percentageProficiency =
				(float)((0.0 + y.proficiencyWith(Weapon.DARK)) / (0.0 + IDEAL_CLONING_PROFICIENCY));
		percentageProficiency = (float)Mathf.Min(1.0f, (float)percentageProficiency);
		bool fluke = (RNGStuff.nextInt(1000) == 0);
		int[] personalValues = new int[6];
		Interest[] interests = new Interest[3];
		Interest[] disinterests = new Interest[3];
		generatePersonality(personalValues, interests, disinterests, loc.getValues());
		Demeanor[] types = Demeanor.values();
		Demeanor demeanor = types[RNGStuff.nextInt(types.Length)];
		if (demeanor.getRarity() > 0)
		{
			demeanor = types[RNGStuff.nextInt(types.Length)];
		}
		CombatTrait[] traits = CombatTrait.values();
		CombatTrait valued = traits[RNGStuff.nextInt(traits.Length)];
		int[] appearance = cloneAppearance(x.getAppearance(), percentageProficiency, fluke);
		int[] maxHPs = cloneMaxHPs(x.getBodyPartsMaximumHP(), percentageProficiency, fluke);
		int[] maxHPsGrowths = cloneMaxHPGrowths(x.getBodyPartsMaximumHPGrowth(), percentageProficiency, fluke);
		bool gend = x.getGender();

		//Clone stats and growths imperfectly
		int mag = x.getMagic();
		int skl = x.getSkill();
		int rfx = x.getReflex();
		int awr = x.getAwareness();
		int res = x.getResistance();
		int magGrowth = x.getMagicGrowth();
		int sklGrowth = x.getSkillGrowth();
		int rfxGrowth = x.getReflexGrowth();
		int awrGrowth = x.getAwarenessGrowth();
		int resGrowth = x.getResistanceGrowth();

		int potMag = Mathf.RoundToInt(mag * percentageProficiency);
		int potSkl = Mathf.RoundToInt(skl * percentageProficiency);
		int potRfx = Mathf.RoundToInt(rfx * percentageProficiency);
		int potAwr = Mathf.RoundToInt(awr * percentageProficiency);
		int potRes = Mathf.RoundToInt(res * percentageProficiency);
		int potMagGrowth = Mathf.RoundToInt(magGrowth * percentageProficiency);
		int potSklGrowth = Mathf.RoundToInt(sklGrowth * percentageProficiency);
		int potRfxGrowth = Mathf.RoundToInt(rfxGrowth * percentageProficiency);
		int potAwrGrowth = Mathf.RoundToInt(awrGrowth * percentageProficiency);
		int potResGrowth = Mathf.RoundToInt(resGrowth * percentageProficiency);
		if (fluke)
		{
			//If there is a fluke, lack of proficiency means better clones and better
			//proficiency means worse clones
			if (percentageProficiency > 1.0)
			{
				mag = potMag + RNGStuff.nextInt(mag - potMag);
				skl = potSkl + RNGStuff.nextInt(skl - potSkl);
				rfx = potRfx + RNGStuff.nextInt(rfx - potRfx);
				awr = potAwr + RNGStuff.nextInt(awr - potAwr);
				res = potRes + RNGStuff.nextInt(res - potRes);
				magGrowth = potMagGrowth + RNGStuff.nextInt(magGrowth - potMagGrowth);
				sklGrowth = potSklGrowth + RNGStuff.nextInt(sklGrowth - potSklGrowth);
				rfxGrowth = potRfxGrowth + RNGStuff.nextInt(rfxGrowth - potRfxGrowth);
				awrGrowth = potAwrGrowth + RNGStuff.nextInt(awrGrowth - potAwrGrowth);
				resGrowth = potResGrowth + RNGStuff.nextInt(resGrowth - potResGrowth);
			}
			else if (percentageProficiency < 1.0)
			{
				mag += RNGStuff.nextInt(potMag - mag);
				skl += RNGStuff.nextInt(potSkl - skl);
				rfx += RNGStuff.nextInt(potRfx - rfx);
				awr += RNGStuff.nextInt(potAwr - awr);
				res += RNGStuff.nextInt(potRes - res);
				magGrowth += RNGStuff.nextInt(potMagGrowth - magGrowth);
				sklGrowth += RNGStuff.nextInt(potSklGrowth - sklGrowth);
				rfxGrowth += RNGStuff.nextInt(potRfxGrowth - rfxGrowth);
				awrGrowth += RNGStuff.nextInt(potAwrGrowth - awrGrowth);
				resGrowth += RNGStuff.nextInt(potResGrowth - resGrowth);
			}
		}
		else
		{
			//Otherwise, lack of proficiency means worse clones and worse proficiency means
			//better clones
			if (percentageProficiency > 1.0)
			{
				mag += RNGStuff.nextInt(potMag - mag);
				skl += RNGStuff.nextInt(potSkl - skl);
				rfx += RNGStuff.nextInt(potRfx - rfx);
				awr += RNGStuff.nextInt(potAwr - awr);
				res += RNGStuff.nextInt(potRes - res);
				magGrowth += RNGStuff.nextInt(potMagGrowth - magGrowth);
				sklGrowth += RNGStuff.nextInt(potSklGrowth - sklGrowth);
				rfxGrowth += RNGStuff.nextInt(potRfxGrowth - rfxGrowth);
				awrGrowth += RNGStuff.nextInt(potAwrGrowth - awrGrowth);
				resGrowth += RNGStuff.nextInt(potResGrowth - resGrowth);
			}
			else if (percentageProficiency < 1.0)
			{
				mag = potMag + RNGStuff.nextInt(mag - potMag);
				skl = potSkl + RNGStuff.nextInt(skl - potSkl);
				rfx = potRfx + RNGStuff.nextInt(rfx - potRfx);
				awr = potAwr + RNGStuff.nextInt(awr - potAwr);
				res = potRes + RNGStuff.nextInt(res - potRes);
				magGrowth = potMagGrowth + RNGStuff.nextInt(magGrowth - potMagGrowth);
				sklGrowth = potSklGrowth + RNGStuff.nextInt(sklGrowth - potSklGrowth);
				rfxGrowth = potRfxGrowth + RNGStuff.nextInt(rfxGrowth - potRfxGrowth);
				awrGrowth = potAwrGrowth + RNGStuff.nextInt(awrGrowth - potAwrGrowth);
				resGrowth = potResGrowth + RNGStuff.nextInt(resGrowth - potResGrowth);
			}
		}

		int mov = x.getMovement();
		int ldr = RNGStuff.nextInt(6);
		if (ldr > 3 && RNGStuff.nextBoolean())
		{
			ldr = RNGStuff.nextInt(2); //Can't have too many good leaders
		}
		string genName = x.getSpecificTemplateIndex() + "-" + x.getAndIncrementNumCopiesMade();
		int age = 18;

		return new Clone(genName, gend, maxHPs, mag, skl, rfx, awr, res, mov, ldr,
				maxHPsGrowths, magGrowth, sklGrowth, rfxGrowth, awrGrowth, resGrowth,
				age, personalValues, appearance, interests, disinterests, demeanor,
				valued, loc);
	}

	private static int[] cloneAppearance(int[] reference, float percentageProficiency, bool fluke)
	{
		//TODO
		return null;
	}

	private static int[] cloneMaxHPs(int[] maxHPs, float percentageProficiency,
			bool fluke)
	{
		int[] ret = new int[maxHPs.Length];
		if (fluke)
		{
			for (int q = 0; q < ret.Length; q++)
			{
				ret[q] = Mathf.RoundToInt(maxHPs[q] / percentageProficiency);
			}
		}
		else
		{
			for (int q = 0; q < ret.Length; q++)
			{
				ret[q] = Mathf.RoundToInt(maxHPs[q] * percentageProficiency);
			}
		}
		return ret;
	}

	private static int[] cloneMaxHPGrowths(int[] hpGrowths, float percentageProficiency,
			bool fluke)
	{
		int[] ret = new int[hpGrowths.Length];
		if (fluke)
		{
			for (int q = 0; q < ret.Length; q++)
			{
				ret[q] = Mathf.RoundToInt(hpGrowths[q] / percentageProficiency);
			}
		}
		else
		{
			for (int q = 0; q < ret.Length; q++)
			{
				ret[q] = Mathf.RoundToInt(hpGrowths[q] * percentageProficiency);
			}
		}
		return ret;
	}

}
