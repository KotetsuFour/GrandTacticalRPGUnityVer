using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BattleGround
{

	private WorldMapTile[] location;
	private bool isHorizontal;
	private List<UnitGroup> combatants;
	private Dictionary<BFTileOccupant, int[]> getCoordsByOccupant;
	private int barrierHealth;
	private Ship[] ships;
	private BattlegroundTile[][] map;

	public BattleGround(WorldMapTile[] location, bool isHorizontal,
			WMTileOccupant[] engagingParties)
	{
		this.getCoordsByOccupant = new Dictionary<BFTileOccupant, int[]>();
		this.location = location;
		for (int q = 0; q < location.Length; q++)
		{
			location[q].setBattle(this);
		}
		this.isHorizontal = isHorizontal;
		this.combatants = new List<UnitGroup>(4);
		this.ships = new Ship[2];
		//Create map to make placing units easier
		getMap();
		//TODO place inanimate objects
		if (engagingParties[0] is Ship) {
			addShip(0, (Ship)engagingParties[0]);
		} else if (engagingParties[0] is UnitGroup) {
			UnitGroup g = (UnitGroup)engagingParties[0];
			addUnitGroup(0, g);
			if (g.getPrisoners() != null)
			{
				addPrisonerGroup(0, g.getPrisoners());
			}
		}
		if (engagingParties[1] is Ship) {
			addShip(1, (Ship)engagingParties[1]);
		} else if (engagingParties[1] is UnitGroup) {
			UnitGroup g = (UnitGroup)engagingParties[1];
			addUnitGroup(1, g);
			if (g.getPrisoners() != null)
			{
				addPrisonerGroup(1, g.getPrisoners());
			}
		}
		//Close battleground after we've placed all units
		exitBattleground();
		//Increment battles in this war
		combatants[0].getAffiliation().getCurrentWarWith(combatants[1].getAffiliation()).incrementBattles();
		//TODO if this battle is happening at a special location, record its beginning in
		//history
	}

	private void addShip(int tile, Ship s)
	{
		ships[tile] = s;
		//We add the assigned group separate from the reserves and prisoners so we can
		//place the group's members on their starting tiles
		if (s.assignedGroup() != null)
		{
			addUnitGroup(tile, s.assignedGroup());
		}
		List<UnitGroup> undeployed = s.getAllUndeployedPassengersAndPrisoners();
		for (int q = 0; q < undeployed.Count; q++)
		{
			UnitGroup group = undeployed[q];
			group.setBattle(this);
		}
	}

	private void addUnitGroup(int tile, UnitGroup group)
	{
		if (group == null)
		{
			throw new Exception("Cannot add a null group to combatants");
		}
		//TODO autoEquip all (for AI groups, take from convoy if necessary/possible)
		//TODO set strategy
		group.setBattgroundPositions();
		combatants.Add(group);
		group.setBattle(this);
		for (int q = 0; q < group.getMembers().Count; q++)
		{
			addUnit(tile, group.getMembers()[q]);
		}
	}

	private void addUnit(int tile, Unit u)
	{
		if (u == null)
		{
			throw new Exception("Cannot add a null unit to battleground");
		}
		//TODO if the spot is occupied, pick nearest available one
		int[] coords = { u.getBattlePositionX(), u.getBattlePositionY() };
		if (tile == 1)
		{
			if (isHorizontal)
			{
				coords[0] = (BattlegroundTileIndex.TILE_DIMENSION * 2) - coords[0];
			}
			else
			{
				int temp = coords[0];
				coords[0] = coords[1];
				coords[1] = (BattlegroundTileIndex.TILE_DIMENSION * 2) - temp;
			}
		}
		else if (!isHorizontal)
		{
			int temp = coords[0];
			coords[0] = BattlegroundTileIndex.TILE_DIMENSION - coords[1];
			coords[1] = temp;
		}
		coords = closestUnoccupiedTile(coords);
		//TODO translate coordinates to correspond to the appropriate tile
		getMap()[coords[0]][coords[1]].placeUnit(u);
		getCoordsByOccupant.Add(u, coords);
	}

	private void addPrisonerGroup(int tile, UnitGroup prisoners)
	{
		if (prisoners == null)
		{
			throw new Exception("Cannot add a null group to combatants");
		}
		combatants.Add(prisoners);
		prisoners.setBattle(this);
		for (int q = 0; q < prisoners.getMembers().Count; q++)
		{
			addPrisoner(tile, prisoners.getMembers()[q]);
		}
	}

	private void addPrisoner(int tile, Unit prisoner)
	{
		if (prisoner == null)
		{
			throw new Exception("Cannot add a null unit to battleground");
		}
		//TODO pick the nearest available prisoner spawn tile
		int[] coords = { 0, 0 };
		//TODO translate coordinates to correspond to the appropriate tile
		getCoordsByOccupant.Add(prisoner, coords);
	}

	private int[] closestUnoccupiedTile(int[] coords)
	{
		//TODO
		if (getMap()[coords[0]][coords[1]].getUnit() != null)
		{
			return new int[] { 0, 0 };
		}
		return coords;
	}

	public BattlegroundTile[][] getMap()
	{
		if (map != null)
		{
			return map;
		}
		//These halves of the map already include any ship or building that might be on the tile
		BattlegroundTile[][] firstHalf = BattlegroundTileIndex.mapToUse(location[0]);
		BattlegroundTile[][] secondHalf = BattlegroundTileIndex.mapToUse(location[1]);
		if (isHorizontal)
		{
			map = new BattlegroundTile[BattlegroundTileIndex.TILE_DIMENSION * location.Length][BattlegroundTileIndex.TILE_DIMENSION]; //That is, 40x20
			for (int q = 0; q < firstHalf.Length; q++)
			{
				for (int w = 0; w < BattlegroundTileIndex.TILE_DIMENSION; w++)
				{
					map[q][w] = firstHalf[q][w];
				}
			}
			for (int q = firstHalf.Length; q < firstHalf.Length + secondHalf.Length; q++)
			{
				for (int w = 0; w < BattlegroundTileIndex.TILE_DIMENSION; w++)
				{
					map[q][w] = secondHalf[q - firstHalf.Length][w];
				}
			}
		}
		else
		{
			map = new BattlegroundTile[BattlegroundTileIndex.TILE_DIMENSION][BattlegroundTileIndex.TILE_DIMENSION * location.Length]; //That is, 20x40
			for (int q = 0; q < BattlegroundTileIndex.TILE_DIMENSION; q++)
			{
				for (int w = 0; w < firstHalf.Length; w++)
				{
					map[q][w] = firstHalf[q][w];
				}
			}
			for (int q = 0; q < BattlegroundTileIndex.TILE_DIMENSION; q++)
			{
				for (int w = firstHalf.Length; w < firstHalf.Length + secondHalf.Length; w++)
				{
					map[q][w] = secondHalf[q][w - firstHalf.Length];
				}
			}
		}
		Dictionary<BFTileOccupant, int[]>.KeyCollection occupants = getCoordsByOccupant.Keys;
		foreach (BFTileOccupant o in occupants)
		{
			//TODO if space is not empty, find the nearest spot
			int[] coords = getCoordsByOccupant[o];
			map[coords[0]][coords[1]].placeOccupant(o);
		}
		return map;
	}

	/**
	 * To save memory, set map to null when the battleground is not currently being
	 * accessed
	 */
	public void exitBattleground()
	{
		map = null;
	}

	public List<UnitGroup> getUnitGroupsBelongingToNation(Nation n)
	{
		List<UnitGroup> ret = new List<UnitGroup>();
		for (int q = 0; q < combatants.Count; q++)
		{
			UnitGroup ug = combatants[q];
			if (ug.getAffiliation() == n)
			{
				ret.Add(ug);
			}
		}
		return ret;
	}

	public void moveUnit(Unit unit, int[] to,
			BattlegroundTile[][] map //Require the use of the map just to ensure the map is
									 //currently constructed. I might decide this is unnecessary
									 //once all battleground functions and AI are done
			)
	{
		GameUnit alreadyHere = map[to[0]][to[1]].getUnit();
		if (alreadyHere != null && alreadyHere != unit)
		{
			throw new Exception("There is already someone here");
		}
		int[] currentCoords = getCoordsByOccupant[unit];
		map[currentCoords[0]][currentCoords[1]].removeUnit();
		map[to[0]][to[1]].placeUnit(unit);
		getCoordsByOccupant.Add(unit, to);
	}

	public void removeOccupant(BFTileOccupant o)
	{
		//Removal from unit group is already done in death sequence
		int[] coords = getCoordsByOccupant[o];
		if (o is Unit) {
			Unit u = (Unit)o;
			if (map != null)
			{
				map[coords[0]][coords[1]].removeUnit();
			}
			if (u.getPassenger() != null)
			{
				BFTileOccupant pass = u.getPassenger();
				getCoordsByOccupant.Add(pass, coords);
				if (map != null)
				{
					map[coords[0]][coords[1]].placeOccupant(pass);
				}
			}
		}
		getCoordsByOccupant.Remove(o);
	}

	public int[] getCoordsOfUnit(BFTileOccupant o)
	{
		return getCoordsByOccupant[o];
	}

	public BattlegroundTile getTileAtCoords(int[] coords)
	{
		return map[coords[0]][coords[1]];
	}

	public int[] getDimensions()
	{
		if (isHorizontal)
		{
			return new int[] { BattlegroundTileIndex.TILE_DIMENSION * 2, BattlegroundTileIndex.TILE_DIMENSION };
		}
		return new int[] { BattlegroundTileIndex.TILE_DIMENSION, BattlegroundTileIndex.TILE_DIMENSION * 2 };
	}

	public bool isActive()
	{
		//TODO debug
		Nation side1 = null;
		int side1Quant = 0;
		Nation side2 = null;
		int side2Quant = 0;
		for (int q = 0; q < combatants.Count; q++)
		{
			UnitGroup group = combatants[q];
			if (group.getMembers().Count == 0)
			{
				combatants.Remove(q);
				q--;
				continue;
			}
			if (side1 == null)
			{
				side1 = group.getAffiliation();
				side1Quant++;
			}
			else if (group.getAffiliation().isAlliedWith(side1))
			{
				side1Quant++;
			}
			else if (side2 == null)
			{
				side2 = group.getAffiliation();
				side2Quant++;
			}
			else if (group.getAffiliation().isAlliedWith(side2))
			{
				side2Quant++;
			}
			else
			{
				throw new Exception("An illegal participant entered the battlefield\n"
						+ group.getLeader().getDisplayName() + ", representing " + group.getAffiliation().getName());
			}
		}

		if (side1Quant > 2 || side2Quant > 2)
		{
			throw new Exception("Rethink your logic for joining a battle. One side\n"
					+ "was able to have more than 2 groups helping it");
		}

		return side1Quant > 0 && side2Quant > 0;
	}

	public void endBattle()
	{
		location[0].setBattle(null);
		location[1].setBattle(null);

		for (int q = 0; q < combatants.Count; q++)
		{
			location[q].sendHere(combatants[q]);
		}
	}

	public bool canEnter(UnitGroup group)
	{
		int friends = 0;
		for (int q = 0; q < combatants.Count; q++)
		{
			if (group.getAffiliation().isAlliedWith(combatants[q].getAffiliation()))
			{
				friends++;
				if (friends > 1)
				{
					return false;
				}
			}
		}
		return barrierHealth <= 0;
	}

	public bool canSurrender(Nation n)
	{
		int friends = 0;
		int idxOfAlly = -1;
		for (int q = 0; q < combatants.Count; q++)
		{
			if (n.isAlliedWith(combatants[q].getAffiliation()))
			{
				friends++;
				//If there is more than just one allied group, you cannot surrender
				if (friends > 1)
				{
					return false;
				}
				idxOfAlly = q;
			}
		}

		//Can surrender if you have one allied group that you directly control
		return idxOfAlly != -1 && combatants[idxOfAlly].getAffiliation() == n;
	}

	public List<UnitGroup> getCombatants()
	{
		for (int q = 0; q < combatants.Count; q++)
		{
			if (combatants[q].getMembers().Count == 0)
			{
				combatants.RemoveAt(q);
				q--;
			}
		}
		return combatants;
	}

	public List<BattlegroundTile> getAllAdjacentTiles(int x, int y)
	{
		List<BattlegroundTile> ret = new List<BattlegroundTile>(4);
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

	public List<BattlegroundTile> getAlliedTiles(Unit u, int x, int y)
	{
		List<BattlegroundTile> adjacent = getAllAdjacentTiles(x, y);
		List<BattlegroundTile> ret = new List<BattlegroundTile>(4);
		for (int q = 0; q < adjacent.Count; q++)
		{
			BattlegroundTile adj = adjacent[q];
			if (!(adj.isVacant())
					&& adj.getUnit().getAffiliation() == u.getAffiliation()
					&& adj.getUnit() != u)
			{
				ret.Add(adj);
			}
		}
		return ret;
	}

	public List<BattlegroundTile> getAdjacentTilesWithCarriableAllies(Unit u, int x, int y)
	{
		if (!(u.canCarryUnit()) || u.getPassenger() != null)
		{
			return new List<BattlegroundTile>();
		}
		List<BattlegroundTile> adjacent = getAllAdjacentTiles(x, y);
		List<BattlegroundTile> ret = new List<BattlegroundTile>(4);
		for (int q = 0; q < adjacent.Count; q++)
		{
			BattlegroundTile adj = adjacent[q];
			if (!(adj.isVacant())
					&& adj.getUnit().getAffiliation() == u.getAffiliation()
					&& adj.getUnit() != u
					&& adj.getUnit().canBeCarried())
			{
				ret.Add(adj);
			}
		}
		return ret;
	}

	public List<BattlegroundTile> getAdjacentAvailableTiles(Unit u, int x, int y)
	{
		List<BattlegroundTile> adjacent = getAllAdjacentTiles(x, y);
		List<BattlegroundTile> ret = new List<BattlegroundTile>(4);
		for (int q = 0; q < adjacent.Count; q++)
		{
			BattlegroundTile adj = adjacent[q];
			if (adj.isVacant() || adj.getUnit() == u)
			{
				ret.Add(adj);
			}
		}
		return ret;
	}

}