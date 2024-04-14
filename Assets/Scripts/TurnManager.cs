using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private GameObject _selectedEffect;

    private UIManager _uiManager;
    private HeroManager _heroManager;
    private TileManager _tileManager;
    private EnemyManager _enemyManager;
    private HeroScript _heroScript;
    private EnemyScript _enemyScript;

    private int _attacksLeft;
    private int _speedLeft;
    private int _currentHero;
    private int _currentEnemy;

    private bool _heroesSpawned;

    private void Awake()
    {
        _tileManager = GameObject.Find("Tile Manager").GetComponent<TileManager>();
        _heroManager = GameObject.Find("Hero Manager").GetComponent<HeroManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    private void Start()
    {
        _selectedEffect = Instantiate(_selectedEffect);
    }

    private void Update()
    {
        _selectedEffect.transform.position = _heroManager.heroesAlive[_currentHero].transform.position;
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
        _speedLeft = _heroScript.GetSpeed();
        _attacksLeft = _heroScript.GetNrOfAttacks();

        _tileManager.GenerateMoveTiles(_heroScript);
    }

    public void OnLevelLoaded()
    {
        _enemyManager = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();
    }

    public void StartEnemyTurns()
    {
        _selectedEffect.SetActive(false);
        _uiManager.DisplayUI(false);

        _tileManager.DestroyMoveTiles();

        _currentEnemy = 0;

        _enemyScript = _enemyManager.enemiesAlive[_currentEnemy].GetComponent<EnemyScript>();

        //_heroTurn = false;
        _enemyScript.StartTurn();
    }

    public void StartHeroTurns()
    {
        _uiManager.DisplayUI(true);
        _selectedEffect.SetActive(true);
        _currentHero = 0;
        //_heroTurn = true;

        _heroScript = _heroManager.heroesAlive[_currentHero].GetComponent<HeroScript>();
        _speedLeft = _heroScript.GetSpeed();
        _attacksLeft = _heroScript.GetNrOfAttacks();

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

    public void CheckLevelProgress()
    {
        if (_heroManager.GetHeroCount() <= 0)
        {
            //_gameOver = true;

            Debug.Log("Heroes Lost");
            _uiManager.DisplayEndOfLevelButtons(false);
        }

        else if (_enemyManager.GetEnemyCount() <= 0)
        {
            //_gameOver = true;

            _tileManager.DestroyMoveTiles();
            //_uiManager.HideSkills();


            for (int i = 0; i < _heroManager.GetHeroCount(); i++)
            {
                _heroManager.heroesAlive[i].GetComponent<HeroScript>().StartWinAniamtion();
            }

            Debug.Log("Heroes Won");
            _uiManager.DisplayEndOfLevelButtons(true);
        }
    }

    public void ResetHeroes()
    {
        for(int i = 0; i < _heroManager.GetHeroCount(); i++) 
        {
            Destroy(_heroManager.heroesAlive[i]);
        }

        _heroManager.SpawnHeroes();
    }

    public int GetCurrentEnemy() { return _currentEnemy; }
    public int GetAttacksLeft() {  return _attacksLeft; }
    public void DecreaseAttacksLeft() { _attacksLeft--; }
    public int GetSpeedLeft() {  return _speedLeft; }
    public void DecreaseSpeedLeft(int x) { _speedLeft -= x;  }
    public int GetCurrentHeroHp() { return _heroScript.GetHp(); }
    public int GetCurrentHeroDamage() {  return _heroScript.GetDamage(); }
}
