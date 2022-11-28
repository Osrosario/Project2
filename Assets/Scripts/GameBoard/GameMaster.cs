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
    private bool hasSecondPlayerGone;
    private int index;
    private string gameState;
    private int unitCount;
    private int unitsPlaced;

    private void Awake()
    {
        hasSecondPlayerGone = false;
        index = 0;
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
            Cursor.SendUnit(P1UnitList[index]);
            index++;
        }
        else
        {
            playerTurn = "P2";
            playerToGoFirst = "P2";
            Cursor.SetPosition(4, 0);
            Cursor.SetGameState(gameState);
            Cursor.SetPlayerTurn(playerTurn);
            Cursor.SendUnit(P2UnitList[index]);
            index++;
        }
    }

    public void RequestUnit()
    {
        if (playerTurn == "P1")
        {
            if (index < P1UnitList.Count)
            {
                Cursor.SendUnit(P1UnitList[index]);
                index++;
            }
            else if (index >= P1UnitList.Count && !hasSecondPlayerGone)
            {
                index = 0;
                Cursor.SetPosition(4, 0);
                playerTurn = "P2";
                Cursor.SetPlayerTurn(playerTurn);
                hasSecondPlayerGone = true;
                RequestUnit();
            }
        }
        else
        {
            if (index < P2UnitList.Count)
            {
                Cursor.SendUnit(P2UnitList[index]);
                index++;
            }
            else if (index >= P2UnitList.Count && !hasSecondPlayerGone)
            {
                index = 0;
                Cursor.SetPosition(3, 7);
                playerTurn = "P1";
                Cursor.SetPlayerTurn(playerTurn);
                hasSecondPlayerGone = true;
                RequestUnit();
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