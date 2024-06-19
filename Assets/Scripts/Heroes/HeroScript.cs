using UnityEngine;

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
public class HeroScript : MonoBehaviour
{

    private TileManager _tileManager;
    private TurnManager _turnManager;
    private HeroManager _heroManager;
    private HealthbarScript _healthbarScript;
    private GameManager _gameManager;
    private UIManager _uiManager;

    private int _xPos;
    private int _yPos;
    [SerializeField] Animator _animator;

    [Header("Camp Positions")]
    [SerializeField] private Sprite[] positionSprites;

    [Header("Hero Attributes")]
    [SerializeField] private int _hp;
    [SerializeField] private int _maxHp;
    [SerializeField] private int _evasion;
    [SerializeField] private int _damage;
    [SerializeField] private int _lowerDamage;
    [SerializeField] private int _upperDamage;
    [SerializeField] private int _speed;
    [SerializeField] private int _nrOfActions;
    [SerializeField] private int _range;
    public GameObject[] skills;
    [SerializeField] private string _skillPrefName;
    [SerializeField] private int _skillUsages;

    [SerializeField] private MovementType _movementType;
    [SerializeField] private AttackType _attackType;

    [SerializeField] private string _prefName;
    private bool _isMoving;
    private GameObject _targetTile;

    public void Awake()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _gameManager.SetManager(ref _tileManager);
        _gameManager.SetManager(ref _heroManager);
        _gameManager.SetManager(ref _uiManager);
        _gameManager.SetManager(ref _turnManager);
        _healthbarScript = GetComponentInChildren<HealthbarScript>();
        
    }

    public void Start()
    {
        if (!PlayerPrefs.HasKey(_prefName))
        {
            PlayerPrefs.SetInt(_prefName, _maxHp);
        }
        _hp = PlayerPrefs.GetInt(_prefName);

        if (!PlayerPrefs.HasKey(_skillPrefName))
        {
            PlayerPrefs.SetInt(_skillPrefName, _skillUsages);
        }

        if (_healthbarScript != null)
        {
            _healthbarScript.SetHp(_hp);
            _healthbarScript.SetMaxHp(_maxHp);
        }

        GetComponentInChildren<SpriteRenderer>().sortingOrder = _xPos;
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
        PlayerPrefs.SetInt(_prefName, _hp);

        _uiManager.DisplayDamage(gameObject, -healValue);

        if (_hp > _maxHp)
        {
            _hp = _maxHp;
            PlayerPrefs.SetInt(_prefName, _hp);
        }

        _healthbarScript.SetHp(_hp);
    }

    public void TakeDamage(int damage)
    {
        int chance = Random.Range(0, 100);

        if(chance <= _evasion)
        {
            damage = 0;
        }

        _uiManager.DisplayDamage(gameObject, damage);

        _hp -= damage;
        PlayerPrefs.SetInt(_prefName, _hp);

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
        int x = tile.GetComponent<MoveTile>().GetXPos();
        int y = tile.GetComponent<MoveTile>().GetYPos();
        SetCoords(x, y);

        _targetTile = _tileManager.tiles[x, y];
        _isMoving = true;

    }

    public void SetCoords(int x, int y)
    {
        GetComponentInChildren<SpriteRenderer>().sortingOrder = x + 1;
        _xPos = x;
        _yPos = y;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Obstacle")
        {
            Debug.Log("YEY");
        }
    }

    public void SetPlayerPrefs()
    {
        _hp = PlayerPrefs.GetInt(_prefName, _maxHp);
        PlayerPrefs.GetInt(_skillPrefName, _skillUsages);
    }

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.SetInt(_prefName, _maxHp);
        PlayerPrefs.SetInt(_skillPrefName, _skillUsages);
    }

    public void DecreaseSkillUsages() 
    { 
        int remaining = PlayerPrefs.GetInt(_skillPrefName);
        PlayerPrefs.SetInt(_skillPrefName, remaining - 1); 
    }

    public void OnMouseUp()
    {
        Debug.Log("YEY");
        //_tileManager.GenerateRangeTiles();
    }

    public int GetUsagesLeft() { return PlayerPrefs.GetInt(_skillPrefName); }

    public bool IsDead() 
    { 
        if(PlayerPrefs.GetInt(_prefName) <= 0)
        {
            return true;
        }
        return false;
    }
    public string GetSkillPrefName() { return _skillPrefName; }
    public int GetLowerDamage() { return _lowerDamage; }
    public int GetUpperDamage() {  return _upperDamage; }
    public Sprite GetCampPosition(int index) { return positionSprites[index]; }
    public int GetXPos() { return _xPos; }
    public int GetYPos() { return _yPos; }
    public string GetMovementType() { return _movementType.ToString().ToLower(); }
    public int GetDamage() { return Random.Range(_lowerDamage, _upperDamage + 1); }
    public int GetSpeed() { return _speed; }
    public int GetHp() { return _hp; }
    public int GetMaxHp() { return _maxHp; }
    public AttackType GetAttackType() {  return _attackType; }
    public int GetRange() { return _range; }
    public int GetNrOfActions() { return _nrOfActions; }
    virtual public bool IsHealer() { return false; }
    virtual public int GetHealAmount() { return 0; }
    public void SetEvasion(int evasion) { _evasion = evasion; }
    public int GetEvasion() {  return _evasion; }
    public string GetPrefName() { return _prefName; }
}
