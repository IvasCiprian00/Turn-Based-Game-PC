using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private int _hp;
    [SerializeField] private int _maxHp;
    [SerializeField] private int _damage;
    [SerializeField] private int _xPos;
    [SerializeField] private int _yPos;

    public void SetCoords(int x, int y)
    {
        _xPos = x;
        _yPos = y;
    }
}
