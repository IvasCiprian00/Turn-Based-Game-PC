using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

abstract public class Enemy : MonoBehaviour
{
    protected enum TargetType
    {
        closest,
        lowestHp
    }

    protected GameManager _gameManager;
    protected EnemyManager _enemyManager;
    protected HeroManager _heroManager;
    protected TileManager _tileManager;
    protected TurnManager _turnManager;
    protected UIManager _uiManager;

    [SerializeField] protected HealthbarScript _healthbarScript;
    [SerializeField] protected TargetType _targetType;
    [SerializeField] protected Animator _animator;

    [Header("Enemy Stats")]
    [SerializeField] protected int _hp;
    [SerializeField] protected int _maxHp;
    [SerializeField] protected int _damage;
    [SerializeField] protected int _speed;
    [SerializeField] protected int _attackCount;


    protected int _xPos;
    protected int _yPos;
    protected GameObject _target;
    protected GameObject[] _unreachableHeroes;
    protected HeroScript _heroScript;



    [Serializable]
    public struct PathCoordinates
    {
        public int x;
        public int y;
    }

    public struct PathGrid
    {
        public int state;
        public int distance;
    }

    protected int minPathLength;
    protected bool _canReach;
    protected int endX;
    protected int endY;
    protected PathCoordinates[] currentPath;
    protected PathCoordinates[] shortestPath;
    protected PathGrid[,] grid;

    protected int[] dl = { -1, 0, 1, 0 };
    protected int[] dc = { 0, 1, 0, -1 };

    protected bool _isMoving;
    protected GameObject _targetTile;
    public void SetManagers()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _turnManager = GameObject.Find("Turn Manager").GetComponent<TurnManager>();
        _enemyManager = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();
        _tileManager = GameObject.Find("Tile Manager").GetComponent<TileManager>();
        _heroManager = GameObject.Find("Hero Manager").GetComponent<HeroManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    public void SetHealthbar()
    {
        _healthbarScript = GetComponentInChildren<HealthbarScript>();
    }

    public void UpdateHealthbar()
    {
        if (_healthbarScript == null)
        {
            return;
        }

        _healthbarScript.SetHp(_hp);
        _healthbarScript.SetMaxHp(_maxHp);
    }

    public void Movement()
    {

        if (_isMoving)
        {
            var step = 10 * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _targetTile.transform.position, step);

            if (Vector3.Distance(transform.position, _targetTile.transform.position) < 0.001f)
            {
                transform.position = _targetTile.transform.position;
                _isMoving = false;
            }
        }
    }

    public void FindTarget()
    {
        List<GameObject> unreachableHeroes = new List<GameObject>();

        do
        {
            GetPossibleTarget(unreachableHeroes);

            _canReach = VerifyTarget();

            unreachableHeroes.Add(_heroScript.gameObject);
        } while (!_canReach);
    }

    public void GetPossibleTarget(List<GameObject> targets)
    {
        int minDistance = 99;

        switch (_targetType)
        {
            case TargetType.closest:

                for (int i = 0; i < _heroManager.GetHeroCount(); i++)
                {
                    HeroScript hsScript = _heroManager.heroesAlive[i].GetComponent<HeroScript>();
                    int distance = Mathf.Abs(_xPos - hsScript.GetXPos()) + Mathf.Abs(_yPos - hsScript.GetYPos());

                    if (distance >= minDistance)
                    {
                        continue;
                    }

                    if (targets.Contains(hsScript.gameObject))
                    {
                        continue;
                    }

                    minDistance = distance;
                    _target = _heroManager.heroesAlive[i];
                }

                break;

            case TargetType.lowestHp:

                int minHp = 99;

                for (int i = 0; i < _heroManager.GetHeroCount(); i++)
                {
                    HeroScript hsScript = _heroManager.heroesAlive[i].GetComponent<HeroScript>();

                    if (hsScript.GetHp() >= minHp)
                    {
                        continue;
                    }

                    if (targets.Contains(hsScript.gameObject))
                    {
                        continue;
                    }

                    minHp = hsScript.GetHp();
                    _target = _heroManager.heroesAlive[i];
                }

                break;
            default: break;
        }

        _heroScript = _target.GetComponent<HeroScript>();
    }


    public bool VerifyTarget()
    {
        minPathLength = 100;

        currentPath = new PathCoordinates[100];
        shortestPath = new PathCoordinates[100];

        endX = _heroScript.GetXPos();
        endY = _heroScript.GetYPos();

        CopyGrid();
        PathFinder(_xPos, _yPos, 0);

        if(minPathLength < 100)
        {
            return true;
        }

        return false;
    }

    public void CopyGrid()
    {
        grid = new PathGrid[_gameManager.GetNrOfRows(), _gameManager.GetNrOfColumns()];

        for (int i = 0; i < _gameManager.GetNrOfRows(); i++)
        {
            for (int j = 0; j < _gameManager.GetNrOfColumns(); j++)
            {
                if (_tileManager.gameBoard[i, j] == null)
                {
                    grid[i, j].state = 0;
                }
                else
                {
                    grid[i, j].state = 1;
                }
            }
        }
    }

    public void PathFinder(int x, int y, int pathLength)
    {
        if (CanAttack(_heroScript, x, y))
        {
            _canReach = true;
            currentPath[pathLength].x = x;
            currentPath[pathLength].y = y;
            pathLength++;

            if (pathLength < minPathLength)
            {
                minPathLength = pathLength;

                for (int i = 0; i < pathLength; i++)
                {
                    shortestPath[i] = currentPath[i];
                }
            }
            return;
        }
        for (int i = 0; i < 4; i++)
        {
            if (PositionIsValid(x + dl[i], y + dc[i], pathLength))
            {
                grid[x, y].state = -1;
                grid[x, y].distance = pathLength;
                currentPath[pathLength].x = x;
                currentPath[pathLength].y = y;
                PathFinder(x + dl[i], y + dc[i], pathLength + 1);
            }
        }
    }

    public bool CanAttack(HeroScript targetScript, int x, int y)
    {
        if (GetDistance(x, targetScript.GetXPos(), y, targetScript.GetYPos()) == 1)
        {
            return true;
        }

        return false;
    }

    public bool CanAttack(HeroScript targetScript)
    {
        if (GetDistance(_xPos, targetScript.GetXPos(), _yPos, targetScript.GetYPos()) == 1)
        {
            return true;
        }

        return false;
    }

    public bool PositionIsValid(int x, int y, int currentDistance)
    {
        if (x < 0 || x >= _gameManager.GetNrOfRows() || y < 0 || y >= _gameManager.GetNrOfColumns())
        {
            return false;
        }
        if (grid[x, y].state == 1)
        {
            return false;
        }
        if (grid[x, y].state == -1 && grid[x, y].distance <= currentDistance + 1)
        {
            return false;
        }
        return true;
    }

    public void UpdatePosition(int x, int y)
    {

        if (minPathLength >= 2)
        {
            _xPos = shortestPath[1].x;
            _yPos = shortestPath[1].y;
        }

        _tileManager.gameBoard[x, y] = null;
        _tileManager.gameBoard[_xPos, _yPos] = gameObject;

        _targetTile = _tileManager.tiles[_xPos, _yPos];
        _isMoving = true;
    }

    public void MoveTowardsTarget()
    {
        int pastXPos = _xPos;
        int pastYPos = _yPos;

        UpdatePosition(pastXPos, pastYPos);
    }

    public virtual void TakeDamage(int damage)
    {
        _hp -= damage;


        UpdateHealthbar();

        if (_hp <= 0)
        {
            _enemyManager.EnemyDeath(gameObject);
            Destroy(gameObject);
        }

        if (_animator != null)
        {
            _animator.SetTrigger("take_damage");
        }
    }

    abstract public void StartTurn();

    public void EndTurn()
    {
        _turnManager.NextEnemy();
    }

    public int GetDistance(int x, int x2, int y, int y2)
    {
        return Mathf.Abs(x - x2) + Mathf.Abs(y - y2);
    }

    public void SetCoords(int x, int y)
    {
        _xPos = x;
        _yPos = y;
    }

    public int GetXPos() { return _xPos; }
    public int GetYPos() { return _yPos; }
    public int GetHp() { return _hp; }
    public int GetMaxHp() { return _maxHp; }
}
