using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRemoveUnit : MonoBehaviour
{
    [Header("Scripts from Selection Master")]
    public SelectionMaster SelectionMaster;
    
    [Header("Unit Objects")]
    public GameObject WhiteUnit;
    public GameObject BlackUnit;

    private bool isP1Done = false;

    private void Update()
    {
        isP1Done = SelectionMaster.SendBool();
    }

    public void AddUnit()
    {
        if (!isP1Done)
        {
            SelectionMaster.AddUnit(WhiteUnit);
        }
        else
        {
            SelectionMaster.AddUnit(BlackUnit);
        }

    }

    public void RemoveUnit()
    {
        if (!isP1Done)
        {
            SelectionMaster.RemoveUnit(WhiteUnit);
        }
        else
        {
            SelectionMaster.RemoveUnit(BlackUnit);
        }
    }
}
