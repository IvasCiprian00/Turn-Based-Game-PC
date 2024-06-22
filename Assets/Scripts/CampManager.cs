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
    private TutorialManager _tutorialManager;
    [SerializeField] private GameObject[] _allHeroesList;
    [SerializeField] private GameObject[] _heroList;
    [SerializeField] private Image[] _slots;
    [SerializeField] private int _slotIndex;
    [SerializeField] private GameObject _changeHeroContainer;
    [SerializeField] private Animator _animator;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private GameObject _continueGameTrigger;
    [SerializeField] private TextMeshProUGUI _daysPassedText;
    [SerializeField] private GameObject _healAnimation;

    const string _daysPassed = "Days_Passed";

    private float _timer;

    public void Awake()
    {
        _animator = GameObject.Find("Canvas").GetComponent<Animator>();
        _tutorialManager = GameObject.Find("Tutorial Manager").GetComponent<TutorialManager>();
    }

    public void Start()
    {
        PlayerPrefs.GetInt(_daysPassed, 0);
        SetPlayerPrefs();
        _tutorialManager.CombatBasicsTutorial();

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

    public void SetPlayerPrefs()
    {
        for (int i = 0; i < _allHeroesList.Length; i++)
        {
            _allHeroesList[i].GetComponent<HeroScript>().SetPlayerPrefs();
        }
    }

    public void SpawnHeroesAtCamp()
    {
        for(int i = 0; i < _heroList.Length; i++)
        {
            if (_heroList[i] == null)
            {
                continue;
            }

            _slotIndex = i;
            AddHeroToCamp(_heroList[i]);
        }
    }

    public void AddHeroToCamp(GameObject hero)
    {
        _heroList[_slotIndex] = hero;

        _slots[_slotIndex].sprite = hero.GetComponent<HeroScript>().GetCampPosition(_slotIndex);
        _slots[_slotIndex].color = new Color(1, 1, 1, 1);
    }

    public void ContinueJourney()
    {
        HeroManager heroManager = GameObject.Find("Hero Manager").GetComponent<HeroManager>();

        heroManager.SetHeroList();

        if(heroManager.GetHeroCount() <= 0)
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
        _animator.SetTrigger("exit slide");

        _heroList[_slotIndex] = null;
        _slots[_slotIndex].sprite = null;
        _slots[_slotIndex].color = new Color(1, 1, 1, 0);
    }

    public void RestHeroes()
    {
        _animator.SetTrigger("rest");
        _soundManager.PlaySound(_soundManager.sleep);

        int x = (PlayerPrefs.GetInt(_daysPassed)) + 1;
        PlayerPrefs.SetInt(_daysPassed, x);
        _daysPassedText.text = x + " days passed";

        for(int i = 0; i < _allHeroesList.Length; i++)
        {
            _allHeroesList[i].GetComponent<HeroScript>().ResetPlayerPrefs();
        }
    }

    public void SpawnHealAnimation()
    {
        _soundManager.PlaySound(_soundManager.heal);
        for(int i = 0; i < 4; i++)
        {
            Instantiate(_healAnimation, _slots[i].transform.position, Quaternion.identity);
        }
    }

    public GameObject GetSelectedHeroAtIndex(int i)
    {
        return _heroList[i];
    }
}
