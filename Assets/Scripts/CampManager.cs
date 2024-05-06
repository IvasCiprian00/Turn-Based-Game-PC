using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _heroSlots;
    [SerializeField] private GameObject[] _selectedHeroes;
    [SerializeField] private int _heroIndex;
    [SerializeField] private GameObject _changeHeroContainer;

    public void OpenHeroMenu(int index)
    {
        GameObject.Find("Canvas").GetComponent<Animator>().SetTrigger("slide");

        _heroIndex = index;

        //_changeHeroContainer.SetActive(false);
    }

    public void SelectHero(GameObject hero)
    {
        GameObject.Find("Canvas").GetComponent<Animator>().SetTrigger("exit slide");

        GameObject reference = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        reference.GetComponent<Button>().enabled = false;
        reference.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;

        string slot = "Slot " + (_heroIndex + 1);

        if (_heroSlots[_heroIndex] != null)
        {
            GameObject menuReference = GameObject.Find(_selectedHeroes[_heroIndex].name);
            menuReference.GetComponent<Button>().enabled = true;
            menuReference.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
        }

        _selectedHeroes[_heroIndex] = reference;

        Destroy(_heroSlots[_heroIndex]);
        _heroSlots[_heroIndex] = Instantiate(hero, GameObject.Find(slot).transform.position, Quaternion.identity, GameObject.Find("Buttons").transform);

        _changeHeroContainer.SetActive(true);
    }

    public void DeleteHero()
    {
        GameObject.Find("Canvas").GetComponent<Animator>().SetTrigger("exit slide");

        if (_heroSlots[_heroIndex] != null)
        {
            GameObject menuReference = GameObject.Find(_selectedHeroes[_heroIndex].name);
            menuReference.GetComponent<Button>().enabled = true;
            menuReference.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
        }

        Destroy(_heroSlots[_heroIndex]);
        _selectedHeroes[_heroIndex] = null;
    }
}
