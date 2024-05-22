using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CampManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _allHeroesList;
    [SerializeField] private GameObject[] _heroSlots;
    [SerializeField] private GameObject[] _selectedHeroes;
    [SerializeField] private int _heroIndex;
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

        if(heroManager.heroList == null)
        {
            return;
        }

        heroManager.GetHeroes(_selectedHeroes);

        SpawnHeroesAtCamp();
    }

    public void Update()
    {
        if(_continueGameTrigger.activeSelf == true)
        {
            SceneManager.LoadScene("Main Game");
        }

        _timer += Time.deltaTime;

        if(_timer <= 5)
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
            _heroIndex = i;

            if (_selectedHeroes[i] == null)
            {
                continue;
            }

            AddHeroToCamp(_selectedHeroes[i]);
        }
    }

    public void AddHeroToCamp(GameObject hero)
    {
        string slot = "Slot " + (_heroIndex + 1);

        GameObject reference = GameObject.Find(hero.name + " Icon");
        reference.GetComponent<Button>().enabled = false;
        reference.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;

        _heroSlots[_heroIndex] = Instantiate(reference, GameObject.Find(slot).transform.position, Quaternion.identity, GameObject.Find("Buttons").transform);
    }

    public void ContinueJourney()
    {
        HeroManager heroManager = GameObject.Find("Hero Manager").GetComponent<HeroManager>();

        heroManager.SetHeroList();

        if(heroManager.heroList.Length <= 0)
        {
            return;
        }

        _animator.SetTrigger("exit camp");
    }

    public void OpenHeroMenu(int index)
    {
        _animator.SetTrigger("slide");

        _heroIndex = index;

        //_changeHeroContainer.SetActive(false);
    }

    public void SelectHero(GameObject hero)
    {
        _animator.SetTrigger("exit slide");

        GameObject reference = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        reference.GetComponent<Button>().enabled = false;
        reference.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;

        string slot = "Slot " + (_heroIndex + 1);

        if (_heroSlots[_heroIndex] != null)
        {
            GameObject menuReference = GameObject.Find(_selectedHeroes[_heroIndex].name);
            menuReference.GetComponent<Button>().enabled = true;
            menuReference.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
        }

        _selectedHeroes[_heroIndex] = hero;

        Destroy(_heroSlots[_heroIndex]);
        _heroSlots[_heroIndex] = Instantiate(reference, GameObject.Find(slot).transform.position, Quaternion.identity, GameObject.Find("Buttons").transform);

        _changeHeroContainer.SetActive(true);
    }

    public void DeleteHero()
    {
        GameObject.Find("Canvas").GetComponent<Animator>().SetTrigger("exit slide");

        if (_heroSlots[_heroIndex] != null)
        {
            GameObject menuReference = GameObject.Find(_selectedHeroes[_heroIndex].name);
            menuReference.GetComponent<Button>().enabled = true;
            menuReference.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
        }

        Destroy(_heroSlots[_heroIndex]);
        _selectedHeroes[_heroIndex] = null;
    }

    public GameObject GetSelectedHeroAtIndex(int i)
    {
        return _selectedHeroes[i];
    }
}
