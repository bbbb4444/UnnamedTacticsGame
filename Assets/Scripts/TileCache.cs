using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileCache
{
    public static Dictionary<Collider, Tile> tileCache = new Dictionary<Collider, Tile>();

    static TileCache()
    {
        GameObject[] tileObjects = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject tileObject in tileObjects)
        {
            Tile tile = tileObject.GetComponent<Tile>();
            Collider collider = tileObject.GetComponent<Collider>();
            tileCache.Add(collider, tile);
        }
    }
    // ReSharper disable Unity.PerformanceAnalysis .. No performance hit if tile is cached
    public static Tile GetTile(Collider collider)
    {
        if (tileCache.TryGetValue(collider, out Tile tile))
        {
            return tile;
        }
        else
        {
            tile = collider.GetComponent<Tile>();
            if (tile != null)
            {
                tileCache.Add(collider, tile);
                return tile;
            }
        }

        return null;
    }
}
