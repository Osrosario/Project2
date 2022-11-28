using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatDisplay : MonoBehaviour
{
    [Header("Unit")]
    public GameObject Unit;

    [Header("Text of Values from Stat Window")]
    public TMP_Text UnitClass;
    public TMP_Text HPValue;
    public TMP_Text ATKValue;
    public TMP_Text ATKRangeValue;
    public TMP_Text MovementValue;
    public TMP_Text PassiveDesc;

    [Header("ATK Point Image Objects from Stat Window")]
    public GameObject FrontImage;
    public GameObject BackImage;
    public GameObject LeftImage;
    public GameObject RightImage;

    private Stats unitStats;

    private void Start()
    {
        unitStats = Unit.GetComponent<Stats>();
    }

    public void DisplayStats()
    {
        UnitClass.text = unitStats.GetClass();
        HPValue.text = unitStats.GetHP().ToString();
        ATKValue.text = unitStats.GetATK().ToString();
        ATKRangeValue.text = unitStats.GetATKRange().ToString();
        MovementValue.text = unitStats.GetMovement().ToString();
        PassiveDesc.text = unitStats.GetPassive();

        FrontImage.GetComponent<Image>().color = unitStats.GetFront().GetComponent<Renderer>().sharedMaterial.color;
        BackImage.GetComponent<Image>().color = unitStats.GetBack().GetComponent<Renderer>().sharedMaterial.color;
        LeftImage.GetComponent<Image>().color = unitStats.GetLeft().GetComponent<Renderer>().sharedMaterial.color;
        RightImage.GetComponent<Image>().color = unitStats.GetRight().GetComponent<Renderer>().sharedMaterial.color;
    }
}
