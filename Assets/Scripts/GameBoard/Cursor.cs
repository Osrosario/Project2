using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Cursor : MonoBehaviour
{
    [Header("Script from Navigation Grid Object")]
    public NavigationMap NavMap;

    [Header("Script from Game Master Object")]
    public GameMaster GameMaster;

    [Header("Stat Window Objects")]
    public GameObject playerOneStats;
    public GameObject playerTwoStats;

    [Header("Notification Window Objects")]
    public GameObject playerOneNoti1;
    public GameObject playerTwoNoti1;

    [Header("Highlight for Notification Window Objects")]
    public GameObject playerOneBorder;
    public GameObject playerTwoBorder;

    [Header("Dummy Game Object")]
    public GameObject DummyObject;

    [Header("Text of Buttons")]
    public TMP_Text DiKeysText;
    public TMP_Text SpacebarText;
    public TMP_Text AKeyText;
    public TMP_Text SKeyText;

    //Persistant Data
    private string gameState;
    private string playerTurn;
    List<Tile> rangeList = new List<Tile>();

    //Map Navigation Data
    private List<List<Tile>> navMap = new List<List<Tile>>();
    private Tile tile;
    private Vector3 prevPos;
    private int CurrXIndex;
    private int CurrYIndex;
    private int PrevXIndex;
    private int PrevYIndex;
    private float adjustOnTop;
    private bool moveDisabled;

    //Selected Unit Data
    private GameObject selectedUnit;
    private bool isUnitSelected;
    private bool isUnitPlaced;
    private int UnitXIndex;
    private int UnitYIndex;
    private int unitMaxMove;
    private int unitsToMove;
    private Attack unitWeapon;
    private bool canAttack;
    private Tile enemyTile;
    
    //Modifed Breadth-First-Search Data
    private GameObject[] tiles;
    private Tile currentTile;
    private List<Tile> tilesInRange = new List<Tile>();

    private void Awake()
    {
        navMap = NavMap.GetMap();
        moveDisabled = false;
        isUnitSelected = false;
        isUnitPlaced = false;
        unitsToMove = 2;
        canAttack = false;

        tiles = GameObject.FindGameObjectsWithTag("Tile");
    }

    private void Start()
    {
        StartCoroutine(Notify());
    }

    private void Update()
    {
        if (!moveDisabled)
        {
            NavigateMap();
        }

        if (gameState == "prep")
        {
            if (playerTurn == "P1")
            {
                playerOneBorder.SetActive(true);
                playerOneBorder.GetComponentInChildren<Image>().color = Color.yellow;
                playerTwoBorder.SetActive(false);
            }
            else
            {
                playerTwoBorder.SetActive(true);
                playerTwoBorder.GetComponentInChildren<Image>().color = Color.yellow;
                playerOneBorder.SetActive(false);
            }

            DiKeysText.text = "Move";
            SpacebarText.text = "Select";
            AKeyText.text = "-";
            SKeyText.text = "-";

            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (navMap[CurrYIndex][CurrXIndex].UnitOnTile == null)
                {
                    initialPlacement();
                }
            }
        }
        else if (gameState == "play")
        {
            if (Input.GetKeyUp(KeyCode.S) && !moveDisabled)
            {
                viewUnitData();

                DiKeysText.text = "-";
                SpacebarText.text = "-";
                AKeyText.text = "-";
                SKeyText.text = "Unselect";
            }
            else if (Input.GetKeyUp(KeyCode.S) && moveDisabled)
            {
                hideUnitData();
            }
            else if (isUnitSelected && isUnitPlaced)
            {
                DiKeysText.text = "Rotate";
                SpacebarText.text = "End Unit Turn";
                AKeyText.text = "-";
                SKeyText.text = "-";

                if (Input.GetKeyUp(KeyCode.UpArrow))
                {
                    turnUnit(0f, "North");
                }
                else if (Input.GetKeyUp(KeyCode.DownArrow))
                {
                    turnUnit(180f, "South");
                }
                else if (Input.GetKeyUp(KeyCode.LeftArrow))
                {
                    turnUnit(-90f, "West");
                }
                else if (Input.GetKeyUp(KeyCode.RightArrow))
                {
                    turnUnit(90f, "East");
                }
                else if (Input.GetKeyUp(KeyCode.Space))
                {
                    endUnitTurn();
                }

                if (canAttack)
                {
                    DiKeysText.text = "Rotate";
                    SpacebarText.text = "End Unit Turn";
                    AKeyText.text = "Attack";
                    SKeyText.text = "-";

                    if (Input.GetKeyUp(KeyCode.A))
                    {
                        unitWeapon.AttackUnit(enemyTile);
                        endUnitTurn();
                    }
                }
                else
                {
                    DiKeysText.text = "Rotate";
                    SpacebarText.text = "End Unit Turn";
                    AKeyText.text = "-";
                    SKeyText.text = "-";
                }
            }
            else if (isUnitSelected)
            {
                DiKeysText.text = "Move";
                SpacebarText.text = "Select";
                AKeyText.text = "-";
                SKeyText.text = "Unselect";

                if (Input.GetKeyUp(KeyCode.Space))
                {
                    confirmPlacement();
                }

                if (Input.GetKeyUp(KeyCode.Z))
                {
                    unselectUnit();
                }
            }
            else if (!isUnitSelected && !moveDisabled)
            {
                DiKeysText.text = "Move";
                SpacebarText.text = "Select";
                AKeyText.text = "-";
                SKeyText.text = "Preview";

                if (playerTurn == "P1")
                {
                    playerOneStats.GetComponent<DisplayStats>().ClearInfo();
                    playerOneStats.SetActive(false);
                }
                else
                {
                    playerTwoStats.GetComponent<DisplayStats>().ClearInfo();
                    playerTwoStats.SetActive(false);
                }

                if (Input.GetKeyUp(KeyCode.Space))
                {
                    selectUnit();
                }
            }

            if (isUnitSelected)
            {
                ShowAttackRange();

                if (playerTurn == "P1")
                {
                    playerOneStats.GetComponent<DisplayStats>().ShowInfo(selectedUnit.GetComponent<Stats>(), navMap[CurrYIndex][CurrXIndex].name);
                    playerOneStats.SetActive(true);
                }
                else
                {
                    playerTwoStats.GetComponent<DisplayStats>().ShowInfo(selectedUnit.GetComponent<Stats>(), navMap[CurrYIndex][CurrXIndex].name);
                    playerTwoStats.SetActive(true);
                }
            }

            selectedUnit.transform.position = transform.position;

            if (unitsToMove == 0)
            {
                if (playerTurn == "P1")
                {
                    SwitchTurn();
                }
                else
                {
                    SwitchTurn();
                }

                unitsToMove = 2;
            }

        }
    }

    private void initialPlacement()
    {
        selectedUnit.transform.position = transform.position;
        selectedUnit.GetComponent<Stats>().SetTerrainBuff(navMap[CurrYIndex][CurrXIndex].name);
        navMap[CurrYIndex][CurrXIndex].UnitOnTile = selectedUnit;
        navMap[CurrYIndex][CurrXIndex].IsUnitOnTile = true;
        GameMaster.AddCount();
        GameMaster.RequestUnit();
    }

    private void selectUnit()
    {
        if (navMap[CurrYIndex][CurrXIndex].UnitOnTile.tag == playerTurn)
        {
            prevPos = transform.position;
            selectedUnit = navMap[CurrYIndex][CurrXIndex].UnitOnTile;
            navMap[CurrYIndex][CurrXIndex].UnitOnTile = null;
            navMap[CurrYIndex][CurrXIndex].IsUnitOnTile = false;
            UnitXIndex = CurrXIndex;
            UnitYIndex = CurrYIndex;
            unitMaxMove = selectedUnit.GetComponent<Stats>().GetMovement();
            unitWeapon = selectedUnit.GetComponentInChildren<Attack>();
            isUnitSelected = true;
            showUnitRange();
        }
    }

    private void unselectUnit()
    {
        if (selectedUnit.name != "Dummy_Object")
        {
            selectedUnit.transform.position = prevPos;
            navMap[UnitYIndex][UnitXIndex].UnitOnTile = selectedUnit;
            navMap[UnitYIndex][UnitXIndex].IsUnitOnTile = true;
            unitMaxMove = 0;
            unitWeapon = null;
            isUnitSelected = false;
            selectedUnit = DummyObject;
            hideUnitRange();
            refreshATKRange();
        }
    }

    private void confirmPlacement()
    {
        selectedUnit.transform.position = transform.position;
        selectedUnit.GetComponent<Stats>().SetTerrainBuff(navMap[CurrYIndex][CurrXIndex].name);
        navMap[CurrYIndex][CurrXIndex].UnitOnTile = selectedUnit;
        navMap[CurrYIndex][CurrXIndex].IsUnitOnTile = true;
        hideUnitRange();
        isUnitPlaced = true;
        moveDisabled = true;
    }

    private void turnUnit(float degree, string face)
    {
        refreshATKRange();
        checkAttackRange(rangeList);
        selectedUnit.transform.localEulerAngles = new Vector3(0, degree, 0);
        selectedUnit.GetComponent<Stats>().SetFace(face);
    }

    private void endUnitTurn()
    {
        refreshATKRange();
        hideUnitRange();

        if (playerTurn == "P1")
        {
            playerOneStats.SetActive(false);
            playerOneStats.GetComponent<DisplayStats>().ClearInfo();
        }
        else
        {
            playerTwoStats.SetActive(false);
            playerTwoStats.GetComponent<DisplayStats>().ClearInfo();
        }

        selectedUnit = DummyObject;
        unitsToMove--;
        isUnitSelected = false;
        isUnitPlaced = false;
        moveDisabled = false;
    }

    private void computeAdjacencyList()
    {
        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors();
        }
    }

    private void showUnitRange()
    {
        computeAdjacencyList();
        currentTile = navMap[CurrYIndex][CurrXIndex];

        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.visited = true;

        while (process.Count > 0)
        {
            Tile t = process.Dequeue();

            tilesInRange.Add(t);

            if (isUnitSelected)
            {
                t.marked = true;

                if (t.distance < unitMaxMove)
                {
                    foreach (Tile tile in t.adjacencyList)
                    {
                        if (!tile.visited)
                        {
                            tile.visited = true;
                            tile.distance = 1 + t.distance;
                            process.Enqueue(tile);
                        }
                    }
                }
            }
            else
            {
                if (t.distance <= unitMaxMove)
                {
                    t.marked = true;
                }
                else if (t.distance > unitMaxMove)
                {
                    t.unitLooking = true;
                }

                if (t.distance < unitMaxMove + selectedUnit.GetComponent<Stats>().GetATKRange())
                {
                    foreach (Tile tile in t.adjacencyList)
                    {
                        if (!tile.visited)
                        {
                            tile.visited = true;
                            tile.distance = 1 + t.distance;
                            process.Enqueue(tile);
                        }
                    }
                }
            }
        }
    }

    private void hideUnitRange()
    {
        foreach (Tile tile in tilesInRange)
        {
            tile.marked = false;
            tile.unitLooking = false;
        }

        tilesInRange.Clear();
    }

    public void ShowAttackRange()
    {
        Stats unitStats = selectedUnit.GetComponent<Stats>();

        if (unitStats.GetFace() == "North")
        {
            rangeList.Clear();

            for (int i = 1; i <= unitStats.GetATKRange(); i++)
            {
                int y = CurrYIndex - i;

                if (y < 0)
                {
                    y = 0;
                }

                rangeList.Add(navMap[y][CurrXIndex]);
            }
        }
        else if (unitStats.GetFace() == "South")
        {
            rangeList.Clear();

            for (int i = 1; i <= unitStats.GetATKRange(); i++)
            {
                int y = CurrYIndex + i;

                if (y > 7)
                {
                    y = 7;
                }

                rangeList.Add(navMap[y][CurrXIndex]);
            }
        }
        else if (unitStats.GetFace() == "West")
        {
            rangeList.Clear();

            for (int i = 1; i <= unitStats.GetATKRange(); i++)
            {
                int x = CurrXIndex - i;

                if (x < 0)
                {
                    x = 0;
                }

                rangeList.Add(navMap[CurrYIndex][x]);
            }
        }
        else if (unitStats.GetFace() == "East")
        {
            rangeList.Clear();

            for (int i = 1; i <= unitStats.GetATKRange(); i++)
            {
                int x = CurrXIndex + i;

                if (x > 7)
                {
                    x = 7;
                }

                rangeList.Add(navMap[CurrYIndex][x]);
            }
        }

        checkAttackRange(rangeList);

        foreach (Tile tile in rangeList)
        {
            tile.unitLooking = true;
        }
    }

    private void refreshATKRange()
    {
        if (rangeList.Count > 0)
        {
            foreach (Tile tile in rangeList)
            {
                tile.unitLooking = false;
            }
        }
    }

    private void viewUnitData()
    {
        if (navMap[CurrYIndex][CurrXIndex].UnitOnTile != null)
        {
            moveDisabled = true;
            selectedUnit = navMap[CurrYIndex][CurrXIndex].UnitOnTile;
            unitMaxMove = selectedUnit.GetComponent<Stats>().GetMovement();
            showUnitRange();

            string unitTag = selectedUnit.tag;

            if (unitTag == "P1")
            {
                playerOneStats.GetComponent<DisplayStats>().ShowInfo(selectedUnit.GetComponent<Stats>(), navMap[CurrYIndex][CurrXIndex].name);
                playerOneStats.SetActive(true);
            }
            else
            {
                playerTwoStats.GetComponent<DisplayStats>().ShowInfo(selectedUnit.GetComponent<Stats>(), navMap[CurrYIndex][CurrXIndex].name);
                playerTwoStats.SetActive(true);
            }
        }
    }

    private void hideUnitData()
    {
        string unitTag = selectedUnit.tag;

        if (unitTag == "P1")
        {
            playerOneStats.GetComponent<DisplayStats>().ClearInfo();
            playerOneStats.SetActive(false);
        }
        else
        {
            playerTwoStats.GetComponent<DisplayStats>().ClearInfo();
            playerTwoStats.SetActive(false);
        }

        selectedUnit = DummyObject;
        unitMaxMove = 0;
        hideUnitRange();
        navMap[CurrYIndex][CurrXIndex].ResetBFSData();
        moveDisabled = false;
    }

    private void checkAttackRange(List<Tile> tiles)
    {
        foreach (Tile t in tiles)
        {
            if (t.UnitOnTile != null && t.UnitOnTile.tag != playerTurn)
            {
                canAttack = true;
                enemyTile = t;
            }
            else
            {
                canAttack = false;
                enemyTile = null;
            }
        }
    }

    IEnumerator Notify()
    {
        if (playerTurn == "P1")
        {
            playerOneNoti1.SetActive(true);
            yield return new WaitForSeconds(2);
            playerOneNoti1.SetActive(false);
        }
        else
        {
            playerTwoNoti1.SetActive(true);
            yield return new WaitForSeconds(2);
            playerTwoNoti1.SetActive(false);
        }
    }

    private void SwitchTurn()
    {
        if (playerTurn == "P1")
        {
            playerTurn = "P2";
            playerTwoBorder.SetActive(true);
            playerTwoBorder.GetComponentInChildren<Image>().color = Color.yellow;
            playerOneBorder.SetActive(false);
        }
        else
        {
            playerTurn = "P1";
            playerOneBorder.SetActive(true);
            playerOneBorder.GetComponentInChildren<Image>().color = Color.yellow;
            playerTwoBorder.SetActive(false);
        }
    }

    private void NavigateMap()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (isUnitSelected)
            {
                refreshATKRange();
            }

            PrevXIndex = CurrXIndex;
            PrevYIndex = CurrYIndex;
            CurrYIndex--;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (isUnitSelected)
            {
                refreshATKRange();
            }

            PrevXIndex = CurrXIndex;
            PrevYIndex = CurrYIndex;
            CurrYIndex++;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (isUnitSelected)
            {
                refreshATKRange();
            }

            PrevXIndex = CurrXIndex;
            PrevYIndex = CurrYIndex;
            CurrXIndex--;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (isUnitSelected)
            {
                refreshATKRange();
            }

            PrevXIndex = CurrXIndex;
            PrevYIndex = CurrYIndex;
            CurrXIndex++;
        }

        if (gameState == "prep")
        {
            if (playerTurn == "P1")
            {
                if (CurrYIndex > 7) { CurrYIndex = 7; }
                else if (CurrYIndex < 6) { CurrYIndex = 6; }
            }
            else
            {
                if (CurrYIndex > 1) { CurrYIndex = 1; }
                else if (CurrYIndex < 0) { CurrYIndex = 0; }
            }
        }

        if (CurrXIndex > 7) { CurrXIndex = 7; }
        else if (CurrXIndex < 0) { CurrXIndex = 0; }
        else if (CurrYIndex > 7) { CurrYIndex = 7; }
        else if (CurrYIndex < 0) { CurrYIndex = 0; }

        if (isUnitSelected)
        {
            int maxMove = Mathf.Abs(UnitXIndex - CurrXIndex) + Mathf.Abs(UnitYIndex - CurrYIndex);

            if (maxMove > unitMaxMove || navMap[CurrYIndex][CurrXIndex].UnitOnTile != null)
            {
                CurrXIndex = PrevXIndex;
                CurrYIndex = PrevYIndex;
            }
        }

        tile = navMap[CurrYIndex][CurrXIndex];

        if (tile.name == "Plain") { adjustOnTop = 0.25f; }
        else if (tile.name == "Forest") { adjustOnTop = 0.50f; }
        else if (tile.name == "Hill") { adjustOnTop = 0.75f; }
        else if (tile.name == "Mountain") { adjustOnTop = 1f; }

        /*
        Debug.Log($"Curr X: {CurrXIndex}");
        Debug.Log($"Curr Y: {CurrYIndex}");
        Debug.Log($"Prev X: {PrevXIndex}");
        Debug.Log($"Prev Y: {PrevYIndex}");
        */

        transform.position = new Vector3(tile.GetPosition().x, tile.GetPosition().y + adjustOnTop, tile.GetPosition().z);
    }

    public void SetPosition(int xIndex, int yIndex)
    {
        CurrXIndex = xIndex;
        CurrYIndex = yIndex;
        PrevXIndex = xIndex;
        PrevYIndex = yIndex;
    }

    public void SendUnit(GameObject unit) { selectedUnit = unit; }

    public void SetGameState(string state) { gameState = state; }

    public void SetPlayerTurn(string turn) 
    { 
        playerTurn = turn;

        if (playerTurn == "P1")
        {
            playerOneBorder.SetActive(true);
            playerOneBorder.GetComponentInChildren<Image>().color = Color.yellow;
            playerTwoBorder.SetActive(false);
        }
        else
        {
            playerTwoBorder.SetActive(true);
            playerTwoBorder.GetComponentInChildren<Image>().color = Color.yellow;
            playerOneBorder.SetActive(false);
        }
    }
}
    

