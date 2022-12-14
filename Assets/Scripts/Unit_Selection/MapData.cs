using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapData : MonoBehaviour
{
    [Header("Script from Selection Master Object")]
    public SelectionMaster SelectionMaster;
    
    [Header("Text of Counts from Map Window")]
    public TMP_Text MarshCount;
    public TMP_Text PlainCount;
    public TMP_Text ForestCount;
    public TMP_Text HillCount;
    public TMP_Text MountainCount;

    private int marshCount;
    private int plainCount;
    private int forestCount;
    private int hillCount;
    private int mountainCount;

    public void LoadMapOne()
    {
        marshCount = 0;
        plainCount = 34;
        forestCount = 20;
        hillCount = 4;
        mountainCount = 6;

        MarshCount.text = "x" + marshCount.ToString();
        PlainCount.text = "x" + plainCount.ToString();
        ForestCount.text = "x" + forestCount.ToString();
        HillCount.text = "x" + hillCount.ToString();
        MountainCount.text = "x" + mountainCount.ToString();

        SelectionMaster.LoadThisScene(3);
    }
}
