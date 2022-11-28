using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [Header("Stats")]
    public string Class;
    public int HP;
    public int ATK;
    public int ATKRange;
    public int Movement;
    public string Passive;

    [Header("ATK Point Objects from Unit")]
    public GameObject Front;
    public GameObject Back;
    public GameObject Left;
    public GameObject Right;

    private int terrainBuff;

    public void SetTerrainBuff(string tile)
    {
        if      (tile == "Plain")    { terrainBuff = 0; }
        else if (tile == "Forest")   { terrainBuff = 1; }
        else if (tile == "Hill")     { terrainBuff = 2; }
        else if (tile == "Mountain") { terrainBuff = 3; }
    }

    public int TerrainBuff()
    {
        return terrainBuff;
    }

    public string GetClass() { return Class; }
    public void SetHP(int damage) { HP += damage; }
    public int GetHP() { return HP; }
    public int GetATK() { return ATK; }
    public int GetATKRange() { return ATKRange; }
    public int GetMovement() { return Movement; }
    public string GetPassive() { return Passive; }
    public GameObject GetFront() { return Front; }
    public GameObject GetBack() { return Back; }
    public GameObject GetLeft() { return Left; }
    public GameObject GetRight() { return Right; }
}
