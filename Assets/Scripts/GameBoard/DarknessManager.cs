using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessManager : MonoBehaviour
{
    private HeroManager _heroManager;
    private GameManager _gameManager;
    private TileManager _tileManager;

    [SerializeField] private GameObject _darkTile;

    public void Awake()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _gameManager.SetManager(ref _heroManager);
        _gameManager.SetManager(ref _tileManager);
    }

    public void SpawnDarkness()
    {
        for(int i = 0; i < _gameManager.GetNrOfRows(); i++)
        {
            for(int j = 0; j < _gameManager.GetNrOfColumns(); j++)
            {
                GameObject reference = Instantiate(_darkTile, _tileManager.tiles[i, j].transform.position, Quaternion.identity);
                reference.GetComponent<Tile>().SetCoords(i, j);
            }
        }

    }
}
