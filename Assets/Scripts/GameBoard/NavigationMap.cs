using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationMap : MonoBehaviour
{
    public List<List<Tile>> Map = new List<List<Tile>>();
    public Tile[] tiles;

    private void Awake()
    {
        tiles = GetComponentsInChildren<Tile>();

        int index = 0;

        for (int x = 0; x < 8; x++)
        {
            Map.Add(new List<Tile>());

            for (int y = 0; y < 8; y++)
            {
                Tile tile = tiles[index];
                tile.SetCoords(x, y);
                Map[x].Add(tile);
                index++;
            }
        } 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Debug.Log(Map[x][y].UnitOnTile);
                }
            }
        }
    }

    public List<List<Tile>> GetMap() { return Map; }
}
