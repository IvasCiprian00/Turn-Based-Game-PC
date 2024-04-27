using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTile : Tile
{
    [SerializeField] private TileManager _tileManager;
    [SerializeField] private HeroManager _heroManager;
    [SerializeField] private TurnManager _turnManager;
    [SerializeField] private UIManager _uiManager;
    private DarknessManager _darknessManager;
    private bool _attackTile;

    [SerializeField] private Sprite _attackTileSprite;

    public void Awake()
    {
        _heroManager = GameObject.Find("Hero Manager").GetComponent<HeroManager>();
        _tileManager = GameObject.Find("Tile Manager").GetComponent<TileManager>();
        _turnManager = GameObject.Find("Turn Manager").GetComponent<TurnManager>();
        //_darknessManager = GameObject.Find("Darkness Manager").GetComponent<DarknessManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    public void Start()
    {
        transform.position -= new Vector3(0, 0, 1);
    }

    public void OnMouseUp()
    {
        HeroScript heroScript = _heroManager.heroesAlive[_turnManager.GetCurrentHero()].GetComponent<HeroScript>();
        int pastXPos = heroScript.GetXPos();
        int pastYPos = heroScript.GetYPos();


        if (_attackTile)
        {
            int damageDealt = heroScript.GetDamage();

            _uiManager.DisplayDamage(_tileManager.gameBoard[_xPos, _yPos].GetComponent<Enemy>().gameObject, damageDealt);
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

        if(_darknessManager != null)
        {
            //_darknessManager.UpdateDarkness();
        }

    }

    public void ShowCoords()
    {
        Debug.Log(_xPos + " " + _yPos);
    }

    override public void SetAttacking(bool attacking)
    {
        if (attacking)
        {
            GetComponent<SpriteRenderer>().sprite = _attackTileSprite;
        }

        _attackTile = attacking;
    }
}
