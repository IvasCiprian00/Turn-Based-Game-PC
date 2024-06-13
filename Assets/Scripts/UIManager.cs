using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameManager _gameManager;
    private TurnManager _turnManager;
    private SkillManager _skillManager;
    private SoundManager _soundManager;

    [SerializeField] private Transform[] _skillSlots;

    [Header("Buttons")]
    [SerializeField] private GameObject _endTurnButton;
    [SerializeField] private GameObject _nextLevelButton;
    [SerializeField] private GameObject _restartLevelButton;
    [SerializeField] private GameObject _goToCampButton;
    [SerializeField] private GameObject _cancelSkillButton;

    [Header("Hero stats")]
    [SerializeField] private GameObject _statsContainer;
    [SerializeField] private TextMeshProUGUI _hpValue;
    [SerializeField] private TextMeshProUGUI _damageValue;


    [SerializeField] private GameObject _damageDealt;

    public void Awake()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _gameManager.SetManager(ref _turnManager);
        _gameManager.SetManager(ref _skillManager);
        _gameManager.SetManager(ref _soundManager);
    }

    public void Update()
    {
        if(!_gameManager.GetLevelLoaded())
        {
            return;
        }

        int hp = _turnManager.GetCurrentHeroHp();
        int damage = _turnManager.GetCurrentHeroDamage();

        _hpValue.text = hp.ToString();
        _damageValue.text = damage.ToString();
    }

    public void DisplayUI(bool condition)
    {
        _endTurnButton.SetActive(condition);
        _statsContainer.SetActive(condition);
    }

    public void DisplayEndOfLevelButtons(bool heroesWon)
    {
        _restartLevelButton.SetActive(true);
        _goToCampButton.SetActive(true);

        if (heroesWon)
        {
            _nextLevelButton.SetActive(true);
            return;
        }
    }

    public void HideEndOfLevelButtons()
    {
        _nextLevelButton.SetActive(false);
        _restartLevelButton.SetActive(false);
        _goToCampButton.SetActive(false);
    }

    public void DisplayDamage(GameObject character, int damage)
    {
        GameObject reference = Instantiate(_damageDealt, character.transform.position, Quaternion.identity, gameObject.transform);

        float rot = Random.Range(-30f, 30f);

        reference.transform.Rotate(0, 0, rot);
        reference.GetComponent<DamageText>().SetText(damage);
    }

    public void DisplayCancelSkill(bool cond)
    {
        _cancelSkillButton.SetActive(cond);
    }

    public void DisplaySkills(GameObject[] skills)
    {
        DestroySkills();

        GameObject reference;

        for (int i = 0; i < skills.Length; i++)
        {
            reference = Instantiate(skills[i], _skillSlots[i].position, Quaternion.identity, gameObject.transform);

            string skillName = GetSkillName(reference.name);

            reference.GetComponent<Button>().onClick.AddListener(() => _skillManager.Invoke(skillName, 0));
        }
    }

    public void DestroySkills()
    {
        _cancelSkillButton.SetActive(false);

        foreach (GameObject skill in GameObject.FindGameObjectsWithTag("Skill Button") )
        {
            Destroy(skill);
        }

        _skillManager.CancelSkill();
    }

    public string GetSkillName(string s)
    {
        return s.Substring(0, s.Length - 7);
    }
}
