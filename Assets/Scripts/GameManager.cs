using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private HeroManager _heroManager;
    private EnemyManager _enemyManager;
    private TileManager _tileManager;
    private TurnManager _turnManager;
    private EnvironmentManager _envManager;
    private DarknessManager _darknessManager;
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


    public void OnEnemiesLoaded()
    {
        _enemyManager = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();

        _enemyManager.SpawnEnemies();

    }

    public void OnEnvironmentLoaded()
    {
        _envManager = GameObject.Find("Environment Manager").GetComponent<EnvironmentManager>();

        _envManager.SpawnEnvironment();
    }

    public void OnDarknessLoaded()
    {
        _darknessManager = GameObject.Find("Darkness Manager").GetComponent<DarknessManager>();

        _darknessManager.SpawnDarkness();
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

    public void RestartLevel()
    {
        SceneManager.UnloadSceneAsync(_levelNumber);

        _turnManager.ResetHeroes();
        _turnManager.StartHeroTurns();
        _uiManager.HideEndOfLevelButtons();

        SceneManager.LoadScene(_levelNumber, LoadSceneMode.Additive);
    }

    public void CenterCamera()
    {
        Vector3 center = _tileManager.tiles[_nrOfRows / 2, _nrOfColumns / 2].transform.position;
        float x = center.x;
        float y = center.y;
        Camera.main.transform.position = new Vector3(x, y, Camera.main.transform.position.z);
    }

    public int GetNrOfRows() {  return _nrOfRows; }
    public int GetNrOfColumns() {  return _nrOfColumns; }
}
