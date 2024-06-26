using System;
public class StationaryWeapon : Weapon, BFTileOccupantData
{

	protected int battlegroundMapLocationX;
	protected int battlegroundMapLocationY;
	protected int reloadStage;
	protected int hp;
	protected int defense;
	protected int resistance;
	protected int battlegroundMinRange;
	//TODO initialize hp

	public StationaryWeapon(string name, int proficiencyRequirement, int minRange, int maxRange,
			int might, int hit, int crit,
			bool isMagic, int proficiencyIndex, int[][] recipe, int initialUses, int approximateWorth,
			int weight, int defense, int resistance, int battlegroundMinRange)
			: base (name, proficiencyRequirement, minRange, maxRange, might, hit, crit, isMagic,
				proficiencyIndex, recipe, initialUses, approximateWorth, weight)
	{
		this.defense = defense;
		this.resistance = resistance;
		this.battlegroundMinRange = battlegroundMinRange;
	}

	public StationaryWeapon clone()
	{
		return new StationaryWeapon(name, proficiencyRequirement, minWeaponRange, maxWeaponRange,
				might, hit, crit, weaponIsMagic, proficiencyIndex, recipe, initialUses, approximateWorth,
				weight, defense, resistance, battlegroundMinRange);
	}

	public void use()
	{
		reloadStage = 2;
		initialUses--;
	}
	public void reload()
	{
		if (initialUses == 0)
		{
			throw new Exception("This weapon has no more ammunition to load.");
		}
		reloadStage--;
	}
	public bool ready()
	{
		return reloadStage == 0 && initialUses > 0;
	}

	public int getDefense()
	{
		return defense;
	}

	public int getResistance()
	{
		return resistance;
	}

	public int getBattlegroundMinRange()
	{
		return battlegroundMinRange;
	}

	public string getDisplayName()
	{
		// TODO Auto-generated method stub
		return null;
	}

	public override int getGeneralItemId()
	{
		return InventoryIndex.STATIONARY_WEAPON;
	}
}
