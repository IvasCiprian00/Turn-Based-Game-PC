using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CampManager : MonoBehaviour
{
    private SoundManager _soundManager;
    [SerializeField] private GameObject[] _allHeroesList;
    [SerializeField] private GameObject[] _heroList;
    [SerializeField] private Image[] _slots;
    [SerializeField] private int _slotIndex;
    [SerializeField] private GameObject _changeHeroContainer;
    [SerializeField] private Animator _animator;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private GameObject _continueGameTrigger;

    private float _timer;

    public void Awake()
    {
        _animator = GameObject.Find("Canvas").GetComponent<Animator>();
    }

    public void Start()
    {
        PlayerPrefs.DeleteAll();
        HeroManager heroManager = GameObject.Find("Hero Manager").GetComponent<HeroManager>();
        _soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
        StartCoroutine(_soundManager.PlayCampSounds());

        _soundManager.PlayMusic(_soundManager.crickets);

        if(heroManager.heroList == null)
        {
            return;
        }

        heroManager.GetHeroes(_heroList);

        SpawnHeroesAtCamp();
    }

    public void Update()
    {
        if(_continueGameTrigger.activeSelf == true)
        {
            SceneManager.LoadScene("Main Game");
        }

        _timer += Time.deltaTime;

        if(_timer <= 5)//change to >= when implementing finaly
        {
            return;
        }

        _timer = 0;

        for(int i = 0; i < _allHeroesList.Length; i++)
        {
            HeroScript heroScript = _allHeroesList[i].GetComponent<HeroScript>();
            string prefName = heroScript.GetPrefName();
            int maxHp = heroScript.GetMaxHp();
            PlayerPrefs.SetInt(prefName, PlayerPrefs.GetInt(prefName, maxHp));
        }
    }

    public void SpawnHeroesAtCamp()
    {
        for(int i = 0; i < 4; i++)
        {
            if (_heroList[i] == null)
            {
                continue;
            }

            _slots[i].sprite = _heroList[i].GetComponentInChildren<SpriteRenderer>().sprite;
            _slots[i].color = new Color(1, 1, 1, 1);
        }
    }

    public void AddHeroToCamp(GameObject hero)
    {
        _heroList[_slotIndex] = hero;

        _slots[_slotIndex].sprite = hero.GetComponentInChildren<SpriteRenderer>().sprite;
        _slots[_slotIndex].color = new Color(1, 1, 1, 1);
    }

    public void ContinueJourney()
    {
        HeroManager heroManager = GameObject.Find("Hero Manager").GetComponent<HeroManager>();

        heroManager.SetHeroList();

        if(heroManager.heroList.Length <= 0)
        {
            return;
        }

        _soundManager.PlayMusic(_soundManager.forestSpooky);
        _soundManager.PlaySound(_soundManager.whoosh);
        _animator.SetTrigger("exit camp");
    }

    public void OpenHeroMenu(int index)
    {
        _animator.SetTrigger("slide");

        _slotIndex = index;

        //_changeHeroContainer.SetActive(false);
    }

    public void SelectHero(GameObject hero)
    {
        _animator.SetTrigger("exit slide");

        for(int i = 0; i < 4; i++)
        {
            if (_heroList[i] == null)
            {
                continue;
            }

            if (_heroList[i] != hero)
            {
                continue;
            }

            _heroList[i] = null;
            _slots[i].sprite = null;
            _slots[i].color = new Color(1, 1, 1, 0);
        }

        AddHeroToCamp(hero);
    }

    public void DeleteHero()
    {
    }

    public GameObject GetSelectedHeroAtIndex(int i)
    {
        return _heroList[i];
    }
}
