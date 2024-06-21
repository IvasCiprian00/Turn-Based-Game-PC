using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private static TutorialManager instance;

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

    public void CampTutorial()
    {
        _prefName = "CampTutorial";
        if (PlayerPrefs.HasKey(_prefName))
        {
            return;
        }

        PlayerPrefs.SetInt(_prefName, 1);
        Debug.Log("YEY");
        //Tutorial which explains how to select heroes, rest them and leave camp
    }

    public void CombatBasicsTutorial()
    {
        _prefName = "CombatBasicsTutorial";
        if (PlayerPrefs.HasKey(_prefName))
        {
            return;
        }

        PlayerPrefs.SetInt(_prefName, 1);
        Debug.Log("YEY");
        //Tutorial which introduces the basics of combat : move tiles, hp, damage
        //At the end maybe trigger skillTutorial
    }

    public void AttackTutorial()
    {
        _prefName = "AttackTutorial";
        if (PlayerPrefs.HasKey(_prefName))
        {
            return;
        }

        PlayerPrefs.SetInt(_prefName, 1);
        Debug.Log("YEY");
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
}
