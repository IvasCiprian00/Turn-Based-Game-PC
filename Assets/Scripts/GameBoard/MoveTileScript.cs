using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTileScript : MonoBehaviour
{
    [SerializeField] private TileManager _tileManager;
    [SerializeField] private HeroManager _heroManager;
    [SerializeField] private TurnManager _turnManager;
    private bool _attackTile;

    [SerializeField] private Sprite _attackTileSprite;

    [SerializeField] private int _xPos;
    [SerializeField] private int _yPos;

    public void Start()
    {
        _heroManager = GameObject.Find("Hero Manager").GetComponent<HeroManager>();
        _tileManager = GameObject.Find("Tile Manager").GetComponent<TileManager>();
        _turnManager = GameObject.Find("Turn Manager").GetComponent<TurnManager>();
    }

    public void SetCoords(int x, int y)
    {
        _xPos = x;
        _yPos = y;
    }

    public void OnMouseUp()
    {
        HeroScript heroScript = _heroManager.heroesAlive[_turnManager.GetCurrentHero()].GetComponent<HeroScript>();
        int pastXPos = heroScript.GetXPos();
        int pastYPos = heroScript.GetYPos();


        if (_attackTile)
        {
            int damageDealt = heroScript.GetDamage();

            _tileManager.gameBoard[_xPos, _yPos].GetComponent<Enemy>().TakeDamage(damageDealt);
            _turnManager.DecreaseAttacksLeft();
            _tileManager.GenerateMoveTiles(heroScript);

            return;
        }

        int distance = Mathf.Abs(_xPos - pastXPos) + Mathf.Abs(_yPos - pastYPos);
        _turnManager.DecreaseSpeedLeft(distance);

        _tileManager.gameBoard[pastXPos, pastYPos] = null;

        heroScript.MoveTo(gameObject);

        _tileManager.gameBoard[_xPos, _yPos] = _heroManager.heroesAlive[_turnManager.GetCurrentHero()];
        _tileManager.GenerateMoveTiles(heroScript);
    }

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
