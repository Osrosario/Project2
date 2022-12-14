using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    [Header("Unit Image")]
    public Sprite UnitImage;
    
    [Header("Stats")]
    public string Class;
    public int HP;
    public int ATK;
    public int ATKRange;
    public int Movement;
    public string Passive;

    [Header("ATK Point Modifiers")]
    public GameObject Front;
    public GameObject Back;
    public GameObject Left;
    public GameObject Right;

    [Header("A Cube from each Attack Point")]
    public GameObject CubeFront;
    public GameObject CubeBack;
    public GameObject CubeLeft;
    public GameObject CubeRight;

    public Dictionary<string, int> pointModfiers = new Dictionary<string, int>();
    public int terrainBuff;
    public int ID;
    private string facing;

    private void Awake()
    {
        pointModfiers.Add("Front", Front.GetComponent<AttackPoint>().GetModifer());
        pointModfiers.Add("Back", Back.GetComponent<AttackPoint>().GetModifer());
        pointModfiers.Add("Left", Left.GetComponent<AttackPoint>().GetModifer());
        pointModfiers.Add("Right", Right.GetComponent<AttackPoint>().GetModifer());
    }

    private void Update()
    {
        if (GetHP() <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetTerrainBuff(string tile)
    {
        if      (tile == "Plain")    { terrainBuff = 0; }
        else if (tile == "Forest")   { terrainBuff = 1; }
        else if (tile == "Hill")     { terrainBuff = 2; }
        else if (tile == "Mountain") { terrainBuff = 3; }
    }

    public int TerrainBuff() { return terrainBuff; }
    public void SetID(int num) { ID = num; }
    public void SetFace(string face) { facing = face; }
    public string GetFace() { return facing; }
    public string GetClass() { return Class; }
    public Sprite GetImage() { return UnitImage; }
    public void TakeDamage(int damage) { HP -= damage; }
    public int GetHP() { return HP; }
    public int GetATK() { return ATK; }
    public int GetATKRange() { return ATKRange; }
    public int GetMovement() { return Movement; }
    public string GetPassive() { return Passive; }
    public GameObject GetFront() { return CubeFront; }
    public GameObject GetBack() { return CubeBack; }
    public GameObject GetLeft() { return CubeLeft; }
    public GameObject GetRight() { return CubeRight; }
}
