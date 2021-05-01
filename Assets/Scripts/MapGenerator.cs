using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Texture2D map;
    public Texture2D mapColor;
    public GameObject MapTile;

    Texture2D mapColorResized;
    Vector3 botLeft;
    float tileScale;
    GameObject Map;

    private void Start()
    {
        botLeft = Camera.main.transform.GetChild(0).gameObject.GetComponent<EdgeCollider2D>().points[1];
        tileScale = MapTile.transform.localScale.x;


        Map = new GameObject("Map");

        GenerateLevel();
    }

    private void GenerateLevel()
    {
        mapColorResized = Instantiate(mapColor);
        TextureScale.Bilinear(mapColorResized, map.width * 2, map.height * 2);
        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                GenerateTile(x, y);
            }
        }
    }

    private void GenerateTile(int x, int y)
    {
        Color color = map.GetPixel(x, y);
        if (color.a == 0)
        {
            return;
        }

        Vector3 pix = new Vector3(x, y);
        Vector3 tileOffset = new Vector3(tileScale / 2, tileScale / 2);
        Vector3 newPos = tileOffset + botLeft + (pix * tileScale);

        GameObject mapTile = Instantiate(MapTile, newPos, Quaternion.identity, Map.transform);
        mapTile.GetComponent<SpriteRenderer>().color = mapColorResized.GetPixel(x, y);
    }
}
