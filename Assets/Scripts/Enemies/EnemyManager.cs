using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private TileManager _tileManager;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TurnManager _turnManager;

    [Serializable]
    public struct EnemyInfo
    {
        public GameObject enemy;
        public int startingXPos;
        public int startingYPos;
    }

    public EnemyInfo[] enemyList;
    public GameObject[] enemiesAlive;
    [SerializeField] private int _enemyCount;

    [SerializeField] private Enemy _enemyScript;

    public void Awake()
    {
        _tileManager = GameObject.Find("Tile Manager").GetComponent<TileManager>();
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _turnManager = GameObject.Find("Turn Manager").GetComponent<TurnManager>();
    }

    public void Start()
    {
        //_gameManager.LoadEnemies();
        _turnManager.OnEnemiesLoaded();
    }

    public void SpawnEnemies()
    {
        _enemyCount = enemyList.Length;
        enemiesAlive = new GameObject[_enemyCount];

        for (int i = 0; i < _enemyCount; i++)
        {
            int linePos = enemyList[i].startingXPos;
            int colPos = enemyList[i].startingYPos;

            enemiesAlive[i] = Instantiate(enemyList[i].enemy);
            enemiesAlive[i].transform.position = _tileManager.tiles[linePos, colPos].transform.position;
            _tileManager.gameBoard[linePos, colPos] = enemiesAlive[i];
            enemiesAlive[i].GetComponent<Enemy>().SetCoords(linePos, colPos);
        }
    }

    public void EnemyDeath(GameObject deadChar)
    {
        Enemy enemyScript = deadChar.GetComponent<Enemy>();

        _tileManager.gameBoard[enemyScript.GetXPos(), enemyScript.GetYPos()] = null;

        for (int i = 0; i < _enemyCount; i++)
        {
            if (deadChar == enemiesAlive[i])
            {
                SetEnemyCount(_enemyCount - 1);

                _turnManager.CheckLevelProgress();

                RemoveDeadEnemy(i);

                return;
            }
        }
    }

    public void RemoveDeadEnemy(int index)
    {
        for (int i = index; i < _enemyCount; i++)
        {
            enemiesAlive[i] = enemiesAlive[i + 1];
        }
    }

    public int GetEnemyCount() { return _enemyCount; }
    public void SetEnemyCount(int count) { _enemyCount = count; }
}
