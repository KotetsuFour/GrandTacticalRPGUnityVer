public abstract class Weapon : ManufacturableItem
{

	public static int SWORD = 0;
public static int LANCE = 1;
public static int AXE = 2;
public static int BOW = 3;
public static int KNIFE = 4;
public static int BALLISTA = 5;
public static int ANIMA = 6;
public static int LIGHT = 7;
public static int DARK = 8;
//Even though staves aren't weapons
public static int STAFF = 9;

protected int proficiencyRequirement;
protected int proficiencyIndex;
protected int minWeaponRange;
protected int maxWeaponRange;
protected int might;
protected int hit;
protected int crit;
protected bool weaponIsMagic;

	public Weapon(string name, int proficiencyRequirement, int minRange, int maxRange,
			int might, int hit, int crit,
			bool isMagic, int proficiencyIndex, int[][] recipe, int initialUses,
			int approximateWorth, int weight)
			: base (name, initialUses, approximateWorth, weight, recipe)
	{
		this.proficiencyRequirement = proficiencyRequirement;
		this.minWeaponRange = minRange;
		this.maxWeaponRange = maxRange;
		this.might = might;
		this.hit = hit;
		this.crit = crit;
		this.weaponIsMagic = isMagic;
		this.proficiencyIndex = proficiencyIndex;
		this.recipe = recipe;
	}

	public bool isMagic()
	{
		return weaponIsMagic;
	}

	public int getMight()
	{
		return might;
	}

	public int getHit()
	{
		return hit;
	}

	public int getProficiencyRequirement()
	{
		return proficiencyRequirement;
	}

	public int getProficiencyIndex()
	{
		return proficiencyIndex;
	}

	public int getCrit()
	{
		return crit;
	}

	public string getProficiencyTypeAsString()
	{
		if (proficiencyIndex == SWORD)
		{
			return "Sword";
		}
		if (proficiencyIndex == LANCE)
		{
			return "Spear";
		}
		if (proficiencyIndex == AXE)
		{
			return "Axe";
		}
		if (proficiencyIndex == BOW)
		{
			return "Bow";
		}
		if (proficiencyIndex == KNIFE)
		{
			return "Knife";
		}
		if (proficiencyIndex == BALLISTA)
		{
			return "Ballista";
		}
		if (proficiencyIndex == ANIMA)
		{
			return "Earth";
		}
		if (proficiencyIndex == LIGHT)
		{
			return "Light";
		}
		if (proficiencyIndex == DARK)
		{
			return "Dark";
		}
		if (proficiencyIndex == STAFF)
		{
			return "Staff";
		}
		return null;
	}

	public bool usesDurabilityWithoutHitting()
	{
		return maxWeaponRange > 1;
	}
	public int maxRange()
	{
		return maxWeaponRange;
	}
	public int minRange()
	{
		return minWeaponRange;
	}

		public override string[] getInformationDisplayArray(int[] itemArray)
	{
		return new string[] {
					$"Durability: {itemArray[2]}/{initialUses}",
					$"Prof.: {getProficiencyTypeAsString()} {proficiencyRequirement}",
					$"Range: {minWeaponRange}-{maxWeaponRange}",
					$"Weight: {weight}",
					$"Power: {might}",
					$"Accuracy: {hit}",
					$"Critical Rate: {crit}",
					$"Magic: {weaponIsMagic}"
			};
	}
}