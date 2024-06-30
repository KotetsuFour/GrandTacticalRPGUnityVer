using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Defendable : Building, Assignable
{

	private UnitGroup assignedGroup;
	private List<StationaryWeapon> defenses;
	protected List<int[]> armors;
	protected List<int[]> staves;
	protected WorldMapTile location;

	public Defendable(string name, int maxStructuralIntegrity, int durability, int resistance,
			Human owner, WorldMapTile location)
			: base(name, maxStructuralIntegrity, durability, resistance, owner)
	{
		this.armors = new List<int[]>();
		this.staves = new List<int[]>();
		this.defenses = new List<StationaryWeapon>();
		this.location = location;
	}


	public void assignGroup(UnitGroup group)
	{
		this.assignedGroup = group;
	}


	public UnitGroup getAssignedGroup()
	{
		if (assignedGroup != null && assignedGroup.getMembers().Count == 0)
		{
			assignedGroup = null;
		}
		return assignedGroup;
	}


	public bool dismissAssignedGroup()
	{
		if (!(location.isVacant()))
		{
			return false;
		}
		location.sendHere(assignedGroup);
		assignedGroup.removeAssignment();
		assignedGroup = null;
		return true;
	}


	public List<StationaryWeapon> getDefenses()
	{
		return defenses;
	}

	public void placeStationaryWeapon(StationaryWeapon w)
	{
		defenses.Add(w);
		//TODO assign a position for the weapon
	}

	public List<int[]> getArmors()
	{
		return armors;
	}

	public List<int[]> getStaves()
	{
		return staves;
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
		for (int q = 0; q < defenses.Count; q++)
		{
			StationaryWeapon s = defenses[q];
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
		ret[4] += getDurability();
		ret[5] += getResistance();
		return ret;
	}


	public override bool canReceiveGoods(int[] goods)
	{
		int type = goods[0];
		return type == InventoryIndex.HANDHELD_WEAPON
				|| type == InventoryIndex.STATIONARY_WEAPON
				|| type == InventoryIndex.ARMOR
				|| type == InventoryIndex.OFFENSIVE_STAFF
				|| type == InventoryIndex.STATIONARY_STAFF
				|| type == InventoryIndex.TILE_STAFF
				|| type == InventoryIndex.SUPPORT_STAFF;
	}


	public new bool receiveGoods(int[] goods)
	{
		if (!canReceiveGoods(goods))
		{
			return false;
		}
		if (goods[0] == InventoryIndex.STATIONARY_WEAPON)
		{
			StationaryWeapon w = (StationaryWeapon)InventoryIndex.getElement(goods);
			for (int q = 0; q < goods[2]; q++)
			{
				placeStationaryWeapon(w.clone());
			}
			return true;
		}
		else if (goods[0] == InventoryIndex.HANDHELD_WEAPON)
		{
			for (int q = 0; q < materials.Count; q++)
			{
				int[] m = materials[q];
				if (InventoryIndex.elementsAreEqual(m, goods))
				{
					m[2] += goods[2];
					return true;
				}
			}
			materials.Add((int[])goods.Clone());
			return true;
		}
		else if (goods[0] == InventoryIndex.ARMOR)
		{
			for (int q = 0; q < armors.Count; q++)
			{
				int[] m = armors[q];
				if (InventoryIndex.elementsAreEqual(m, goods))
				{
					m[2] += goods[2];
					return true;
				}
			}
			armors.Add((int[])goods.Clone());
			return true;
		}
		else if (goods[0] == InventoryIndex.SUPPORT_STAFF
			  || goods[0] == InventoryIndex.OFFENSIVE_STAFF
			  || goods[0] == InventoryIndex.STATIONARY_STAFF
			  || goods[0] == InventoryIndex.TILE_STAFF)
		{
			for (int q = 0; q < staves.Count; q++)
			{
				int[] m = staves[q];
				if (InventoryIndex.elementsAreEqual(m, goods))
				{
					m[2] += goods[2];
					return true;
				}
			}
			staves.Add((int[])goods.Clone());
			return true;
		}
		return false;
	}



}
