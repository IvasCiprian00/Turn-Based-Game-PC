using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    private TileManager _tileManager;
    private GameManager _gameManager;
    [Serializable]
    public struct Obstacle
    {
        public GameObject obstacle;
        public int _xPos;
        public int _yPos;
    }

    [SerializeField] private int _nrOfRows;
    [SerializeField] private int _nrOfColumns;
    [SerializeField] private Obstacle[] _obstacles;

    public void Awake()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _gameManager.SetManager(ref _tileManager);
    }

    /*public void Start()
    {
        _gameManager.LoadEnvironment();
    }*/

    public void SpawnEnvironment()
    {
        for(int i = 0; i < _obstacles.Length; i++)
        {
            GameObject reference = Instantiate(_obstacles[i].obstacle);
            reference.transform.position = _tileManager.tiles[_obstacles[i]._xPos, _obstacles[i]._yPos].transform.position;
            _tileManager.gameBoard[_obstacles[i]._xPos, _obstacles[i]._yPos] = reference;
        }
    }

    public int GetNrOfRows() { return _nrOfRows; }
    public int GetNrOfColumns() {  return _nrOfColumns; }
}
