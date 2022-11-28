using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerTwoSO : ScriptableObject
{
    [SerializeField]
    private List<GameObject> unitList;

    public List<GameObject> UnitList
    {
        get { return unitList; }
        set { unitList = value; }
    }
}
