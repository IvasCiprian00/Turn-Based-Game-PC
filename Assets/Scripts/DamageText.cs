using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1.5f);

        Destroy(gameObject);
    }

    public void SetText(int damage)
    {
        if(damage == 0)
        {
            gameObject.GetComponent<TextMeshProUGUI>().text = "MISS";
            gameObject.GetComponent<TextMeshProUGUI>().fontSize = 45;
            return;
        }

        if(damage > 0)
        {
            gameObject.GetComponent<TextMeshProUGUI>().text = "-" + damage.ToString();
            return;
        }

        gameObject.GetComponent<TextMeshProUGUI>().text = "+" + (-damage).ToString();
        gameObject.GetComponent<TextMeshProUGUI>().color = Color.green;
    }
}
