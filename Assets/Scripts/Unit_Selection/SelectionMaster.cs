using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SelectionMaster : MonoBehaviour
{
    [Header("Player Scriptable Objects")]
    public PlayerOneSO PlayerOneSO;
    public PlayerTwoSO PlayerTwoSO;

    [Header("Text of Counts from Player One Window")]
    public TMP_Text P1_SwordCount;
    public TMP_Text P1_ArcherCount;
    public TMP_Text P1_BerserkerCount;
    public TMP_Text P1_MageCount;

    [Header("Text of Counts from Player Two Window")]
    public TMP_Text P2_SwordCount;
    public TMP_Text P2_ArcherCount;
    public TMP_Text P2_BerserkerCount;
    public TMP_Text P2_MageCount;

    [Header("Done and Start Buttons")]
    public Button doneButton;
    public Button startButton;

    private bool isP1Done = false;
    private int thisScene = 0;

    public void AddUnit(GameObject unit)
    {
        if (!isP1Done)
        {
            int count = 0;
            for (var i = 0; i < PlayerOneSO.UnitList.Count; i++)
            {
                if (PlayerOneSO.UnitList[i] == unit)
                {
                    count += 1;
                }
            }

            if (count < 2)
            {
                PlayerOneSO.UnitList.Add(unit);
            }
        }
        else
        {
            int count = 0;
            for (var i = 0; i < PlayerTwoSO.UnitList.Count; i++)
            {
                if (PlayerTwoSO.UnitList[i] == unit)
                {
                    count += 1;
                }
            }

            if (count < 2)
            {
                PlayerTwoSO.UnitList.Add(unit);
            }
        }

        AdjustCount();
        CheckLength();
    }

    public void RemoveUnit(GameObject unit)
    {
        if (!isP1Done)
        {
            PlayerOneSO.UnitList.Remove(unit);
        }
        else
        {
            PlayerTwoSO.UnitList.Remove(unit);
        }

        AdjustCount();
        CheckLength();
    }

    private void AdjustCount()
    {
        int swordCount = 0;
        int archerCount = 0;
        int berserkerCount = 0;
        int mageCount = 0;

        if (!isP1Done)
        {
            for (var i = 0; i < PlayerOneSO.UnitList.Count; i++)
            {
                if (PlayerOneSO.UnitList[i].name == "W_Sword") { swordCount++; }
                if (PlayerOneSO.UnitList[i].name == "W_Archer") { archerCount++; }
                if (PlayerOneSO.UnitList[i].name == "W_Berserker") { berserkerCount++; }
                if (PlayerOneSO.UnitList[i].name == "W_Mage") { mageCount++; }
            }

            P1_SwordCount.text = "x" + swordCount.ToString();
            P1_ArcherCount.text = "x" + archerCount.ToString();
            P1_BerserkerCount.text = "x" + berserkerCount.ToString();
            P1_MageCount.text = "x" + mageCount.ToString();
        }
        else
        {
            for (var i = 0; i < PlayerTwoSO.UnitList.Count; i++)
            {
                if (PlayerTwoSO.UnitList[i].name == "B_Sword") { swordCount++; }
                if (PlayerTwoSO.UnitList[i].name == "B_Archer") { archerCount++; }
                if (PlayerTwoSO.UnitList[i].name == "B_Berserker") { berserkerCount++; }
                if (PlayerTwoSO.UnitList[i].name == "B_Mage") { mageCount++; }
            }

            P2_SwordCount.text = "x" + swordCount.ToString();
            P2_ArcherCount.text = "x" + archerCount.ToString();
            P2_BerserkerCount.text = "x" + berserkerCount.ToString();
            P2_MageCount.text = "x" + mageCount.ToString();
        }
    }

    private void CheckLength()
    {
        if (!isP1Done)
        {
            if (PlayerOneSO.UnitList.Count == 2)
            {
                doneButton.interactable = true;
            }
            else
            {
                doneButton.interactable = false;
            }
        }
        else
        {
            if (PlayerTwoSO.UnitList.Count == 2)
            {
                startButton.interactable = true;
            }
            else
            {
                startButton.interactable = false;
            }
        }
    }
    
    public void ChangeTurn()
    {
        isP1Done = true;
        doneButton.interactable = false;
    }

    public bool SendBool()
    {
        return isP1Done;
    }

    public void LoadThisScene(int index)
    {
        thisScene = index;
    }
    
    public void ChangeScene()
    {
        if (thisScene != 0)
        {
            SceneManager.LoadScene(thisScene);
        }
    }
}
