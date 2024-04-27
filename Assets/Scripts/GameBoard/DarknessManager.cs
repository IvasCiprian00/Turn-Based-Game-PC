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
        _tileManager = GameObject.Find("Tile Manager").GetComponent<TileManager>();
        _heroManager = GameObject.Find("Hero Manager").GetComponent<HeroManager>();
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    public void Start()
    {
        _gameManager.OnDarknessLoaded();
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

        //UpdateDarkness();
    }

    /*public void UpdateDarkness()
    {
        for(int k = 0; k < _heroManager.GetHeroCount(); k++)
        {
            HeroScript heroScript = _heroManager.heroesAlive[k].GetComponent<HeroScript>();
            int x = heroScript.GetXPos();
            int y = heroScript.GetYPos();

            for(int i = x - 2; i <= x + 2; i++)
            {
                for(int j = y - 2; j <= y + 2; j++)
                {
                    if (!_tileManager.PositionIsValid(i, j))
                    {
                        continue;
                    }

                    darkTiles[i, j].SetActive(false);
                }
            }
        }
    }*/
}
