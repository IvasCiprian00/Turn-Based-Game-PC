using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private GameManager _gameManager;
    private TileManager _tileManager;
    private TurnManager _turnManager;
    private SoundManager _soundManager;
    private HeroManager _heroManager;
    private UIManager _uiManager;

    private HeroScript _heroScript;
    [SerializeField] private List<GameObject> _skillTiles;

    [SerializeField] private GameObject _rangeTile;
    [SerializeField] private GameObject _timerTile;
    [SerializeField] private GameObject _healTile;
    [SerializeField] private GameObject _interactTile;

    private bool _canCast;

    public void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _gameManager.SetManager(ref _tileManager);
        _gameManager.SetManager(ref _heroManager);
        _gameManager.SetManager(ref _turnManager);
        _gameManager.SetManager(ref _uiManager);
        _gameManager.SetManager(ref _soundManager);
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
        ResetSkill();
        if (!_canCast)
        {
            return;
        }

        int xPos;
        int yPos;
        GameObject reference;

        foreach (GameObject hero in _heroManager.heroesAlive)
        {
            _heroScript = hero.GetComponent<HeroScript>();

            xPos = _heroScript.GetXPos();
            yPos = _heroScript.GetYPos();

            reference = _tileManager.SpawnTile(_interactTile, xPos, yPos);
            DirectInteractTile tile = reference.GetComponent<DirectInteractTile>();
            tile.SetTileType(TileType.Heal);
            tile.SetInteractValue(Random.Range(2, 4));
            _skillTiles.Add(reference);
        }

        _soundManager.PrepareSound(_soundManager.heal);
    }

    public void PommelStrike()
    {
        ResetSkill();
        _soundManager.PrepareSound(_soundManager.stun);
        if (!_canCast)
        {
            return;
        }

        _heroScript = _turnManager.GetCurrentHero().GetComponent<HeroScript>();
        int xPos = _heroScript.GetXPos();
        int yPos = _heroScript.GetYPos();

        GameObject reference;

        for (int i = -1; i <= 1; i++)
        {
            for(int j = -1; j <= 1; j++)
            {
                xPos = _heroScript.GetXPos() + i;
                yPos = _heroScript.GetYPos() + j;

                if (!_tileManager.PositionIsValid(xPos, yPos))
                {
                    continue;
                }

                if (_tileManager.gameBoard[xPos, yPos] == null)
                {
                    reference = _tileManager.SpawnTile(_rangeTile, xPos, yPos);

                    _skillTiles.Add(reference);
                    continue;
                }

                if (_tileManager.gameBoard[xPos, yPos].tag == "Enemy")
                {
                    reference = _tileManager.SpawnTile(_interactTile, xPos, yPos);
                    DirectInteractTile tile = reference.GetComponent<DirectInteractTile>();
                    tile.SetTileType(TileType.Debuff);
                    tile.SetStatusType(StatusType.Stun);
                    tile.SetStatusDuration(1);
                    _skillTiles.Add(reference);
                }
            }
        }

    }

    public void Ignite()
    {
        ResetSkill();
        _soundManager.PrepareSound(_soundManager.burn);
        if (!_canCast)
        {
            return;
        }

        _heroScript = _turnManager.GetCurrentHero().GetComponent<HeroScript>();
        int startXPos = _heroScript.GetXPos();
        int startYPos = _heroScript.GetYPos();
        int xPos;
        int yPos;

        GameObject reference;

        for(int i = -3; i <= 3; i++)
        {
            for(int j = -3; j <= 3; j++)
            {
                xPos = _heroScript.GetXPos() + i;
                yPos = _heroScript.GetYPos() + j;
                
                if (Mathf.Abs(xPos - startXPos) + Mathf.Abs(yPos - startYPos) > 2)
                {
                    continue;
                }


                if (!_tileManager.PositionIsValid(xPos, yPos))
                {
                    continue;
                }

                if (_tileManager.gameBoard[xPos, yPos] == null)
                {
                    reference = _tileManager.SpawnTile(_rangeTile, xPos, yPos);

                    _skillTiles.Add(reference);
                    continue;
                }

                if (_tileManager.gameBoard[xPos, yPos].tag == "Enemy")
                {
                    reference = _tileManager.SpawnTile(_interactTile, xPos, yPos);
                    DirectInteractTile tile = reference.GetComponent<DirectInteractTile>();
                    tile.SetTileType(TileType.Debuff);
                    tile.SetStatusType(StatusType.Burn);
                    tile.SetStatusDamage(2);
                    tile.SetStatusDuration(2);
                    _skillTiles.Add(reference);
                }
            }
        }
    }


    public void Bleed()
    {
        ResetSkill();
        _soundManager.PrepareSound(_soundManager.bleed);

        if (!_canCast)
        {
            return;
        }

        _heroScript = _turnManager.GetCurrentHero().GetComponent<HeroScript>();
        int xPos = _heroScript.GetXPos();
        int yPos = _heroScript.GetYPos();

        GameObject reference;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                xPos = _heroScript.GetXPos() + i;
                yPos = _heroScript.GetYPos() + j;

                if (!_tileManager.PositionIsValid(xPos, yPos))
                {
                    continue;
                }

                if (_tileManager.gameBoard[xPos, yPos] == null)
                {
                    reference = _tileManager.SpawnTile(_rangeTile, xPos, yPos);

                    _skillTiles.Add(reference);
                    continue;
                }

                if (_tileManager.gameBoard[xPos, yPos].tag == "Enemy")
                {
                    reference = _tileManager.SpawnTile(_interactTile, xPos, yPos);
                    DirectInteractTile tile = reference.GetComponent<DirectInteractTile>();
                    tile.SetTileType(TileType.Debuff);
                    tile.SetStatusType(StatusType.Bleed);
                    tile.SetStatusDuration(3);
                    tile.SetStatusDamage(1);
                    _skillTiles.Add(reference);
                }
            }
        }

    }

    public void ResetSkill()
    {
        _canCast = true;
        CancelSkill();

        if (_turnManager.GetCurrentHero().GetComponent<HeroScript>().GetUsagesLeft() <= 0)
        {
            _canCast = false;
            return;
        }

        _tileManager.DisableMoveTiles();
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
