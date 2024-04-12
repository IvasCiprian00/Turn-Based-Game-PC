using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyScript : MonoBehaviour
{
    enum TargetType
    {
        closest,
        lowestHp
    }

    [SerializeField] private GameManager _gameManager;
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private HeroManager _heroManager;
    [SerializeField] private TileManager _tileManager;
    [SerializeField] private TurnManager _turnManager;
    [SerializeField] private GameObject _target;
    [SerializeField] private HeroScript _heroScript;
    [SerializeField] private TargetType _targetType;
    [SerializeField] private Animator _animator;

    [Header("Enemy Stats")]
    [SerializeField] private int _hp;
    [SerializeField] private int _maxHp;
    [SerializeField] private int _damage;
    [SerializeField] private int _speed;
    [SerializeField] private int _attackCount;
    private int _xPos;
    private int _yPos;

    private bool _canAttack;
    private bool _isMoving;
    private GameObject _targetTile;

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

    private int minPathLength;
    private int endX;
    private int endY;
    private PathCoordinates[] currentPath;
    private PathCoordinates[] shortestPath;
    private PathGrid[,] grid;

    private int[] dl = { -1, 0, 1, 0 };
    private int[] dc = { 0, 1, 0, -1 };

    public void Awake()
    {
        _turnManager = GameObject.Find("Turn Manager").GetComponent<TurnManager>();
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _enemyManager = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();
        _tileManager = GameObject.Find("Tile Manager").GetComponent<TileManager>();
        _heroManager = GameObject.Find("Hero Manager").GetComponent<HeroManager>();
    }

    public void Start()
    {
        _hp = _maxHp;
    }
    public void Update()
    {
        if (_isMoving)
        {
            var step = 10 * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _targetTile.transform.position, step);

            if (Vector3.Distance(transform.position, _targetTile.transform.position) < 0.001f)
            {
                transform.position = _targetTile.transform.position;
                _isMoving = false;
                //Camera.main.transform.parent = null;
            }
        }
    }

    public void StartTurn()
    {
        StartCoroutine(TakeTurn());
    }

    public IEnumerator TakeTurn()
    {
        int speedLeft = _speed;
        int attacksLeft = _attackCount;

        while(speedLeft > 0 || attacksLeft > 0)
        {
            FindTarget();

            if(CanAttack(_heroScript) && attacksLeft == 0)
            {
                break;
            }

            if(!CanAttack(_heroScript) && speedLeft == 0)
            {
                break;
            }

            if (CanAttack(_heroScript))
            {
                _heroScript.TakeDamage(_damage);
                attacksLeft--;
            }
            else if (speedLeft > 0)
            {
                MoveTowardsTarget();
                speedLeft--;
            }

            yield return new WaitForSeconds(0.2f);
        }

        EndTurn();
    }

    public void EndTurn()
    {
        _turnManager.NextEnemy();
    }

    public void MoveTowardsTarget()
    {
        //Camera.main.transform.parent = transform;
        //Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
        int pastXPos = _xPos;
        int pastYPos = _yPos;

        endX = _heroScript.GetXPos();
        endY = _heroScript.GetYPos();

        minPathLength = 30;
        currentPath = new PathCoordinates[100];
        shortestPath = new PathCoordinates[100];

        CopyGrid();
        PathFinder(_xPos, _yPos, 0);

        if (minPathLength >= 2)
        {
            _xPos = shortestPath[1].x;
            _yPos = shortestPath[1].y;
        }

        UpdatePosition(pastXPos, pastYPos);
    }

    public void UpdatePosition(int x, int y)
    {
        _tileManager.gameBoard[x, y] = null;
        _tileManager.gameBoard[_xPos, _yPos] = gameObject;

        _targetTile = _tileManager.tiles[_xPos, _yPos];
        _isMoving = true;
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

    public void FindTarget()
    {
        int minDistance = 99;

        switch (_targetType)
        {
            case TargetType.closest:

                for (int i = 0; i < _heroManager.GetHeroCount(); i++)
                {
                    HeroScript hsScript = _heroManager.heroesAlive[i].GetComponent<HeroScript>();
                    int distance = Mathf.Abs(_xPos - hsScript.GetXPos()) + Mathf.Abs(_yPos - hsScript.GetYPos());

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        _target = _heroManager.heroesAlive[i];
                    }
                }

                break;

            case TargetType.lowestHp:

                int minHp = 99;

                for (int i = 0; i < _heroManager.GetHeroCount(); i++)
                {
                    HeroScript hsScript = _heroManager.heroesAlive[i].GetComponent<HeroScript>();

                    if (hsScript.GetHp() < minHp)
                    {
                        minHp = hsScript.GetHp();
                        _target = _heroManager.heroesAlive[i];
                    }
                }

                break;
            default: break;
        }

        _heroScript = _target.GetComponent<HeroScript>();
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

    public int GetDistance(int x, int x2, int y, int y2)
    {
        return Mathf.Abs(x - x2) + Mathf.Abs(y - y2);
    }

    public void TakeDamage(int damage)
    {
        _hp -= damage;
        //_uiManager.DisplayDamageDealt(gameObject, damage);

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

    public void SetCoords(int x, int y)
    {
        _xPos = x;
        _yPos = y;
    }

    public int GetXPos() { return _xPos; }
    public int GetYPos() { return _yPos; }
}
