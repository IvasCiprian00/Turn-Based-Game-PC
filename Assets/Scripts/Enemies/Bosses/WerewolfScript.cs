using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WerewolfScript : Enemy
{
    [Serializable]
    public struct HpThreshold
    {
        public int speed;
        public int lowerDamage;
        public int upperDamage;
        public int attackCount;
    }

    [SerializeField] HpThreshold[] _hpThreshold;

    override
    public void StartTurn()
    {
        StartCoroutine(TakeTurn());
    }

    public IEnumerator TakeTurn()
    {
        TickStatusEffects();

        if (_stunned)
        {
            EndTurn();
            yield break;
        }

        StartCoroutine(AttackAndMove());

        EndTurn();
    }

    public override void TakeUndodgeableDamage(int damage)
    {
        _hp -= damage;
        _uiManager.DisplayDamage(gameObject, damage);

        UpdateHealthbar();

        if (_hp <= 0)
        {
            _enemyManager.EnemyDeath(gameObject);
            StopAllCoroutines();
            //Destroy(gameObject);
            _gameManager.TriggerCutscene("Werewolf Cutscene");
        }

        if (_animator != null)
        {
            _animator.SetTrigger("take_damage");
        }
    }

    public override void TakeDamage(int damage)
    {
        int chance = UnityEngine.Random.Range(1, 100);

        if (chance <= _evasion)
        {
            damage = 0;
        }

        _hp -= damage;
        _uiManager.DisplayDamage(gameObject, damage);

        UpdateHealthbar();

        if (_hp <= 0)
        {
            _enemyManager.EnemyDeath(gameObject);
            StopAllCoroutines();
            //Destroy(gameObject);
            _gameManager.TriggerCutscene("Werewolf Cutscene");
        }

        if (_animator != null)
        {
            _animator.SetTrigger("take_damage");
        }
        
        if(_hp <= 0.3 * _maxHp)
        {
            ChangeThreshold(1);
            return;
        }

        if(_hp <= 0.7 * _maxHp)
        {
            ChangeThreshold(0);
        }
    }

    public void ChangeThreshold(int x)
    {
        _speed = _hpThreshold[x].speed;
        _lowerDamage = _hpThreshold[x].lowerDamage;
        _upperDamage = _hpThreshold[x].upperDamage;
        _attackCount = _hpThreshold[x].attackCount;
    }
}
