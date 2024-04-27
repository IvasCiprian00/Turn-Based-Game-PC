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
        if(GameObject.Find("Darkness Manager") != null)
        {
            _darknessManager = GameObject.Find("Darkness Manager").GetComponent<DarknessManager>();
        }
        if (GameObject.Find("Environment Manager") != null)
        {
            _envManager = GameObject.Find("Environment Manager").GetComponent<EnvironmentManager>();
        }
        if (GameObject.Find("Enemy Manager") != null)
        {
            _enemyManager = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();
        }
        if (GameObject.Find("Game Manager") != null)
        {
            _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        }
    }

    private void Start()
    {
        if(_darknessManager != null)
        {
            _darknessManager.SpawnDarkness();
        }

        if(_envManager != null)
        {
            _envManager.SpawnEnvironment();
        }

        if(_enemyManager != null)
        {
            _enemyManager.SpawnEnemies();
        }

        _gameManager.StartLevel();
    }
}
