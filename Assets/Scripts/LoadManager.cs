using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadManager : MonoBehaviour
{
    private DarknessManager _darknessManager;
    private EnvironmentManager _envManager;
    private EnemyManager _enemyManager;
    private GameManager _gameManager;

    void Awake()
    {
        if (GameObject.Find("Game Manager") != null)
        {
            _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        }
        if (GameObject.Find("Darkness Manager") != null)
        {
            _gameManager.SetManager(ref _darknessManager);
        }
        if (GameObject.Find("Environment Manager") != null)
        {
            _gameManager.SetManager(ref _envManager);
        }
        if (GameObject.Find("Enemy Manager") != null)
        {
            _gameManager.SetManager(ref _enemyManager);
        }
    }

    private void Start()
    {
        if (_envManager != null)
        {
            int x = _envManager.GetNrOfRows();
            int y = _envManager.GetNrOfColumns();

            _gameManager.SetGridSize(x, y);
            _gameManager.InitializeGrid();

            _envManager.SpawnEnvironment();
        }

        if (_darknessManager != null)
        {
            _darknessManager.SpawnDarkness();
        }

        if(_enemyManager != null)
        {
            _enemyManager.SpawnEnemies();
        }

        _gameManager.StartLevel();
    }
}
