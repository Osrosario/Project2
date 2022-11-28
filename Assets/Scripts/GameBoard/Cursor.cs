using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    [Header("Script from Navigation Grid Object")]
    public NavigationMap NavMap;

    [Header("Script from Game Grid Object")]
    public GameMap GameMap;

    [Header("Script from Game Master Object")]
    public GameMaster GameMaster;

    [Header("Cursor Start Position on Grid")]
    public int CurrXIndex;
    public int CurrYIndex;

    [Header("Dummy Game Object")]
    public GameObject DummyObject;

    private List<List<Transform>> navMap = new List<List<Transform>>();
    private List<List<GameObject>> gameMap = new List<List<GameObject>>();
    private string gameState;
    private string playerTurn;
    private Transform tile;
    private float adjustOnTop;
    private bool moveDisabled;
    private GameObject selectedUnit;
    private bool isUnitSelected;
    private bool isUnitPlaced;
    private int PrevXIndex;
    private int PrevYIndex;
    private int UnitXIndex;
    private int UnitYIndex;
    private int maxMove;
    private int unitMaxMove;
    private int unitsToMove;
    private Attack unitWeapon;

    private void Awake()
    {
        navMap = NavMap.GetMap();
        gameMap = GameMap.GetMap();
        moveDisabled = false;
        isUnitSelected = false;
        isUnitPlaced = false;
        unitsToMove = 2;
    }

    private void Update()
    {
        if (!moveDisabled)
        {
            NavigateGrid();
        }

        if (gameState == "prep")
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (gameMap[CurrYIndex][CurrXIndex] == null)
                {
                    selectedUnit.transform.position = transform.position;
                    selectedUnit.GetComponent<Stats>().SetTerrainBuff(navMap[CurrYIndex][CurrXIndex].tag);
                    gameMap[CurrYIndex][CurrXIndex] = selectedUnit;
                    GameMaster.AddCount();
                    GameMaster.RequestUnit();
                }
            }
        }
        else if (gameState == "play")
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isUnitSelected)
            {
                SelectUnit();
            }
            else if (Input.GetKeyDown(KeyCode.Space) && !moveDisabled && isUnitSelected)
            {
                ConfirmPlacement();
            }
            else if (isUnitPlaced)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))         { selectedUnit.transform.localEulerAngles = new Vector3(0, 0, 0); }
                else if (Input.GetKeyDown(KeyCode.DownArrow))  { selectedUnit.transform.localEulerAngles = new Vector3(0, 180f, 0); }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))  { selectedUnit.transform.localEulerAngles = new Vector3(0, -90f, 0); }
                else if (Input.GetKeyDown(KeyCode.RightArrow)) { selectedUnit.transform.localEulerAngles = new Vector3(0, 90f, 0); }
                else if (Input.GetKeyDown(KeyCode.Space))      { EndUnitTurn(); }

                if (unitWeapon.CanAttack())
                {
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        unitWeapon.AttackUnit();
                        EndUnitTurn();
                    }
                }
            }

            selectedUnit.transform.position = transform.position;

            if (unitsToMove == 0)
            {
                if (playerTurn == "P1")
                {
                    playerTurn = "P2";
                }
                else
                {
                    playerTurn = "P1";
                }

                unitsToMove = 2;
            }
        }
    }

    private void SelectUnit()
    {
        if (gameMap[CurrYIndex][CurrXIndex] != null && gameMap[CurrYIndex][CurrXIndex].tag == playerTurn)
        {
            selectedUnit = gameMap[CurrYIndex][CurrXIndex];
            gameMap[CurrYIndex][CurrXIndex] = null;
            UnitXIndex = CurrXIndex;
            UnitYIndex = CurrYIndex;
            unitMaxMove = selectedUnit.GetComponent<Stats>().GetMovement();
            unitWeapon = selectedUnit.GetComponentInChildren<Attack>();
            isUnitSelected = true;
        }
    }

    private void ConfirmPlacement()
    {
        if (gameMap[CurrXIndex][CurrYIndex] == null)
        {
            selectedUnit.transform.position = transform.position;
            selectedUnit.GetComponent<Stats>().SetTerrainBuff(navMap[CurrYIndex][CurrXIndex].tag);
            gameMap[CurrYIndex][CurrXIndex] = selectedUnit;
            isUnitPlaced = true;
            moveDisabled = true;
        }
    }

    private void EndUnitTurn()
    {
        selectedUnit = DummyObject;
        unitsToMove--;
        isUnitSelected = false;
        isUnitPlaced = false;
        moveDisabled = false;
    }
    
    public void SetPosition(int xIndex, int yIndex)
    {
        CurrXIndex = xIndex;
        CurrYIndex = yIndex;
        PrevXIndex = xIndex;
        PrevYIndex = yIndex;
    }

    private void NavigateGrid()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) 
        {
            PrevXIndex = CurrXIndex;
            PrevYIndex = CurrYIndex;
            CurrYIndex--;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) 
        {
            PrevXIndex = CurrXIndex;
            PrevYIndex = CurrYIndex;
            CurrYIndex++;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) 
        {
            PrevXIndex = CurrXIndex;
            PrevYIndex = CurrYIndex;
            CurrXIndex--;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) 
        {
            PrevXIndex = CurrXIndex;
            PrevYIndex = CurrYIndex;
            CurrXIndex++;
        }

        if (gameState == "prep")
        {
            if (playerTurn == "P1")
            {
                if (CurrYIndex > 7)      { CurrYIndex = 7; }
                else if (CurrYIndex < 6) { CurrYIndex = 6; }
            }
            else
            {
                if (CurrYIndex > 1)      { CurrYIndex = 1; }
                else if (CurrYIndex < 0) { CurrYIndex = 0; }
            }
        }

        if (CurrXIndex > 7)      { CurrXIndex = 7; }
        else if (CurrXIndex < 0) { CurrXIndex = 0; }
        else if (CurrYIndex > 7) { CurrYIndex = 7; }
        else if (CurrYIndex < 0) { CurrYIndex = 0; }

        if (isUnitSelected)
        {
            maxMove = Mathf.Abs(UnitXIndex - CurrXIndex) + Mathf.Abs(UnitYIndex - CurrYIndex);

            if (maxMove > unitMaxMove || gameMap[CurrYIndex][CurrXIndex] != null)
            {
                CurrXIndex = PrevXIndex;
                CurrYIndex = PrevYIndex;
            }
        }

        tile = navMap[CurrYIndex][CurrXIndex];

        if (tile.tag == "Plain")         { adjustOnTop = 0.25f; }
        else if (tile.tag == "Forest")   { adjustOnTop = 0.375f; }
        else if (tile.tag == "Hill")     { adjustOnTop = 0.5f; }
        else if (tile.tag == "Mountain") { adjustOnTop = 0.625f; }

        /*
        Debug.Log($"Curr X: {CurrXIndex}");
        Debug.Log($"Curr Y: {CurrYIndex}");
        Debug.Log($"Prev X: {PrevXIndex}");
        Debug.Log($"Prev Y: {PrevYIndex}");
        */

        transform.position = new Vector3(tile.position.x, tile.position.y + adjustOnTop, tile.position.z);
    }
    
    public void SendUnit(GameObject unit) { selectedUnit = unit; }

    public void SetGameState(string state) { gameState = state; }

    public void SetPlayerTurn(string turn) { playerTurn = turn; }
}
