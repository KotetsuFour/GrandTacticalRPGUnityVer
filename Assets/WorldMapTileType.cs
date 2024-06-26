using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapTileType
{
	public static WorldMapTileType PLAIN = new WorldMapTileType("Plain", 1, 1, 6, /*Color.LAWNGREEN*/Color.black);
	public static WorldMapTileType DESERT = new WorldMapTileType("Desert", 2, 0.2, 7, /*Color.YELLOW */ Color.black);
	public static WorldMapTileType FOREST = new WorldMapTileType("Forest", 2, 7, 6, /*Color.FORESTGREEN*/Color.black);
	public static WorldMapTileType DENSE_FOREST = new WorldMapTileType("Dense Forest", 5, 0.5, 4, /*Color.DARKGREEN*/Color.black);
	public static WorldMapTileType MOUNTAIN = new WorldMapTileType("Mountain", 4, 5, 10, /*Color.MEDIUMPURPLE*/Color.black);
	public static WorldMapTileType SHALLOW_WATER = new WorldMapTileType("Shallow Water", 5, 0.3, 1, /*Color.LIGHTBLUE*/Color.black);
	public static WorldMapTileType DEEP_WATER = new WorldMapTileType("Deep Water", int.MaxValue, 0, 0, /*Color.BLUE*/Color.black);
	public static WorldMapTileType SNOWY_PLAIN = new WorldMapTileType("Snowy Plain", 1, 0.3, 5, /*Color.WHITE*/Color.black);
	public static WorldMapTileType SNOWY_MOUNTAIN = new WorldMapTileType("Snowy Mountain", 4, 0.3, 8, /*Color.CADETBLUE*/Color.black);
	public static WorldMapTileType SWAMP = new WorldMapTileType("Swamp", 4, 0.5, 4, /*Color.DARKOLIVEGREEN*/Color.black);
	public static WorldMapTileType WASTELAND = new WorldMapTileType("Wasteland", 1, 0, 5, /*Color.TAN*/Color.black);
	public static WorldMapTileType GLACIER = new WorldMapTileType("Glacier", 2, 0.2, 2, /*Color.ALICEBLUE*/Color.black);


	private string name;
	private int moveCost;
	private double proliferability;
	private int minability;
	private Color displayColor;
	private WorldMapTileType(string name, int moveCost, double proliferability,
			int minability, Color displayColor)
	{
		this.name = name;
		this.moveCost = moveCost;
		//It is easier to grow certain animals and plants depending on the climate
		//0 means nothing can be grown. Otherwise...
		//Multiple of 2 means temperate, multiple of 3 is cold, 5 is hot, 7 is dry,
		//11 is wet, 13 is elevated, 17 is dark
		this.proliferability = proliferability;
		this.minability = minability;
		this.displayColor = displayColor;
	}

	public string getName()
	{
		return name;
	}

	public int moveCostOnFoot()
	{
		return moveCost;
	}

	public int getMinability()
	{
		return minability;
	}
	public double getProliferability()
	{
		return proliferability;
	}
	public Color getDisplayColor()
	{
		return displayColor;
	}
}
