using UnityEngine;
using UnityEngine.SceneManagement;

public class AuxiliaryScript : MonoBehaviour
{
    public GameManager gameManager;
    public CampManager campManager;
    public void LoadMainGame()
    {
        SceneManager.LoadScene("Main Game");
    }

    public void SpawnHealAnimation()
    {
        campManager.SpawnHealAnimation();
    }

    public void StartLevel()
    {
        gameManager.PrepareLevel();
    }
}
