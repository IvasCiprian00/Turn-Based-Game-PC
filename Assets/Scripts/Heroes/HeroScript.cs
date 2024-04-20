using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HeroScript : MonoBehaviour
{
    public enum MovementType
    {
        slow,
        fast
    }

    public enum AttackType
    {
        melee,
        ranged,
        mixed
    }

    [SerializeField] private TileManager _tileManager;
    [SerializeField] private HeroManager _heroManager;
    [SerializeField] private HealthbarScript _healthbarScript;
    //[SerializeField] private GameManager _gmManager;
    //[SerializeField] private UIManager _uiManager;

    [SerializeField] private int _xPos;
    [SerializeField] private int _yPos;
    [SerializeField] Animator _animator;

    [Header("Hero Attributes")]
    [SerializeField] private int _hp;
    [SerializeField] private int _maxHp;
    [SerializeField] private int _damage;
    [SerializeField] private int _speed;
    [SerializeField] private int _nrOfAttacks;
    [SerializeField] private int _range;
    public GameObject[] skills;

    [SerializeField] private MovementType _movementType;
    [SerializeField] private AttackType _attackType;


    private bool _isMoving;
    private GameObject _targetTile;

    public void Awake()
    {
        _tileManager = GameObject.Find("Tile Manager").GetComponent<TileManager>();
        _heroManager = GameObject.Find("Hero Manager").GetComponent<HeroManager>();
        _healthbarScript = GetComponentInChildren<HealthbarScript>();
        
    }

    public void Start()
    {
        _hp = _maxHp; 

        if (_healthbarScript != null)
        {
            _healthbarScript.SetHp(_hp);
            _healthbarScript.SetMaxHp(_maxHp);
        }
        //gmManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        //uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    public void Update()
    {
        if (_isMoving)
        {
            var step = 10 * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _targetTile.transform.position, step);

            if (Vector3.Distance(transform.position, _targetTile.transform.position) < 0.001f)
            {
                transform.position = _targetTile.transform.position;
                _isMoving = false;
            }
        }
    }

    public void Heal(int healValue)
    {
        _hp += healValue;

        if (_hp > _maxHp)
        {
            _hp = _maxHp;
        }
    }

    public void TakeDamage(int damage)
    {
        //uiManager.DisplayDamage(gameObject, damage);
        _hp -= damage;

        if (_healthbarScript != null)
        {
            _healthbarScript.SetHp(_hp);
            _healthbarScript.SetMaxHp(_maxHp);
        }

        if (_animator != null)
        {
            _animator.SetTrigger("take_damage");
        }

        if (_hp <= 0)
        {
            _heroManager.HeroDeath(gameObject);
            Destroy(gameObject);
        }
    }

    public void StartWinAniamtion()
    {
        if (_animator != null)
        {
            _animator.SetTrigger("level_complete");
        }
    }

    public void StopWinAnimation()
    {
        if (_animator != null)
        {
            _animator.SetTrigger("begin_new_level");
        }
    }

    public void MoveTo(GameObject tile)
    {
        int x = tile.GetComponent<MoveTileScript>().GetXPos();
        int y = tile.GetComponent<MoveTileScript>().GetYPos();
        SetCoords(x, y);

        _targetTile = _tileManager.tiles[x, y];
        _isMoving = true;

    }

    public void SetCoords(int x, int y)
    {
        _xPos = x;
        _yPos = y;
    }

    public int GetXPos() { return _xPos; }
    public int GetYPos() { return _yPos; }
    public string GetMovementType() { return _movementType.ToString().ToLower(); }
    public int GetDamage() { return _damage; }
    public int GetSpeed() { return _speed; }
    public int GetHp() { return _hp; }
    public int GetMaxHp() { return _maxHp; }
    public string GetAttackType() { return _attackType.ToString(); }
    public int GetRange() { return _range; }
    public int GetNrOfAttacks() { return _nrOfAttacks; }
}
