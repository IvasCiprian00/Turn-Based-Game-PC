using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerTile : Tile
{
    private int _timer;

    public void SetTimer(int timer)
    {
        _timer = timer;
    }
    public void TickTimer()
    {
        _timer--;

        if(_timer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
