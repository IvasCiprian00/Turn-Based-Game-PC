using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private HeroManager _heroManager;
    [SerializeField] private TileManager _tileManager;
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private HeroScript _heroScript;

    private int _currentHero;

    private bool _heroesSpawned;

    private void Awake()
    {
        _tileManager = GameObject.Find("Tile Manager").GetComponent<TileManager>();
        _heroManager = GameObject.Find("Hero Manager").GetComponent<HeroManager>();
        _enemyManager = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();
    }

    /*public void StartHeroTurns()
    {
        _currentHero = 0;
        _heroScript = _heroManager.heroesAlive[_currentHero].GetComponent<HeroScript>();
        //spawn move tiles
        // spawn attack tiles
    }

    public void EndTurn()
    {
        _currentHero++;

        if(_currentHero >= _heroManager.GetHeroCount())
        {
            _currentHero = 0;
        }
    }*/

    public void EndTurn()
    {
        //_uiManager.HideSkills();
        //_uiManager.CancelSkill();
        _currentHero++;

        if (_currentHero >= _heroManager.GetHeroCount() || _currentHero == -1)
        {
            _currentHero = 0;

            //StartEnemyTurns();

            return;
        }

        _heroScript = _heroManager.heroesAlive[_currentHero].GetComponent<HeroScript>();
        //speedLeft = _heroScript.GetSpeed();
        //attacksLeft = _heroScript.GetSpeed();

        //_uiManager.DisplaySkills(_heroScript.skills);
        //GenerateMoveTiles();
    }

    /*public void StartEnemyTurns()
    {
        DestroyMoveTiles();

        currentEnemy = 0;

        enemyScript = _enemyManager.enemiesAlive[currentEnemy].GetComponent<EnemyScript>();

        _heroTurn = false;
        enemyScript.StartTurn();
    }*/

    public void StartHeroTurns()
    {
        _currentHero = 0;
        //_heroTurn = true;

        _heroScript = _heroManager.heroesAlive[_currentHero].GetComponent<HeroScript>();
        //speedLeft = _heroScript.GetSpeed();
        //attacksLeft = _heroScript.GetNumberOfAttacks();

        //_uiManager.DisplaySkills(_heroScript.skills);

        //GenerateMoveTiles();
    }

    /*public void GenerateMoveTiles()
    {
        DestroyMoveTiles();

        CreateMoveTiles();

        if (attacksLeft != 0)
        {
            CreateAttackTiles();
        }

    }


    public void DestroyMoveTiles()
    {
        GameObject[] moveTiles = GameObject.FindGameObjectsWithTag("Move Tile");

        for (int i = 0; i < moveTiles.Length; i++)
        {
            Destroy(moveTiles[i]);
        }
    }

    public void CreateMoveTiles()
    {
        string mvmt = _heroScript.GetMovementType();

        switch (mvmt)
        {
            case "basic":
                SpawnBasicTiles(speedLeft, _heroScript.GetXPos(), _heroScript.GetYPos());
                break;
            case "fast":
                SpawnBasicTiles(speedLeft, _heroScript.GetXPos(), _heroScript.GetYPos());
                break;
            case "teleport":
                SpawnTeleportTiles();
                break;
            default: break;
        }

    }

    public void CreateAttackTiles()
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
    }

    public bool PositionIsValid(int x, int y)
    {
        if (x >= 0 && x < _gameManager.nrOfLines && y >= 0 && y < _gameManager.numberOfColumns)
        {
            return true;
        }

        return false;
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

    public void SpawnTeleportTiles()
    {

    }

    public void SetTurnOrder()
    {
        //later implement this with character initiative
    }*/
}
