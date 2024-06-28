using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SportingEvent : MajorEvent
{

	private Nation nation1;
	private Nation nation2;
	private bool bracket;
	/**
	 * Round types are as follows:
	 * 0 = friendly battle
	 * 1 = battle to the death
	 * 2 = strength test
	 * 3 = accuracy test
	 * 4 = avoidance test
	 */
	private int[] rounds;
	private int currentRound;
	private List<Unit> participants;
	private Coliseum location;

	public SportingEvent(Nation nation1, Nation nation2, Coliseum location, long startDate)
			: base(startDate)
	{
		this.nation1 = nation1;
		this.nation2 = nation2;
		this.location = location;
		//TODO set name
	}
	public void setBracket(int[] rounds, List<Unit> participants)
	{
		if (participants.Count != Mathf.Pow(rounds.Length, 2))
		{
			throw new Exception();
		}
		this.bracket = true;
		this.rounds = rounds;
		this.participants = participants;
	}
	public void setNormalRounds(int[] rounds, List<Unit> participants)
	{
		this.rounds = rounds;
		this.participants = participants;
	}
}
