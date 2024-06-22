using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private static TutorialManager instance;
    [SerializeField] private GameObject _tutorialOverlay;
    [SerializeField] private GameObject _currentTutorial;

    [Header("Tutorial Windows")]
    [SerializeField] private GameObject _campTutorial;
    [SerializeField] private GameObject _combatBasicsTutorial;

    private string _prefName;
    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        PlayerPrefs.DeleteAll();
    }

    public void CampTutorial()
    {
        if (!VerifyTutorial("CampTutorial"))
        {
            return;
        }

        _campTutorial.SetActive(true);
        _currentTutorial = _campTutorial;
    }

    public void CombatBasicsTutorial()
    {
        if (!VerifyTutorial("CombatBasicsTutorial"))
        {
            return;
        }

        _combatBasicsTutorial.SetActive(true);
        _currentTutorial = _combatBasicsTutorial;
        //Tutorial which introduces the basics of combat : move tiles, hp, damage
        //At the end maybe trigger skillTutorial
    }

    public void AttackTutorial()
    {
        if (!VerifyTutorial("AttackTutorial"))
        {
            return;
        }
        //Tutorial which is triggered at the first attack tile instantiation
    }

    public void SkillsTutorial()
    {
        _prefName = "SkillsTutorial";
        if (PlayerPrefs.HasKey(_prefName))
        {
            return;
        }

        PlayerPrefs.SetInt(_prefName, 1);
        Debug.Log("YEY");
        //Tutorial explaining skills and introduction to status effects
        //Is triggered when a player clicks a skill
    }

    public void StatusEffectsTutorial()
    {
        _prefName = "StatusEffectsTutorial";
        if (PlayerPrefs.HasKey(_prefName))
        {
            return;
        }

        PlayerPrefs.SetInt(_prefName, 1);
        Debug.Log("YEY");
        //Tutorial explaining status effects
        //Triggered when a status effect is first applied
    }

    public void HeroDeathTutorial()
    {
        _prefName = "HeroDeathEffectsTutorial";
        if (PlayerPrefs.HasKey(_prefName))
        {
            return;
        }

        PlayerPrefs.SetInt(_prefName, 1);
        Debug.Log("YEY");

        //Short tutorial triggered when a hero dies
        //It suggests returning to camp to rest
    }

    public bool VerifyTutorial(string name)
    {
        _prefName = name;
        if (PlayerPrefs.HasKey(_prefName))
        {
            return false;
        }

        _tutorialOverlay.SetActive(true);
        PlayerPrefs.SetInt(_prefName, 1);

        return true;
    }

    public void ExitTutorial()
    {
        _currentTutorial.SetActive(false);
        _tutorialOverlay.SetActive(false);
    }
}
