using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private GameObject _selectedEffect;

    private GameManager _gameManager;
    private UIManager _uiManager;
    private HeroManager _heroManager;
    private TileManager _tileManager;
    private EnemyManager _enemyManager;
    private HeroScript _heroScript;
    private Enemy _enemyScript;

    private int _actionsLeft;
    private int _speedLeft;
    private int _currentHero;
    private int _currentEnemy;

    private bool _gameOver;

    private void Awake()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _gameManager.SetManager(ref _uiManager);
        _gameManager.SetManager(ref _tileManager);
        _gameManager.SetManager(ref _heroManager);
    }

    private void Start()
    {
        _selectedEffect = Instantiate(_selectedEffect);
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
        _actionsLeft = _heroScript.GetNrOfActions();

        _selectedEffect.transform.position = _heroManager.heroesAlive[_currentHero].transform.position;
        _selectedEffect.transform.parent = _heroManager.heroesAlive[_currentHero].transform;

        _uiManager.DisplaySkills(_heroScript.skills);

        _tileManager.GenerateMoveTiles(_heroScript);
    }

    public void OnEnemiesLoaded()
    {
        _gameManager.SetManager(ref _enemyManager);
    }

    public void StartEnemyTurns()
    {
        _uiManager.DestroySkills();

        _selectedEffect.transform.parent = null;
        _selectedEffect.SetActive(false);
        _uiManager.DisplayUI(false);

        _tileManager.DestroyMoveTiles();

        _currentEnemy = 0;

        _enemyScript = _enemyManager.enemiesAlive[_currentEnemy].GetComponent<Enemy>();

        //_heroTurn = false;
        _enemyScript.StartTurn();
    }

    public void StartHeroTurns()
    {
        _uiManager.DisplayUI(true);
        _selectedEffect.SetActive(true);
        _currentHero = 0;
        //_heroTurn = true;

        _selectedEffect.transform.position = _heroManager.heroesAlive[_currentHero].transform.position;
        _selectedEffect.transform.parent = _heroManager.heroesAlive[_currentHero].transform;

        _heroScript = _heroManager.heroesAlive[_currentHero].GetComponent<HeroScript>();
        _speedLeft = _heroScript.GetSpeed();
        _actionsLeft = _heroScript.GetNrOfActions();

        _uiManager.DisplaySkills(_heroScript.skills);

        _tileManager.GenerateMoveTiles(_heroScript);
    }

    
    public void SetTurnOrder()
    {
        //later implement this with character initiative
    }


    public void NextEnemy()
    {
        _currentEnemy++;

        if(_currentEnemy >= _enemyManager.GetEnemyCount())
        {
            _currentEnemy = 0;

            StartHeroTurns();

            return;
        }

        _enemyManager.enemiesAlive[_currentEnemy].GetComponent<Enemy>().StartTurn();
    }

    public void CheckLevelProgress()
    {
        if (_heroManager.GetHeroCount() <= 0)
        {
            //_gameOver = true;

            _uiManager.DisplayEndOfLevelButtons(false);
        }

        else if (_enemyManager.GetEnemyCount() <= 0)
        {
            _gameOver = true;
            _gameManager.IncreaseLevelNumber();

            _tileManager.DestroyMoveTiles();
            //_uiManager.HideSkills();


            for (int i = 0; i < _heroManager.GetHeroCount(); i++)
            {
                if (_heroManager.heroesAlive[i] == null)
                {
                    continue;
                }

                _heroManager.heroesAlive[i].GetComponent<HeroScript>().StartWinAniamtion();
            }

            _uiManager.DisplayEndOfLevelButtons(true);
        }
    }

    public void ResetHeroes()
    {
        _selectedEffect.transform.parent = null;

        for(int i = 0; i < _heroManager.GetHeroCount(); i++) 
        {
            Destroy(_heroManager.heroesAlive[i]);
        }
    }


    public int GetCurrentHeroIndex() { return _currentHero; }
    public GameObject GetCurrentHero() { return _heroManager.heroesAlive[_currentHero]; }
    public int GetCurrentEnemy() { return _currentEnemy; }
    public int GetActionsLeft() {  return _actionsLeft; }
    public void DecreaseActionsLeft() { _actionsLeft--; }
    public int GetSpeedLeft() {  return _speedLeft; }
    public void DecreaseSpeedLeft(int x) { _speedLeft -= x;  }
    public int GetCurrentHeroHp() { return _heroScript.GetHp(); }
    public string GetCurrentHeroDamage() {  return _heroScript.GetLowerDamage() + " - " + _heroScript.GetUpperDamage(); }
    public bool IsGameOver() {  return _gameOver; }
    public void SetGameOver(bool pp) { _gameOver = pp; }
}
