using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Equippable
{

	public int[][] getInventory();

	public void useWeapon(bool hit);

	public bool canUseBallista();

	public bool canUseMagicTurrets();

	public bool canUse(StationaryWeapon weapon);

	public Armor getArmor();

	public void destroyArmor();

	public string getArmorName();

	public Item getEquippedItem();

	public string getWeaponName();

	public HandheldWeapon getEquippedWeapon();

	public void autoEquip();

	public int getEquipmentHeuristic(int[] item);

	public void equip(int idx);

	public bool receiveNewArmor(int[] armor);

	public bool receiveNewItem(int[] item);

	public int armStrength();

	public int proficiencyWith(int type);

}
