using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static HeroScript;

public class TileManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private HeroScript _heroScript;
    [SerializeField] private TurnManager _turnManager;

    [Header("Tile types")]
    [SerializeField] private GameObject _tile;
    [SerializeField] private GameObject _moveTile;
    [SerializeField] private GameObject _interactTile;
    private List<GameObject> _moveTileList;

    public GameObject[,] tiles;
    public GameObject[,] gameBoard;

    private bool _tilesLoaded;


    int[] lineDir = { -1, 0, 1, 0 };
    int[] colDir = { 0, 1, 0, -1 };

    int[] diagonalLineDir = { -1, -1, 1, 1 };
    int[] diagonalColDir = { -1, 1, 1, -1 };

    public void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _gameManager.SetManager(ref _turnManager);
    }

    public void GenerateGameBoard(int sizeX, int sizeY)
    {
        float xPos = 0;
        float yPos = 0.125f;
        //float positionIncrement = Mathf.Abs(xPos) * 2 / (sizeY - 1);
        float positionIncrement = 0.25f; //1 tile is 25 pixels wide so an increment of 0.25 at 100 pixels per unit would get us consecutive tiles with no space inbetween

        tiles = new GameObject[sizeX, sizeY];
        gameBoard = new GameObject[sizeX, sizeY];

        for (int i = 0; i < sizeX; i++)
        {
            xPos = -0.125f;

            for (int j = 0; j < sizeY; j++)
            {
                tiles[i, j] = Instantiate(_tile, new Vector3(xPos, yPos, 0), Quaternion.identity, GameObject.Find("Tile Container").transform);
                tiles[i, j].GetComponent<Tile>().SetCoords(i, j);

                xPos += positionIncrement;

                if((i + j) % 2 == 1)
                {
                    tiles[i, j].GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.9f, 0.9f);
                }
            }

            yPos -= positionIncrement;
        }

        _tilesLoaded = true;
        //_gameManager.CenterCamera();
    }

    public void GenerateMoveTiles(HeroScript heroScript)
    {
        _heroScript = heroScript;

        DestroyMoveTiles();

        if(_turnManager.GetSpeedLeft() != 0)
        {
            CreateMoveTiles(heroScript);
        }

        if (_turnManager.GetActionsLeft() != 0)
        {
            CreateActionTiles();
        }
    }

    public void DestroyMoveTiles()
    {
        GameObject[] moveTiles = GameObject.FindGameObjectsWithTag("Move Tile");

        for (int i = 0; i < moveTiles.Length; i++)
        {
            MoveTile tempTile = moveTiles[i].GetComponent<MoveTile>();

            /*if (!tempTile.IsAttacking() && !tempTile.IsHealing())
            {
                gameBoard[tempTile.GetXPos(), tempTile.GetYPos()] = null;
            }*/

            Destroy(moveTiles[i]);
        }
    }

    public void CreateMoveTiles(HeroScript heroScript)
    {
        SpawnBasicTiles(_heroScript.GetSpeed(), heroScript.GetXPos(), heroScript.GetYPos());
    }

    public void SpawnBasicTiles(int speed, int x, int y)
    {
        if (speed != 0)
        {
            for (int i = 0; i < 4; i++)
            {
                int nextX = x + lineDir[i];
                int nextY = y + colDir[i];

                if (!PositionIsValid(nextX, nextY))
                {
                    continue;
                }

                if (gameBoard[nextX, nextY] != null)
                {
                    continue;
                }

                GameObject reference = SpawnTile(_moveTile, nextX, nextY);
                //gameBoard[nextX, nextY] = reference;
                reference.GetComponent<MoveTile>().SetAttacking(false);
                SpawnBasicTiles(speed - 1, nextX, nextY);
            }
        }
    }


    public void CreateActionTiles()
    {
        string attackType = _heroScript.GetAttackType();

        if (attackType == "mixed")
        {
            int range = _heroScript.GetRange();

            for(int i = 0; i < 4; i++)
            {
                DirectionalCheck(lineDir[i], colDir[i], range);
                DirectionalCheck(diagonalLineDir[i], diagonalColDir[i], range);
            }
        }

        if (attackType == "melee")
        {
            for(int i = 0; i < 4; i++)
            {
                DirectionalCheck(lineDir[i], colDir[i], 1);
            }
        }

        if(attackType == "ranged")
        {
            int range = _heroScript.GetRange();

            for (int i = 0; i < 4; i++)
            {
                DirectionalCheckRanged(lineDir[i], colDir[i], range);
                DirectionalCheckRanged(diagonalLineDir[i], diagonalColDir[i], range);
            }
        }
    }
    
    public void DirectionalCheck(int line, int col, int n)
    {
        int currentLine = _heroScript.GetXPos();
        int currentCol = _heroScript.GetYPos();

        for (int i = 0; i < n; i++)
        {
            currentLine += line;
            currentCol += col;

            if (!PositionIsValid(currentLine, currentCol))
            {
                continue;
            }

            if (gameBoard[currentLine, currentCol] == null)
            {
                continue;
            }

            if (gameBoard[currentLine, currentCol].tag == "Hero")
            {
                if (_heroScript.IsHealer())
                {
                    GameObject reference = SpawnTile(_moveTile, currentLine, currentCol);
                    reference.GetComponent<MoveTile>().SetHealing(true);
                }
            }

            if (gameBoard[currentLine, currentCol].tag == "Enemy")
            {
                GameObject reference = SpawnTile(_moveTile, currentLine, currentCol);
                reference.GetComponent<MoveTile>().SetAttacking(true);
            }
        }
    }

    public void DirectionalCheckRanged(int line, int col, int n)
    {
        int currentLine = _heroScript.GetXPos();
        int currentCol = _heroScript.GetYPos();

        for (int i = 0; i < n; i++)
        {
            currentLine += line;
            currentCol += col;

            if (!PositionIsValid(currentLine, currentCol))
            {
                continue;
            }

            if (gameBoard[currentLine, currentCol] == null)
            {
                continue;
            }

            if (gameBoard[currentLine, currentCol].tag != "Enemy")
            {
                return;
            }

            if(i == 0)
            {
                continue;
            }

            GameObject reference = SpawnTile(_moveTile, currentLine, currentCol);
            reference.GetComponent<MoveTile>().SetAttacking(true);
        }
    }

    public GameObject SpawnTile(GameObject tile, int line, int col)
    {
        Vector3 tilePosition = tiles[line, col].transform.position;

        GameObject reference = Instantiate(tile, tilePosition, Quaternion.identity);
        reference.GetComponent<Tile>().SetCoords(line, col);

        return reference;
    }

    public bool PositionIsValid(int x, int y)
    {
        if (x >= 0 && x < _gameManager.GetNrOfRows() && y >= 0 && y < _gameManager.GetNrOfColumns())
        {
            return true;
        }

        return false;
    }

    [ContextMenu("Test disable")]
    public void DisableMoveTiles()
    {
        SetActiveMoveTiles();

        foreach (GameObject tile in _moveTileList)
        {
            tile.SetActive(false);
        }
    }

    [ContextMenu("Test enable")]
    public void EnableMoveTiles()
    {
        if(_moveTileList.Count <= 0)
        {
            return;
        }

        foreach (GameObject tile in _moveTileList)
        {
            tile.SetActive(true);
        }
    }

    public void SetActiveMoveTiles()
    {
        _moveTileList = GameObject.FindGameObjectsWithTag("Move Tile").ToList();
    }

    public bool TilesAreLoaded() { return _tilesLoaded; }
}
