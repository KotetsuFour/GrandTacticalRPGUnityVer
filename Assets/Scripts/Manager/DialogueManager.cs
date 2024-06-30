using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DialogueManager
{

	public static int STRONG = 70;
	public static int GOOD = 30;
	public static int CASUAL = 0;
	public static int BAD = -40;

	public static string getDeathQuote(Human victim, Unit killer)
	{
		if (victim.getCurrentHPOfBodyPart(0) > 0
				&& victim.getCurrentHPOfBodyPart(1) > -10)
		{

			if (victim.getSupportPartner() == killer)
			{

				Human partner = victim.getSupportPartner();
				Demeanor d = victim.getDemeanor();
				int relationship = victim.getRelationshipWithSupportPartner();

				if (victim.marriedToSupportPartner())
				{ //Romantic
					return $"{partner.getName()}, my darling... {d.disappointedExpression()}";
				}
				else if (relationship > STRONG)
				{ //Strong
					return $"{partner.getName()}, my dear friend... {d.disappointedExpression()}";
				}
				else if (relationship > GOOD)
				{ //Good
					return $"{partner.getName()} ...my friend... {d.ironicExpression()}";
				}
				else if (relationship > CASUAL)
				{ //Casual
					return $"{partner.getName()}... you... {d.ironicExpression()}";
				}
				else if (relationship > BAD)
				{ //Bad
					return $"{partner.getName()}... you git... {d.vengefulExpression()}";
				}
				else
				{ //Horrible
					return $"{partner.getName()}... you rat!... {d.vengefulExpression()}";
				}
			}
			else
			{

				Demeanor d = victim.getDemeanor();

				if (victim.getMilitarism() >= 70
						&& victim.getMilitarism() >= victim.getAltruism()
						&& victim.getMilitarism() >= victim.getFamilism()
						&& victim.getMilitarism() >= victim.getNationalism())
				{
					if (victim.getConfidence() >= 80)
					{
						return $"{victim.getAffiliation().getName()} will remember me....";
					}
					else if (victim.getConfidence() >= 60)
					{
						return "Such a... nice place... to die....";
					}
					else if (victim.getConfidence() >= 40)
					{
						return "You were... a worthy opponent....";
					}
					else if (victim.getConfidence() >= 20)
					{
						return "Was I... wrong...?";
					}
					else
					{
						return $"No... I want to go... back to {victim.getHome().getName()}....";
					}
				}
				else if (victim.getAltruism() >= 70
					  && victim.getAltruism() >= victim.getFamilism()
					  && victim.getAltruism() >= victim.getNationalism())
				{
					//TODO
				}
				else if (victim.getFamilism() >= 70
					  && victim.getFamilism() >= victim.getNationalism())
				{
					if (victim is Offspring) {
						Offspring off = (Offspring)victim;
						if (off.getMother().isAlive() && off.getFather().isAlive())
						{
							return $"Mother, Father, {d.aliveParent()}";
						}
						else if (off.getMother().isAlive())
						{
							int relationship = off.getRelationshipWithMother();
							if (relationship > CASUAL)
							{
								return $"Mother, {d.aliveParent()}";
							}
							else
							{
								return $"Father, {d.deadParent()}";
							}
						}
						else if (off.getFather().isAlive())
						{
							int relationship = off.getRelationshipWithFather();
							if (relationship > CASUAL)
							{
								return $"Father, {d.aliveParent()}";
							}
							else
							{
								return $"Mother, {d.deadParent()}";
							}
						}
						else
						{
							return $"Mother, Father, {d.deadParent()}";
						}
					} else if (victim.getSupportPartner() != null && victim.getRelationshipWithSupportPartner() > GOOD)
					{
						Human partner = victim.getSupportPartner();
						if (victim.mayReproduce())
						{
							return $"{partner.getName()} {d.romanticRelationship()}";
						}
						else if (victim.getRelationshipWithSupportPartner() > STRONG)
						{
							return $"{partner.getName()} {d.strongRelationship()}";
						}
						else
						{
							return $"{partner.getName()} {d.goodRelationship()}";
						}
					}
					else
					{
						if (victim.getConfidence() >= 80)
						{
							return $"You'll never lay a hand on {victim.getHome().getName()}...";
						}
						else if (victim.getConfidence() >= 60)
						{
							return $"The people of {victim.getHome().getName()}... will stop you...";
						}
						else if (victim.getConfidence() >= 40)
						{
							return $"I hope... {victim.getHome().getName()}... is safe...";
						}
						else if (victim.getConfidence() >= 20)
						{
							return $"You can't... {victim.getHome().getName()}... be safe...";
						}
						else
						{
							return $"Please... spare... {victim.getHome().getName()}...";
						}
					}
				}
				else if (victim.getNationalism() >= 70)
				{
					if (victim.getConfidence() >= 80)
					{
						return $"For the glory of {victim.getAffiliation().getName()}...";
					}
					else if (victim.getConfidence() >= 60)
					{
						return $"Long live {victim.getAffiliation().getName()}...";
					}
					else if (victim.getConfidence() >= 40)
					{
						return $"You won't defeat {victim.getAffiliation().getName()}...";
					}
					else if (victim.getConfidence() >= 20)
					{
						return $"No.... {victim.getAffiliation().getName()} mustn't lose....";
					}
					else
					{
						return $"{victim.getAffiliation().getName()}... I've failed you....";
					}
				}
				else
				{
					if (victim.getConfidence() >= 80)
					{
						//TODO
					}
					else if (victim.getConfidence() >= 60)
					{
						//TODO
					}
					else if (victim.getConfidence() >= 40)
					{
						//TODO
					}
					else if (victim.getConfidence() >= 20)
					{
						//TODO
					}
					else
					{
						//TODO
					}
				}
				//TODO death quote based on personality
			}
		}
		//TODO
		return null;
	}

	public static string getSupportPartnerDespairQuote(Human victim, Unit killer)
	{
		if (victim.getSupportPartner() != null
				&& victim.getGroup() == victim.getSupportPartner().getGroup())
		{

			Human partner = victim.getSupportPartner();
			Demeanor d = victim.getDemeanor();
			int relationship = victim.getRelationshipWithSupportPartner();

			if (partner == killer)
			{
				if (victim.marriedToSupportPartner())
				{ //Romantic
					return $"{d.regretfulExpression()}, my dear. I'll always love you.";
				}
				else if (relationship > STRONG)
				{ //Strong
					return $"{d.regretfulExpression()}, old friend. I will miss you.";
				}
				else if (relationship > GOOD)
				{ //Good
					return $"{d.shamefulExpression()}, my friend.";
				}
				else if (relationship > CASUAL)
				{ //Casual
					return $"{d.shamefulExpression()}.";
				}
				else if (relationship > BAD)
				{ //Bad
					return $"{d.gladnessExpression()}, {partner.getName()}!";
				}
				else
				{ //Horrible
					return $"{d.gladnessExpression()}, you scum!";
				}
			}
			else
			{
				//TODO death quote based on personality and relationship
			}
		}
		return null;
	}

	public static string standardKillNotification(Unit victim, Unit killer,
			StationaryWeapon weapon, BattleGround battle)
	{
		// TODO Auto-generated method stub
		return null;
	}

	/**
	 * Tells the strength of the relationship between two (non-player) support partners
	 * @param a
	 * @param b
	 * @return
	 */
	public static string tellSupportRelationship(Human a, Human b)
	{
		return evaluateOneSidedRelationship(a, b, true)
				+ evaluateOneSidedRelationship(b, a, true);
	}
	public static string tellRelationshipWithPlayer(Human h)
	{
		Human p = GeneralGameplayManager.getPlayer();
		if (h.getSupportPartner() == p)
		{
			return evaluateOneSidedRelationship(h, p, true);
		}
		return evaluateOneSidedRelationship(h, p, false);
	}
	private static string evaluateOneSidedRelationship(Human a, Human b, bool bIsPartner)
	{
		string sb = "";
		int ar;
		if (bIsPartner)
		{
			ar = a.getRelationshipWithSupportPartner();
		}
		else
		{
			//b is Player
			ar = a.getRelationshipWithPlayer();
		}
		sb += (a.getName());
		if (ar >= 90)
		{
			sb += (" feels a very deep connection to ");
		}
		else if (ar >= 70)
		{
			sb += (" is very fond of ");
		}
		else if (ar >= 50)
		{
			sb += (" greatly admires ");
		}
		else if (ar >= 30)
		{
			sb += (" is glad to be partnered with ");
		}
		else if (ar >= 10)
		{
			sb += (" thinks well of ");
		}
		else if (ar >= 0)
		{
			sb += (" feels alright about ");
		}
		else if (ar >= -10)
		{
			sb += (" feels unsure about ");
		}
		else if (ar >= -30)
		{
			sb += (" thinks poorly of ");
		}
		else if (ar >= -50)
		{
			sb += (" wants to be separated from ");
		}
		else if (ar >= -70)
		{
			sb += (" is disturbed by ");
		}
		else if (ar >= -90)
		{
			sb += (" holds contempt for ");
		}
		else
		{
			sb += (" deeply despises ");
		}
		sb += (b.getName() + ".\n");
		return sb;
	}

	public static string shipKillNotification(Unit victim, Unit killer, StationaryWeapon weapon, BattleGround battle,
			Ship dfdShip, WorldMapTile worldLocation)
	{
		// TODO Auto-generated method stub
		return null;
	}

	public static string shipDestructionNotification(Unit killer, StationaryWeapon weapon,
			BattleGround battle,
			Ship victimShip, List<Unit> victims, WorldMapTile worldLocation)
	{
		string sb = $"The ship \"{victimShip.getName()}\" was sunk by {killer.getDisplayName()}, using {weapon.getName()}.\n"
						+ "The following combatants drowned as the ship sank:\n";
		for (int q = 0; q < victims.Count; q++)
		{
			sb += (victims[q].getDisplayName());
			sb += ("\n");
		}
		//TODO maybe add battle description of some kind (ship was sunk during the Battle of/at ___)
		return sb;
	}
}
