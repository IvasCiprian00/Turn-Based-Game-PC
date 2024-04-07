using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTileScript : MonoBehaviour
{
    [SerializeField] private TileManager _tileManager;
    //public GameManager gmManager;
    //public HeroManager heroManager;
    private bool _attackTile;

    [SerializeField] private Sprite _attackTileSprite;

    [SerializeField] private int _xPos;
    [SerializeField] private int _yPos;

    public void Start()
    {
        //gmManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        //heroManager = GameObject.Find("Hero Manager").GetComponent<HeroManager>();
        _tileManager = GameObject.Find("Tile Manager").GetComponent<TileManager>();
    }

    public void SetCoords(int x, int y)
    {
        _xPos = x;
        _yPos = y;
    }

    /*public void OnMouseUp()
    {
        HeroScript hsScript = heroManager.heroesAlive[gmManager.currentHero].GetComponent<HeroScript>();
        int pastXPos = hsScript.GetXPos();
        int pastYPos = hsScript.GetYPos();


        if (_attackTile)
        {
            int damageDealt = hsScript.GetDamage();

            gmManager.gameBoard[_xPos, _yPos].GetComponent<EnemyScript>().TakeDamage(damageDealt);
            gmManager.attacksLeft--;
            gmManager.GenerateMoveTiles();

            return;
        }

        gmManager.speedLeft -= Mathf.Abs(_xPos - pastXPos) + Mathf.Abs(_yPos - pastYPos);

        gmManager.gameBoard[pastXPos, pastYPos] = null;

        hsScript.MoveTo(gameObject);

        gmManager.gameBoard[_xPos, _yPos] = heroManager.heroesAlive[gmManager.currentHero];
        gmManager.GenerateMoveTiles();
    }*/

    public void ShowCoords()
    {
        Debug.Log(_xPos + " " + _yPos);
    }

    public void SetAttacking(bool attacking)
    {
        if (attacking)
        {
            GetComponent<SpriteRenderer>().sprite = _attackTileSprite;
        }

        _attackTile = attacking;
    }

    public int GetXPos() { return _xPos; }
    public int GetYPos() { return _yPos; }
}
