using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float cameraMoveSpeed;
    [SerializeField] private float cameraBorder;
    [SerializeField] private float camDistance;

    private int wmX, wmY;
    [SerializeField] private Transform cursor;

    [SerializeField] private LayerMask tileLayer;

    private SelectionMode selectionMode;

    public void initialize()
    {
        updateHUD();
        viewMyNation();
    }
    private void setWMCursor(int x, int y)
    {
        wmX = x;
        wmY = y;
        setWMCursor(GeneralGameplayManager.getWorldMap().at(x, y).tileModel);
    }
    private void setWMCursor(Tile tile)
    {
        wmX = tile.x;
        wmY = tile.y;
        cursor.position = tile.getCursorPosition();
    }
    private void moveWMCursor(int xDisplacement, int yDisplacement)
    {
        wmX = Mathf.Clamp(wmX + xDisplacement, 0, WorldMap.SQRT_OF_MAP_SIZE - 1);
        wmY = Mathf.Clamp(wmY + yDisplacement, 0, WorldMap.SQRT_OF_MAP_SIZE - 1);
        setWMCursor(wmX, wmY);
    }
    public void setCameraPosition(Tile lookAt)
    {
        cam.transform.position = new Vector3(lookAt.transform.position.x - camDistance, camDistance, lookAt.transform.position.z);
        cam.transform.rotation = Quaternion.LookRotation(lookAt.transform.position - cam.transform.position);
    }
    public void setCameraPosition()
    {
        cam.transform.position = new Vector3(cursor.transform.position.x - camDistance, camDistance, cursor.transform.position.z);
        cam.transform.rotation = Quaternion.LookRotation(cursor.transform.position - cam.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (selectionMode == SelectionMode.MAP_ROAM)
        {
            Vector3 direction = new Vector3();
            if (Input.GetKey(KeyCode.W))
            {
                direction.x = 1;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                direction.x = -1;
            }
            if (Input.GetKey(KeyCode.A))
            {
                direction.z = 1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                direction.z = -1;
            }
            direction = direction.normalized * Time.deltaTime * cameraMoveSpeed;
            Vector3 camPosition = new Vector3(
                Mathf.Clamp(cam.transform.position.x + direction.x, cameraBorder, WorldMap.SQRT_OF_MAP_SIZE + cameraBorder),
                cam.transform.position.y,
                Mathf.Clamp(cam.transform.position.z + direction.z, cameraBorder, WorldMap.SQRT_OF_MAP_SIZE - cameraBorder)
                );
            cam.transform.position = camPosition;

            RaycastHit hit;
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, float.MaxValue, tileLayer))
            {
                Tile tile = hit.collider.GetComponent<Tile>();
                setWMCursor(tile);
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
//                    setCameraPosition(tile);
                    getTileInfoAndOptions(tile.getTile());
                    selectionMode = SelectionMode.EXAMINE_TILE;
                }
            }

        }
    }
    public void back()
    {
        if (selectionMode == SelectionMode.EXAMINE_TILE)
        {
            StaticData.findDeepChild(transform, "Back").gameObject.SetActive(false);
            StaticData.findDeepChild(transform, "TileInfo").gameObject.SetActive(false);
            StaticData.findDeepChild(transform, "GroupInfo").gameObject.SetActive(false);
            StaticData.findDeepChild(transform, "BuildingInfo").gameObject.SetActive(false);
            selectionMode = SelectionMode.MAP_ROAM;
        }
    }
    public void updateHUD()
    {
        StaticData.findDeepChild(transform, "HUDPortrait").GetComponent<Portrait>()
            .draw(GeneralGameplayManager.getPlayer());
        StaticData.findDeepChild(transform, "PlayerName").GetComponent<TextMeshProUGUI>()
            .text = $"{GeneralGameplayManager.getPlayer().getDisplayName()}, {GeneralGameplayManager.getPlayerNation().getFullName()}";
        StaticData.findDeepChild(transform, "TimeString").GetComponent<TextMeshProUGUI>()
            .text = $"{HistoricalRecord.getTimeAsString(GeneralGameplayManager.getDaysSinceGameStart())}";
        StaticData.findDeepChild(transform, "ActionsLeft").GetComponent<TextMeshProUGUI>()
            .text = $"{GeneralGameplayManager.getActionsLeft()}";
    }
    public void getTileInfoAndOptions(WorldMapTile tileData)
    {
        StaticData.findDeepChild(transform, "TileInfo").gameObject.SetActive(true);
        StaticData.findDeepChild(transform, "Back").gameObject.SetActive(true);
        if (tileData.getOwner() == null)
        {
            CityState cs = null;
            if (tileData.tileModel.x > 0)
            {
                WorldMapTile check = GeneralGameplayManager.getWorldMap().at(tileData.tileModel.x + 1, tileData.tileModel.y);
                if (check.getOwner() != null && check.getOwner().getNation() == GeneralGameplayManager.getPlayerNation()
                    && check.getOwner().size() < CityState.MAX_SIZE)
                {
                    cs = check.getOwner();
                }
            }
            if (cs == null && tileData.tileModel.x < WorldMap.SQRT_OF_MAP_SIZE - 1)
            {
                WorldMapTile check = GeneralGameplayManager.getWorldMap().at(tileData.tileModel.x - 1, tileData.tileModel.y);
                if (check.getOwner() != null && check.getOwner().getNation() == GeneralGameplayManager.getPlayerNation()
                    && check.getOwner().size() < CityState.MAX_SIZE)
                {
                    cs = check.getOwner();
                }
            }
            if (cs == null && tileData.tileModel.y > 0)
            {
                WorldMapTile check = GeneralGameplayManager.getWorldMap().at(tileData.tileModel.x, tileData.tileModel.y + 1);
                if (check.getOwner() != null && check.getOwner().getNation() == GeneralGameplayManager.getPlayerNation()
                    && check.getOwner().size() < CityState.MAX_SIZE)
                {
                    cs = check.getOwner();
                }
            }
            if (cs == null && tileData.tileModel.y < WorldMap.SQRT_OF_MAP_SIZE - 1)
            {
                WorldMapTile check = GeneralGameplayManager.getWorldMap().at(tileData.tileModel.x, tileData.tileModel.y - 1);
                if (check.getOwner() != null && check.getOwner().getNation() == GeneralGameplayManager.getPlayerNation()
                    && check.getOwner().size() < CityState.MAX_SIZE)
                {
                    cs = check.getOwner();
                }
            }
            if (cs == null)
            {
                StaticData.findDeepChild(transform, "TileOwnerName").GetComponent<TextMeshProUGUI>()
                    .text = "None";
            }
            else
            {
                StaticData.findDeepChild(transform, "TileOwnerName").GetComponent<TextMeshProUGUI>()
                    .text = "None (Claim?)";
                Button.ButtonClickedEvent claim = new Button.ButtonClickedEvent();
                claim.AddListener(delegate { GeneralGameplayManager.claimTileForNation(cs, tileData); back(); });
                StaticData.findDeepChild(transform, "TileOwner").GetComponent<Button>()
                    .onClick = claim;
            }

        }
        else
        {
            CityState cs = tileData.getOwner();
            StaticData.findDeepChild(transform, "TileOwnerName").GetComponent<TextMeshProUGUI>()
                .text = $"{cs.getName()}, {cs.getNation().getName()}";
            Button.ButtonClickedEvent examine = new Button.ButtonClickedEvent();
            examine.AddListener(delegate { back(); examineNation(cs.getNation()); });
            StaticData.findDeepChild(transform, "TileOwner").GetComponent<Button>()
                .onClick = examine;
        }
        StaticData.findDeepChild(transform, "TerrainType").GetComponent<TextMeshProUGUI>()
            .text = $"{tileData.getType().getName()}";
        StaticData.findDeepChild(transform, "MagicType").GetComponent<Image>()
            .sprite = AssetDictionary.getImage(tileData.getMagicTypeAsString());
        StaticData.findDeepChild(transform, "MagicPotency").GetComponent<TextMeshProUGUI>()
            .text = $"{tileData.getMagicPotency()}";

        if (tileData.getGroupPresent() == null)
        {
            StaticData.findDeepChild(transform, "GroupInfo").gameObject.SetActive(false);
        }
        else
        {
            StaticData.findDeepChild(transform, "GroupInfo").gameObject.SetActive(true);
            WMTileOccupant occ = tileData.getGroupPresent();
            StaticData.findDeepChild(transform, "GroupLeader").GetComponent<Portrait>()
                .draw(occ.getLeader());
            StaticData.findDeepChild(transform, "GroupName").GetComponent<TextMeshProUGUI>()
                .text = $"{occ.getName()}";
            Button.ButtonClickedEvent examine = new Button.ButtonClickedEvent();
            examine.AddListener(delegate { back(); examineGroup(occ); });
            StaticData.findDeepChild(transform, "ViewGroup").GetComponent<Button>()
                .onClick = examine;
            StaticData.findDeepChild(transform, "GroupPower").GetComponent<TextMeshProUGUI>()
                .text = $"{occ.getPower()}";
            if (occ is UnitGroup && ((UnitGroup)occ).getPrisoners() != null)
            {
                StaticData.findDeepChild(transform, "PrisonerLeader").GetComponent<Portrait>()
                    .draw(((UnitGroup)occ).getPrisoners().getLeader());
            }
            else
            {
                StaticData.findDeepChild(transform, "PrisonerLeader").gameObject.SetActive(false);
            }
        }

        if (tileData.getBuilding() == null)
        {
            StaticData.findDeepChild(transform, "BuildingInfo").gameObject.SetActive(false);
        }
        else
        {
            StaticData.findDeepChild(transform, "BuildingInfo").gameObject.SetActive(true);
            Building build = tileData.getBuilding();
            StaticData.findDeepChild(transform, "BuildingName").GetComponent<TextMeshProUGUI>()
                .text = $"{build.getNameAndType()}";
            StaticData.findDeepChild(transform, "BuildingHealth").GetComponent<TextMeshProUGUI>()
                .text = $"{build.getCurrentHP()}/{build.getMaximumHP()}";
            StaticData.findDeepChild(transform, "BuildingDefense").GetComponent<TextMeshProUGUI>()
                .text = $"{build.getDurability()}";
            StaticData.findDeepChild(transform, "BuildingResistance").GetComponent<TextMeshProUGUI>()
                .text = $"{build.getResistance()}";
            if (tileData.getOwner().getNation() == GeneralGameplayManager.getPlayerNation())
            {
                StaticData.findDeepChild(transform, "EnterBuilding").gameObject.SetActive(true);
                Button.ButtonClickedEvent enter = new Button.ButtonClickedEvent();
                enter.AddListener(delegate { back(); examineBuilding(build); });
                StaticData.findDeepChild(transform, "EnterBuilding").GetComponent<Button>()
                    .onClick = enter;
            }
            else
            {
                StaticData.findDeepChild(transform, "EnterBuilding").gameObject.SetActive(true);
            }
        }

        if (tileData.getBattle() == null)
        {
            //TODO
        }
        else
        {
            //TODO
        }
    }
    public void examineNation(Nation n)
    {
        //TODO
    }
    public void examineGroup(WMTileOccupant occ)
    {
        //TODO
    }
    public void examineUnit(Unit u)
    {
        //TODO
    }
    public void examineBuilding(Building b)
    {
        //TODO
    }
    public void viewMyNation()
    {
        List<Castle> castles = GeneralGameplayManager.getPlayerNation().getCapital().getNobleResidences();
        if (castles.Count > 0)
        {
            setWMCursor(castles[0].getLocation().tileModel);
            setCameraPosition();
            return;
        }
        List<Village> villages = GeneralGameplayManager.getPlayerNation().getCapital().getResidentialAreas();
        if (villages.Count > 0)
        {
            setWMCursor(villages[0].getLocation().tileModel);
            setCameraPosition();
            return;
        }
        List<Building> others = GeneralGameplayManager.getPlayerNation().getCapital().getOtherBuildings();
        if (others.Count > 0)
        {
            setWMCursor(others[0].getLocation().tileModel);
            setCameraPosition();
            return;
        }
    }
    public enum SelectionMode
    {
        MAP_ROAM, EXAMINE_TILE
    }
}
