using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class GeneralGameplayManager
{

	private static WorldMap worldMap;
	private static Nation playerNation;
	private static Human player;
	private static long daysSinceGameStart;
	private static bool aging;
	private static int actionsLeft;
	private static bool isInEventTime;

	private static List<Nation> npcNations;

	public static int DAYS_IN_MONTH = 30;
	public static int DAYS_IN_YEAR = DAYS_IN_MONTH * HistoricalRecord.MONTH_NAMES.Length;
	public static int ACTIONS_PER_DAY = 5;
	public static int INITIAL_NPC_NATION_COUNT = 4;
	public static string[] INITIAL_NATION_BUILDINGS = { Building.CASTLE, Building.VILLAGE, Building.FARM,
		Building.VILLAGE, Building.STOREHOUSE, Building.MINING_FACILITY, Building.VILLAGE,
		Building.TRAINING_FACILITY};

	/**
	 * Tells if the game is being played in event time
	 * @return true if the player's nation is in the middle of a major event, in which case,
	 * processes should move day-by-day instead of month-by-month
	 */
	public static bool eventTime()
	{
		return isInEventTime;
	}

	public static void switchToEventTime()
	{
		if (!isInEventTime)
		{
			isInEventTime = true;
			if (actionsLeft < ACTIONS_PER_DAY * DAYS_IN_MONTH)
			{
				//TODO determine day based on actions used
			}
			else
			{
				actionsLeft = 0;
			}
		}
	}

	public static long getDaysSinceGameStart()
	{
		return daysSinceGameStart;
	}

	public static int getActionsLeft()
	{
		return actionsLeft;
	}
	public static void resetMonthActions()
	{
		actionsLeft = DAYS_IN_MONTH * ACTIONS_PER_DAY;
	}
	public static void resetDayActions()
	{
		actionsLeft += ACTIONS_PER_DAY;
	}

	public static void endPlayerTurn()
	{
		//TODO un-exhaust all units
		if (eventTime())
		{
			resetDayActions();
			daysSinceGameStart++;
			getPlayerNation().passDay(aging && daysSinceGameStart % DAYS_IN_YEAR == 0);
		}
		else
		{
			resetMonthActions();
			daysSinceGameStart += DAYS_IN_MONTH;
			getPlayerNation().passMonth(aging && daysSinceGameStart % DAYS_IN_YEAR == 0);
			//TODO everyone else does their turn, then it's the player's turn again
		}
	}

	public static Nation getPlayerNation()
	{
		return playerNation;
	}

	public static void indexesInitialization()
	{
		InventoryIndex.initialize();
		UnitClassIndex.initialize();
		ShipIndex.initialize();
		BattlegroundTileIndex.initialize();
		ArtificialHumanIndex.initialize();
	}

	public static Human getPlayer()
	{
		return player;
	}

	public static void addBuildingToCityAndMap(Building b, CityState cs, WorldMapTile tile)
	{
		tile.setBuilding(b);
		cs.addBuilding(b);
	}

	public static void claimTileForNation(CityState cs, WorldMapTile tile)
	{
		tile.setOwner(cs);
		cs.incrementSize();
	}

	public static void initializePlayerNation(string name, string capitalName, int type,
			int language)
	{
		worldMap = new WorldMap();
		isInEventTime = false;
		playerNation = new Nation(name, capitalName, type, language);
		playerNation.setRuler(player);
		resetMonthActions();
	}

	public static void initializeNPCNations()
	{
		npcNations = new List<Nation>(9);
		setNPCNationNearPosition(0, 0);
		setNPCNationNearPosition(0, WorldMap.SQRT_OF_MAP_SIZE - 1);
		setNPCNationNearPosition(WorldMap.SQRT_OF_MAP_SIZE - 1, 0);
		setNPCNationNearPosition(WorldMap.SQRT_OF_MAP_SIZE - 1, WorldMap.SQRT_OF_MAP_SIZE - 1);
	}

	private static void setNationNearPosition(Nation n, CityState cs, Human r, int x, int y)
    {
		int[] startCoords = nearestStartingPointForNation(x, y);
		List<int[]> area = buildableAreaOfSizeAroundPoint(startCoords[0], startCoords[1], INITIAL_NATION_BUILDINGS.Length);
		for (int q = 0; q < area.Count; q++)
		{
			int[] coords = area[q];
			claimTileForNation(cs, worldMap.at(coords[0], coords[1]));
			if (INITIAL_NATION_BUILDINGS[q] == Building.CASTLE)
			{
				setCastle(cs, coords[0], coords[1], r);
			}
			else
			{
				setBuilding(cs, coords[0], coords[1], INITIAL_NATION_BUILDINGS[q]);
			}
			worldMap.at(coords[0], coords[1]);
		}
	}

	private static void setNPCNationNearPosition(int x, int y)
    {
		Nation n = new Nation();
		CityState cs = n.getCapital();
		Human r = n.getRuler();
		npcNations.Add(n);

		setNationNearPosition(n, cs, r, x, y);
	}

	private static int[] nearestStartingPointForNation(int x, int y)
    {
		Queue<int[]> queue = new Queue<int[]>();
		queue.Enqueue(new int[] { x, y });
		while (worldMap.at(queue.Peek()[0], queue.Peek()[1]).getType() == WorldMapTile.WorldMapTileType.DEEP_WATER
			|| worldMap.at(queue.Peek()[0], queue.Peek()[1]).getType() == WorldMapTile.WorldMapTileType.SHALLOW_WATER)
        {
			int[] next = queue.Dequeue();
			if (next[0] > 0)
            {
				queue.Enqueue(new int[] { next[0] - 1, next[1] });
            }
			if (next[0] < WorldMap.SQRT_OF_MAP_SIZE - 1)
			{
				queue.Enqueue(new int[] { next[0] + 1, next[1] });
			}
			if (next[1] > 0)
			{
				queue.Enqueue(new int[] { next[0], next[1] - 1 });
			}
			if (next[1] < WorldMap.SQRT_OF_MAP_SIZE - 1)
			{
				queue.Enqueue(new int[] { next[0], next[1] + 1 });
			}
		}
		return queue.Dequeue();
	}
	private static List<int[]> buildableAreaOfSizeAroundPoint(int x, int y, int targetSize)
    {
		List<int[]> ret = new List<int[]>();
		Queue<int[]> queue = new Queue<int[]>();
		queue.Enqueue(new int[] { x, y });
		while (queue.Count > 0 && ret.Count < targetSize)
        {
			int[] next = queue.Dequeue();
			if (worldMap.at(next[0], next[1]).getType() != WorldMapTile.WorldMapTileType.DEEP_WATER
				&& worldMap.at(next[0], next[1]).getType() != WorldMapTile.WorldMapTileType.SHALLOW_WATER)
            {
				bool cont = false;
				for (int q = 0; q < ret.Count; q++)
				{
					if (ret[q][0] == next[0] && ret[q][1] == next[1])
					{
						cont = true;
					}
				}
				if (cont)
                {
					continue;
                }
				ret.Add(next);
				if (next[0] > 0)
				{
					queue.Enqueue(new int[] { next[0] - 1, next[1] });
				}
				if (next[0] < WorldMap.SQRT_OF_MAP_SIZE - 1)
				{
					queue.Enqueue(new int[] { next[0] + 1, next[1] });
				}
				if (next[1] > 0)
				{
					queue.Enqueue(new int[] { next[0], next[1] - 1 });
				}
				if (next[1] < WorldMap.SQRT_OF_MAP_SIZE - 1)
				{
					queue.Enqueue(new int[] { next[0], next[1] + 1 });
				}

			}
		}
		return ret;
    }

	public static void setBuilding(CityState city, int x, int y, string buildingType)
    {
		if (buildingType == Building.BARRACKS)
        {
			addBuildingToCityAndMap(new Barracks(RNGStuff.newLocationName(city.getLanguage()),
				Human.completelyRandomHuman(city), worldMap.at(x, y)),
				city, worldMap.at(x, y));
		}
		else if (buildingType == Building.COLISEUM)
        {
			addBuildingToCityAndMap(new Coliseum(RNGStuff.newLocationName(city.getLanguage()),
				Human.completelyRandomHuman(city), worldMap.at(x, y)),
				city, worldMap.at(x, y));
		}
		else if (buildingType == Building.FACTORY)
		{
			addBuildingToCityAndMap(new Factory(RNGStuff.newLocationName(city.getLanguage()),
				Human.completelyRandomHuman(city), worldMap.at(x, y)),
				city, worldMap.at(x, y));
		}
		else if (buildingType == Building.FARM)
		{
			addBuildingToCityAndMap(new Farm(RNGStuff.newLocationName(city.getLanguage()),
				Human.completelyRandomHuman(city), worldMap.at(x, y)),
				city, worldMap.at(x, y));
		}
		else if (buildingType == Building.FORTRESS)
		{
			addBuildingToCityAndMap(new Fortress(RNGStuff.newLocationName(city.getLanguage()),
				Human.completelyRandomHuman(city), worldMap.at(x, y)),
				city, worldMap.at(x, y));
		}
		else if (buildingType == Building.HOSPITAL)
		{
			addBuildingToCityAndMap(new Hospital(RNGStuff.newLocationName(city.getLanguage()),
				Human.completelyRandomHuman(city), worldMap.at(x, y)),
				city, worldMap.at(x, y));
		}
		else if (buildingType == Building.MAGIC_PROCESSING_FACILITY)
		{
			addBuildingToCityAndMap(new MagicProcessingFacility(RNGStuff.newLocationName(city.getLanguage()),
				Human.completelyRandomHuman(city), worldMap.at(x, y)),
				city, worldMap.at(x, y));
		}
		else if (buildingType == Building.MINING_FACILITY)
		{
			addBuildingToCityAndMap(new MiningFacility(RNGStuff.newLocationName(city.getLanguage()),
				Human.completelyRandomHuman(city), worldMap.at(x, y)),
				city, worldMap.at(x, y));
		}
		else if (buildingType == Building.PORT)
		{
			addBuildingToCityAndMap(new Port(RNGStuff.newLocationName(city.getLanguage()),
				Human.completelyRandomHuman(city), worldMap.at(x, y)),
				city, worldMap.at(x, y));
		}
		else if (buildingType == Building.PRISON)
		{
			addBuildingToCityAndMap(new Prison(RNGStuff.newLocationName(city.getLanguage()),
				Human.completelyRandomHuman(city), worldMap.at(x, y)),
				city, worldMap.at(x, y));
		}
		else if (buildingType == Building.RANCH)
		{
			addBuildingToCityAndMap(new Ranch(RNGStuff.newLocationName(city.getLanguage()),
				Human.completelyRandomHuman(city), worldMap.at(x, y)),
				city, worldMap.at(x, y));
		}
		else if (buildingType == Building.RESEARCH_CENTER)
		{
			addBuildingToCityAndMap(new ResearchCenter(RNGStuff.newLocationName(city.getLanguage()),
				Human.completelyRandomHuman(city), worldMap.at(x, y)),
				city, worldMap.at(x, y));
		}
		else if (buildingType == Building.SHIPYARD)
		{
			addBuildingToCityAndMap(new Shipyard(RNGStuff.newLocationName(city.getLanguage()),
				Human.completelyRandomHuman(city), worldMap.at(x, y)),
				city, worldMap.at(x, y));
		}
		else if (buildingType == Building.STOREHOUSE)
		{
			addBuildingToCityAndMap(new Storehouse(RNGStuff.newLocationName(city.getLanguage()),
				Human.completelyRandomHuman(city), worldMap.at(x, y)),
				city, worldMap.at(x, y));
		}
		else if (buildingType == Building.TRADE_CENTER)
		{
			addBuildingToCityAndMap(new TradeCenter(RNGStuff.newLocationName(city.getLanguage()),
				Human.completelyRandomHuman(city), worldMap.at(x, y)),
				city, worldMap.at(x, y));
		}
		else if (buildingType == Building.TRAINING_FACILITY)
		{
			addBuildingToCityAndMap(new TrainingFacility(RNGStuff.newLocationName(city.getLanguage()),
				Human.completelyRandomHuman(city), worldMap.at(x, y)),
				city, worldMap.at(x, y));
		}
		else if (buildingType == Building.VILLAGE)
		{
			addBuildingToCityAndMap(new Village(RNGStuff.newLocationName(city.getLanguage()), city, worldMap.at(x, y)),
				city, worldMap.at(x, y));
		}
		else if (buildingType == Building.WARP_PAD)
		{
			addBuildingToCityAndMap(new WarpPad(RNGStuff.newLocationName(city.getLanguage()),
				Human.completelyRandomHuman(city), worldMap.at(x, y)),
				city, worldMap.at(x, y));
		}
	}
	public static void setCastle(CityState city, int x, int y, Human ruler)
	{
		Castle castle = new Castle(ruler, worldMap.at(x, y));
		new UnitGroup(ruler);
		castle.assignGroup(ruler.getGroup());
		addBuildingToCityAndMap(castle, city, worldMap.at(x, y));
	}

	public static void initializePlayer(string pName, bool pGender, int pFace, int pNose, int pLips, int pEar,
			int pEye, int pIris, int pBrow, int pHair, int pStache, int pBeard, int pInterest1, int pInterest2,
			int pInterest3, int pInterest4, int pInterest5, int pInterest6, int pTrait, int pDemeanor, int pHpBoon,
			int pHpBane, int pAttributeBoon, int pAttributeBane, int pHairColor,
			float skinRed, float skinGreen, float skinBlue,
			int pEyeColor)
	{
		Human.Interest[] interestsList = Human.Interest.values();
		int[] bodyPartsMaximumHP = {15/**Head*/, 23/**Torso*/, 15, 15/**Arms*/,
				17, 17/**Legs*/, 7, 7/**Eyes*/, 0/**Mount*/};
		int[] bodyPartsMaximumHPGrowth = {35/**Head*/, 45/**Torso*/, 40, 40/**Arms*/,
				45, 45/**Legs*/, 0, 0/**Eyes*/, 0/**Mount*/};
		if (pHpBoon == 0)
		{
			bodyPartsMaximumHP[Human.HEAD] += 5;
			bodyPartsMaximumHPGrowth[Human.HEAD] = (int)(bodyPartsMaximumHPGrowth[Human.HEAD] * 1.5);
		}
		else if (pHpBoon == 1)
		{
			bodyPartsMaximumHP[Human.TORSO] += 5;
			bodyPartsMaximumHPGrowth[Human.TORSO] = (int)(bodyPartsMaximumHPGrowth[Human.TORSO] * 1.5);
		}
		else if (pHpBoon == 2)
		{
			bodyPartsMaximumHP[Human.RIGHT_ARM] += 5;
			bodyPartsMaximumHPGrowth[Human.RIGHT_ARM] = (int)(bodyPartsMaximumHPGrowth[Human.RIGHT_ARM] * 1.5);
			bodyPartsMaximumHP[Human.LEFT_ARM] = bodyPartsMaximumHP[Human.RIGHT_ARM];
			bodyPartsMaximumHPGrowth[Human.LEFT_ARM] = bodyPartsMaximumHPGrowth[Human.RIGHT_ARM];
		}
		else if (pHpBoon == 3)
		{
			bodyPartsMaximumHP[Human.RIGHT_LEG] += 5;
			bodyPartsMaximumHPGrowth[Human.RIGHT_LEG] = (int)(bodyPartsMaximumHPGrowth[Human.RIGHT_LEG] * 1.5);
			bodyPartsMaximumHP[Human.LEFT_LEG] = bodyPartsMaximumHP[Human.RIGHT_LEG];
			bodyPartsMaximumHPGrowth[Human.LEFT_LEG] = bodyPartsMaximumHPGrowth[Human.RIGHT_LEG];
		}
		if (pHpBane == 0)
		{
			bodyPartsMaximumHP[Human.HEAD] -= 5;
			bodyPartsMaximumHPGrowth[Human.HEAD] /= 2;
		}
		else if (pHpBane == 1)
		{
			bodyPartsMaximumHP[Human.TORSO] -= 5;
			bodyPartsMaximumHPGrowth[Human.TORSO] /= 2;
		}
		else if (pHpBane == 2)
		{
			bodyPartsMaximumHP[Human.RIGHT_ARM] -= 5;
			bodyPartsMaximumHPGrowth[Human.RIGHT_ARM] /= 2;
			bodyPartsMaximumHP[Human.LEFT_ARM] = bodyPartsMaximumHP[Human.RIGHT_ARM];
			bodyPartsMaximumHPGrowth[Human.LEFT_ARM] = bodyPartsMaximumHPGrowth[Human.RIGHT_ARM];
		}
		else if (pHpBane == 3)
		{
			bodyPartsMaximumHP[Human.RIGHT_LEG] -= 5;
			bodyPartsMaximumHPGrowth[Human.RIGHT_LEG] /= 2;
			bodyPartsMaximumHP[Human.LEFT_LEG] = bodyPartsMaximumHP[Human.RIGHT_LEG];
			bodyPartsMaximumHPGrowth[Human.LEFT_LEG] = bodyPartsMaximumHPGrowth[Human.RIGHT_LEG];
		}
		int[] attributes = {5/**Magic*/, 6/**Skill*/, 5/**Reflex*/, 6/**Awareness*/,
				4/**Resistance*/};
		int[] attributeGrowths = {15/**Magic*/, 25/**Skill*/, 25/**Reflex*/, 25/**Awareness*/,
				5/**Resistance*/};
		attributes[pAttributeBoon] *= 2;
		attributeGrowths[pAttributeBoon] *= 2;
		attributes[pAttributeBane] /= 2;
		attributeGrowths[pAttributeBane] /= 2;
		int ldr = 5;
		int mov = 6;
		int age = 25;
		int[] values = { 50, 50, 50, 50, 50, 50 };
		int red = (int)Mathf.RoundToInt(skinRed * 255);
		int green = (int)Mathf.RoundToInt(skinGreen * 255);
		int blue = (int)Mathf.RoundToInt(skinBlue * 255);
		int[] appearance = {pFace, pLips, pNose, pEar, pEye, pIris, pBrow, pHair, pStache,
				pBeard, pHairColor, red, green, blue, pEyeColor};
		Human.Interest[] interests = {interestsList[pInterest1], interestsList[pInterest2],
				interestsList[pInterest3]};
		Human.Interest[] disinterests = {interestsList[pInterest4], interestsList[pInterest5],
				interestsList[pInterest6]};

		player = new Human(pName, pGender, bodyPartsMaximumHP, attributes[0], attributes[1],
				attributes[2], attributes[3], attributes[4], mov, ldr, bodyPartsMaximumHPGrowth,
				attributeGrowths[0], attributeGrowths[1], attributeGrowths[2],
				attributeGrowths[3], attributeGrowths[4], age, values, appearance, interests,
				disinterests, Demeanor.values()[pDemeanor], Human.CombatTrait.values()[pTrait],
				playerNation.getCapital());
		player.toggleImportance();
		player.toggleMortality();
		playerNation.setRuler(player);
		playerNation.getArmy().Add(player);
		new UnitGroup(player); //The constructor for UnitGroup automatically adds itself the members' nation
							   //TODO figure out actual coordinates

		setNationNearPosition(playerNation, playerNation.getCapital(), playerNation.getRuler(), WorldMap.SQRT_OF_MAP_SIZE / 2, WorldMap.SQRT_OF_MAP_SIZE / 2);
	}

	public static WorldMap getWorldMap()
	{
		return worldMap;
	}

	public static void setAging(bool shouldAge)
	{
		//TODO set this value in database
		aging = shouldAge;
	}

	public static void addPlayerCityState(WorldMapTile tile)
	{
		addPlayerCityState(RNGStuff.newLocationName(playerNation.getNationalLanguage()), tile);
	}

	public static void addPlayerCityState(string name, WorldMapTile tile)
	{
		//City-state automatically adds itself to the nation's city list
		CityState cs = new CityState(name, getPlayerNation());
		claimTileForNation(cs, tile);
		addBuildingToCityAndMap(new Village(cs, tile), cs, tile);
	}

	public static Dictionary<WorldMapTile, object> getTraversableWorldMapTilesPeaceTime(UnitGroup group,
			int x, int y)
	{
		Dictionary<WorldMapTile, object> traversable = new Dictionary<WorldMapTile, object>(800);
		LinkedQueue<int[]> searchList = new LinkedQueue<int[]>(); //[0] = x, [1] = y, [2] = remainingMovement
		searchList.add(new int[] { x, y, group.getMovement() });
		while (!(searchList.isEmpty()))
		{
			int[] from = searchList.pop();
			WorldMapTile fromTile = worldMap.at(from[0], from[1]);
			traversable.Add(fromTile, from[2]);
			if (from[2] == 0)
			{
				continue;
			}
			if (from[0] > 0)
			{
				int checkX = from[0] - 1;
				int checkY = from[1];
				WorldMapTile check = worldMap.at(from[0] - 1, from[1]);
				if ((!traversable.ContainsKey(check) || (int)traversable[check] < from[2])
						&& from[2] - check.getMoveCost(group) >= 0
						&& check.isVacant()
						&& (check.getOwner() == null || check.getAffiliation() == group.getAffiliation())
						&& check.getBattle() == null)
				{
					if (check.getOwner() == null || check.getOwner() != group.getLocation().getOwner())
					{
						searchList.add(new int[] { checkX, checkY, from[2] - check.getMoveCost(group) });
					}
					else
					{
						searchList.add(new int[] { checkX, checkY, Mathf.Max(1, from[2] - check.getMoveCost(group)) });
					}
				}
			}
			if (from[0] < WorldMap.SQRT_OF_MAP_SIZE - 1)
			{
				int checkX = from[0] + 1;
				int checkY = from[1];
				WorldMapTile check = worldMap.at(from[0] + 1, from[1]);
				if ((!traversable.ContainsKey(check) || (int)traversable[check] < from[2])
						&& from[2] - check.getMoveCost(group) >= 0
						&& check.isVacant()
						&& (check.getOwner() == null || check.getAffiliation() == group.getAffiliation())
						&& check.getBattle() == null)
				{
					if (check.getOwner() == null || check.getOwner() != group.getLocation().getOwner())
					{
						searchList.add(new int[] { checkX, checkY, from[2] - check.getMoveCost(group) });
					}
					else
					{
						searchList.add(new int[] { checkX, checkY, Mathf.Max(1, from[2] - check.getMoveCost(group)) });
					}
				}
			}
			if (from[1] > 0)
			{
				int checkX = from[0];
				int checkY = from[1] - 1;
				WorldMapTile check = worldMap.at(from[0], from[1] - 1);
				if ((!traversable.ContainsKey(check) || (int)traversable[check] < from[2])
						&& from[2] - check.getMoveCost(group) >= 0
						&& check.isVacant()
						&& (check.getOwner() == null || check.getAffiliation() == group.getAffiliation())
						&& check.getBattle() == null)
				{
					if (check.getOwner() == null || check.getOwner() != group.getLocation().getOwner())
					{
						searchList.add(new int[] { checkX, checkY, from[2] - check.getMoveCost(group) });
					}
					else
					{
						searchList.add(new int[] { checkX, checkY, Mathf.Max(1, from[2] - check.getMoveCost(group)) });
					}
				}
			}
			if (from[1] < WorldMap.SQRT_OF_MAP_SIZE - 1)
			{
				int checkX = from[0];
				int checkY = from[1] + 1;
				WorldMapTile check = worldMap.at(from[0], from[1] + 1);
				if ((!traversable.ContainsKey(check) || (int)traversable[check] < from[2])
						&& from[2] - check.getMoveCost(group) >= 0
						&& check.isVacant()
						&& (check.getOwner() == null || check.getAffiliation() == group.getAffiliation())
						&& check.getBattle() == null)
				{
					if (check.getOwner() == null || check.getOwner() != group.getLocation().getOwner())
					{
						searchList.add(new int[] { checkX, checkY, from[2] - check.getMoveCost(group) });
					}
					else
					{
						searchList.add(new int[] { checkX, checkY, Mathf.Max(1, from[2] - check.getMoveCost(group)) });
					}
				}
			}
		}
		return traversable;
	}

	public static Dictionary<WorldMapTile, object> getTraversableWorldMapTilesEventTime(UnitGroup group,
			int x, int y)
	{
		Dictionary<WorldMapTile, object> traversable = new Dictionary<WorldMapTile, object>(800);
		LinkedQueue<int[]> searchList = new LinkedQueue<int[]>(); //[0] = x, [1] = y, [2] = remainingMovement
		searchList.add(new int[] { x, y, group.getMovement() });
		while (!(searchList.isEmpty()))
		{
			int[] from = searchList.pop();
			WorldMapTile fromTile = worldMap.at(from[0], from[1]);
			traversable.Add(fromTile, from[2]);
			if (from[2] == 0)
			{
				continue;
			}
			//TODO Maybe change to allow traversal of allied defended buildings
			if (from[0] > 0)
			{
				int checkX = from[0] - 1;
				int checkY = from[1];
				WorldMapTile check = worldMap.at(from[0] - 1, from[1]);
				if ((!traversable.ContainsKey(check) || (int)traversable[check] < from[2])
						&& from[2] - check.getMoveCost(group) >= 0
						&& check.isVacant()
						&& (check.getOwner() == null
						|| check.getOwner().getNation() == group.getAffiliation()
						|| check.getOwner().getNation().isAlliedWith(check.getOwner().getNation())
						|| group.getAffiliation().isAtWarWith(check.getOwner().getNation()))
						&& (!(check.getBuilding() is Defendable)
						|| check.getAffiliation() == group.getAffiliation()
						|| ((Defendable)check.getBuilding()).getAssignedGroup() == null)
						&& check.getBattle() == null) {
					searchList.add(new int[] { checkX, checkY, from[2] - check.getMoveCost(group) });
				}
			}
			if (from[0] < WorldMap.SQRT_OF_MAP_SIZE - 1)
			{
				int checkX = from[0] + 1;
				int checkY = from[1];
				WorldMapTile check = worldMap.at(from[0] + 1, from[1]);
				if ((!traversable.ContainsKey(check) || (int)traversable[check] < from[2])
						&& from[2] - check.getMoveCost(group) >= 0
						&& check.isVacant()
						&& (check.getOwner() == null
						|| check.getOwner().getNation() == group.getAffiliation()
						|| check.getOwner().getNation().isAlliedWith(check.getOwner().getNation())
						|| group.getAffiliation().isAtWarWith(check.getOwner().getNation()))
						&& (!(check.getBuilding() is Defendable)
						|| check.getAffiliation() == group.getAffiliation()
						|| ((Defendable)check.getBuilding()).getAssignedGroup() == null)
						&& check.getBattle() == null) {
					searchList.add(new int[] { checkX, checkY, from[2] - check.getMoveCost(group) });
				}
			}
			if (from[1] > 0)
			{
				int checkX = from[0];
				int checkY = from[1] - 1;
				WorldMapTile check = worldMap.at(from[0], from[1] - 1);
				if ((!traversable.ContainsKey(check) || (int)traversable[check] < from[2])
						&& from[2] - check.getMoveCost(group) >= 0
						&& check.isVacant()
						&& (check.getOwner() == null
						|| check.getOwner().getNation() == group.getAffiliation()
						|| check.getOwner().getNation().isAlliedWith(check.getOwner().getNation())
						|| group.getAffiliation().isAtWarWith(check.getOwner().getNation()))
						&& (!(check.getBuilding() is Defendable)
						|| check.getAffiliation() == group.getAffiliation()
						|| ((Defendable)check.getBuilding()).getAssignedGroup() == null)
						&& check.getBattle() == null) {
					searchList.add(new int[] { checkX, checkY, from[2] - check.getMoveCost(group) });
				}
			}
			if (from[1] < WorldMap.SQRT_OF_MAP_SIZE - 1)
			{
				int checkX = from[0];
				int checkY = from[1] + 1;
				WorldMapTile check = worldMap.at(from[0], from[1] + 1);
				if ((!traversable.ContainsKey(check) || (int)traversable[check] < from[2])
						&& from[2] - check.getMoveCost(group) >= 0
						&& check.isVacant()
						&& (check.getOwner() == null
						|| check.getOwner().getNation() == group.getAffiliation()
						|| check.getOwner().getNation().isAlliedWith(check.getOwner().getNation())
						|| group.getAffiliation().isAtWarWith(check.getOwner().getNation()))
						&& (!(check.getBuilding() is Defendable)
						|| check.getAffiliation() == group.getAffiliation()
						|| ((Defendable)check.getBuilding()).getAssignedGroup() == null)
						&& check.getBattle() == null) {
					searchList.add(new int[] { checkX, checkY, from[2] - check.getMoveCost(group) });
				}
			}
		}
		return traversable;
	}

	public static Dictionary<WorldMapTile, object> getTraversableTilesForShipPeaceTime(Ship group,
			int x, int y)
	{
		Dictionary<WorldMapTile, object> traversable = new Dictionary<WorldMapTile, object>(200);
		LinkedQueue<int[]> searchList = new LinkedQueue<int[]>(); //[0] = x, [1] = y, [2] = remainingMovement
		searchList.add(new int[] { x, y, group.getMovement() });
		while (!(searchList.isEmpty()))
		{
			int[] from = searchList.pop();
			WorldMapTile fromTile = worldMap.at(from[0], from[1]);
			traversable.Add(fromTile, from[2]);
			if (from[2] == 0)
			{
				continue;
			}
			if (from[0] > 0)
			{
				int checkX = from[0] - 1;
				int checkY = from[1];
				WorldMapTile check = worldMap.at(from[0] - 1, from[1]);
				if ((!traversable.ContainsKey(check) || (int)traversable[check] < from[2])
						&& from[2] - check.getMoveCost(group) >= 0
						&& check.isVacant()
						&& check.getBattle() == null)
				{
					searchList.add(new int[] { checkX, checkY, from[2] - check.getMoveCost(group) });
				}
			}
			if (from[0] < WorldMap.SQRT_OF_MAP_SIZE - 1)
			{
				int checkX = from[0] + 1;
				int checkY = from[1];
				WorldMapTile check = worldMap.at(from[0] + 1, from[1]);
				if ((!traversable.ContainsKey(check) || (int)traversable[check] < from[2])
						&& from[2] - check.getMoveCost(group) >= 0
						&& check.isVacant())
				{
					if ((!traversable.ContainsKey(check) || (int)traversable[check] < from[2])
							&& from[2] - check.getMoveCost(group) >= 0
							&& check.isVacant()
							&& check.getBattle() == null)
					{
						searchList.add(new int[] { checkX, checkY, from[2] - check.getMoveCost(group) });
					}
				}
			}
			if (from[1] > 0)
			{
				int checkX = from[0];
				int checkY = from[1] - 1;
				WorldMapTile check = worldMap.at(from[0], from[1] - 1);
				if ((!traversable.ContainsKey(check) || (int)traversable[check] < from[2])
						&& from[2] - check.getMoveCost(group) >= 0
						&& check.isVacant())
				{
					if ((!traversable.ContainsKey(check) || (int)traversable[check] < from[2])
							&& from[2] - check.getMoveCost(group) >= 0
							&& check.isVacant()
							&& check.getBattle() == null)
					{
						searchList.add(new int[] { checkX, checkY, from[2] - check.getMoveCost(group) });
					}
				}
			}
			if (from[1] < WorldMap.SQRT_OF_MAP_SIZE - 1)
			{
				int checkX = from[0];
				int checkY = from[1] + 1;
				WorldMapTile check = worldMap.at(from[0], from[1] + 1);
				if ((!traversable.ContainsKey(check) || (int)traversable[check] < from[2])
						&& from[2] - check.getMoveCost(group) >= 0
						&& check.isVacant())
				{
					if ((!traversable.ContainsKey(check) || (int)traversable[check] < from[2])
							&& from[2] - check.getMoveCost(group) >= 0
							&& check.isVacant()
							&& check.getBattle() == null)
					{
						searchList.add(new int[] { checkX, checkY, from[2] - check.getMoveCost(group) });
					}
				}
			}
		}
		return traversable;
	}

	public static Dictionary<WorldMapTile, object> getTraversableTilesForShipEventTime(Ship group,
			int x, int y)
	{
		Dictionary<WorldMapTile, object> traversable = new Dictionary<WorldMapTile, object>(200);
		LinkedQueue<int[]> searchList = new LinkedQueue<int[]>(); //[0] = x, [1] = y, [2] = remainingMovement
		searchList.add(new int[] { x, y, group.getMovement() });
		while (!(searchList.isEmpty()))
		{
			int[] from = searchList.pop();
			WorldMapTile fromTile = worldMap.at(from[0], from[1]);
			traversable.Add(fromTile, from[2]);
			if (from[2] == 0)
			{
				continue;
			}
			if (from[0] > 0)
			{
				int checkX = from[0] - 1;
				int checkY = from[1];
				WorldMapTile check = worldMap.at(from[0] - 1, from[1]);
				if ((!traversable.ContainsKey(check) || (int)traversable[check] < from[2])
						&& from[2] - check.getMoveCost(group) >= 0
						&& check.isVacant()
						&& (check.getOwner() == null
						|| check.getOwner().getNation() == group.getAffiliation()
						|| check.getOwner().getNation().isAlliedWith(check.getOwner().getNation())
						|| group.getAffiliation().isAtWarWith(check.getOwner().getNation())))
				{
					searchList.add(new int[] { checkX, checkY, from[2] - check.getMoveCost(group) });
				}
			}
			if (from[0] < WorldMap.SQRT_OF_MAP_SIZE - 1)
			{
				int checkX = from[0] + 1;
				int checkY = from[1];
				WorldMapTile check = worldMap.at(from[0] + 1, from[1]);
				if ((!traversable.ContainsKey(check) || (int)traversable[check] < from[2])
						&& from[2] - check.getMoveCost(group) >= 0
						&& check.isVacant()
						&& (check.getOwner() == null
						|| check.getOwner().getNation() == group.getAffiliation()
						|| check.getOwner().getNation().isAlliedWith(check.getOwner().getNation())
						|| group.getAffiliation().isAtWarWith(check.getOwner().getNation())))
				{
					searchList.add(new int[] { checkX, checkY, from[2] - check.getMoveCost(group) });
				}
			}
			if (from[1] > 0)
			{
				int checkX = from[0];
				int checkY = from[1] - 1;
				WorldMapTile check = worldMap.at(from[0], from[1] - 1);
				if ((!traversable.ContainsKey(check) || (int)traversable[check] < from[2])
						&& from[2] - check.getMoveCost(group) >= 0
						&& check.isVacant()
						&& (check.getOwner() == null
						|| check.getOwner().getNation() == group.getAffiliation()
						|| check.getOwner().getNation().isAlliedWith(check.getOwner().getNation())
						|| group.getAffiliation().isAtWarWith(check.getOwner().getNation())))
				{
					searchList.add(new int[] { checkX, checkY, from[2] - check.getMoveCost(group) });
				}
			}
			if (from[1] < WorldMap.SQRT_OF_MAP_SIZE - 1)
			{
				int checkX = from[0];
				int checkY = from[1] + 1;
				WorldMapTile check = worldMap.at(from[0], from[1] + 1);
				if ((!traversable.ContainsKey(check) || (int)traversable[check] < from[2])
						&& from[2] - check.getMoveCost(group) >= 0
						&& check.isVacant()
						&& (check.getOwner() == null
						|| check.getOwner().getNation() == group.getAffiliation()
						|| check.getOwner().getNation().isAlliedWith(check.getOwner().getNation())
						|| group.getAffiliation().isAtWarWith(check.getOwner().getNation())))
				{
					searchList.add(new int[] { checkX, checkY, from[2] - check.getMoveCost(group) });
				}
			}
		}
		return traversable;
	}

	public static Dictionary<BattlegroundTile, object> getTraversableBattlegroundTiles(BattleGround battleground, Unit u, int x, int y)
	{
		Dictionary<BattlegroundTile, object> traversable = new Dictionary<BattlegroundTile, object>(800);
		LinkedQueue<int[]> searchList = new LinkedQueue<int[]>(); //[0] = x, [1] = y, [2] = remainingMovement
		searchList.add(new int[] { x, y, u.getMovement() });
		int[] dimensions = battleground.getDimensions();
		while (!(searchList.isEmpty()))
		{
			int[] from = searchList.pop();
			BattlegroundTile fromTile = battleground.getMap()[from[0]][from[1]];
			traversable.Add(fromTile, from[2]);
			if (from[2] == 0)
			{
				continue;
			}
			if (from[0] > 0)
			{
				int checkX = from[0] - 1;
				int checkY = from[1];
				BattlegroundTile check = battleground.getMap()[checkX][checkY];
				if ((!traversable.ContainsKey(check) || (int)traversable[check] < from[2])
						&& from[2] - check.getMoveCost(u) >= 0
						&& check.isVacant())
				{
					searchList.add(new int[] { checkX, checkY, from[2] - check.getMoveCost(u) });
				}
			}
			if (from[0] < dimensions[0] - 1)
			{
				int checkX = from[0] + 1;
				int checkY = from[1];
				BattlegroundTile check = battleground.getMap()[checkX][checkY];
				if ((!traversable.ContainsKey(check) || (int)traversable[check] < from[2])
						&& from[2] - check.getMoveCost(u) >= 0
						&& check.isVacant())
				{
					searchList.add(new int[] { checkX, checkY, from[2] - check.getMoveCost(u) });
				}
			}
			if (from[1] > 0)
			{
				int checkX = from[0];
				int checkY = from[1] - 1;
				BattlegroundTile check = battleground.getMap()[checkX][checkY];
				if ((!traversable.ContainsKey(check) || (int)traversable[check] < from[2])
						&& from[2] - check.getMoveCost(u) >= 0
						&& check.isVacant())
				{
					searchList.add(new int[] { checkX, checkY, from[2] - check.getMoveCost(u) });
				}
			}
			if (from[1] < dimensions[1] - 1)
			{
				int checkX = from[0];
				int checkY = from[1] + 1;
				BattlegroundTile check = battleground.getMap()[checkX][checkY];
				if ((!traversable.ContainsKey(check) || (int)traversable[check] < from[2])
						&& from[2] - check.getMoveCost(u) >= 0
						&& check.isVacant())
				{
					searchList.add(new int[] { checkX, checkY, from[2] - check.getMoveCost(u) });
				}
			}
		}
		return traversable;
	}

	public static Dictionary<BattlegroundTile, object> getAttackableBattlegroundTilesFromDestination(
			BattleGround battleground, Unit u, int x, int y, StationaryWeapon w)
	{
		int minRange = 1;
		int maxRange = 1;
		if (u is Equippable) {
			if (w == null)
			{
				int[] used = ((Equippable)u).getInventory()[u.getRanges()[0]];
				if (used != null && used[0] == InventoryIndex.HANDHELD_WEAPON)
				{
					maxRange = ((HandheldWeapon)InventoryIndex.getElement(used)).maxRange();
				}
			}
			else
			{
				minRange = w.minRange();
				maxRange = w.maxRange();
			}
		}
		Dictionary<BattlegroundTile, object> traversable = new Dictionary<BattlegroundTile, object>(800);
		Dictionary<BattlegroundTile, object> attackable = new Dictionary<BattlegroundTile, object>(800); //Gives distance from attacker for each target
		LinkedQueue<int[]> searchList = new LinkedQueue<int[]>(); //[0] = x, [1] = y, [2] = remainingMovement
		searchList.add(new int[] { x, y, maxRange });
		int[] dimensions = battleground.getDimensions();
		while (!(searchList.isEmpty()))
		{
			int[] from = searchList.pop();
			BattlegroundTile fromTile = battleground.getMap()[from[0]][from[1]];
			traversable.Add(fromTile, from[2]);
			int distance = maxRange - from[2];
			if (distance >= minRange
					&& fromTile.getUnit() != null && fromTile.getUnit().isEnemyOf(u))
			{
				attackable.Add(fromTile, distance);
			}
			if (from[2] == 0)
			{
				continue;
			}
			if (from[0] > 0)
			{
				int checkX = from[0] - 1;
				int checkY = from[1];
				BattlegroundTile check = battleground.getMap()[checkX][checkY];
				if ((!traversable.ContainsKey(check) || (int)traversable[check] < from[2])
						&& from[2] - 1 >= 0)
				{
					searchList.add(new int[] { checkX, checkY, from[2] - 1 });
				}
			}
			if (from[0] < dimensions[0] - 1)
			{
				int checkX = from[0] + 1;
				int checkY = from[1];
				BattlegroundTile check = battleground.getMap()[checkX][checkY];
				if ((!traversable.ContainsKey(check) || (int)traversable[check] < from[2])
						&& from[2] - 1 >= 0)
				{
					searchList.add(new int[] { checkX, checkY, from[2] - 1 });
				}
			}
			if (from[1] > 0)
			{
				int checkX = from[0];
				int checkY = from[1] - 1;
				BattlegroundTile check = battleground.getMap()[checkX][checkY];
				if ((!traversable.ContainsKey(check) || (int)traversable[check] < from[2])
						&& from[2] - 1 >= 0)
				{
					searchList.add(new int[] { checkX, checkY, from[2] - 1 });
				}
			}
			if (from[1] < dimensions[1] - 1)
			{
				int checkX = from[0];
				int checkY = from[1] + 1;
				BattlegroundTile check = battleground.getMap()[checkX][checkY];
				if ((!traversable.ContainsKey(check) || (int)traversable[check] < from[2])
						&& from[2] - 1 >= 0)
				{
					searchList.add(new int[] { checkX, checkY, from[2] - 1 });
				}
			}
		}
		return attackable;
	}
}
