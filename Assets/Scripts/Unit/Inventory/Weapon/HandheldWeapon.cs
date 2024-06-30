public class HandheldWeapon : Weapon
{

	public HandheldWeapon(string name, int proficiencyRequirement, int maxRange, int might, int hit, int crit,
			bool isMagic, int proficiencyIndex, int[][] recipe, int initialUses, int approximateWorth,
			int weight)
		: base (name, proficiencyRequirement, 1, maxRange, might, hit, crit, isMagic,
			proficiencyIndex, recipe, initialUses, approximateWorth, weight)
	{
		// TODO Auto-generated constructor stub
	}

	public override int getGeneralItemId()
	{
		return InventoryIndex.HANDHELD_WEAPON;
	}

}
