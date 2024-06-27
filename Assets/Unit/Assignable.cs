using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Assignable
{
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
	public int[] getPower();

	public void assignGroup(UnitGroup group);

	public UnitGroup getAssignedGroup();

	public bool dismissAssignedGroup();

	public string getName();

	List<StationaryWeapon> getDefenses();

	void placeStationaryWeapon(StationaryWeapon w);

}
