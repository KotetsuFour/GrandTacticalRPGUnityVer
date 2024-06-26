using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ArtificialHumanTemplate
{

	private string templateName;
	private int[] bodyPartsMaximumHP;
	private int magic;
	private int skill;
	private int reflex;
	private int awareness;
	private int resistance;
	private int movement;
	private int[] bodyPartsMaximumHPGrowth;
	private int magicGrowth;
	private int skillGrowth;
	private int reflexGrowth;
	private int awarenessGrowth;
	private int resistanceGrowth;
	private bool gender; //True for female, false for male
	private int[] appearance;
	private int specificTemplateIndex;
	private int numCopiesMade;

	public ArtificialHumanTemplate(Human h)
	{
		int level = h.effectiveLevel();
		this.templateName = h.getDisplayName();
		this.bodyPartsMaximumHP = new int[h.getBodyPartsMaximumHP().Length];
		bodyPartsMaximumHP[Human.HEAD] = h.getMaximumHPOfBodyPart(Human.HEAD)
				- Mathf.RoundToInt((float)(level * (0.0 + h.getGrowthRateOfBodyPart(Human.HEAD)) / 100));
		bodyPartsMaximumHP[Human.TORSO] = h.getMaximumHPOfBodyPart(Human.TORSO)
				- Mathf.RoundToInt((float)(level * (0.0 + h.getGrowthRateOfBodyPart(Human.TORSO)) / 100));
		int right = h.getMaximumHPOfBodyPart(Human.RIGHT_ARM)
				- Mathf.RoundToInt((float)(level * (0.0 + h.getGrowthRateOfBodyPart(Human.RIGHT_ARM)) / 100));
		int left = h.getMaximumHPOfBodyPart(Human.LEFT_ARM)
				- Mathf.RoundToInt((float)(level * (0.0 + h.getGrowthRateOfBodyPart(Human.LEFT_ARM)) / 100));
		int arms = Mathf.Max(right, left);
		bodyPartsMaximumHP[Human.RIGHT_ARM] = arms;
		bodyPartsMaximumHP[Human.LEFT_ARM] = arms;
		right = h.getMaximumHPOfBodyPart(Human.RIGHT_LEG)
				- Mathf.RoundToInt((float)(level * (0.0 + h.getGrowthRateOfBodyPart(Human.RIGHT_LEG)) / 100));
		left = h.getMaximumHPOfBodyPart(Human.LEFT_LEG)
				- Mathf.RoundToInt((float)(level * (0.0 + h.getGrowthRateOfBodyPart(Human.LEFT_LEG)) / 100));
		int legs = Mathf.Max(right, left);
		bodyPartsMaximumHP[Human.RIGHT_LEG] = legs;
		bodyPartsMaximumHP[Human.LEFT_LEG] = legs;
		right = h.getMaximumHPOfBodyPart(Human.RIGHT_EYE)
				- Mathf.RoundToInt((float)(level * (0.0 + h.getGrowthRateOfBodyPart(Human.RIGHT_EYE)) / 100));
		left = h.getMaximumHPOfBodyPart(Human.LEFT_EYE)
				- Mathf.RoundToInt((float)(level * (0.0 + h.getGrowthRateOfBodyPart(Human.LEFT_EYE)) / 100));
		int eyes = Mathf.Max(right, left);
		bodyPartsMaximumHP[Human.RIGHT_EYE] = eyes;
		bodyPartsMaximumHP[Human.LEFT_EYE] = eyes;
		bodyPartsMaximumHP[Human.MOUNT] = 0;
		this.magic = h.getMagic() - Mathf.RoundToInt((float)(level * (0.0 + h.getMagicGrowth()) / 100));
		this.skill = h.getSkill() - Mathf.RoundToInt((float)(level * (0.0 + h.getSkillGrowth()) / 100));
		this.reflex = h.getReflex() - Mathf.RoundToInt((float)(level * (0.0 + h.getReflexGrowth()) / 100));
		this.awareness = h.getAwareness() - Mathf.RoundToInt((float)(level * (0.0 + h.getAwarenessGrowth()) / 100));
		this.resistance = h.getResistance() - Mathf.RoundToInt((float)(level * (0.0 + h.getResistanceGrowth()) / 100));
		this.movement = h.getBaseMovement();

		this.bodyPartsMaximumHPGrowth = (int[])h.getBodyPartsMaximumHPGrowth().Clone();
		this.magicGrowth = h.getMagicGrowth();
		this.skillGrowth = h.getSkillGrowth();
		this.reflexGrowth = h.getReflexGrowth();
		this.awarenessGrowth = h.getAwarenessGrowth();
		this.resistanceGrowth = h.getResistanceGrowth();
		if (h.getUnitClass() != null)
		{
			int[] gMods = h.getUnitClass().getClassTreeGrowthModifiers();
			bodyPartsMaximumHPGrowth[Human.HEAD] -= gMods[0];
			bodyPartsMaximumHPGrowth[Human.TORSO] -= gMods[1];
			int armsGrowth = Mathf.Max(bodyPartsMaximumHPGrowth[Human.RIGHT_ARM] - gMods[2],
					bodyPartsMaximumHPGrowth[Human.LEFT_ARM] - gMods[2]);
			bodyPartsMaximumHPGrowth[Human.RIGHT_ARM] = armsGrowth;
			bodyPartsMaximumHPGrowth[Human.LEFT_ARM] = armsGrowth;
			int legsGrowth = Mathf.Max(bodyPartsMaximumHPGrowth[Human.RIGHT_LEG] - gMods[3],
					bodyPartsMaximumHPGrowth[Human.LEFT_LEG] - gMods[3]);
			bodyPartsMaximumHPGrowth[Human.RIGHT_LEG] = legsGrowth;
			bodyPartsMaximumHPGrowth[Human.LEFT_LEG] = legsGrowth;
			bodyPartsMaximumHPGrowth[Human.MOUNT] = 0;
			magicGrowth -= gMods[4];
			skillGrowth -= gMods[5];
			reflexGrowth -= gMods[6];
			awarenessGrowth -= gMods[7];
			resistanceGrowth -= gMods[8];
		}
		this.gender = h.getGender();
		this.appearance = (int[])h.getAppearance().Clone();
	}

	public string getTemplateName()
	{
		return templateName;
	}

	public int[] getBodyPartsMaximumHP()
	{
		return bodyPartsMaximumHP;
	}

	public int getMagic()
	{
		return magic;
	}

	public int getSkill()
	{
		return skill;
	}

	public int getReflex()
	{
		return reflex;
	}

	public int getAwareness()
	{
		return awareness;
	}

	public int getResistance()
	{
		return resistance;
	}

	public int getMovement()
	{
		return movement;
	}

	public int[] getBodyPartsMaximumHPGrowth()
	{
		return bodyPartsMaximumHPGrowth;
	}

	public int getMagicGrowth()
	{
		return magicGrowth;
	}

	public int getSkillGrowth()
	{
		return skillGrowth;
	}

	public int getReflexGrowth()
	{
		return reflexGrowth;
	}

	public int getAwarenessGrowth()
	{
		return awarenessGrowth;
	}

	public int getResistanceGrowth()
	{
		return resistanceGrowth;
	}

	public bool getGender()
	{
		return gender;
	}

	public int[] getAppearance()
	{
		return appearance;
	}

	public int getSpecificTemplateIndex()
	{
		return specificTemplateIndex;
	}

	public void setSpecificTemplateIndex(int specificTemplateIndex)
	{
		this.specificTemplateIndex = specificTemplateIndex;
	}

	public int getAndIncrementNumCopiesMade()
	{
		int ret = numCopiesMade;
		numCopiesMade++;
		return ret;
	}

}
