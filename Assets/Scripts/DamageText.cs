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
        gameObject.GetComponent<TextMeshProUGUI>().text = damage.ToString();
    }
}
