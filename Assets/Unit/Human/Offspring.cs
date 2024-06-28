using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Offspring : Human
{

	private Human mother;
	private Human father;
	private string displayName;

	private Offspring(string name, bool gender, int[] maxHPs, int magic, int skill, int reflex, int awareness,
			int resistance, int movement, int leadership, int[] maxHPGrowths, int magGrowth, int sklGrowth,
			int rfxGrowth, int awrGrowth, int resGrowth, int age, int[] personality, int[] appearance,
			Interest[] interests, Interest[] disinterests, Demeanor demeanor, CombatTrait valuedTrait, CityState home,
			Human mother, Human father)
			: base(name, gender, maxHPs, magic, skill, reflex, awareness, resistance, movement, leadership, maxHPGrowths, magGrowth,
				sklGrowth, rfxGrowth, awrGrowth, resGrowth, age, personality, appearance, interests, disinterests, demeanor,
				valuedTrait, home)
	{
		this.mother = mother;
		this.father = father;
		string dn = name + " of " + home.getName() + ", ";
		if (gender)
		{
			dn += ("daughter of ");
		}
		else
		{
			dn += ("son of ");
		}
		dn += (father.getName() + " and " + mother.getName());
		this.displayName = dn;
	}

	public Human getMother()
	{
		return mother;
	}

	public Human getFather()
	{
		return father;
	}

	public int getRelationshipWithMother()
	{
		//TODO
		return 0;
	}

	public int getRelationshipWithFather()
	{
		//TODO
		return 0;
	}

	public static Offspring getChild(Human parent1, Human parent2)
	{
		Human mum;
		Human dad;
		if (parent1.getGender())
		{
			mum = parent1;
			dad = parent2;
		}
		else
		{
			dad = parent1;
			mum = parent2;
		}

		//TODO create child's stats
		return null;
	}
}
