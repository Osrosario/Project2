using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoint : MonoBehaviour
{
    [Header("Point Modifier")]
    public int Modifier;

    public int GetModifer() { return Modifier; }
}
