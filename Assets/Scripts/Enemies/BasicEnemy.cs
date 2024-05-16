using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BasicEnemy : Enemy
{

    override
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

}
