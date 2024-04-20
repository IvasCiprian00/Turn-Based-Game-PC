using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private TileManager _tileManager;
    [SerializeField] private GameManager _gameManager;

    [SerializeField] private GameObject _timerTile;

    public void Start()
    {
        _tileManager = GameObject.Find("Tile Manager").GetComponent<TileManager>();
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    public void SlimeSlamAttack(int range, int xPos, int yPos)
    {
        for(int i = xPos - range; i < xPos + range; i++)
        {
            for(int j = yPos - range; j < yPos + range; j++)
            {
                if(!_tileManager.PositionIsValid(i, j))
                {
                    continue;
                }

                GameObject reference = _tileManager.SpawnTile(_timerTile, i, j);
                //reference.GetComponent<TimerTile>().SetTimer(2);
            }
        }
    }
}
