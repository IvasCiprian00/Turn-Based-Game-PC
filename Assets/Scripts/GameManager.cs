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

    private bool _levelLoaded;

    private void Awake()
    {
        SetManager(ref _tileManager);
        SetManager(ref _heroManager);
        SetManager(ref _turnManager);
        SetManager(ref _uiManager);
    }

    public void Start()
    {
        _levelNumber++;
        SceneManager.LoadScene(_levelNumber, LoadSceneMode.Additive);
    }

    public void InitializeGrid()
    {
        _tileManager.GenerateGameBoard(_nrOfRows, _nrOfColumns);
        _heroManager.SpawnHeroes();
    }

    public void StartLevel()
    {
        _levelLoaded = true;
        _turnManager.StartHeroTurns();
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
    public bool GetLevelLoaded() { return _levelLoaded; }
    public void SetGridSize(int x, int y)
    {
        _nrOfRows = x;
        _nrOfColumns = y;
    }

    public void SetManager(ref TurnManager manager)
    {
        manager = GameObject.Find("Turn Manager").GetComponent<TurnManager>();
    }
    public void SetManager(ref TileManager manager)
    {
        manager = GameObject.Find("Tile Manager").GetComponent<TileManager>();
    }
    public void SetManager(ref LoadManager manager)
    {
        manager = GameObject.Find("Load Manager").GetComponent<LoadManager>();
    }
    public void SetManager(ref HeroManager manager)
    {
        manager = GameObject.Find("Hero Manager").GetComponent<HeroManager>();
    }
    public void SetManager(ref EnemyManager manager)
    {
        manager = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();
    }
    public void SetManager(ref UIManager manager)
    {
        manager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }
    public void SetManager(ref EnvironmentManager manager)
    {
        manager = GameObject.Find("Environment Manager").GetComponent<EnvironmentManager>();
    }
    public void SetManager(ref DarknessManager manager)
    {
        manager = GameObject.Find("Darkness Manager").GetComponent<DarknessManager>();
    }
    public void SetManager(ref SkillManager manager)
    {
        manager = GameObject.Find("Skill Manager").GetComponent<SkillManager>();
    }
}
