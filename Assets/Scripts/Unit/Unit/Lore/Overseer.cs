using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Overseer
{

	private string name;
	private Human favorite;

	public bool checkOpinionOfRuler(Human h)
	{
		if (h != null && h.isAlive())
		{
			return false;
		}
		bool ret = isFavorite(h);
		if (ret)
		{
			this.favorite = h;
			bless();
		}
		return ret;
	}

	public string getName()
	{
		return name;
	}

	/**
	 * Given a nation ruler, determine whether or not they excel in this Overseer's
	 * jurisdiction.
	 * @param h
	 * @return
	 */
	protected abstract bool isFavorite(Human h);

	/**
	 * Buffs the favorite ruler according to the Overseer's jurisdiction
	 */
	protected abstract void bless();

	private class OverseerOfSpace : Overseer
	{


		protected override bool isFavorite(Human h)
		{
			// TODO Auto-generated method stub
			return false;
		}


		protected override void bless()
		{
			// TODO Auto-generated method stub

		}

	}
	private class OverseerOfLight : Overseer
	{


		protected override bool isFavorite(Human h)
		{
			// TODO Auto-generated method stub
			return false;
		}


		protected override void bless()
		{
			// TODO setup the favorite's nation so that the next person recruited will
			//be a Light Magic Champion (with high proficiency in light magic,
			//a high magic growth, and high resistance)
		}

	}
	private class OverseerOfElements : Overseer
	{


		protected override bool isFavorite(Human h)
		{
			// TODO Auto-generated method stub
			return false;
		}


		protected override void bless()
		{
			// TODO setup the favorite's nation so that the next person recruited will
			//be an Anima Magic Champion (with high proficiency in anima magic,
			//a high magic growth, and high resistance)

		}

	}
	private class OverseerOfTime : Overseer
	{


		protected override bool isFavorite(Human h)
		{
			//TODO
			return false;
		}


		protected override void bless()
		{
			//TODO
		}

	}
	private class OverseerOfRealityPerception : Overseer
	{


		protected override bool isFavorite(Human h)
		{
			// TODO Auto-generated method stub
			return false;
		}


		protected override void bless()
		{
			// TODO Auto-generated method stub

		}

	}
	private class OverseerOfDark : Overseer
	{


		protected override bool isFavorite(Human h)
		{
			// TODO Auto-generated method stub
			return false;
		}


		protected override void bless()
		{
			// TODO setup the favorite's nation so that the next person recruited will
			//be a Dark Magic Champion (with high proficiency in dark magic,
			//a high magic growth, and high resistance)
		}

	}
	private class OverseerOfLuck : Overseer
	{


		protected override bool isFavorite(Human h)
		{
			// TODO Auto-generated method stub
			return false;
		}


		protected override void bless()
		{
			// TODO Auto-generated method stub

		}

	}
	private class OverseerOfWar : Overseer
	{


		protected override bool isFavorite(Human h)
		{
			// TODO Auto-generated method stub
			return false;
		}


		protected override void bless()
		{
			// TODO Auto-generated method stub

		}

	}
	private class OverseerOfPeace : Overseer
	{


		protected override bool isFavorite(Human h)
		{
			// TODO Auto-generated method stub
			return false;
		}


		protected override void bless()
		{
			// TODO Auto-generated method stub

		}

	}
	private class OverseerOfConnection : Overseer
	{


		protected override bool isFavorite(Human h)
		{
			// TODO Auto-generated method stub
			return false;
		}


		protected override void bless()
		{
			// TODO Auto-generated method stub

		}

	}
	private class OverseerOfLife : Overseer
	{


		protected override bool isFavorite(Human h)
		{
			//Right now, the average life span of a human unit is around 67.
			//There is about a one percent chance that a human will survive until 90
			return h.getAge() >= 90;
		}


		protected override void bless()
		{
			if (favorite.isMortal())
			{
				favorite.toggleMortality();
				int[] growths = favorite.getBodyPartsMaximumHPGrowth();
				//TODO increase growths (restore unit to youth, but leave age the same)
			}
		}

	}
	private class OverseerOfDeath : Overseer
	{


		protected override bool isFavorite(Human h)
		{
			// TODO Auto-generated method stub
			return false;
		}


		protected override void bless()
		{
			// TODO Auto-generated method stub

		}

	}
}
