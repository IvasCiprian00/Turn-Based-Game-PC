using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerTile : Tile
{
    private int _timer;
    [SerializeField] private Sprite _finalTick;

    public void SetTimer(int timer)
    {
        _timer = timer;
    }
    public void TickTimer()
    {
        _timer--;

        if(_timer == 1)
        {
            GetComponent<SpriteRenderer>().sprite = _finalTick;
        }

        if(_timer > 0)
        {
            return;
        }

        if (_tileManager.gameBoard[_xPos, _yPos] == null )
        {
            Destroy(gameObject);
            return;
        }
        if (_tileManager.gameBoard[_xPos, _yPos].tag == "Hero" ) 
        { 
            HeroScript heroScript = _tileManager.gameBoard[_xPos, _yPos].GetComponent<HeroScript>();

            heroScript.TakeDamage(1);
        }

        Destroy(gameObject);
    }
}
