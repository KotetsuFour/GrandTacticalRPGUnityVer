using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class MajorEvent
{

	protected long startDate;
	protected List<HistoricalRecord> relevantOccurences;
	protected string name;

	public MajorEvent(long startDate)
	{
		this.startDate = startDate;
		//TODO relevant occurrences is probably not needed. We'll probably only store
		//those in each nation's history list
		relevantOccurences = new List<HistoricalRecord>();
	}

	public string getName()
	{
		return name;
	}

}
