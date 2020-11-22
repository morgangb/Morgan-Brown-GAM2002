using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BuildingTheCommune;

// A class for managing commune functions and variables needed by all controllers
public class CommuneManager : MonoBehaviour
{
    // Needs variables
    public int[] consumableNeeds = new int[2];

    // Resources variables
    public int[] resourcesCount = new int[2];

    // "Readouts" (basically counts of how many of a thing there is)
    [Header("Readouts")]
    private int beds = 0;
    public int time = 0;
    private int people = 0;
    
    // UI variables
    [Header("Consumable Needs UI")]
    [SerializeField] private TMP_Text[] consumableNeedsTxt = new TMP_Text[2];

    [Header("Resources UI")]
    [SerializeField] private TMP_Text[] resourcesCountTxt = new TMP_Text[2];

    [Header("Readouts")]
    [SerializeField] private TMP_Text bedTxt;
    [SerializeField] private TMP_Text timeTxt;
    [SerializeField] private TMP_Text peopleTxt;

    // Task queue, keeps a list of tasks
    public List<Task> taskQueue = new List<Task>();
    
    // Make the CommuneManager a Singleton
    // Find any other instances of CommuneManager
    private static CommuneManager _instance;

    public static CommuneManager Instance 
    {
        get 
        {
            return _instance; 
        } 
    }

    private void Awake()
    {
        // Destroy this CommuneManager of another exists
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } 
        else 
        {
            _instance = this;
        }
    }

    private void Update()
    {
        // Set consumableNeedsTxt to consumableNeeds
        for (int i = 0; i < consumableNeeds.Length; i++)
        {
            consumableNeedsTxt[i].text = consumableNeeds[i].ToString();
        }

        // Set resourcesCountTxt to resourcesCount
        for (int i = 0; i < resourcesCount.Length; i++)
        {
            resourcesCountTxt[i].text = resourcesCount[i].ToString();
        }
    }

    public void AddTask(Task task)
    {
        // Add a task to the task queue with parameters given
        taskQueue.Add(task);
    }

    public bool RemoveTask(Task task)
    {
        // Check for task in taskqueue, if found delete it and return true, else return false
        if (taskQueue.Remove(task))
        {
            return true;
        }
        else {
            return false;
        }
    }
}
