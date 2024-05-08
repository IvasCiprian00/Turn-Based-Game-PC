using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroManager : MonoBehaviour
{
    [SerializeField] private TileManager _tileManager;
    [SerializeField] private TurnManager _turnManager;

    [Serializable]
    public struct HeroInfo
    {
        public GameObject hero;
        public int startingXPos;
        public int startingYPos;
    }

    public HeroInfo[] heroList;
    public GameObject[] heroesAlive;
    [SerializeField] private int _heroCount;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SpawnHeroes()
    {
        _tileManager = GameObject.Find("Tile Manager").GetComponent<TileManager>();
        _turnManager = GameObject.Find("Turn Manager").GetComponent<TurnManager>();

        _heroCount = heroList.Length;
        heroesAlive = new GameObject[_heroCount];

        for (int i = 0; i < _heroCount; i++)
        {
            int linePos = heroList[i].startingXPos;
            int colPos = heroList[i].startingYPos;

            heroesAlive[i] = Instantiate(heroList[i].hero);
            heroesAlive[i].transform.position = _tileManager.tiles[linePos, colPos].transform.position;
            _tileManager.gameBoard[linePos, colPos] = heroesAlive[i];
            heroesAlive[i].GetComponent<HeroScript>().SetCoords(linePos, colPos);
        }
    }

    public void HeroDeath(GameObject deadChar)
    {
        HeroScript heroScript = deadChar.GetComponent<HeroScript>();

        _tileManager.gameBoard[heroScript.GetXPos(), heroScript.GetYPos()] = null;

        for (int i = 0; i < _heroCount; i++)
        {
            if (deadChar == heroesAlive[i])
            {
                SetHeroCount(_heroCount - 1);

                _turnManager.CheckLevelProgress();

                RemoveDeadHero(i);

                return;
            }
        }
    }

    public void RemoveDeadHero(int index)
    {
        for (int i = index; i < _heroCount; i++)
        {
            heroesAlive[i] = heroesAlive[i + 1];
        }

    }

    public void SetHeroList()
    {
        CampManager campManager = GameObject.Find("Camp Manager").GetComponent<CampManager>();

        int heroCount = 0;
        for (int i = 0; i < 4; i++)
        {
            if(campManager.GetSelectedHeroAtIndex(i) == null)
            {
                continue;
            }

            heroCount++;
        }

        heroList = new HeroInfo[heroCount];

        int j = 0;

        for(int i = 0; i < 4; i++)
        {
            if (campManager.GetSelectedHeroAtIndex(i) == null)
            {
                continue;
            }

            heroList[j].hero = campManager.GetSelectedHeroAtIndex(i);
            heroList[j].startingXPos = j % 2;
            heroList[j].startingYPos = j / 2;

            j++;
        }
    }

    public int GetHeroCount() { return _heroCount; }
    public void SetHeroCount(int count) { _heroCount = count; }
}
