using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private TileManager _tileManager;
    [SerializeField] private GameManager _gameManager;

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

    [SerializeField] private EnemyScript _enemyScript;

    public void Awake()
    {
        _tileManager = GameObject.Find("Tile Manager").GetComponent<TileManager>();
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    public void Start()
    {
        _gameManager.OnLevelLoaded();
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
            enemiesAlive[i].GetComponent<EnemyScript>().SetCoords(linePos, colPos);
        }
    }

    public int GetEnemyCount() { return _enemyCount; }
    public void SetEnemyCount(int count) { _enemyCount = count; }
}
