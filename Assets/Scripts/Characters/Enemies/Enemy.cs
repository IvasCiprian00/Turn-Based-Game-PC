using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected enum TargetType
    {
        closest,
        lowestHp
    }

    protected SkillManager _skillManager;
    protected GameManager _gameManager;
    protected EnemyManager _enemyManager;
    protected HeroManager _heroManager;
    protected TileManager _tileManager;
    protected TurnManager _turnManager;
    protected UIManager _uiManager;
    protected SoundManager _soundManager;
    protected TutorialManager _tutorialManager;

    [SerializeField] protected HealthbarScript _healthbarScript;
    [SerializeField] protected TargetType _targetType;
    [SerializeField] protected Animator _animator;

    [Header("Status Section")]
    [SerializeField] protected List<Status> _statusList;
    [SerializeField] protected bool _stunned;
    [SerializeField] protected bool _bleeding;
    [SerializeField] protected int _bleedingDamage;
    [SerializeField] protected List<TextMeshProUGUI> _statusIcons;
    [SerializeField] protected Transform _statusIconsParent;
    [SerializeField] protected GameObject _stunIcon;
    [SerializeField] protected GameObject _bleedIcon;
    [SerializeField] protected GameObject _burnIcon;

    [Header("Enemy Stats")]
    [SerializeField] protected int _hp;
    [SerializeField] protected int _maxHp;
    [SerializeField] protected int _evasion;
    [SerializeField] protected int _damage;
    [SerializeField] protected int _lowerDamage;
    [SerializeField] protected int _upperDamage;
    [SerializeField] protected int _speed;
    [SerializeField] protected int _attackCount;
    [SerializeField] protected float _waitDuration;
    [SerializeField] protected float _actionSpeed;


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
    protected PathCoordinates[] currentPath;
    protected PathCoordinates[] shortestPath;
    protected PathGrid[,] grid;

    protected int[] dl = { -1, 0, 1, 0 };
    protected int[] dc = { 0, 1, 0, -1 };

    protected bool _isMoving;
    protected GameObject _targetTile;

    public void Awake()
    {
        SetManagers();
        SetHealthbar();
    }

    virtual public void Start()
    {
        _hp = _maxHp;
        UpdateHealthbar();
        GetComponentInChildren<SpriteRenderer>().sortingOrder = _xPos;
    }

    virtual public void Update()
    {
        Movement();
    }

    public void SetManagers()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _gameManager.SetManager(ref _turnManager);
        _gameManager.SetManager(ref _enemyManager);
        _gameManager.SetManager(ref _tileManager);
        _gameManager.SetManager(ref _heroManager);
        _gameManager.SetManager(ref _skillManager);
        _gameManager.SetManager(ref _uiManager);
        _gameManager.SetManager(ref _soundManager);
        _gameManager.SetManager(ref _tutorialManager);
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
            var step = _actionSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _targetTile.transform.position, step);

            if (Vector3.Distance(transform.position, _targetTile.transform.position) < 0.001f)
            {
                transform.position = _targetTile.transform.position;
                _isMoving = false;
            }
        }
    }

    virtual public void FindTarget()
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


    virtual public bool VerifyTarget()
    {
        minPathLength = 100;

        currentPath = new PathCoordinates[100];
        shortestPath = new PathCoordinates[100];

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

    virtual public void PathFinder(int x, int y, int pathLength)
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

    virtual public bool CanAttack(HeroScript targetScript, int x, int y)
    {
        if (GetDistance(x, targetScript.GetXPos(), y, targetScript.GetYPos()) == 1)
        {
            return true;
        }

        return false;
    }

    virtual public bool CanAttack(HeroScript targetScript)
    {
        /*if (_isMoving)
        {
            return false;
        }*/

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

    public IEnumerator AttackAndMove()
    {
        int speedLeft = _speed;
        int attacksLeft = _attackCount;

        while (speedLeft > 0 || attacksLeft > 0)
        {
            if(_hp <= 0)
            {
                yield break;
            }
            FindTarget();

            if (CanAttack(_heroScript) && attacksLeft == 0)
            {
                break;
            }

            if (!CanAttack(_heroScript) && speedLeft == 0)
            {
                break;
            }

            if (CanAttack(_heroScript))
            {
                _soundManager.PlaySound(_soundManager.bite);
                _heroScript.TakeDamage(GetDamage());
                attacksLeft--;
            }
            else if (speedLeft > 0)
            {
                _soundManager.PlaySound(_soundManager.moveSound);
                MoveTowardsTarget();
                speedLeft--;
            }
        }

        yield return new WaitForSeconds(0.2f);
    }


    public IEnumerator TakeBasicTurn()
    {
        TickStatusEffects();

        if (_stunned)
        {
            EndTurn();
            yield break;
        }
        StartCoroutine(AttackAndMove());

        EndTurn();
    }

    public void MoveTowardsTarget()
    {
        if (_bleeding)
        {
            TakeUndodgeableDamage(_bleedingDamage);
        }

        int pastXPos = _xPos;
        int pastYPos = _yPos;

        UpdatePosition(pastXPos, pastYPos);
    }

    public virtual void TakeUndodgeableDamage(int damage)
    {
        _hp -= damage;
        _uiManager.DisplayDamage(gameObject, damage);

        UpdateHealthbar();

        if (_hp <= 0)
        {
            _enemyManager.EnemyDeath(gameObject);
            StopAllCoroutines();
            Destroy(gameObject);
        }

        if (_animator != null)
        {
            _animator.SetTrigger("take_damage");
        }
    }

    public virtual void TakeDamage(int damage)
    {
        int chance = UnityEngine.Random.Range(1, 100);

        if(chance <= _evasion)
        {
            damage = 0;
        }

        TakeUndodgeableDamage(damage);
    }

    virtual public void StartTurn()
    {
        StartCoroutine(TakeBasicTurn());
    }

    public void TickStatusEffects()
    {
        ResetStatuses();

        for (int i = 0; i < _statusList.Count; i++)
        {
            if (_statusList[i] == null)
            {
                continue;
            }

            _statusList[i].duration--;

            if (_statusList[i].duration < 0)
            {
                _statusList.Remove(_statusList[i]);
                Destroy(_statusIcons[i].transform.parent.gameObject);
                _statusIcons.Remove(_statusIcons[i]);
                i--;
                continue;
            }

            switch (_statusList[i].statusType)
            {
                case StatusType.Stun:
                    _stunned = true;
                    break;

                case StatusType.Burn:
                    TakeUndodgeableDamage(_statusList[i].damage);
                    break;

                case StatusType.Bleed:
                    _bleeding = true;
                    if(_bleedingDamage < _statusList[i].damage)
                    {
                        _bleedingDamage = _statusList[i].damage;
                    }
                    break;

                default:
                    break;
            }
        }
    }

    public void ApplyStatus(StatusType statusType, int duration, int damage)
    {
        _tutorialManager.StatusEffectsTutorial();
        _statusList.Add(new Status(statusType, duration, damage));

        GameObject reference;
        switch(statusType)
        {
            case StatusType.Stun:
                reference = Instantiate(_stunIcon, _statusIconsParent);
                break;
            case StatusType.Bleed:
                reference = Instantiate(_bleedIcon, _statusIconsParent);
                break;
            case StatusType.Burn:
                reference = Instantiate(_burnIcon, _statusIconsParent);
                break;
            default: return;
        }

        _statusIcons.Add(reference.GetComponentInChildren<TextMeshProUGUI>());
        //_statusIcons[_statusIcons.Count - 1].text = duration.ToString();
    }

    public void ResetStatuses()
    {
        _stunned = false;
        _bleeding = false;
        _bleedingDamage = 0;
    }

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
        GetComponentInChildren<SpriteRenderer>().sortingOrder = x + 1;
        _xPos = x;
        _yPos = y;
    }

    public int GetXPos() { return _xPos; }
    public int GetYPos() { return _yPos; }
    public int GetHp() { return _hp; }
    public int GetMaxHp() { return _maxHp; }
    public int GetEvasion() { return _evasion; }
    public void SetEvasion(int evasion) {  _evasion = evasion; }

    public int GetDamage() { return UnityEngine.Random.Range(_lowerDamage, _upperDamage); }
    public void SetIsStunned(bool stunned) { _stunned = stunned; }
}
