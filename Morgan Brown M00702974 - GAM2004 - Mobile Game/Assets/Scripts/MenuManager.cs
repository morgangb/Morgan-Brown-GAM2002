using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void GoToScene(int SceneTo)
    {
        // Load a particular scene
        SceneManager.LoadScene(SceneTo);
    }

    public void Exit()
    {
        // Exit application
        Application.Quit();
    }
}
