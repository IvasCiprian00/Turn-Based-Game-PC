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

    private static HeroManager instance;

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void SpawnHeroes()
    {
        _tileManager = GameObject.Find("Tile Manager").GetComponent<TileManager>();
        _turnManager = GameObject.Find("Turn Manager").GetComponent<TurnManager>();

        heroesAlive = new GameObject[_heroCount];

        int j = 0;

        for (int i = 0; i < 4; i++)
        {
            if (heroList[i].hero == null)
            {
                continue;
            }

            Debug.Log(heroList[i].hero.GetComponent<HeroScript>().IsDead());
            if (heroList[i].hero.GetComponent<HeroScript>().IsDead())
            {
                continue;
            }

            int linePos = heroList[i].startingXPos;
            int colPos = heroList[i].startingYPos;

            heroesAlive[j] = Instantiate(heroList[i].hero);
            heroesAlive[j].transform.position = _tileManager.tiles[linePos, colPos].transform.position;
            _tileManager.gameBoard[linePos, colPos] = heroesAlive[j];
            heroesAlive[j].GetComponent<HeroScript>().SetCoords(linePos, colPos);

            j++;
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

        int[] startingX = { 2, 1, 1, 0 };
        int[] startingY = { 1, 0, 2, 1 };

        int heroCount = 0;
        heroList = new HeroInfo[4];

        for (int i = 0; i < 4; i++)
        {
            if(campManager.GetSelectedHeroAtIndex(i) == null)
            {
                continue;
            }

            heroList[i].hero = campManager.GetSelectedHeroAtIndex(i);
            heroList[i].startingXPos = startingX[i];
            heroList[i].startingYPos = startingY[i];

            heroCount++;
        }

        _heroCount = heroCount;
    }

    public void GetHeroes(GameObject[] list)
    {
        for(int i = 0; i < heroList.Length; i++)
        {
            list[i] = heroList[i].hero;
        }
    }

    public int GetHeroCount() { return _heroCount; }
    public void SetHeroCount(int count) { _heroCount = count; }
}
