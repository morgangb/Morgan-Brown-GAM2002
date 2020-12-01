using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BuildingTheCommune;

// A class for managing commune functions and variables needed by all controllers
public class CommuneManager : MonoBehaviour
{
    // Store arrays of names
    public string[] firstNames = {
        
    };

    public string[] surNames = {

    };

    // Resources variables
    public int[] resourcesCount = new int[4];

    // An array to store the amount of resources needed based on the current population and taskQueue
    public int[] resourcesNeeded = new int[4];

    // "Readouts" (basically counts of how many of a thing there is)
    [Header("Readouts")]
    private int beds = 0;
    public float time = 0;
    public int days = 0;
    
    // UI Variables
    [Header("Resources UI")]
    [SerializeField] private TMP_Text[] resourcesCountTxt = new TMP_Text[4];

    [Header("Readouts")]
    [SerializeField] private TMP_Text bedTxt;
    [SerializeField] private TMP_Text timeTxt;
    [SerializeField] private TMP_Text peopleTxt;

    // Task queue, keeps a list of tasks
    public List<Task> taskQueue = new List<Task>();

    // List of Commune Members
    public GameObject[] communeMembers;
    
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

    private void Start()
    {
        // Get a list of commune members at start of game
        communeMembers = GameObject.FindGameObjectsWithTag("CommuneMember");
    }

    private void Update()
    {
        // Increase time by time so that 1 in-game hour = 1 minute
        time += Time.deltaTime / 60;

        // Set resourcesCountTxt to resourcesCount
        for (int i = 0; i < resourcesCount.Length; i++)
        {
            resourcesCountTxt[i].text = resourcesCount[i].ToString();
        }

        // Set beds to the number of beds in the scene
        beds = 0;
        var buildings = FindObjectsOfType<Building>();
        foreach (Building building in buildings)
        {
            if (building.built)
            {
                beds += building.bedsCount;
            }
        }
        
        // Set beds text to beds
        bedTxt.text = beds.ToString();

        // Set peopleTxt to number of communeMembers
        peopleTxt.text = communeMembers.Length.ToString();

        // Go to next day
        if (time >= 16)
        {
            days += 1;
            time = 0;
        }

        // Generate timeTxt from time
        var hour = Mathf.Floor(time) + 6;
        timeTxt.text = hour.ToString("00") + ":" + Mathf.Floor((time - hour) * 60f).ToString("00");

        // Set the resources needed based on commune members and tasks
        // Reset resources needed
        Array.Clear(resourcesNeeded, 0, resourcesNeeded.Length);

        // Set the consumable resources to the number of commune members, since these will be consumed daily
        resourcesNeeded[0] = resourcesNeeded[1] = communeMembers.Length;

        // Set the building resources to the resources needed by the tasks in the task queue
        foreach (Task task in taskQueue)
        {
            // If the task's skill is building
            if (task.skill == 2) 
            {
                // Loop through the build resources of the task's target and add them to the resourcesNeeded array
                for (int i = 0; i < task.target.GetComponent<Building>().buildResources.Length; i++)
                {
                    resourcesNeeded[i] += task.target.GetComponent<Building>().buildResources[i];
                }
            }
        }

        // Subtract currently held resources from resources needed
        for (int i = 0; i < resourcesNeeded.Length; i++)
        {
            resourcesNeeded[i] -= resourcesCount[i];
        }
    }

    public void AddTask(Task task)
    {
        // Add a task to the task queue with parameters given
        taskQueue.Add(task);
    }

    public bool RemoveTask(Task task)
    {
        // Set task not removed as default
        bool taskRemoved = false;
        // Check for task in taskqueue, if found delete it and return true, else return false
        while (taskQueue.Remove(task))
        {
            // Toggle off task marker
            task.target.GetComponentInChildren<Clickable>().taskMarker.SetActive(false);
            // Remove the task for the communeMember so they're not trying to do a deleted task
            foreach (GameObject communeMember in communeMembers)
            {
                if (communeMember.GetComponent<CommuneMember>().targetTask.target == task.target)
                {
                    communeMember.GetComponent<CommuneMember>().targetTask = null;
                }
            }

            // Mark that task has been removed
            taskRemoved = true;
        }
        return taskRemoved;
    }

    public void ChangeResources (int[] newResources, bool remove)
    {
        // Add to resources the newResources or remove them
        for (int i = 0; i < newResources.Length; i++)
        {
            if (remove)
            {
                resourcesCount[i] -= newResources[i];
            }
            else
            {
                resourcesCount[i] += newResources[i];
            }
        }
    }

    public bool CheckResources (int[] checkResources)
    {
        // Check if the player has less than the required resources
        for (int i = 0; i < checkResources.Length; i++)
        {
            if (resourcesCount[i] < checkResources[i])
            {
                return false;
            }
        }

        // Return true otherwise
        return true;
    }
}
