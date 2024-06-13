using JetBrains.Annotations;
using UnityEngine;
public enum TileType
{
    Heal,
    Damage,
    Debuff
}

public class DirectInteractTile : Tile
{
    [SerializeField] private SkillManager _skillManager;
    [SerializeField] private int _interactValue;
    [SerializeField] private TileType _tileType;

    [Header("Status Section")]
    [SerializeField] private StatusType _statusType;
    [SerializeField] private int _statusDuration;
    [SerializeField] private int _statusDamage;

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
        _soundManager.PlayPreparedSound();
        HeroScript heroScript = _turnManager.GetCurrentHero().GetComponent<HeroScript>();
        heroScript.DecreaseSkillUsages();

        switch (_tileType){
            case TileType.Heal:
                _tileManager.gameBoard[_xPos, _yPos].GetComponent<HeroScript>().Heal(_interactValue);
                break;

            case TileType.Damage:
                _tileManager.gameBoard[_xPos, _yPos].GetComponent<Enemy>().TakeDamage(_interactValue);
                break;

            case TileType.Debuff:
                _tileManager.gameBoard[_xPos, _yPos].GetComponent<Enemy>().ApplyStatus(_statusType, _statusDuration, _statusDamage);
                break;

            default:
                break;
        }

        _skillManager.CancelSkill();
    }

    public void SetTileType(TileType tileType) { _tileType = tileType; }
    public void SetStatusType(StatusType statusType) { _statusType = statusType; }
    public void SetStatusDuration(int duration) { _statusDuration = duration; }
    public void SetStatusDamage(int damage) {  _statusDamage = damage; }
    public void SetInteractValue(int value) { _interactValue = value; }
}
