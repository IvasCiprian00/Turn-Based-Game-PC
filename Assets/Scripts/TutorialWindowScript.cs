using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWindowScript : MonoBehaviour
{
    [SerializeField] private GameObject[] _pages;
    [SerializeField] private GameObject _leftArrow;
    [SerializeField] private GameObject _rightArrow;
    private int _pageIndex;

    public void Start()
    {
        _pages[0].SetActive(true);
        if(_pages.Length > 1)
        {
            _rightArrow.SetActive(true);
        }
    }

    public void ChangePage(int direction)
    {
        _leftArrow.SetActive(true);
        _rightArrow.SetActive(true);

        _pages[_pageIndex].SetActive(false);
        _pageIndex += direction;
        _pages[_pageIndex].SetActive(true);

        if(_pageIndex == 0)
        {
            _leftArrow.SetActive(false);
        }

        if(_pageIndex == _pages.Length - 1)
        {
            _rightArrow.SetActive(false);
        }
    }
}
