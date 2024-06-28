using System.Collections.Generic;
using UnityEngine;

public class UnitGroup : WMTileOccupant
{

	private List<Unit> members;
	private WorldMapTile location;
	private BattleGround battle;
	private int[] generalObjective; //I might not need this, depending on how the AI works
	private int[] battleGroundObjective;
	private bool groupIsAIControlled;
	private bool customPositions;
	private Assignable assignedThing;
	private UnitGroup prisoners;

	public static int CAPACITY = 20;
	public static int[][] DEFAULT_BATTLE_POSITIONS = {
			new int[] {12, 8}, new int[] {12, 11}, new int[] {11, 6}, new int[] {11, 13}, new int[] {10, 4}, new int[] {10, 15}, new int[] {9, 6}, new int[] {9, 13}, new int[] {8, 8}, new int[] {8, 11},
			new int[] {10, 8}, new int[] {10, 11}, new int[] {13, 4}, new int[] {13, 15}, new int[] {11, 2}, new int[] {11, 17}, new int[] {8, 2}, new int[] {8, 17}, new int[] {8, 4}, new int[] {8, 15}
			};
	public static int[][] DEFAULT_SMALL_SHIP_POSITIONS = {
			new int[] {10, 9}, new int[] {10, 10}, new int[] {9, 8}, new int[] {9, 11}, new int[] {11, 8}, new int[] {11, 11}, new int[] {12, 9}, new int[] {12, 10}, new int[] {10, 8}, new int[] {10, 11},
			new int[] {8, 9}, new int[] {8, 10}, new int[] {7, 8}, new int[] {7, 11}, new int[] {8, 8}, new int[] {8, 11}, new int[] {6, 9}, new int[] {6, 10}, new int[] {13, 8}, new int[] {13, 11}
	};
	public static int[][] DEFAULT_MEDIUM_SHIP_POSITIONS = {
			new int[] {11, 9}, new int[] {11, 10}, new int[] {10, 8}, new int[] {10, 11}, new int[] {9, 7}, new int[] {9, 12}, new int[] {11, 7}, new int[] {11, 12}, new int[] {9, 9}, new int[] {9, 10},
			new int[] {12, 8}, new int[] {12, 11}, new int[] {8, 8}, new int[] {8, 11}, new int[] {7, 7}, new int[] {7, 12}, new int[] {13, 9}, new int[] {13, 10}, new int[] {7, 9}, new int[] {7, 10}
	};
	public static int[][] DEFAULT_LARGE_SHIP_POSITIONS = {
			new int[] {13, 9}, new int[] {13, 10}, new int[] {11, 7}, new int[] {11, 12}, new int[] {9, 7}, new int[] {9, 12}, new int[] {10, 9}, new int[] {10, 10}, new int[] {12, 6}, new int[] {12, 13},
			new int[] {7, 8}, new int[] {7, 11}, new int[] {8, 6}, new int[] {8, 13}, new int[] {13, 7}, new int[] {13, 12}, new int[] {15, 7}, new int[] {15, 12}, {5, 7}, {5, 12}
	};

	public UnitGroup(Unit firstMember)
	{
		members = new List<Unit>(CAPACITY);
		members.Add(firstMember); //Ensures that the group is never empty
		firstMember.assignGroup(this);
		//Initializing location isn't necessary
		//Same with battle and objectives

		//Groups that are not under the player's control are AI-controlled
		groupIsAIControlled = firstMember.getAffiliation() != GeneralGameplayManager.getPlayerNation();

		//Add yourself to the nation's unitgroup list
		firstMember.getAffiliation().getUnitGroups().Add(this);
	}

	public UnitGroup(List<Human> u)
	{
		members = new List<Unit>(CAPACITY);
		members.Add(u[0]);
		members[0].assignGroup(this);
		for (int q = 1; q < u.Count; q++)
		{
			add(u[q]);
		}
		//Groups that are not under the player's control are AI-controlled
		isAIControlled = u[0].getAffiliation() != GeneralGameplayManager.getPlayerNation();

		//Add yourself to the nation's unitgroup list
		u[0].getAffiliation().getUnitGroups().Add(this);

	}

	public int getLeadershipBonus(Unit unit)
	{
		if (members[0] == unit)
		{
			return 0;
		}
		return members[0].getLeadership() * 5;
	}

	public bool containsUnit(Unit u)
	{
		return members.Contains(u);
	}

	public WorldMapTile getLocation()
	{
		return location;
	}

	public int getMovement()
	{
		PriorityQueue<int> canBeCarriedMoves = new PriorityQueue<int>();
		PriorityQueue<int> canCarryMoves = new PriorityQueue<int>();
		for (int q = 0; q < members.Count; q++)
		{
			Unit u = members[q];
			if (u.canBeCarried())
			{
				canBeCarriedMoves.add(u.getMovement());
			}
			else
			{ //If cannot be carried, then can carry
				canCarryMoves.add(u.getMovement());
			}
		}
		if (canCarryMoves.size() >= canBeCarriedMoves.size())
		{
			return canCarryMoves.pop();
		}
		while (canCarryMoves.size() < canBeCarriedMoves.size() - 1)
		{
			canBeCarriedMoves.pop();
		}
		return canBeCarriedMoves.pop();
	}

	public bool canFly()
	{
		int fliers = 0;
		int ground = 0;
		for (int q = 0; q < members.Count; q++)
		{
			if (members[q].canFly())
			{
				fliers++;
			}
			else if (members[q].canCarryUnit())
			{
				return false;
			}
			else
			{
				ground++;
			}
		}
		return fliers >= ground;
	}

	public List<Equippable> getUnitsWithItem(int[] item)
	{
		List<Equippable> possessors = new List<Equippable>();
		for (int q = 0; q < members.Count; q++)
		{
			if (members[q] is Equippable)
			{
				Equippable u = (Equippable)members[q];
				int[][] inv = u.getInventory();
				if (inv == null)
				{
					continue;
				}
				for (int w = 0; w < inv.Length; w++)
				{
					if (InventoryIndex.elementsAreEqual(inv[w], item))
					{
						possessors.Add(u);
						break;
					}
				}
			}
		}
		return possessors;
	}

	public void sendTo(WorldMapTile tile)
	{
		this.location = tile;
	}

	public Nation getAffiliation()
	{
		return members[0].getAffiliation();
	}

	public BattleGround getBattle()
	{
		return battle;
	}

	public void setBattle(BattleGround battle)
	{
		this.battle = battle;
		for (int q = 0; q < members.Count; q++)
		{
			members[q].incrementBattles();
		}
	}

	public void setBattleGroundObjective()
	{
		//TODO based on general objective, members, and surroundings
	}

	public int[] getBattleGroundObjective()
	{
		return battleGroundObjective;
	}

	public void setGeneralObjective(int[] obj)
	{
		this.generalObjective = obj;
	}

	public void setBattgroundPositions()
	{
		if (!customPositions)
		{
			int[][] positions = null;
			if (assignedThing is Ship)
			{
				Ship s = (Ship)assignedThing;
				if (s.getSize() == Ship.SMALL_SIZE)
				{
					positions = DEFAULT_SMALL_SHIP_POSITIONS;
				}
				else if (s.getSize() == Ship.MEDIUM_SIZE)
				{
					positions = DEFAULT_MEDIUM_SHIP_POSITIONS;
				}
				else if (s.getSize() == Ship.LARGE_SIZE)
				{
					positions = DEFAULT_LARGE_SHIP_POSITIONS;
				}
			}
			else
			{
				positions = DEFAULT_BATTLE_POSITIONS;
			}
			for (int q = 0; q < members.Count; q++)
			{
				int[] coords = positions[q];
				members[q].setCoords(coords[0], coords[1]);
			}
		}
	}

	/**
	 * Gives the military strength of the group
	 * @return array with the following indexes:
	 * [0] = physical strength
	 * [1] = magical strength
	 * [2] = accuracy
	 * [3] = avoidance (Basic. that is, torso for units)
	 * [4] = crit rate
	 * [5] = crit avoidance (Basic. that is, torso for units)
	 * [6] = average attack speed
	 * [7] = defense (Basic. that is, torso for units)
	 * [8] = resistance
	 * [9] = head HP
	 * [10] = torso HP
	 */
	public int[] getPower()
	{
		int[] ret = new int[11];
		for (int q = 0; q < members.Count; q++)
		{
			int[] current = members[q].getPower();
			for (int w = 0; w < current.Length; w++)
			{
				ret[w] += current[w];
			}
		}
		ret[6] /= Mathf.Max(1, members.Count); //Make sure that attack speed is always the average
											   //		if (assignedThing != null) {
											   //			int[] basePower = assignedThing.getPower();
											   //			ret[0] += basePower[0]; //Physical
											   //			ret[1] += basePower[1]; //Magical
											   //			ret[2] += basePower[2]; //Accuracy
											   //			ret[4] += basePower[3]; //Crit
											   //			ret[7] += basePower[4]; //Defense
											   //			ret[8] += basePower[5]; //Resistance
											   //		}
		return ret;
	}

	public Assignable getAssignedThing()
	{
		return assignedThing;
	}

	public void giveAssignment(Assignable a)
	{
		this.assignedThing = a;
		this.location = null;
		a.assignGroup(this);
		this.customPositions = false;
	}

	public void removeAssignment()
	{
		assignedThing = null;
	}

	public bool add(Unit u)
	{
		if (u.getAffiliation() != getAffiliation())
		{
			return false;
		}
		//Monsters can only be grouped with other monsters with the same master
		//(it makes figuring out affiliation easier)
		if (u is Monster)
		{
			Unit check = members[0];
			if (!(check is Monster)
							|| ((Monster)check).getMaster() != ((Monster)u).getMaster())
			{
				return false;
			}
		}
		if (members.Count < CAPACITY)
		{
			members.Add(u);
			u.assignGroup(this);
			customPositions = false;
			return true;
		}
		return false;
	}

	public void remove(Unit unit)
	{
		Nation n = unit.getAffiliation();
		int idx = members.IndexOf(unit);
		members.Remove(unit);
		if (unit.getGroup() != null)
		{
			unit.removeGroup();
		}
		if (idx == 0)
		{
			if (members.Count == 0)
			{
				n.getUnitGroups().Remove(this);
				if (location != null && location.getGroupPresent() == this)
				{
					location.removeGroupOrShip();
				}
			}
			else
			{
				autoAssignLeader();
			}
		}
	}

	public void toggleAIControl()
	{
		groupIsAIControlled = !groupIsAIControlled;
	}

	public bool isAIControlled()
	{
		return groupIsAIControlled;
	}

	public void exhaust()
	{
		for (int q = 0; q < members.Count; q++)
		{
			members[q].exhaust();
		}
	}

	public bool isExhausted()
	{
		for (int q = 0; q < members.Count; q++)
		{
			if (!(members[q].isExhausted()))
			{
				return false;
			}
		}
		return true;
	}

	public void assignLeader(int idx)
	{
		Unit newLead = members[idx];
		members.Remove(newLead);
		members.Insert(0, newLead);
	}

	public void autoAssignLeader()
	{
		Unit lead = members[0];
		for (int q = 0; q < members.Count; q++)
		{
			Unit cand = members[q];
			if (cand.getLeadership() > lead.getLeadership())
			{
				lead = cand;
			}
		}
		members.Remove(lead);
		members.Insert(0, lead);
	}

	public List<Unit> getMembers()
	{
		return members;
	}

	public int size()
	{
		return members.Count;
	}

	public bool isFull()
	{
		return members.Count >= CAPACITY;
	}

	public Unit get(int q)
	{
		return members[q];
	}

	public UnitGroup getPrisoners()
	{
		//Because I dunno another foolproof way to do this
		if (prisoners != null && prisoners.size() == 0)
		{
			prisoners = null;
		}
		return prisoners;
	}

	public UnitGroup removePrisoners()
	{
		UnitGroup ret = prisoners;
		prisoners = null;
		return ret;
	}
	public Unit getLeader()
	{
		return members[0];
	}

	/**
	 * Method used for testing
	 * @return
	 */
	public string getGroupAsString()
	{
		string sb = "Members:\n";
		for (int q = 0; q < members.Count; q++)
		{
			Unit u = members[q];
			sb += $"{u.getName()} {u.getUnitClassName()}\n";
		}
		int[] power = getPower();
		sb += $"Power:\nPhysical: {power[0]}, Magical: {power[1]}, Accuracy: {power[2]}, Avoidance: {power[3]},\n";
		sb += $"Crit Rate: {power[4]}, Crit Avoid: {power[5]}, AS: {power[6]}, DEF: {power[7]}, RES: {power[8]}\n";
		sb += $"Head HP: {power[9]}, Torso HP: {power[10]}\n";

		return sb;
	}

}
