using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private HeroManager _heroManager;
    private EnemyManager _enemyManager;
    private SoundManager _soundManager;
    private TileManager _tileManager;
    private TurnManager _turnManager;
    private EnvironmentManager _envManager;
    private DarknessManager _darknessManager;
    private UIManager _uiManager;
    private Animator _cameraAnimator;
    private Animator _canvasAnimator;
    private string _preparedScene;
    [SerializeField] private GameObject _cameraSlideTrigger;

    [SerializeField] private int _levelNumber;

    [Header("Game Board Section")]
    [SerializeField] private int _nrOfRows;
    [SerializeField] private int _nrOfColumns;

    private const string _levelPref = "LevelNumber";

    private bool _levelLoaded;

    private void Awake()
    {
        SetManager(ref _tileManager);
        SetManager(ref _heroManager);
        SetManager(ref _turnManager);
        SetManager(ref _uiManager);
        SetManager(ref _soundManager);
        _cameraAnimator = GameObject.Find("Camera Parent").GetComponent<Animator>();
        _canvasAnimator = GameObject.Find("Canvas").GetComponent<Animator>();
    }

    public void Start()
    {
        if (!PlayerPrefs.HasKey(_levelPref))
        {
            PlayerPrefs.SetInt(_levelPref, 3);
        }
        _cameraAnimator.SetTrigger("slide up");
        _levelNumber++;
        SceneManager.LoadScene(PlayerPrefs.GetInt(_levelPref), LoadSceneMode.Additive);
    }

    public void InitializeGrid()
    {
        GameObject[] activeTiles = GameObject.FindGameObjectsWithTag("Tile");
        for(int i = 0; i < activeTiles.Length; i++)
        {
            Destroy(activeTiles[i]);
        }

        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        for(int i = 0; i < obstacles.Length; i++)
        {
            Destroy(obstacles[i]);
        }

        _tileManager.GenerateGameBoard(_nrOfRows, _nrOfColumns);
        _heroManager.SpawnHeroes();
    }

    public void StartLevel()
    {
        _turnManager.SetGameOver(false);
        _levelLoaded = true;
        _turnManager.StartHeroTurns();
    }

    public void PrepareLevel()
    {
        _turnManager.ResetHeroes();

        _levelNumber++;
        SceneManager.LoadScene(PlayerPrefs.GetInt(_levelPref), LoadSceneMode.Additive);
        _cameraAnimator.SetTrigger("slide trigger");
    }

    public void GoToNextLevel()
    {
        SceneManager.UnloadSceneAsync(PlayerPrefs.GetInt(_levelPref) - 1);
        _soundManager.PlaySound(_soundManager.whoosh);

        _cameraAnimator.SetTrigger("next level");
        _uiManager.HideEndOfLevelButtons();
    }

    public void TriggerCutscene(string scene)
    {
        _preparedScene = scene;
        _canvasAnimator.SetTrigger("fade");
    }

    public void LoadCutscene()
    {
        SceneManager.LoadScene(_preparedScene);
    }

    public void RestartLevel()
    {
        SceneManager.UnloadSceneAsync(_levelNumber);

        _turnManager.ResetHeroes();
        _uiManager.HideEndOfLevelButtons();

        SceneManager.LoadScene(_levelNumber, LoadSceneMode.Additive);
    }

    public void GoToCamp()
    {
        SceneManager.LoadScene("Camp Screen");
    }

    public void CenterCamera()
    {
        Vector3 center = _tileManager.tiles[_nrOfRows / 2, _nrOfColumns / 2].transform.position;
        float x = center.x;
        float y = center.y;
        Camera.main.transform.position = new Vector3(x, y, Camera.main.transform.position.z);
    }

    public void IncreaseLevelNumber()
    {
        PlayerPrefs.SetInt(_levelPref, PlayerPrefs.GetInt(_levelPref) + 1);
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
    public void SetManager(ref SoundManager manager)
    {
        manager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
    }
}
