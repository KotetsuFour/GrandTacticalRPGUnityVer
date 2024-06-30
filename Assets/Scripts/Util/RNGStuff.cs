using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RNGStuff
{
	/*
	public static NamingConvention[] LANGUAGES = {new PrimitiveNamingConvention(), new BoundedNamingConvention(),
			new SyllabicNamingConvention(), new ReverseSyllabicNamingConvention()};
public static NamingConvention[] LANGUAGES_IN_USE = {new PrimitiveNamingConvention(), new BoundedNamingConvention(),
			new SyllabicNamingConvention(), new ReverseSyllabicNamingConvention()};
	*/

	public static ColorSet[] SKIN_COLORS = {
	new ColorSet("Realistic", new Color[] {
					new Color(243.0f/255, 213.0f/255, 208.0f/255, 1),
					new Color(218.0f/255, 185.0f/255, 176.0f/255, 1),
					new Color(233.0f/255, 185.0f/255, 149.0f/255, 1),
					new Color(225.0f/255, 158.0f/255, 149.0f/255, 1),
					new Color(242.0f/255, 170.0f/255, 146.0f/255, 1),
					new Color(205.0f/255, 161.0f/255, 132.0f/255, 1),
					new Color(147.0f/255, 97.0f/255, 74.0f/255, 1),
					new Color(117.0f/255, 57.0f/255, 21.0f/255, 1),
			}),
			new ColorSet("Ants", new Color[] {
					new Color(193.0f/255, 44.0f/255, 25.0f/255, 1),
					new Color(59.0f/255, 51.0f/255, 63.0f/255, 1),
					new Color(209.0f/255, 143.0f/255, 42.0f/255, 1),
					new Color(122.0f/255, 159.0f/255, 84.0f/255, 1),
					new Color(166.0f/255, 94.0f/255, 64.0f/255, 1),
					new Color(33.0f/255, 31.0f/255, 30.0f/255, 1),
					new Color(209.0f/255, 108.0f/255, 70.0f/255, 1),
					new Color(206.0f/255, 109.0f/255, 2.0f/255, 1),
			}),
			new ColorSet("Gems", new Color[] {
					new Color(239.0f/255,175.0f/255,214.0f/255,1),
					new Color(15.0f/255,196.0f/255,244.0f/255,1),
					new Color(243.0f/255,238.0f/255,97.0f/255,1),
					new Color(245.0f/255, 245.0f/255, 245.0f/255, 1),
					new Color(184.0f/255,123.0f/255,200.0f/255, 1),
					new Color(172.0f/255,73.0f/255,91.0f/255, 1),
					new Color(138.0f/255,231.0f/255,102.0f/255, 1),
					new Color(140.0f/255,163.0f/255,201.0f/255, 1),
			}),
			new ColorSet("Rainbow", new Color[] {
					new Color(247.0f/255, 0, 0, 1),
					new Color(247.0f/255, 160.0f/255, 0, 1),
					new Color(247.0f/255, 247.0f/255, 0, 1),
					new Color(0, 124.0f/255, 0, 1),
					new Color(0, 0, 247.0f/255, 1),
					new Color(73.0f/255, 0, 126.0f/255, 1),
					new Color(231.0f/255, 126.0f/255, 231.0f/255, 1),
					new Color(164.0f/255, 164.0f/255, 164.0f/255, 1),
			})
	};
	public static ColorSet[] HAIR_COLORS = {
	new ColorSet("Realistic", new Color[] {
					new Color(8.0f/255,8.0f/255,6.0f/255, 1),
					new Color(107.0f/255,78.0f/255,64.0f/255, 1),
					new Color(166.0f/255,132.0f/255,105.0f/255, 1),
					new Color(164.0f/255,108.0f/255,71.0f/255, 1),
					new Color(184.0f/255,65.0f/255,49.0f/255, 1),
					new Color(254.0f/255,246.0f/255,225.0f/255, 1),
					new Color(202.0f/255,193.0f/255,178.0f/255, 1),
					new Color(202.0f/255,164.0f/255,120.0f/255, 1)
			}),
			new ColorSet("Anime", new Color[] {
					new Color(48.0f/255,104.0f/255,216.0f/255, 1),
					new Color(216.0f/255,232.0f/255,232.0f/255, 1),
					new Color(200.0f/255,24.0f/255,16.0f/255, 1),
					new Color(32.0f/255,160.0f/255,16.0f/255, 1),
					new Color(248.0f/255,192.0f/255,24.0f/255, 1),
					new Color(129.0f/255,70.0f/255,40.0f/255, 1),
					new Color(248.0f/255,96.0f/255,144.0f/255, 1),
					new Color(8.0f/255,8.0f/255,6.0f/255, 1),
			}),
			new ColorSet("Grey-Scale", new Color[] {
					new Color(0, 0, 0, 1),
					new Color(36.0f/255, 36.0f/255, 36.0f/255, 1),
					new Color(72.0f/255, 72.0f/255, 72.0f/255, 1),
					new Color(108.0f/255, 108.0f/255, 108.0f/255, 1),
					new Color(144.0f/255, 144.0f/255, 144.0f/255, 1),
					new Color(180.0f/255, 180.0f/255, 180.0f/255, 1),
					new Color(216.0f/255, 216.0f/255, 216.0f/255, 1),
					new Color(1, 1, 1, 1),
			}),
			new ColorSet("Grass And Grapes", new Color[] {
					new Color(3.0f/255,55.0f/255,7.0f/255, 1),
					new Color(196.0f/255,173.0f/255,120.0f/255, 1),
					new Color(85.0f/255,67.0f/255,52.0f/255, 1),
					new Color(87.0f/255,20.0f/255,72.0f/255, 1),
					new Color(196.0f/255,211.0f/255,23.0f/255, 1),
					new Color(152.0f/255,39.0f/255,39.0f/255, 1),
					new Color(82.0f/255,114.0f/255,165.0f/255, 1),
					new Color(97.0f/255,153.0f/255,55.0f/255, 1),
			})
	};
	public static ColorSet[] EYE_COLORS = {
	new ColorSet("Realistic", new Color[] {
					new Color(78.0f/255,96.0f/255,163.0f/255, 1),
					new Color(176.0f/255,185.0f/255,217.0f/255, 1),
					new Color(62.0f/255,68.0f/255,66.0f/255, 1),
					new Color(102.0f/255,114.0f/255,78.0f/255, 1),
					new Color(123.0f/255,92.0f/255,51.0f/255, 1),
					new Color(104.0f/255,23.0f/255,17.0f/255, 1),
					new Color(77.0f/255,54.0f/255,35.0f/255, 1),
					new Color(159.0f/255,174.0f/255,112.0f/255, 1),
			}),
			new ColorSet("Human Rare", new Color[] {
					new Color(221.0f/255,179.0f/255,50.0f/255, 1),
					new Color(198.0f/255,193.0f/255,191.0f/255, 1),
					new Color(11.0f/255,11.0f/255,20.0f/255, 1),
					new Color(155.0f/255,29.0f/255,27.0f/255, 1),
					new Color(114.0f/255,79.0f/255,124.0f/255, 1),
					new Color(0,240.0f/255,241.0f/255, 1),
					new Color(234.0f/255,2.0f/255,245.0f/255, 1),
					new Color(253.0f/255,253.0f/255,253.0f/255, 1),
			}),
			new ColorSet("Grey-Scale", new Color[] {
					new Color(0, 0, 0, 1),
					new Color(36.0f/255, 36.0f/255, 36.0f/255, 1),
					new Color(72.0f/255, 72.0f/255, 72.0f/255, 1),
					new Color(108.0f/255, 108.0f/255, 108.0f/255, 1),
					new Color(144.0f/255, 144.0f/255, 144.0f/255, 1),
					new Color(180.0f/255, 180.0f/255, 180.0f/255, 1),
					new Color(216.0f/255, 216.0f/255, 216.0f/255, 1),
					new Color(1, 1, 1, 1),
			}),
			new ColorSet("Rainbow", new Color[] {
					new Color(247.0f/255,0,0, 1),
					new Color(247.0f/255,160.0f/255,0, 1),
					new Color(247.0f/255,247.0f/255,0, 1),
					new Color(0,124.0f/255,0, 1),
					new Color(0,0,247.0f/255, 1),
					new Color(73.0f/255,0,126.0f/255, 1),
					new Color(231.0f/255,126.0f/255,231.0f/255, 1),
					new Color(164.0f/255,164.0f/255,164.0f/255, 1),
			})
	};

	public static ColorSet SKIN_COLORS_IN_USE = new ColorSet("Skin Colors", null);
	public static ColorSet HAIR_COLORS_IN_USE = new ColorSet("Hair Colors", null);
	public static ColorSet EYE_COLORS_IN_USE = new ColorSet("Eye Colors", null);

	/**
	 * Used for percentage chance to perform an action or increment a stat
	 * If whatever value > this return value, perform the action
	 * @return random value
	 */
	public static int random0To99()
	{
		return Random.Range(0, 100);
	}

	/**
	 * Used for generating stats ranging from 0 to 100
	 * @return random value
	 */
	public static int random0To100()
	{
		return Random.Range(0, 101);
	}

	public static int nextInt(int range)
	{
		return Random.Range(0, range) + 1;
	}

	public static bool nextBoolean()
	{
		return Random.Range(0, 2) == 0;
	}

	/**
	 * Gives a randomly generated name for a character
	 * @param style represents the kind of name generation, meant to individualize different cultures
	 * @return randomly generated name
	 */
	public static string randomName(int style)
	{
		//TODO make this different depending on style
		return FantasyNames.getName();
	}

	/*
public static int numberOfLanguages()
{
	return LANGUAGES_IN_USE.Length;
}
	*/

	public static string newLocationName(int language)
	{
		//TODO maybe change this to differentiate names of people and nations
		return randomName(language);
	}

	/*
public static void useLanguage(List<Integer> langs)
{
	LANGUAGES_IN_USE = new NamingConvention[langs.size()];
	for (int q = 0; q < LANGUAGES_IN_USE.length; q++)
	{
		LANGUAGES_IN_USE[q] = LANGUAGES[langs.get(q)];
	}
	//TODO save this in database
}

	*/
	public static void useColors(List<int> hair, List<int> skin, List<int> eye)
	{
		for (int q = 0; q < hair.Count; q++)
		{
			HAIR_COLORS_IN_USE.addColors(HAIR_COLORS[hair[q]]);
		}
		for (int q = 0; q < skin.Count; q++)
		{
			SKIN_COLORS_IN_USE.addColors(SKIN_COLORS[skin[q]]);
		}
		for (int q = 0; q < eye.Count; q++)
		{
			EYE_COLORS_IN_USE.addColors(EYE_COLORS[eye[q]]);
		}
		//TODO save in database
	}

	public static int getRandomHairColor()
	{
		return nextInt(HAIR_COLORS_IN_USE.size());
	}
	public static int getRandomSkinColor()
	{
		return nextInt(SKIN_COLORS_IN_USE.size());
	}
	public static int getRandomEyeColor()
	{
		return nextInt(EYE_COLORS_IN_USE.size());
	}
}
