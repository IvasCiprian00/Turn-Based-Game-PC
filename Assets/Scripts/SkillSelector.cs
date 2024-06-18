using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillSelector : MonoBehaviour
{
    private SkillManager _skillManager;
    [SerializeField] private string _prefName;
    [SerializeField] private TextMeshProUGUI _usagesLeft;

    private void Awake()
    {
        _skillManager = GameObject.Find("Skill Manager").GetComponent<SkillManager>();
    }

    public void Update()
    {
        _usagesLeft.text = PlayerPrefs.GetInt(_prefName).ToString();
    }

    public void CastIgnite()
    {
        _skillManager.Ignite();
    }

    public void CastPommelStrike()
    {
        _skillManager.PommelStrike();
    }

    public void CastHealingWord()
    {
        _skillManager.HealingWord();
    }

    public void CastBleed()
    {
        _skillManager.Bleed();
    }

    public void SetPrefName(string name)
    {
        _prefName = name;
    }
}
