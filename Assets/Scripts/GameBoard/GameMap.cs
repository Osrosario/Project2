using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMap : MonoBehaviour
{
    public List<List<GameObject>> Map = new List<List<GameObject>>();

    private void Awake()
    {
        for (int x = 0; x < 8; x++)
        {
            Map.Add(new List<GameObject>());

            for (int y = 0; y < 8; y++)
            {
                Map[x].Add(null);
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
                    Debug.Log(Map[x][y]);
                }
            }
        }
    }

    public List<List<GameObject>> GetMap() { return Map; }
}
