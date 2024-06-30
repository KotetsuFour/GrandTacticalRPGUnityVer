using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WorldMapAIManager
{

	public static void sortDecisions(WorldMap map, List<UnitGroup> moveable, List<UnitGroup> enemies)
	{
		//TODO
	}


	public enum ActionType
	{
		ENGAGE,
		CAMP,
		IMPRISON,
		OPTIMIZE,
		JOIN,
		SPLIT,
		DESTROY,
		SEIZE,
		ENTER,
		WARP
	}
}
