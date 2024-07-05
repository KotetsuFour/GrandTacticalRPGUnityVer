using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapDisplay : MonoBehaviour
{
    private Tile[,] tiles;
    [SerializeField] private Tile tile;

    [SerializeField] private Material plain;
    [SerializeField] private Material desert;
    [SerializeField] private Material forest;
    [SerializeField] private Material dense_forest;
    [SerializeField] private Material mountain;
    [SerializeField] private Material shallow_water;
    [SerializeField] private Material deep_water;
    [SerializeField] private Material snowy_plain;
    [SerializeField] private Material snowy_mountain;
    [SerializeField] private Material swamp;
    [SerializeField] private Material wasteland;
    [SerializeField] private Material glacier;
    // Start is called before the first frame update
    void Start()
    {
        Dictionary<WorldMapTile.WorldMapTileType, Material> materials = new Dictionary<WorldMapTile.WorldMapTileType, Material>();
        materials.Add(WorldMapTile.WorldMapTileType.PLAIN, plain);
        materials.Add(WorldMapTile.WorldMapTileType.DESERT, desert);
        materials.Add(WorldMapTile.WorldMapTileType.FOREST, forest);
        materials.Add(WorldMapTile.WorldMapTileType.DENSE_FOREST, dense_forest);
        materials.Add(WorldMapTile.WorldMapTileType.MOUNTAIN, mountain);
        materials.Add(WorldMapTile.WorldMapTileType.SHALLOW_WATER, shallow_water);
        materials.Add(WorldMapTile.WorldMapTileType.DEEP_WATER, deep_water);
        materials.Add(WorldMapTile.WorldMapTileType.SNOWY_PLAIN, snowy_plain);
        materials.Add(WorldMapTile.WorldMapTileType.SNOWY_MOUNTAIN, snowy_mountain);
        materials.Add(WorldMapTile.WorldMapTileType.SWAMP, swamp);
        materials.Add(WorldMapTile.WorldMapTileType.WASTELAND, wasteland);
        materials.Add(WorldMapTile.WorldMapTileType.GLACIER, glacier);
        WorldMap map = GeneralGameplayManager.getWorldMap();
        if (map != null)
        {
            tiles = new Tile[WorldMap.SQRT_OF_MAP_SIZE, WorldMap.SQRT_OF_MAP_SIZE];
            for (int q = 0; q < WorldMap.SQRT_OF_MAP_SIZE; q++)
            {
                for (int w = 0; w < WorldMap.SQRT_OF_MAP_SIZE; w++)
                {
                    Debug.Log($"Making tile {q},{w}");
                    Tile toPlace = Instantiate(tile, new Vector3(q, 0, w), Quaternion.identity);
                    tile.setTile(map.at(q, w), q, w);
                    tile.adjustHeight();
                    StaticData.findDeepChild(tile.transform, "Model")
                        .GetComponent<MeshRenderer>().material = materials[map.at(q, w).getType()];
                    tiles[q, w] = toPlace;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
