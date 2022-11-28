using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationMap : MonoBehaviour
{
    public List<List<Transform>> Map = new List<List<Transform>>();
    public Transform[] tileTranforms;
    
    private void Awake()
    {
        tileTranforms = GetComponentsInChildren<Transform>();

        int index = 1;

        for (int x = 0; x < 8; x++)
        {
            Map.Add(new List<Transform>());

            for (int y = 0; y < 8; y++)
            {
                Transform transformOfTile = tileTranforms[index];
                Map[x].Add(transformOfTile);
                index++;
            }
        } 
    }

    public List<List<Transform>> GetMap() { return Map; }
}
