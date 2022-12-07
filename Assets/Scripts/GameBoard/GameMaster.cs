using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private int aIndex;
    private int bIndex;
    private string gameState;
    private int unitCount;
    private int unitsPlaced;
    
    private void Awake()
    {
        aIndex = 0;
        bIndex = 0;
        gameState = "prep";
        unitCount = P1UnitList.Count + P2UnitList.Count;
        unitsPlaced = 0;

        Cursor.SetGameState(gameState);

        int diceRoll = Random.Range(1, 7);

        if (diceRoll >= 1 && diceRoll <= 3)
        {
            playerTurn = "P1";
            playerToGoFirst = "P1";
            Cursor.SetPosition(3, 7);
            Cursor.SetGameState(gameState);
            Cursor.SetPlayerTurn(playerTurn);
            Cursor.SendUnit(P1UnitList[aIndex]);
            aIndex++;
        }
        else
        {
            playerTurn = "P2";
            playerToGoFirst = "P2";
            Cursor.SetPosition(4, 0);
            Cursor.SetGameState(gameState);
            Cursor.SetPlayerTurn(playerTurn);
            Cursor.SendUnit(P2UnitList[bIndex]);
            bIndex++;
        }
    }

    public void RequestUnit()
    {
        if (playerTurn == "P1")
        {
            if (bIndex < P2UnitList.Count)
            {
                Cursor.SendUnit(P2UnitList[bIndex]);
                Cursor.SetPosition(4, 0);
                playerTurn = "P2";
                Cursor.SetPlayerTurn(playerTurn);
                bIndex++;
            }
        }
        else
        {
            if (aIndex < P1UnitList.Count)
            {
                Cursor.SendUnit(P1UnitList[aIndex]);
                Cursor.SetPosition(3, 7);
                playerTurn = "P1";
                Cursor.SetPlayerTurn(playerTurn);
                aIndex++;
            }
        }

        if (unitsPlaced >= unitCount)
        {
            gameState = "play";
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