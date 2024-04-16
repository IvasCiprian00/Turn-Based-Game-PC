using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarScript : MonoBehaviour
{
    private int _hp;
    private int _maxHp;

    public Slider slider;
    public Image fill;

    public float redColor;
    public float greenColor;

    public void Update()
    {
        slider.value = (float)_hp / _maxHp;

        SetHpColor();
    }

    public void SetHpColor()
    {
        float hpPercent = (float)_hp / _maxHp;

        if (hpPercent <= 0.5)
        {
            redColor = 1 - hpPercent + 0.3f;
            greenColor = 1 * hpPercent;
        }
        else
        {
            redColor = 1 - hpPercent + 0.3f;
            greenColor = 1 * hpPercent + 0.1f;
        }
        fill.color = new Color(redColor, greenColor, 0, 255);
    }

    public void SetHp(int hp)
    {
        _hp = hp;
    }

    public void SetMaxHp(int maxHp)
    {
        _maxHp = maxHp;
    }
}
