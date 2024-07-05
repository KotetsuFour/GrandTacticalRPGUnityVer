using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibNoise;
using System.IO;

public class WorldMap
{

	private WorldMapTile[][] map;
	public static int SQRT_OF_MAP_SIZE = 64;

	/**
	 * Generate world map for testing
	 */
	public WorldMap()
	{
		map = new WorldMapTile[SQRT_OF_MAP_SIZE][];
		for (int q = 0; q < map.Length; q++)
		{
			map[q] = new WorldMapTile[SQRT_OF_MAP_SIZE];
		}
		for (int q = 0; q < map.Length; q++)
		{
			for (int w = 0; w < map[q].Length; w++)
			{
				map[q][w] = new WorldMapTile(WorldMapTile.WorldMapTileType.PLAIN, 100, 0);
			}
		}
	}

	public void generateTerrain()
    {
		Perlin heatMap = new Perlin();
		Perlin moistureMap = new Perlin();
		Perlin heightMap = new Perlin();
		Perlin magicTypeMap = new Perlin();
		Perlin magicStrengthMap = new Perlin();
		heatMap.Seed = 108439482;
		Debug.Log(heatMap.Seed);
		//set frequency, persistence (between 0 and 1), lacunarity (odd number), and octave count
		setPerlinSettings(new Perlin[] { heatMap, moistureMap, heightMap, magicTypeMap, magicStrengthMap },
			0.05, 0.5, 3, 2);

		StreamWriter output = new StreamWriter("Assets/noise.txt");

		for (int q = 0; q < map.Length; q++)
        {
			for (int w = 0; w < map[q].Length; w++)
            {
				double heat = heatMap.GetValue(q, w, 1);
				double moisture = moistureMap.GetValue(q, w, 0);
				double height = heightMap.GetValue(q, w, 0);
				double magic = magicTypeMap.GetValue(q, w, 0);
				double magicStrength = magicStrengthMap.GetValue(q, w, 0);

				int magicType = magic > 0.67 ? 0 : magic > 0.33 ? 1 : 2;

				output.Write($"{(float)heat}");

				if (height < -0.2)
				{
					map[q][w] = new WorldMapTile(WorldMapTile.WorldMapTileType.DEEP_WATER, Mathf.RoundToInt((float)magicStrength * 100), magicType);
				}
				else if (height < -0.1)
                {
					if (heat < -0.5)
                    {
						map[q][w] = new WorldMapTile(WorldMapTile.WorldMapTileType.GLACIER, Mathf.RoundToInt((float)magicStrength * 100), magicType);
					}
					else
                    {
						map[q][w] = new WorldMapTile(WorldMapTile.WorldMapTileType.SHALLOW_WATER, Mathf.RoundToInt((float)magicStrength * 100), magicType);
					}
				}
				else if (height < 0.7)
                {
					if (heat < -0.5)
                    {
						map[q][w] = new WorldMapTile(WorldMapTile.WorldMapTileType.SNOWY_PLAIN, Mathf.RoundToInt((float)magicStrength * 100), magicType);
					}
					else if (heat < 0.7)
                    {
						if (moisture < -0.5)
                        {
							map[q][w] = new WorldMapTile(WorldMapTile.WorldMapTileType.DESERT, Mathf.RoundToInt((float)magicStrength * 100), magicType);
						}
						else if (moisture < 0.3)
                        {
							map[q][w] = new WorldMapTile(WorldMapTile.WorldMapTileType.PLAIN, Mathf.RoundToInt((float)magicStrength * 100), magicType);
						}
						else if (moisture < 0.8)
                        {
							map[q][w] = new WorldMapTile(WorldMapTile.WorldMapTileType.FOREST, Mathf.RoundToInt((float)magicStrength * 100), magicType);
						}
						else
                        {
							map[q][w] = new WorldMapTile(WorldMapTile.WorldMapTileType.DENSE_FOREST, Mathf.RoundToInt((float)magicStrength * 100), magicType);
						}
					}
					else
                    {
						if (moisture < 0.2)
                        {
							map[q][w] = new WorldMapTile(WorldMapTile.WorldMapTileType.DESERT, Mathf.RoundToInt((float)magicStrength * 100), magicType);
						} else
                        {
							map[q][w] = new WorldMapTile(WorldMapTile.WorldMapTileType.SWAMP, Mathf.RoundToInt((float)magicStrength * 100), magicType);
						}
					}
				}
				else if (height < 0.9)
                {
					if (heat < -0.5)
                    {
						map[q][w] = new WorldMapTile(WorldMapTile.WorldMapTileType.SNOWY_MOUNTAIN, Mathf.RoundToInt((float)magicStrength * 100), magicType);
					}
					else
                    {
						map[q][w] = new WorldMapTile(WorldMapTile.WorldMapTileType.MOUNTAIN, Mathf.RoundToInt((float)magicStrength * 100), magicType);
					}
				}
				else
                {
					map[q][w] = new WorldMapTile(WorldMapTile.WorldMapTileType.SNOWY_MOUNTAIN, Mathf.RoundToInt((float)magicStrength * 100), magicType);
				}
			}
			output.Write("\n");
		}
		output.Close();
	}

	private void setPerlinSettings(Perlin[] perlins, double freq, double persist, double lacun,
		int octave)
    {
		foreach (Perlin p in perlins)
        {
			p.Frequency = freq;
			p.Persistence = persist;
			p.Lacunarity = lacun;
			p.OctaveCount = octave;
        }
    }
	public WorldMapTile at(int x, int y)
	{
		return map[x][y];
	}

	public WorldMapTile getNearestUnoccupiedTile(int x, int y)
	{
		//TODO
		return null;
	}

	public WorldMapTile getAdjacentUnoccupiedTile(int x, int y)
	{
		if (x != 0 && map[x - 1][y].isVacant())
		{
			return map[x - 1][y];
		}
		if (x < map.Length - 1 && map[x + 1][y].isVacant())
		{
			return map[x + 1][y];
		}
		if (y != 0 && map[x][y - 1].isVacant())
		{
			return map[x][y - 1];
		}
		if (y < map[x].Length - 1 && map[x][y + 1].isVacant())
		{
			return map[x][y + 1];
		}
		return null;
	}

	public List<WorldMapTile> getAllAdjacentTiles(int x, int y)
	{
		List<WorldMapTile> ret = new List<WorldMapTile>(4);
		if (x != 0)
		{
			ret.Add(map[x - 1][y]);
		}
		if (x < map.Length - 1)
		{
			ret.Add(map[x + 1][y]);
		}
		if (y != 0)
		{
			ret.Add(map[x][y - 1]);
		}
		if (y < map[x].Length - 1)
		{
			ret.Add(map[x][y + 1]);
		}
		return ret;
	}

	public List<WorldMapTile> getAdjacentTilesWithAllies(WMTileOccupant group, int x, int y)
	{
		//TODO don't allow units above deep water to interact. Too many ways to exploit that
		List<WorldMapTile> adjacent = getAllAdjacentTiles(x, y);
		List<WorldMapTile> ret = new List<WorldMapTile>(4);
		for (int q = 0; q < adjacent.Count; q++)
		{
			WorldMapTile adj = adjacent[q];
			if (!(adj.isVacant())
					&& adj.getGroupPresent() is UnitGroup
					&& adj.getGroupPresent().getAffiliation() == group.getAffiliation()
					&& adj.getGroupPresent() != group)
			{
				ret.Add(adj);
			}
		}
		return ret;
	}
	public List<WorldMapTile> getAdjacentTilesWithUnassignedShips(UnitGroup group, int x, int y)
	{
		List<WorldMapTile> adjacent = getAllAdjacentTiles(x, y);
		List<WorldMapTile> ret = new List<WorldMapTile>(4);
		for (int q = 0; q < adjacent.Count; q++)
		{
			WorldMapTile adj = adjacent[q];
			if (!(adj.isVacant())
					&& adj.getGroupPresent() is Ship)
			{
				Nation aff = adj.getGroupPresent().getAffiliation();
				if (aff == null || aff == group.getAffiliation())
				{
					ret.Add(adj);
				}
			}
		}
		return ret;
	}

	public List<WorldMapTile> getAllTraversableAdjacentTiles(WMTileOccupant group, int x, int y)
	{
		List<WorldMapTile> adj = getAllAdjacentTiles(x, y);
		List<WorldMapTile> ret = new List<WorldMapTile>();
		if (group is UnitGroup)
		{
			for (int q = 0; q < adj.Count; q++)
			{
				WorldMapTile tile = adj[q];
				if (tile.isVacant()
						//This method is used to determine when a unit can split,
						//so whether or not it is a flying unit, deep water tiles
						//don't count
						&& tile.getBattle() == null
						&& tile.getType().moveCostOnFoot() < int.MaxValue
						&& (tile.getAffiliation() == null
						|| tile.getAffiliation() == group.getAffiliation()
						|| group.getAffiliation().isAtWarWith(tile.getAffiliation())
						|| group.getAffiliation().isAlliedWith(tile.getAffiliation())))
				{
					ret.Add(tile);
				}
			}
		}
		return ret;
	}

	public List<WorldMapTile> getAdjacentTilesWithAttackableEnemies(WMTileOccupant group, int x, int y)
	{
		List<WorldMapTile> adjacent = getAllAdjacentTiles(x, y);
		List<WorldMapTile> ret = new List<WorldMapTile>(4);
		for (int q = 0; q < adjacent.Count; q++)
		{
			WorldMapTile adj = adjacent[q];
			if (!(adj.isVacant())
					&& group.getAffiliation().isAtWarWith(adj.getGroupPresent().getAffiliation())
					&& (adj.getGroupPresent() is UnitGroup
								|| group is Ship
								|| (group is UnitGroup && ((UnitGroup)group).canFly())))
			{
				ret.Add(adj);
			}
			else if (adj.getBuilding() is Defendable
							  && adj.getBattle() == null
							  && ((Defendable)adj.getBuilding()).getAssignedGroup() != null
							  && group.getAffiliation().isAtWarWith(((Defendable)adj.getBuilding()).getAssignedGroup().getAffiliation()))
			{
				ret.Add(adj);
			}
		}
		return ret;
	}

	public List<WorldMapTile> getTilesAttackableWithShip(Ship group, int destX, int destY)
	{
		// TODO Auto-generated method stub
		return null;
	}
}
