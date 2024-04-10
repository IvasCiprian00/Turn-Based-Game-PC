using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;

    [SerializeField] private GameObject _tile;
    [SerializeField] private GameObject _moveTile;

    public GameObject[,] tiles;
    public GameObject[,] gameBoard;

    private bool _tilesLoaded;

    int[] lineDir = { -1, 0, 1, 0 };
    int[] colDir = { 0, 1, 0, -1 };

    public void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    public void GenerateGameBoard(int sizeX, int sizeY)
    {
        float xPos = -1.2f;
        float yPos = 0.65f;
        float positionIncrement = Mathf.Abs(xPos) * 2 / (sizeY - 1);

        tiles = new GameObject[sizeX, sizeY];
        gameBoard = new GameObject[sizeX, sizeY];

        for (int i = 0; i < sizeX; i++)
        {
            xPos = -1.2f;

            for (int j = 0; j < sizeY; j++)
            {
                tiles[i, j] = Instantiate(_tile, new Vector3(xPos, yPos, 0), Quaternion.identity, GameObject.Find("Tile Container").transform);
                tiles[i, j].GetComponent<TileScript>().SetCoords(i, j);

                xPos += positionIncrement;
            }

            yPos -= positionIncrement;
        }

        _tilesLoaded = true;
    }

    public void GenerateMoveTiles(HeroScript heroScript)
    {
        DestroyMoveTiles();

        CreateMoveTiles(heroScript);

        /*if (attacksLeft != 0)
        {
            CreateAttackTiles();
        }*/
    }

    public void DestroyMoveTiles()
    {
        GameObject[] moveTiles = GameObject.FindGameObjectsWithTag("Move Tile");

        for (int i = 0; i < moveTiles.Length; i++)
        {
            Destroy(moveTiles[i]);
        }
    }

    public void CreateMoveTiles(HeroScript heroScript)
    {
        SpawnBasicTiles(1, heroScript.GetXPos(), heroScript.GetYPos());
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

                SpawnTile(false, nextX, nextY);
                SpawnBasicTiles(speed - 1, nextX, nextY);
            }
        }
    }

    public void SpawnTile(bool isAttackTile, int line, int col)
    {
        Vector3 tilePosition = tiles[line, col].transform.position;
        tilePosition -= new Vector3(0, 0, 1);

        GameObject reference = Instantiate(_moveTile, tilePosition, Quaternion.identity);
        reference.GetComponent<MoveTileScript>().SetCoords(line, col);
        reference.GetComponent<MoveTileScript>().SetAttacking(isAttackTile);
    }

    public bool PositionIsValid(int x, int y)
    {
        if (x >= 0 && x < _gameManager.GetNrOfRows() && y >= 0 && y < _gameManager.GetNrOfColumns())
        {
            return true;
        }

        return false;
    }


    /* public void CreateAttackTiles()
     {
         string attackType = _heroScript.GetAttackType();

         if (attackType == "mixed" || attackType == "ranged")
         {
             int range = _heroScript.GetRange();

             DirectionalCheck(0, 1, range);
             DirectionalCheck(1, 0, range);
             DirectionalCheck(0, -1, range);
             DirectionalCheck(-1, 0, range);
             DirectionalCheck(1, 1, range);
             DirectionalCheck(1, -1, range);
             DirectionalCheck(-1, -1, range);
             DirectionalCheck(-1, 1, range);
         }

         if (attackType == "melee")
         {
             DirectionalCheck(0, 1, 1);
             DirectionalCheck(1, 0, 1);
             DirectionalCheck(0, -1, 1);
             DirectionalCheck(-1, 0, 1);
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

             if (_tileManager.gameBoard[currentLine, currentCol] == null)
             {
                 continue;
             }

             if (_tileManager.gameBoard[currentLine, currentCol].tag == "Enemy")
             {
                 SpawnTile(true, currentLine, currentCol);
             }
         }
     }*/


    public bool TilesAreLoaded() { return _tilesLoaded; }
}
