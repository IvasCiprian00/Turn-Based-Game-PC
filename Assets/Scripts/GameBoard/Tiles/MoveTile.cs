using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTile : Tile
{
    private GameManager _gameManager;
    private TileManager _tileManager;
    private HeroManager _heroManager;
    private TurnManager _turnManager;
    private UIManager _uiManager;
    private bool _attackTile;
    private bool _healTile;

    [SerializeField] private Sprite _attackTileSprite;
    [SerializeField] private Sprite _healTileSprite;

    public void Awake()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _gameManager.SetManager(ref _turnManager);
        _gameManager.SetManager(ref _heroManager);
        _gameManager.SetManager(ref _tileManager);
        _gameManager.SetManager(ref _uiManager);
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

        if (_healTile)
        {
            int healAmount = heroScript.GetHealAmount();

            _tileManager.gameBoard[_xPos, _yPos].GetComponent<HeroScript>().Heal(healAmount);

            _turnManager.DecreaseActionsLeft();

            if (!_turnManager.IsGameOver())
            {
                _tileManager.GenerateMoveTiles(heroScript);
            }

            return;
        }

        if (_attackTile)
        {
            int damageDealt = heroScript.GetDamage();

            _tileManager.gameBoard[_xPos, _yPos].GetComponent<Enemy>().TakeDamage(damageDealt);
            _turnManager.DecreaseActionsLeft();

            if (!_turnManager.IsGameOver())
            {
                _tileManager.GenerateMoveTiles(heroScript);
            }

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

    override public void SetAttacking(bool attacking)
    {
        if (attacking)
        {
            GetComponent<SpriteRenderer>().sprite = _attackTileSprite;
        }

        _attackTile = attacking;
    }

    public void SetHealing(bool healing)
    {
        if(healing)
        {
            GetComponent<SpriteRenderer>().sprite = _healTileSprite;
        }

        _healTile = healing;
    }

    public bool IsAttacking() { return _attackTile; }
    public bool IsHealing() { return _healTile; }
}
