using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BattlegroundTileIndex
{

	private static Dictionary<WorldMapTile.WorldMapTileType, char[,]> maps;
	private static Dictionary<char, BattlegroundTile.BattlegroundTileType> tileMap;
	public static int TILE_DIMENSION = 20;

	public static void initialize()
	{
		maps = new Dictionary<WorldMapTile.WorldMapTileType, char[,]>();
		tileMap = new Dictionary<char, BattlegroundTile.BattlegroundTileType>();
		tileMap.Add('C', BattlegroundTile.BattlegroundTileType.CAVE);
		tileMap.Add('c', BattlegroundTile.BattlegroundTileType.CHEST);
		tileMap.Add('d', BattlegroundTile.BattlegroundTileType.DECK);
		tileMap.Add('D', BattlegroundTile.BattlegroundTileType.DEEP_WATER);
		tileMap.Add('o', BattlegroundTile.BattlegroundTileType.DOCK);
		tileMap.Add('f', BattlegroundTile.BattlegroundTileType.FLOOR);
		tileMap.Add('g', BattlegroundTile.BattlegroundTileType.GATE);
		tileMap.Add('G', BattlegroundTile.BattlegroundTileType.GRASS);
		tileMap.Add('H', BattlegroundTile.BattlegroundTileType.HOUSE);
		tileMap.Add('h', BattlegroundTile.BattlegroundTileType.HOUSE_DOOR);
		tileMap.Add('m', BattlegroundTile.BattlegroundTileType.MAGMA);
		tileMap.Add('M', BattlegroundTile.BattlegroundTileType.MOUNTAIN);
		tileMap.Add('P', BattlegroundTile.BattlegroundTileType.PEAK);
		tileMap.Add('p', BattlegroundTile.BattlegroundTileType.PILLAR);
		tileMap.Add('R', BattlegroundTile.BattlegroundTileType.ROAD);
		tileMap.Add('r', BattlegroundTile.BattlegroundTileType.RUBBLE);
		tileMap.Add('S', BattlegroundTile.BattlegroundTileType.SAND);
		tileMap.Add('s', BattlegroundTile.BattlegroundTileType.SHALLOW_WATER);
		tileMap.Add('N', BattlegroundTile.BattlegroundTileType.SNOW);
		tileMap.Add('I', BattlegroundTile.BattlegroundTileType.THICKET);
		tileMap.Add('t', BattlegroundTile.BattlegroundTileType.THRONE);
		tileMap.Add('T', BattlegroundTile.BattlegroundTileType.TREE);
		tileMap.Add('a', BattlegroundTile.BattlegroundTileType.WALL);
		tileMap.Add('r', BattlegroundTile.BattlegroundTileType.WARP_TILE);
		tileMap.Add('w', BattlegroundTile.BattlegroundTileType.WASTELAND);
		tileMap.Add('W', BattlegroundTile.BattlegroundTileType.WETLAND);

		initializeBattlegroundMaps(tileMap);
	}

	private static void initializeBattlegroundMaps(Dictionary<char, BattlegroundTile.BattlegroundTileType> tileMap)
	{
		//TODO fix this map and make the others
		char[,] plain = {
			{'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G'},
			{'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G'},
			{'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G'},
			{'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G'},
			{'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G'},
			{'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G'},
			{'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G'},
			{'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G'},
			{'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G'},
			{'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G'},
			{'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G'},
			{'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G'},
			{'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G'},
			{'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G'},
			{'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G'},
			{'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G'},
			{'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G'},
			{'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G'},
			{'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G'},
			{'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G'},
		};
		maps.Add(WorldMapTile.WorldMapTileType.PLAIN, plain);


		//TODO When you're finished adding all the tiles, use the commented code below to make
		//sure they're all the right size
		Dictionary<WorldMapTile.WorldMapTileType, char[,]>.KeyCollection vals = maps.Keys;
		foreach (WorldMapTile.WorldMapTileType m in vals)
		{
			char[,] c = maps[m];
			if (c.Length != 20 || c.Length != 20)
			{
				throw new Exception("The map for " + m.getName() +
						" is " + c.Length + " by " + c.Length);
			}
		}
	}

	public static BattlegroundTile[][] mapToUse(WorldMapTile wmTile)
	{
		//Get a copy of the map of tile types (NOT the original)
		char[,] m = (char[,])maps[wmTile.getType()].Clone();
		if (wmTile.getGroupPresent() is Ship)
		{
			//TODO Replace the appropriate tiles with DECK (based on ship size and orientation)
		}
		else if (wmTile.getBuilding() != null)
		{
			//TODO Replace the appropriate tiles with those corresponding to the building
		}
		BattlegroundTile[][] ret = new BattlegroundTile[m.Length][];
		for (int q = 0; q < ret.Length; q++)
		{
			ret[q] = new BattlegroundTile[m.Length];
		}
		for (int q = 0; q < m.Length; q++)
		{
			for (int w = 0; w < m.Length; w++)
			{
				ret[q][w] = new BattlegroundTile(tileMap[m[q, w]]);
			}
		}
		return ret;
	}
}
