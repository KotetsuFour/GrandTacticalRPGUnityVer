using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private WorldMapTile tile;
    private int x, y;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void setTile(WorldMapTile tile, int x, int y)
    {
        this.tile = tile;
        this.x = x;
        this.y = y;

        Debug.Log(tile.getType().getName());
    }
    public void adjustHeight()
    {
        WorldMap map = GeneralGameplayManager.getWorldMap();
        float height = tile.getType().getHeight();
        Vector3[] vertices = StaticData.findDeepChild(transform, "Model").GetComponent<MeshFilter>().sharedMesh.vertices;
        float left = x > 0 ? map.at(x - 1, y).getType().getHeight() : height;
        float right = x < WorldMap.SQRT_OF_MAP_SIZE - 1 ? map.at(x + 1, y).getType().getHeight() : height;
        float top = y < WorldMap.SQRT_OF_MAP_SIZE - 1 ? map.at(x, y + 1).getType().getHeight() : height;
        float bottom = y > 0 ? map.at(x, y - 1).getType().getHeight() : height;
        float topLeft = x > 0 && y < WorldMap.SQRT_OF_MAP_SIZE - 1 ? map.at(x - 1, y + 1).getType().getHeight() : height;
        float bottomLeft = x > 0 && y > 0 ? map.at(x - 1, y - 1).getType().getHeight() : height;
        float topRight = x < WorldMap.SQRT_OF_MAP_SIZE - 1 && y < WorldMap.SQRT_OF_MAP_SIZE - 1 ? map.at(x + 1, y + 1).getType().getHeight() : height;
        float bottomRight = x < WorldMap.SQRT_OF_MAP_SIZE - 1 && y > 0 ? map.at(x + 1, y - 1).getType().getHeight() : height;

        float topLeftCornerHeight = (top + left + topLeft + height) / 4;
        float topRightCornerHeight = (top + right + topRight + height) / 4;
        float bottomLeftCornerHeight = (bottom + left + bottomLeft + height) / 4;
        float bottomRightCornerHeight = (bottom + right + bottomRight + height) / 4;

        for (int q = 0; q < vertices.Length; q++)
        {
            Vector3 vertex = vertices[q];
            if (vertex == new Vector3(-0.5f, 0.5f, -0.5f))
            {
                vertices[q] = new Vector3(-0.5f, topLeftCornerHeight, -0.5f);
            }
            if (vertex == new Vector3(0.5f, 0.5f, -0.5f))
            {
                vertices[q] = new Vector3(0.5f, topRightCornerHeight, -0.5f);
            }
            if (vertex == new Vector3(-0.5f, 0.5f, 0.5f))
            {
                vertices[q] = new Vector3(-0.5f, bottomLeftCornerHeight, 0.5f);
            }
            if (vertex == new Vector3(-0.5f, 0.5f, -0.5f))
            {
                vertices[q] = new Vector3(0.5f, bottomRightCornerHeight, 0.5f);
            }
        }
        StaticData.findDeepChild(transform, "Model").GetComponent<MeshFilter>().sharedMesh.RecalculateBounds();
    }
}
