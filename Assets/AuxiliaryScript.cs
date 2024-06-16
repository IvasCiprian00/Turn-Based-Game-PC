using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AuxiliaryScript : MonoBehaviour
{
    public void LoadMainGame()
    {
        SceneManager.LoadScene("Main Game");
    }

}
