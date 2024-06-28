using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ArtificialHumanIndex
{

	private static List<List<ArtificialHumanTemplate>> index;

	public static int CLONE = 0;
	public static int TITAN = 1;

	public static void initialize()
	{
		index = new List<List<ArtificialHumanTemplate>>();
		for (int q = 0; q < 2; q++)
		{ //One for clones, one for titans, which will be added later
			index.Add(new List<ArtificialHumanTemplate>());
		}
		addDefaultTitans();
	}

	private static void addTemplate(int type, ArtificialHumanTemplate t)
	{
		t.setSpecificTemplateIndex(index[type].Count);
		index[type].Add(t);
	}

	public static ArtificialHumanTemplate addCloneTemplate(Human h)
	{
		ArtificialHumanTemplate ret = new ArtificialHumanTemplate(h);
		addTemplate(CLONE, ret);
		return ret;
	}

	public static void addDefaultTitans()
	{
		//TODO add this in a later update
	}
}
