using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField] private int _range;
    public GameObject temp;
    private GameObject _tempTarget;

    int[] rangedl = { -1, -1, -1, 0, 1, 1, 1, 0 };
    int[] rangedc = { -1, 0, 1, 1, 1, 0, -1, -1 };

    override
    public void StartTurn()
    {
        StartCoroutine(TakeTurn());
    }

    public IEnumerator TakeTurn()
    {
        TickStatusEffects();

        if (_stunned)
        {
            EndTurn();
            yield break;
        }

        int speedLeft = _speed;
        int attacksLeft = _attackCount;

        while (speedLeft > 0 || attacksLeft > 0)
        {
            VerifyTarget();

            if (CanAttack() && attacksLeft == 0)
            {
                break;
            }

            if (!CanAttack() && speedLeft == 0)
            {
                break;
            }

            if (CanAttack())
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

    override public void PathFinder(int x, int y, int pathLength)
    {
        if (CanAttack(x, y))
        {
            _canReach = true;
            currentPath[pathLength].x = x;
            currentPath[pathLength].y = y;
            pathLength++;

            if (pathLength < minPathLength)
            {
                _heroScript = _tempTarget.GetComponent<HeroScript>();
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

    public bool CanAttack(int xPos, int yPos)
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

    public bool CanAttack()
    {
        return CanAttack(_xPos, _yPos);
    }

    public bool DirectionalCheck(int lineDirection, int colDirection, int startingXPos, int startingYPos)
    {
        int currentLine = startingXPos;
        int currentCol = startingYPos;

        for(int i = 0; i < _range; i++)
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
                if(i == 0)
                {
                    return false;
                }

                _tempTarget = _tileManager.gameBoard[currentLine, currentCol];
                return true;
            }
        }

        return false;
    }
}
