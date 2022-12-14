using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUnits : MonoBehaviour
{
    [Header("Player Scriptable Objects")]
    public PlayerOneSO PlayerOneSO;
    public PlayerTwoSO PlayerTwoSO;

    [Header("Script from Game Master Object")]
    public GameMaster GameMaster;

    [Header("Spawner Position to Place Units")]
    public Vector3 P1_StartPos;
    public Vector3 P2_StartPos;

    private GameObject thisUnit;
    
    private void Awake()
    {
        transform.position = P1_StartPos;

        for (var i = 0; i < PlayerOneSO.UnitList.Count; i++)
        {
            switch (PlayerOneSO.UnitList[i].name)
            {
                case "W_Sword":
                    GameObject Sword = Instantiate(PlayerOneSO.UnitList[i], transform.position, transform.rotation);
                    thisUnit = Sword;
                    break;
                case "W_Archer":
                    GameObject Archer = Instantiate(PlayerOneSO.UnitList[i], transform.position, transform.rotation);
                    thisUnit = Archer;
                    break;
                case "W_Berserker":
                    GameObject Berserker = Instantiate(PlayerOneSO.UnitList[i], transform.position, transform.rotation);
                    thisUnit = Berserker;
                    break;
                case "W_Mage":
                    GameObject Mage = Instantiate(PlayerOneSO.UnitList[i], transform.position, transform.rotation);
                    thisUnit = Mage;
                    break;
            }

            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1f);
            thisUnit.tag = "P1";
            thisUnit.GetComponent<Stats>().SetID(i);
            thisUnit.GetComponent<Stats>().SetFace("North");
            GameMaster.AddToP1(thisUnit);
        }

        transform.position = P2_StartPos;
        transform.localEulerAngles = new Vector3(0, 180f, 0);

        for (var i = 0; i < PlayerTwoSO.UnitList.Count; i++)
        {
            switch (PlayerTwoSO.UnitList[i].name)
            {
                case "B_Sword":
                    GameObject Sword = Instantiate(PlayerTwoSO.UnitList[i], transform.position, transform.rotation);
                    thisUnit = Sword;
                    break;
                case "B_Archer":
                    GameObject Archer = Instantiate(PlayerTwoSO.UnitList[i], transform.position, transform.rotation);
                    thisUnit = Archer;
                    break;
                case "B_Berserker":
                    GameObject Berserker = Instantiate(PlayerTwoSO.UnitList[i], transform.position, transform.rotation);
                    thisUnit = Berserker;
                    break;
                case "B_Mage":
                    GameObject Mage = Instantiate(PlayerTwoSO.UnitList[i], transform.position, transform.rotation);
                    thisUnit = Mage;
                    break;
            }

            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1f);
            thisUnit.tag = "P2";
            thisUnit.GetComponent<Stats>().SetID(i);
            thisUnit.GetComponent<Stats>().SetFace("South");
            GameMaster.AddToP2(thisUnit);
        }
    }
}
