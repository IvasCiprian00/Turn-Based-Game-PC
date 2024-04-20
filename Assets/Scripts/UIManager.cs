using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TurnManager _turnManager;
    [SerializeField] private GameObject _endTurnButton;
    [SerializeField] private TextMeshProUGUI _hpValue;
    [SerializeField] private TextMeshProUGUI _damageValue;
    [SerializeField] private GameObject _statsContainer;
    [SerializeField] private GameObject _nextLevelButton;
    [SerializeField] private GameObject _restartLevelButton;
    [SerializeField] private GameObject _damageDealt;

    public void Awake()
    {
        _turnManager = GameObject.Find("Turn Manager").GetComponent<TurnManager>();
    }

    public void Update()
    {
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
    }

    public void DisplayDamage(GameObject character, int damage)
    {
        GameObject reference = Instantiate(_damageDealt, character.transform.position, Quaternion.identity, gameObject.transform);

        float rot = Random.Range(-30f, 30f);

        reference.transform.Rotate(0, 0, rot);
        reference.GetComponent<DamageText>().SetText(damage);
    }
}
