using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillSelector : MonoBehaviour
{
    private SkillManager _skillManager;
    private TutorialManager _tutorialManager;
    [SerializeField] private string _prefName;
    [SerializeField] private TextMeshProUGUI _usagesLeft;

    private void Awake()
    {
        _skillManager = GameObject.Find("Skill Manager").GetComponent<SkillManager>();
        _tutorialManager = GameObject.Find("Tutorial Manager").GetComponent<TutorialManager>();
    }

    public void Update()
    {
        _usagesLeft.text = PlayerPrefs.GetInt(_prefName).ToString();
    }

    public void CastIgnite()
    {
        _tutorialManager.SkillsTutorial();
        _skillManager.Ignite();
    }

    public void CastPommelStrike()
    {
        _tutorialManager.SkillsTutorial();
        _skillManager.PommelStrike();
    }

    public void CastHealingWord()
    {
        _tutorialManager.SkillsTutorial();
        _skillManager.HealingWord();
    }

    public void CastBleed()
    {
        _tutorialManager.SkillsTutorial();
        _skillManager.Bleed();
    }

    public void SetPrefName(string name)
    {
        _prefName = name;
    }
}
