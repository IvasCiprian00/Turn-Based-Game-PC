using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private HeroManager _heroManager;
    [SerializeField] private TileManager _tileManager;
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private HeroScript _heroScript;
    [SerializeField] private EnemyScript _enemyScript;

    private int _currentHero;
    private int _currentEnemy;

    private bool _heroesSpawned;

    private void Awake()
    {
        _tileManager = GameObject.Find("Tile Manager").GetComponent<TileManager>();
        _heroManager = GameObject.Find("Hero Manager").GetComponent<HeroManager>();
    }

    public void EndTurn()
    {
        _currentHero++;

        if (_currentHero >= _heroManager.GetHeroCount() || _currentHero == -1)
        {
            _currentHero = 0;

            StartEnemyTurns();

            return;
        }

        _heroScript = _heroManager.heroesAlive[_currentHero].GetComponent<HeroScript>();
        //speedLeft = _heroScript.GetSpeed();
        //attacksLeft = _heroScript.GetSpeed();

        _tileManager.GenerateMoveTiles(_heroScript);
    }

    public void StartEnemyTurns()
    {
        _tileManager.DestroyMoveTiles();

        _currentEnemy = 0;

        _enemyManager = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();
        _enemyScript = _enemyManager.enemiesAlive[_currentEnemy].GetComponent<EnemyScript>();

        //_heroTurn = false;
        _enemyScript.StartTurn();
    }

    public void StartHeroTurns()
    {
        _currentHero = 0;
        //_heroTurn = true;

        _heroScript = _heroManager.heroesAlive[_currentHero].GetComponent<HeroScript>();
        //speedLeft = _heroScript.GetSpeed();
        //attacksLeft = _heroScript.GetNumberOfAttacks();

        _tileManager.GenerateMoveTiles(_heroScript);
    }

    
    public void SetTurnOrder()
    {
        //later implement this with character initiative
    }

    public int GetCurrentHero() { return _currentHero; }

    public void NextEnemy()
    {
        _currentEnemy++;

        if(_currentEnemy >= _enemyManager.GetEnemyCount())
        {
            _currentEnemy = 0;

            StartHeroTurns();

            return;
        }

        _enemyManager.enemiesAlive[_currentEnemy].GetComponent<EnemyScript>().StartTurn();
    }

    public int GetCurrentEnemy() { return _currentEnemy; }
}
