using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Script from Navigation Map Object")]
    public NavigationMap NavMap;

    [Header("Sprite Objects attached to this Object")]
    public GameObject MoveSprite;
    public GameObject AttackSprite;

    [Header("Unit on Tile")]
    public GameObject UnitOnTile;

    //Persistant Data
    private List<List<Tile>> navMap = new List<List<Tile>>();
    public int xCoord;
    public int yCoord;
    public bool IsUnitOnTile = false;

    //Modified Breadth-First-Search Data
    public List<Tile> adjacencyList = new List<Tile>();
    public bool visited = false;
    public int distance = 0;
    public bool marked = false;
    public bool unitLooking = false;

    private void Awake()
    {
        navMap = NavMap.GetMap();
    }

    private void Update()
    {
        if (unitLooking)
        {
            ShowAttackSprite();
        }
        else
        {
            if (marked)
            {
                ShowMoveSprite();
            }
            else
            {
                ShowNoSprite();
            }
        }
    }


    public void ResetBFSData()
    {
        adjacencyList.Clear();
        visited = false;
        marked = false;
        distance = 0;
    }

    public void FindNeighbors()
    {
        ResetBFSData();

        CheckTile("North");
        CheckTile("South");
        CheckTile("West");
        CheckTile("East");
    }

    public void CheckTile(string direction)
    {
        int x = 0;
        int y = 0;

        if (direction == "North")
        {
            x = xCoord;
            y = yCoord - 1;

            if (y < 0)
            {
                y = 0;
            }
        }
        else if (direction == "South")
        {
            x = xCoord;
            y = yCoord + 1;

            if (y > 7)
            {
                y = 7;
            }
        }
        else if (direction == "West")
        {
            x = xCoord - 1;
            y = yCoord;

            if (x < 0)
            {
                x = 0;
            }
        }
        else if (direction == "East")
        {
            x = xCoord + 1;
            y = yCoord;

            if (x > 7)
            {
                x = 7;
            }
        }

        Tile tile = navMap[y][x];

        if (tile.UnitOnTile == null)
        {
            adjacencyList.Add(tile);
        }
    }

    public void SetCoords(int x, int y)
    {
        xCoord = y;
        yCoord = x;
    }

    public (int, int) GetCoords()
    {
        return (xCoord, yCoord);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    private void ShowAttackSprite()
    {
        MoveSprite.SetActive(false);
        AttackSprite.SetActive(true);
    }

    private void ShowMoveSprite()
    {
        MoveSprite.SetActive(true);
        AttackSprite.SetActive(false);
    }

    private void ShowNoSprite()
    {
        MoveSprite.SetActive(false);
        AttackSprite.SetActive(false);
    }
}
