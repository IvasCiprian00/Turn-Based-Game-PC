using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private HeroManager _heroManager;
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private TileManager _tileManager;
    [SerializeField] private TurnManager _turnManager;
    private UIManager _uiManager;

    [SerializeField] private int _levelNumber;

    [Header("Game Board Section")]
    [SerializeField] private int _nrOfRows;
    [SerializeField] private int _nrOfColumns;

    private void Awake()
    {
        _tileManager = GameObject.Find("Tile Manager").GetComponent<TileManager>();
        _heroManager = GameObject.Find("Hero Manager").GetComponent<HeroManager>();
        _turnManager = GameObject.Find("Turn Manager").GetComponent<TurnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    public void Start()
    {
        _tileManager.GenerateGameBoard(_nrOfRows, _nrOfColumns);
        _heroManager.SpawnHeroes();

        _levelNumber++;
        SceneManager.LoadScene(_levelNumber, LoadSceneMode.Additive);

        _turnManager.StartHeroTurns();
    }


    public void OnLevelLoaded()
    {
        _enemyManager = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();

        _enemyManager.SpawnEnemies();

    }

    public void GoToNextLevel()
    {
        SceneManager.UnloadSceneAsync(_levelNumber);

        _turnManager.ResetHeroes();
        _turnManager.StartHeroTurns();
        _uiManager.HideEndOfLevelButtons();

        _levelNumber++;
        SceneManager.LoadScene(_levelNumber, LoadSceneMode.Additive);
    }

    public int GetNrOfRows() {  return _nrOfRows; }
    public int GetNrOfColumns() {  return _nrOfColumns; }
}
