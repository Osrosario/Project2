using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    [Header("Player Units")]
    public List<GameObject> P1UnitList;
    public List<GameObject> P2UnitList;

    [Header("Script from Cursor Object")]
    public Cursor Cursor;

    [Header("Dummy Game Object")]
    public GameObject DummyObject;

    private string playerToGoFirst;
    private string playerTurn;
    private int index1 = 0;
    private int index2 = 0;
    private string gameState = "Prep";
    private int unitCount;
    private int unitsPlaced;
    
    private void Awake()
    {     
        unitCount = P1UnitList.Count + P2UnitList.Count;
        Cursor.SetGameState(gameState);

        int diceRoll = Random.Range(1, 7);

        if (diceRoll >= 1 && diceRoll <= 3)
        {
            playerTurn = "P1";
            playerToGoFirst = "P1";
            Cursor.SetPosition(3, 7);
            Cursor.SetGameState(gameState);
            Cursor.SetPlayerTurn(playerTurn);
            Cursor.SendUnit(P1UnitList[index1]);
            index1++;
        }
        else
        {
            playerTurn = "P2";
            playerToGoFirst = "P2";
            Cursor.SetPosition(4, 0);
            Cursor.SetGameState(gameState);
            Cursor.SetPlayerTurn(playerTurn);
            Cursor.SendUnit(P2UnitList[index2]);
            index2++;
        }
    }

    private void Update()
    {
        for (int i = 0; i < P1UnitList.Count; i++)
        {
            if (P1UnitList[i] == null)
            {
                P1UnitList.Remove(P1UnitList[i]);
            }
        }

        for (int i = 0; i < P2UnitList.Count; i++)
        {
            if (P2UnitList[i] == null)
            {
                P2UnitList.Remove(P2UnitList[i]);
            }
        }

        if (P1UnitList.Count <= 0 || P2UnitList.Count <= 0)
        {
            SceneManager.LoadScene(4);
        }
    }

    public void RequestUnit()
    {
        if (playerTurn == "P1")
        {
            if (index2 < P2UnitList.Count)
            {
                Cursor.SendUnit(P2UnitList[index2]);
                Cursor.SetPosition(4, 0);
                playerTurn = "P2";
                Cursor.SetPlayerTurn(playerTurn);
                index2++;
            }
        }
        else
        {
            if (index1 < P1UnitList.Count)
            {
                Cursor.SendUnit(P1UnitList[index1]);
                Cursor.SetPosition(3, 7);
                playerTurn = "P1";
                Cursor.SetPlayerTurn(playerTurn);
                index1++;
            }
        }

        if (unitsPlaced >= unitCount)
        {
            gameState = "Play";
            playerTurn = playerToGoFirst;
            Cursor.SetPlayerTurn(playerToGoFirst);
            Cursor.SetGameState(gameState);
            Cursor.SendUnit(DummyObject);
        }
    }

    public void AddCount() { unitsPlaced++; }
    
    public void AddToP1(GameObject unit) { P1UnitList.Add(unit); }

    public void AddToP2(GameObject unit) { P2UnitList.Add(unit); }
}