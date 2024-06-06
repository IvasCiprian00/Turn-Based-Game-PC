using UnityEngine;

public class DirectInteractTile : Tile
{
    [SerializeField] private SkillManager _skillManager;
    [SerializeField] private int _interactValue;
    enum TileType
    {
        Heal,
        Damage,
        Debuff
    }
    [SerializeField] private TileType _tileType;

    override public void Awake()
    {
        base.SetManagers();
        _gameManager.SetManager(ref _skillManager);
    }

    public void Start()
    {
        transform.position -= new Vector3(0, 0, 1);
    }

    public void OnMouseUp()
    {
        switch (_tileType){
            case TileType.Heal:
                _tileManager.gameBoard[_xPos, _yPos].GetComponent<HeroScript>().Heal(_interactValue);
                break;

            case TileType.Damage:
                _tileManager.gameBoard[_xPos, _yPos].GetComponent<Enemy>().TakeDamage(_interactValue);
                break;

            case TileType.Debuff:
                break;

            default:
                break;
        }

        _skillManager.CancelSkill();
    }
}
