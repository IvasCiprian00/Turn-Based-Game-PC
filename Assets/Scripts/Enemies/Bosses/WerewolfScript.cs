using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class WerewolfScript : Enemy
{
    [Serializable]
    public struct HpThreshold
    {
        public int speed;
        public int lowerDamage;
        public int upperDamage;
        public int attackCount;
    }

    [SerializeField] HpThreshold[] _hpThreshold;

    override
    public void StartTurn()
    {
        StartCoroutine(TakeTurn());
    }

    public IEnumerator TakeTurn()
    {
        int speedLeft = _speed;
        int attacksLeft = _attackCount;

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

            if (CanAttack(_heroScript))
            {
                _heroScript.TakeDamage(GetDamage());
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

    public override void TakeDamage(int dmg)// tb sa aflu diferenta intre new si override
    {
        base.TakeDamage(dmg);
        
        if(_hp <= 0.3 * _maxHp)
        {
            ChangeThreshold(1);
            return;
        }

        if(_hp <= 0.7 * _maxHp)
        {
            ChangeThreshold(0);
        }
    }

    public void ChangeThreshold(int x)
    {
        _speed = _hpThreshold[x].speed;
        _lowerDamage = _hpThreshold[x].lowerDamage;
        _upperDamage = _hpThreshold[x].upperDamage;
        _attackCount = _hpThreshold[x].attackCount;
    }
}
