using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CommandMenu : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Quiting");
        Application.Quit();
    }

    public void Menu()
    {
        Debug.Log("Menu");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
