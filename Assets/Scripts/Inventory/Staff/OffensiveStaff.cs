using System;
public class OffensiveStaff : Staff
{
	private int statusEffect;
	private int power;

	public OffensiveStaff(string name, int initialUses, int approximateWorth, int weight,
			int range, int effect, int power, int[][] recipe)
			: base (name, initialUses, approximateWorth, weight, range, recipe)
	{
		// TODO Auto-generated constructor stub
		if (effect == Staff.BERSERK || effect == Staff.POISON || effect == Staff.SLEEP
				|| effect == Staff.INJURY)
		{
			this.statusEffect = effect;
		}
		else
		{
			throw new Exception("Invalid status effect type");
		}
		this.power = power;
	}

	public bool effect(Unit user, Unit target, int bodyPart)
	{
		//TODO
		//If status effect is injury (that is, if this is a disintegration staff)
		if (false/*TODO preconditions fail*/)
		{
			return false;
		}
		if (statusEffect == Staff.INJURY)
		{
			if (target is Equippable && target.defense(false, bodyPart) > 0) {
		Equippable e = (Equippable)target;
		e.destroyArmor();
		} else
		{
			//Crit the unit such that their current HP is reduced to 1
			//Disintegration staff OP
			target.takeCriticalDamage(false, bodyPart, target.getCurrentHPOfBodyPart(bodyPart) - 1);
		}
				} else
		{
			target.getStatusEffects()[statusEffect] += power;
		}
		return true;
	}
	public override int getGeneralItemId()
	{
		return InventoryIndex.OFFENSIVE_STAFF;
	}
}
