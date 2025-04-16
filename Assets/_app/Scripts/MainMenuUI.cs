using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{

    public void PlayGame()
    {
        SceneManager.LoadScene("Main"); // Replace "GameScene" with your actual scene name
    }

}
