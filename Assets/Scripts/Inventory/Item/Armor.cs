using UnityEngine;

public class Armor : ManufacturableItem
{

	//Before you get any funny ideas, future me, armor should not be able to defend against
	//magic, because resistance is a naturally occurring trait in units. If you want to 
	//differentiate between different kinds of physical damage, be my guest
	private int[] defenses;
	private int unitType;
	private Color[] colorScheme;

	public Armor(string name, int initialUses, int approximateWorth, int weight,
			Armor basicForm, Color[] colorScheme)
			: base(name, basicForm.initialUses, basicForm.approximateWorth, basicForm.weight, basicForm.recipe)
	{
		//TODO copy stats of basicForm onto this armor
		this.defenses = basicForm.defenses;
		this.unitType = basicForm.unitType;
		this.colorScheme = colorScheme;
	}

	public Armor(string name, int initialUses, int approximateWorth, int weight,
			int[] defenses, int unitType,
			int[][] recipe, Color[] colorScheme)
			: base (name, initialUses, approximateWorth, weight, recipe)
	{
		this.defenses = defenses;
		this.unitType = unitType;
		this.colorScheme = colorScheme;
	}

	public int getDefenseFor(int bodyPart)
	{
		return defenses[bodyPart];
	}
	public int[] getDefenses()
	{
		return defenses;
	}
	public Color getColorFor(int bodyPart)
	{
		return colorScheme[bodyPart];
	}
	public int getUnitType()
	{
		return unitType;
	}
	public override int getGeneralItemId()
	{
		return InventoryIndex.ARMOR;
	}

	public override string[] getInformationDisplayArray(int[] itemArray)
	{
		// TODO Auto-generated method stub
		return null;
	}
}
