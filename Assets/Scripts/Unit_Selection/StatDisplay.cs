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
    public Image UnitImage;
    public TMP_Text HPValue;
    public TMP_Text ATKValue;
    public TMP_Text ATKRangeValue;
    public TMP_Text MovementValue;
    public TMP_Text PassiveDesc;

    [Header("ATK Point Image Objects from Stat Window")]
    public GameObject FrontImage1;
    public GameObject FrontImage2;
    public GameObject BackImage1;
    public GameObject BackImage2;
    public GameObject LeftImage1;
    public GameObject LeftImage2;
    public GameObject RightImage1;
    public GameObject RightImage2;

    private Stats unitStats;

    private void Start()
    {
        unitStats = Unit.GetComponent<Stats>();
    }

    public void DisplayStats()
    {
        UnitClass.text = unitStats.GetClass();
        UnitImage.sprite = unitStats.GetImage();
        HPValue.text = unitStats.GetHP().ToString();
        ATKValue.text = unitStats.GetATK().ToString();
        ATKRangeValue.text = unitStats.GetATKRange().ToString();
        MovementValue.text = unitStats.GetMovement().ToString();
        PassiveDesc.text = unitStats.GetPassive();

        FrontImage1.GetComponent<Image>().color = unitStats.GetFront().GetComponent<Renderer>().sharedMaterial.color;
        FrontImage2.GetComponent<Image>().color = unitStats.GetFront().GetComponent<Renderer>().sharedMaterial.color;
        BackImage1.GetComponent<Image>().color = unitStats.GetBack().GetComponent<Renderer>().sharedMaterial.color;
        BackImage2.GetComponent<Image>().color = unitStats.GetBack().GetComponent<Renderer>().sharedMaterial.color;
        LeftImage1.GetComponent<Image>().color = unitStats.GetLeft().GetComponent<Renderer>().sharedMaterial.color;
        LeftImage2.GetComponent<Image>().color = unitStats.GetLeft().GetComponent<Renderer>().sharedMaterial.color;
        RightImage1.GetComponent<Image>().color = unitStats.GetRight().GetComponent<Renderer>().sharedMaterial.color;
        RightImage2.GetComponent<Image>().color = unitStats.GetRight().GetComponent<Renderer>().sharedMaterial.color;
    }
}
