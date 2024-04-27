using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkTile : Tile
{
    private int _lightSources;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag != "Light Source")
        {
            return;
        }

        _lightSources++;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag != "Light Source")
        {
            return;
        }

        _lightSources--;

        if(_lightSources == 0)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
