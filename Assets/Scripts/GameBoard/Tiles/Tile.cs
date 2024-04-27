using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    protected int _xPos;
    protected int _yPos;

    public void SetCoords(int x, int y)
    {
        _xPos = x;
        _yPos = y;
    }

    virtual public void SetAttacking(bool attacking) { }
    public int GetXPos() { return _xPos; }
    public int GetYPos() { return _yPos; }

}
