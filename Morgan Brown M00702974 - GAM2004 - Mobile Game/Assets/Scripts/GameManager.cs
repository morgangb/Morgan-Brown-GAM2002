using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Make the GameManager a Singleton
    // Find any other instances of GameManager
    private static GameManager _instance;

    public static GameManager Instance 
    {
        get 
        {
            return _instance; 
        } 
    }

    private void Awake()
    {
        // Destroy this GameManager of another exists
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } 
        else 
        {
            _instance = this;
        }

        // Don't destroy this go when loading a scene
        DontDestroyOnLoad(gameObject);
    }

    public void NewGame()
    {
        // Load the first scene
        SceneManager.LoadScene(1);
    }
}
