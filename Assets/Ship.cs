using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Ship : WMTileOccupant, Assignable
{

	private WorldMapTile location;
	protected int bluePrint;
	protected Hull hull;
	protected Helm helm;
	public ShipType type;
	protected int size;
	protected List<StationaryWeapon> weapons;
	protected UnitGroup assignedUnitGroup;
	protected List<ShipBarracks> barracks;
	protected List<ShipStorage> storage;
	protected List<ShipPrison> prison;
	protected BattleGround battle;
	protected bool shipIsImportant;
	protected bool hasAPlayerNationUnit;

	public static int SMALL_SIZE = 0;
	public static int MEDIUM_SIZE = 1;
	public static int LARGE_SIZE = 2;

	public Ship(ShipType type)
	{
		this.hull = new Hull(type);
		this.helm = new Helm(type);
		this.weapons = new List<StationaryWeapon>();
		barracks = new List<ShipBarracks>();
		storage = new List<ShipStorage>();
		prison = new List<ShipPrison>();
	}

	public string getName()
	{
		// TODO Auto-generated method stub
		return null;
	}
	public void setBluePrint(int bluePrint)
	{
		this.bluePrint = bluePrint;
	}
	public int getShipPartHP(int shipPart)
	{
		return getShipPart(shipPart).getCurrentHP();
	}

	public bool partIsIntact(int shipPart)
	{
		return getShipPartHP(shipPart) >= 0;
	}

	public bool isDestroyed()
	{
		//Check for hull HP
		return !partIsIntact(0);
	}
	public int getDefense(bool isMagicAttack)
	{
		if (isMagicAttack)
		{
			return type.getResistance();
		}
		return type.getDefense();
	}

	public void takeDamage(bool magic, int shipPart, int might)
	{
		int damage = Mathf.Max(0, might - getDefense(magic));
		getShipPart(shipPart).takeDirectDamage(damage);
	}
	public int avoidance(int shipPart)
	{
		if (shipPart == 0 || shipPart == 1)
		{
			//If hull or helm, avoidance is better when more ship parts
			//Otherwise, avoidance is 0
			return (barracks.Count + storage.Count) * 20;
		}
		return 0;
	}

	public WorldMapTile getLocation()
	{
		return location;
	}

	public int getMovement()
	{
		// TODO Auto-generated method stub
		return 0;
	}

	public string getShipDescription()
	{
		if (assignedUnitGroup == null)
		{
			return "Unoccupied Ship";
		}
		return assignedUnitGroup.getLeader().getName() + "'s Ship";
	}
	public UnitGroup getAssignedGroup()
	{
		if (assignedUnitGroup != null && assignedUnitGroup.getMembers().Count == 0)
		{
			assignedUnitGroup = null;
		}
		return assignedUnitGroup;
	}

	public void assignGroup(UnitGroup group)
	{
		this.assignedUnitGroup = group;
		setStartingPositions(group);
	}
	public bool dismissAssignedGroup()
	{
		// TODO Auto-generated method stub
		return false;
	}
	public Unit getLeader()
	{
		if (assignedUnitGroup == null)
		{
			return null;
		}
		return assignedUnitGroup.getLeader();
	}

	public List<StationaryWeapon> getDefenses()
	{
		return weapons;
	}
	public void placeStationaryWeapon(StationaryWeapon w)
	{
		// TODO Auto-generated method stub

	}
	public Nation getAffiliation()
	{
		if (assignedUnitGroup == null)
		{
			return null;
		}
		return assignedUnitGroup.getAffiliation();
	}

	public bool assignGroupToBarracks(UnitGroup group)
	{
		for (int q = 0; q < barracks.Count; q++)
		{
			if (barracks[q].getPassengers() == null)
			{
				barracks[q].assignGroup(group);
				return true;
			}
		}
		return false;
	}

	private void setStartingPositions(UnitGroup group)
	{
		//TODO set the starting battle positions of the unit group
		//so that they are always physically on the ship, in optimal spots
	}

	private ShipPart getShipPart(int part)
	{
		if (part == 0)
		{
			return hull;
		}
		if (part == 1)
		{
			return helm;
		}
		part -= 2;
		if (part < barracks.Count)
		{
			return barracks[part];
		}
		part -= barracks.Count;
		if (part < storage.Count)
		{
			return storage[part];
		}
		part -= storage.Count;
		return prison[part];
	}
	public BattleGround getBattle()
	{
		return battle;
	}

	/**
	 * Gives all units in assigned group and all units below deck
	 * @return
	 */
	public List<Unit> getAllPassengers()
	{
		List<Unit> ret = new List<Unit>();
		shipIsImportant = false;
		hasAPlayerNationUnit = false;
		List<Unit> toAdd = assignedUnitGroup.getMembers();
		for (int q = 0; q < toAdd.Count; q++)
		{
			Unit u = toAdd[q];
			ret.Add(u);
			if (u.isImportant())
			{
				shipIsImportant = true;
			}
			else if (u.getAffiliation() == GeneralGameplayManager.getPlayerNation())
			{
				hasAPlayerNationUnit = true;
			}
		}
		for (int q = 0; q < barracks.Count; q++)
		{
			toAdd = barracks[q].getUnitsCurrentlyInBarracks();
			if (toAdd != null)
			{
				for (int w = 0; w < toAdd.Count; w++)
				{
					Unit u = toAdd[q];
					ret.Add(u);
					if (u.isImportant())
					{
						shipIsImportant = true;
					}
					else if (u.getAffiliation() == GeneralGameplayManager.getPlayerNation())
					{
						hasAPlayerNationUnit = true;
					}
				}
			}
		}
		for (int q = 0; q < prison.Count; q++)
		{
			toAdd = prison[q].getPrisoners().getMembers();
			if (toAdd != null)
			{
				for (int w = 0; w < toAdd.Count; w++)
				{
					Unit u = toAdd[q];
					ret.Add(u);
					if (u.isImportant())
					{
						shipIsImportant = true;
					}
					else if (u.getAffiliation() == GeneralGameplayManager.getPlayerNation())
					{
						hasAPlayerNationUnit = true;
					}
				}
			}
		}
		return ret;
	}

	/**
	 * Gives all units (in the ship or otherwise, allied or enemy) who are physically
	 * aboard the ship and cannot escape if the ship sinks
	 * @return
	 */
	public List<Unit> getPassengersAboard()
	{
		// TODO Auto-generated method stub
		//TODO while getting passengers, if one of them is important or part of the player's
		//nation, mark the ship as such
		List<Unit> ret = new List<Unit>();
		//TODO add all units in battle who are physically on the ship
		for (int q = 0; q < barracks.Count; q++)
		{
			List<Unit> check = barracks[q].getUnitsCurrentlyInBarracks();
			for (int w = 0; w < check.Count; w++)
			{
				Unit u = check[w];
				//Only need to record one, really
				if (u.isImportant())
				{
					this.shipIsImportant = true;
				}
				else if (u.getAffiliation() == GeneralGameplayManager.getPlayerNation())
				{
					this.hasAPlayerNationUnit = true;
				}
				ret.Add(u);
			}
		}
		return ret;
	}

	public List<UnitGroup> getPrisoners()
	{
		List<UnitGroup> ret = new List<UnitGroup>();
		for (int q = 0; q < prison.Count; q++)
		{
			ret.Add(prison[q].getPrisoners());
		}
		return ret;
	}

	public List<UnitGroup> getAllUndeployedPassengersAndPrisoners()
	{
		List<UnitGroup> ret = new List<UnitGroup>();
		for (int q = 0; q < barracks.Count; q++)
		{
			ret.Add(barracks[q].getPassengers());
		}
		for (int q = 0; q < prison.Count; q++)
		{
			ret.Add(prison[q].getPrisoners());
		}
		return ret;
	}

	public void destructionSequence()
	{
		// TODO remove ship from WorldMapTile and BattleGround
	}

	public bool isImportant()
	{
		return shipIsImportant;
	}

	public bool hasPlayerNationUnit()
	{
		return hasAPlayerNationUnit;
	}
	public UnitGroup assignedGroup()
	{
		return assignedUnitGroup;
	}


	public Ship clone()
	{
		Ship ret = new Ship(type);
		for (int q = 0; q < weapons.Count; q++)
		{
			ret.weapons.Add(weapons[q].clone());
		}
		for (int q = 0; q < barracks.Count; q++)
		{
			ret.barracks.Add((ShipBarracks)barracks[q].clone());
		}
		for (int q = 0; q < storage.Count; q++)
		{
			ret.storage.Add((ShipStorage)storage[q].clone());
		}
		for (int q = 0; q < prison.Count; q++)
		{
			ret.prison.Add((ShipPrison)prison[q].clone());
		}
		return ret;
	}

	/**
	 * Gives power of the assignable thing
	 * @return an array with indexes:
	 * [0] = physical strength
	 * [1] = magical strength
	 * [2] = accuracy
	 * [3] = critRate
	 * [4] = defense
	 * [5] = resistance
	 */
	public int[] getPower()
	{
		int[] ret = new int[6];
		for (int q = 0; q < weapons.Count; q++)
		{
			StationaryWeapon s = weapons[q];
			if (s.isMagic())
			{
				ret[1] = s.getMight();
			}
			else
			{
				ret[0] = s.getMight();
			}
			ret[2] = s.getHit();
			ret[3] = s.getCrit();
			ret[4] = s.getDefense();
			ret[5] = s.getResistance();
		}
		ret[4] += type.getDefense();
		ret[5] += type.getResistance();
		return ret;
	}

	public int getSize()
	{
		return size;
	}

	public abstract class ShipPart
	{
		private int currentHP;
		protected ShipType type;
		public ShipPart(ShipType type)
		{
			this.currentHP = type.getMaxHP();
			this.type = type;
		}
		public int getCurrentHP()
		{
			return currentHP;
		}
		public void takeDirectDamage(int damage)
		{
			currentHP -= damage;
		}
		public abstract ShipPart clone();
	}
	public class Hull : ShipPart
	{
		public Hull(ShipType type)
			: base(type)
		{
		}
		public override ShipPart clone()
		{
			return new Hull(type);
		}
	}
	public class Helm : ShipPart
	{
		public Helm(ShipType type)
			: base(type)
		{
		}
		public override ShipPart clone()
		{
			return new Helm(type);
		}
	}
	public class ShipBarracks : ShipPart
	{
		private UnitGroup passengers;
		private List<Unit> passengersCurrentlyInBarracks;
		private List<Unit> deployedPassengers;
		public ShipBarracks(ShipType type)
					: base(type)
		{
			passengersCurrentlyInBarracks = new List<Unit>();
			deployedPassengers = new List<Unit>();
		}
		public override ShipPart clone()
		{
			return new ShipBarracks(type);
		}
		public List<Unit> getDeployedUnits()
		{
			return deployedPassengers;
		}
		public List<Unit> getUnitsCurrentlyInBarracks()
		{
			return passengersCurrentlyInBarracks;
		}
		public UnitGroup getPassengers()
		{
			return passengers;
		}
		public void assignGroup(UnitGroup group)
		{
			this.passengers = group;
			passengersCurrentlyInBarracks.AddRange(group.getMembers());
		}
		public Unit deployUnit(int idx)
		{
			Unit ret = passengersCurrentlyInBarracks[idx];
			passengersCurrentlyInBarracks.RemoveAt(idx);
			deployedPassengers.Add(ret);
			return ret;
		}
		public void returnAllDeployedUnitsToBarracks()
		{
			while (deployedPassengers.Count != 0)
			{
				Unit u = deployedPassengers[deployedPassengers.Count - 1];
				deployedPassengers.RemoveAt(deployedPassengers.Count - 1);
				if (u.isAlive())
				{
					passengersCurrentlyInBarracks.Add(u);
				}
			}
		}
	}
	public class ShipPrison : ShipPart
	{
		private UnitGroup prisoners;
		public ShipPrison(ShipType type)
					: base(type)
		{
		}
		public override ShipPart clone()
		{
			return new ShipPrison(type);
		}
		public UnitGroup getPrisoners()
		{
			return prisoners;
		}
	}
	public class ShipStorage : ShipPart
	{
		private List<int[]> materials;
		private List<int[]> weapons;
		private List<int[]> armor;
		private int currentHP;
		public ShipStorage(ShipType type)
					: base(type)
		{
			materials = new List<int[]>();
			weapons = new List<int[]>();
			armor = new List<int[]>();
		}
		public override ShipPart clone()
		{
			return new ShipStorage(type);
		}
	}

	public class ShipType
	{
		//TODO initialize values
		public static ShipType WOOD = new ShipType(0, 0, 0, 0, 0, 0, 0, 0, 0);
		public static ShipType STONE = new ShipType(0, 0, 0, 0, 0, 0, 0, 0, 0);
		public static ShipType CLAY = new ShipType(0, 0, 0, 0, 0, 0, 0, 0, 0);
		public static ShipType BRONZE = new ShipType(0, 0, 0, 0, 0, 0, 0, 0, 0);
		public static ShipType IRON = new ShipType(0, 0, 0, 0, 0, 0, 0, 0, 0);
		public static ShipType STEEL = new ShipType(0, 0, 0, 0, 0, 0, 0, 0, 0);
		public static ShipType SILVER = new ShipType(0, 0, 0, 0, 0, 0, 0, 0, 0);
		public static ShipType GOLD = new ShipType(0, 0, 0, 0, 0, 0, 0, 0, 0);
		public static ShipType OBSIDIAN = new ShipType(0, 0, 0, 0, 0, 0, 0, 0, 0);
		public static ShipType GLASS = new ShipType(0, 0, 0, 0, 0, 0, 0, 0, 0);

		private int maxHP;
		private int defense;
		private int resistance;
		private int smallCapacity;
		private int mediumCapacity;
		private int largeCapacity;
		private int smallMovement;
		private int mediumMovement;
		private int largeMovement;
		private ShipType(int maxHP, int defense, int resistance, int smallCapacity,
				int mediumCapacity, int largeCapacity, int smallMovement, int mediumMovement,
				int largeMovement)
		{
			this.maxHP = maxHP;
			this.defense = defense;
			this.resistance = resistance;
			this.smallCapacity = smallCapacity;
			this.mediumCapacity = mediumCapacity;
			this.largeCapacity = largeCapacity;
			this.smallMovement = smallMovement;
			this.mediumMovement = mediumMovement;
			this.largeMovement = largeMovement;
		}
		public int getMaxHP()
		{
			return maxHP;
		}
		public int getDefense()
		{
			return defense;
		}
		public int getResistance()
		{
			return resistance;
		}
		public int getSmallCapacity()
		{
			return smallCapacity;
		}
		public int getMediumCapacity()
		{
			return mediumCapacity;
		}
		public int getLargeCapacity()
		{
			return largeCapacity;
		}
		public int getSmallMovement()
		{
			return smallMovement;
		}
		public int getMediumMovement()
		{
			return mediumMovement;
		}
		public int getLargeMovement()
		{
			return largeMovement;
		}
	}

}
