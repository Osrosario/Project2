using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayStats : MonoBehaviour
{
    [Header("Text of Values from Player Stat Window")]
    public TMP_Text UnitClass;
    public TMP_Text HPValue;
    public TMP_Text ATKValue;
    public TMP_Text ATKRangeValue;
    public TMP_Text MovementValue;

    [Header("Images of Terrain from Player Stat Window")]
    public GameObject Plain;
    public GameObject Forest;
    public GameObject Hill;
    public GameObject Mountain;

    private Color green = new Color(44, 205, 0, 255);

    public void ShowInfo(Stats stats, string terrain)
    {
        UnitClass.text = stats.GetClass();
        HPValue.text = stats.GetHP().ToString();
        
        if (stats.TerrainBuff() > 0)
        {
            ATKValue.text = (stats.GetATK() + stats.TerrainBuff()).ToString();
            ATKValue.color = green;
        }
        else
        {
            ATKValue.text = stats.GetATK().ToString();
            ATKValue.color = Color.white;
        }
        
        ATKRangeValue.text = stats.GetATKRange().ToString();
        MovementValue.text = stats.GetMovement().ToString();

        if (terrain == "Plain")
        {
            Plain.SetActive(true);
            
            Forest.SetActive(false);
            Hill.SetActive(false);
            Mountain.SetActive(false);
        }
        else if (terrain == "Forest")
        {
            Forest.SetActive(true);

            Plain.SetActive(false);
            Hill.SetActive(false);
            Mountain.SetActive(false);
        }
        else if (terrain == "Hill")
        {
            Hill.SetActive(true);

            Plain.SetActive(false);
            Forest.SetActive(false);
            Mountain.SetActive(false);
        }
        else if (terrain == "Mountain")
        {
            Mountain.SetActive(true);

            Plain.SetActive(false);
            Forest.SetActive(false);
            Hill.SetActive(false);
        }
    }

    public void ClearInfo()
    {
        UnitClass.text = "";
        HPValue.text = "";

        ATKValue.text = "";
        ATKValue.color = Color.white;

        ATKRangeValue.text = "";
        MovementValue.text = "";

        Plain.SetActive(false);
        Forest.SetActive(false);
        Hill.SetActive(false);
        Mountain.SetActive(false);
    }
}
