using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField] private int _range;

    int[] rangedl = { -1, -1, -1, 0, 1, 1, 1, 0 };
    int[] rangedc = { -1, 0, 1, 1, 1, 0, -1, -1 };

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
                _uiManager.DisplayDamage(_heroScript.gameObject, _damage);
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

    public override void FindTarget()
    {
        VerifyTarget();
    }

    override public bool CanAttack(HeroScript targetScript, int xPos, int yPos)
    {
        for(int i = 0; i < rangedl.Length; i++)
        {
            if(DirectionalCheck(rangedl[i], rangedc[i], xPos, yPos))
            {
                return true;
            }
        }

        return false;
    }

    public override bool CanAttack(HeroScript targetScript)
    {
        return CanAttack(targetScript, _yPos, _xPos);
    }

    public bool DirectionalCheck(int lineDirection, int colDirection, int startingXPos, int startingYPos)
    {
        int currentLine = startingXPos + lineDirection;
        int currentCol = startingYPos + colDirection;

        for(int i = 1; i < _range; i++)
        {
            currentLine += lineDirection;
            currentCol += colDirection;

            if(!_tileManager.PositionIsValid(currentLine, currentCol))
            {
                continue;
            }

            if (_tileManager.gameBoard[currentLine, currentCol] == null)
            {
                continue;
            }

            if (_tileManager.gameBoard[currentLine, currentCol].tag == "Hero")
            {
                _heroScript = _tileManager.gameBoard[currentLine, currentCol].GetComponent<HeroScript>();
                return true;
            }
        }

        return false;
    }
}
