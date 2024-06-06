using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TileManager _tileManager;
    private HeroManager _heroManager;
    private UIManager _uiManager;

    private HeroScript _heroScript;
    [SerializeField] private List<GameObject> _skillTiles;

    [SerializeField] private GameObject _timerTile;
    [SerializeField] private GameObject _healTile;

    public void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _gameManager.SetManager(ref _tileManager);
        _gameManager.SetManager(ref _heroManager);
        _gameManager.SetManager(ref _uiManager);
    }

    public void SlimeSlamAttack(int range, int xPos, int yPos)
    {
        for(int i = xPos - range; i <= xPos + range; i++)
        {
            for(int j = yPos - range; j <= yPos + range; j++)
            {
                if(!_tileManager.PositionIsValid(i, j))
                {
                    continue;
                }
                if(i == xPos && j == yPos)
                {
                    continue;
                }

                GameObject reference = _tileManager.SpawnTile(_timerTile, i, j);
                reference.GetComponent<TimerTile>().SetTimer(2);
            }
        }
    }

    public void HealingWord()
    {
        _tileManager.DisableMoveTiles();

        int xPos;
        int yPos;
        GameObject reference;

        foreach (GameObject hero in _heroManager.heroesAlive)
        {
            _heroScript = hero.GetComponent<HeroScript>();

            xPos = _heroScript.GetXPos();
            yPos = _heroScript.GetYPos();

            reference = _tileManager.SpawnTile(_healTile, xPos, yPos);
            _skillTiles.Add(reference);
        }

        _uiManager.DisplayCancelSkill(true);
    }

    public void CancelSkill()
    {
        foreach(GameObject tile in _skillTiles)
        {
            Destroy(tile);
        }

        _skillTiles.Clear();

        _uiManager.DisplayCancelSkill(false);

        _tileManager.EnableMoveTiles();
    }
}
