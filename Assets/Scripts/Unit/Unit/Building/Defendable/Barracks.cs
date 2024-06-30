using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Barracks : Defendable
{

	//TODO decide actual values
	public static int[] materialsNeededForConstruction = { };
	public static int MAX_INTEGRITY = 10;
	public static int DURABILITY = 10;
	public static int RESISTANCE = 10;

	public Barracks(string name, Human owner, WorldMapTile location)
			: base(name, MAX_INTEGRITY, DURABILITY, RESISTANCE, owner, location)
	{
		// TODO Auto-generated constructor stub
	}


	public override string getType()
	{
		return Building.BARRACKS;
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

}
