using UnityEngine;

public class Tile : MonoBehaviour
{

    protected GameManager _gameManager;
    protected TileManager _tileManager;
    protected HeroManager _heroManager;
    protected TurnManager _turnManager;
    protected SoundManager _soundManager;
    protected UIManager _uiManager;

    protected int _xPos;
    protected int _yPos;

    virtual public void Awake()
    {
        SetManagers();
    }

    public void SetManagers()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _gameManager.SetManager(ref _turnManager);
        _gameManager.SetManager(ref _heroManager);
        _gameManager.SetManager(ref _tileManager);
        _gameManager.SetManager(ref _uiManager);
        _gameManager.SetManager(ref _soundManager);
    }

    public void SetCoords(int x, int y)
    {
        _xPos = x;
        _yPos = y;
    }

    virtual public void SetAttacking(bool attacking) { }
    public int GetXPos() { return _xPos; }
    public int GetYPos() { return _yPos; }

}
