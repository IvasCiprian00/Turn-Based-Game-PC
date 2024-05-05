using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    [SerializeField] private int _evasionAmount;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Hero")
        {
            HeroScript heroScript = collision.GetComponent<HeroScript>();

            heroScript.SetEvasion(heroScript.GetEvasion() + _evasionAmount);

            return;
        }

        if(collision.tag == "Enemy")
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            enemy.SetEvasion(enemy.GetEvasion() + _evasionAmount);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Hero")
        {
            HeroScript heroScript = collision.GetComponent<HeroScript>();

            heroScript.SetEvasion(heroScript.GetEvasion() - _evasionAmount);

            return;
        }

        if (collision.tag == "Enemy")
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            enemy.SetEvasion(enemy.GetEvasion() - _evasionAmount);
        }
    }
}
