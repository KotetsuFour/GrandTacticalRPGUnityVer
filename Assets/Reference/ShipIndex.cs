using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ShipIndex
{

	private static List<Ship> ships;

	public Ship getShip(int idx)
	{
		try
		{
			return ships[idx];
		}
		catch (Exception e)
		{
			return null;
		}
	}

	public static void initialize()
	{
		ships = new List<Ship>();
		addDefaultShips();
	}

	public static void addShip(Ship s)
	{
		s.setBluePrint(ships.Count);
		ships.Add(s);
	}

	public static void addDefaultShips()
	{
		//TODO make actual default ships
	}
}
