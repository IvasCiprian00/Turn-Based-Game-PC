using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBossScript : Enemy
{
    [SerializeField] private SkillManager _skillManager;

    [Header("Skills")]
    [SerializeField] private GameObject[] _slamTiles;
    [SerializeField] private int _slamDamage;
    [SerializeField] private int _slamCooldown;
    [SerializeField] private int _slamTimer;
    [SerializeField] private int _slamRange;
    //

    public void Awake()
    {
        _skillManager = GameObject.Find("Skill Manager").GetComponent<SkillManager>();
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

            if (CanAttack(_heroScript))
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
        return true;
    }
}
