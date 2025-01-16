using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDetector : MonoBehaviour
{
    private List<Tile> tileCloseList = new List<Tile>();

    public void AddTile(Tile tile)
    {
        if (!tileCloseList.Contains(tile))
        {
            tileCloseList.Add(tile);
        }
    }

    public void RemoveTile(Tile tile)
    {
        tileCloseList.Remove(tile);
    }

    public Tile GetClosestTile(Vector3 playerPosition)
    {
        if (tileCloseList.Count == 0) return null;

        Tile closestTile = null;
        float minDistance = Mathf.Infinity;

        foreach (Tile tile in tileCloseList)
        {
            float distance = Vector3.Distance(playerPosition, tile.transform.position);
            if (distance < minDistance)
            {
                closestTile = tile;
                minDistance = distance;
            }
        }

        return closestTile;
    }
}
