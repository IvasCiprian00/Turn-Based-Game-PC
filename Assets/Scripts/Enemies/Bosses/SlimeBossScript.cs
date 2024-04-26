using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBossScript : Enemy
{
    [Header("Skills")]
    [SerializeField] private GameObject[] _slamTiles;
    [SerializeField] private int _slamDamage;
    [SerializeField] private int _slamCooldown;
    [SerializeField] private int _slamTimer;
    [SerializeField] private int _slamRange;

    public void Awake()
    {
        SetManagers();
        SetHealthbar();
    }

    public void Start()
    {
        _hp = _maxHp;

        _slamTimer = _slamCooldown;

        UpdateHealthbar();
    }

    public void Update()
    {
        Movement();
    }

    override public void StartTurn()
    {
        StartCoroutine(TakeTurn());
    }

    public IEnumerator TakeTurn()
    {
        int speedLeft = _speed;
        int attacksLeft = _attackCount;

        _slamTimer++;

        _slamTiles = GameObject.FindGameObjectsWithTag("Timer Tile");

        if(_slamTiles.Length > 0)
        {
            for(int i = 0; i < _slamTiles.Length; i++)
            {
                _slamTiles[i].GetComponent<TimerTile>().TickTimer();
            }

            speedLeft = 0;
            attacksLeft = 0;
        }

        while (speedLeft > 0 || attacksLeft > 0)
        {
            FindTarget();

            if (CanAttack(_heroScript) && attacksLeft == 0)
            {
                break;
            }

            if (!CanAttack(_heroScript) && speedLeft == 0)
            {
                break;
            }

            if(HeroesInSlamRange() && _slamTimer >= _slamCooldown)
            {
                _slamTimer = 0;
                speedLeft = 0;
                attacksLeft = 0;

                _skillManager.SlimeSlamAttack(_slamRange, _xPos, _yPos);
            }

            if (CanAttack(_heroScript) && attacksLeft > 0)
            {
                _uiManager.DisplayDamage(_heroScript.gameObject, _damage);
                _heroScript.TakeDamage(_damage);
                attacksLeft--;
            }
            else if (speedLeft > 0)
            {
                MoveTowardsTarget();
                speedLeft--;
            }

            yield return new WaitForSeconds(_waitDuration);
        }

        EndTurn();
    }

    public bool HeroesInSlamRange()
    {
        int count = 0;

        for(int i = _xPos - _slamRange; i <= _xPos + _slamRange; i++)
        {
            for(int j =  _yPos - _slamRange; j <= _yPos + _slamRange; j++)
            {
                if (!_tileManager.PositionIsValid(i, j))
                {
                    continue;
                }

                if (_tileManager.gameBoard[i, j] == null)
                {
                    continue;
                }

                if (_tileManager.gameBoard[i, j].tag == "Hero")
                {
                    count++;
                }
            }
        }

        if(count >= 2)
        {
            return true;
        }

        return false;
    }
}
