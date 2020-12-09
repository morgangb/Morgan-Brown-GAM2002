using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BuildingTheCommune;
using UnityEngine.SceneManagement;

// A class for managing commune functions and variables needed by all controllers
public class CommuneManager : MonoBehaviour
{
    // Store arrays of names
    public string[] names = {
        "Judith",
        "Slavoj",
        "Ernesto",
        "Noam",
        "Cornel",
        "Tavis",
        "Mohandas",
        "Emma",
        "Abbie",
        "Peter",
        "Karl",
        "Jean",
        "Paul",
        "Leo",
        "Fred",
        "Rosa",
        "Angela"
    };

    // Number of resources
    public const int RESOURCESNUM = 5;

    // Resources variables
    public int[] resourcesCount = new int[RESOURCESNUM];

    // An array to store the amount of resources needed based on the current population and taskQueue
    public int[] resourcesNeeded = new int[RESOURCESNUM];
    // An array to store the maximum number of each resource
    public int[] resourcesMax = new int[RESOURCESNUM];
    // An array to store the amount of resources consumed each night
    public int[] resourcesConsumed = new int[RESOURCESNUM];

    // "Readouts" (basically counts of how many of a thing there is)
    [Header("Readouts")]
    private int beds = 0;
    public float time = 0;
    public int days = 0;
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private TMP_Text messageTxt;
    
    // UI Variables
    [Header("Resources UI")]
    [SerializeField] private TMP_Text[] resourcesCountTxt = new TMP_Text[4];

    [Header("Readouts")]
    [SerializeField] private TMP_Text bedTxt;
    [SerializeField] private TMP_Text timeTxt;
    [SerializeField] private TMP_Text peopleTxt;

    [Header("Buttons")]
    [SerializeField] private ToggleButton mute;
    [SerializeField] private ToggleButton paused;

    // Task queue, keeps a list of tasks
    public List<Task> taskQueue = new List<Task>();

    // List of Commune Members
    public GameObject[] communeMembers;

    // Commune member prefab
    [SerializeField] private GameObject communeMember;
    
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
        NewMember();

        // Get a list of commune members at start of game
        communeMembers = GameObject.FindGameObjectsWithTag("CommuneMember");
    }

    private void Update()
    {
        // Re-get communeMembers
        communeMembers = GameObject.FindGameObjectsWithTag("CommuneMember");

        // Go to gameover if necessary
        if (communeMembers.Length <= 0)
        {
            SceneManager.LoadScene(2);
        }

        // Pause game if the paused button is on
        if (paused.isOn)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        // Mute game with mute button
        if (mute.isOn)
        {
            AudioListener.volume = 0f;
        }
        else 
        {
            AudioListener.volume = 1f;
        }

        // Increase time by time so that 8 in-game hours = 1 real-world minute
        time += Time.deltaTime / 7.5f;

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
            // Inform player
            DisplayMessage("Day " + days.ToString() + " complete!");

            // Reduce conusmables and needs for each communemember
            int tempBeds = beds;

            // Overall satisfaction; an average of the satisfaction of all members
            float overallSatisfaction = 0f;

            for (int i = 0; i < communeMembers.Length; i++)
            {
                // Bool for if the member's needs have been met today
                bool sated = true;

                // Reduce beds or cause dissatisfaction
                if (tempBeds > 0)
                {
                    tempBeds -= 1;
                }
                else
                {
                    communeMembers[i].GetComponent<CommuneMember>().satisfaction -= 25f;
                    sated = false;
                }

                // Reduce consumables or cause dissatisfaction
                for (int j = 0; j < resourcesCount.Length; j++)
                {
                    if (resourcesCount[j] > resourcesConsumed[j])
                    {
                        resourcesCount[j] -= resourcesConsumed[j];
                    }
                    else
                    {
                        communeMembers[i].GetComponent<CommuneMember>().satisfaction -= 25f;
                        sated = false;
                    }
                }

                // Add to overallsatisfaction the satisfaction of this members
                overallSatisfaction += communeMembers[i].GetComponent<CommuneMember>().satisfaction;

                // Add today as unsated to the member
                communeMembers[i].GetComponent<CommuneMember>().daysUnsated += 1;
            }

            // Find average satisfaction by dividing by total number of members
            overallSatisfaction = overallSatisfaction / communeMembers.Length;

            // Add a new member if satisfaction is high
            if (overallSatisfaction > 50f)
            {
                NewMember();
            }

            // Reset time to morning on next day
            days += 1;
            time = 0;
        }

        // Generate timeTxt from time
        timeTxt.text = (Mathf.Floor(time) + 6).ToString("00") + ":00";

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

        // Set the maximum number of resources to be held
        for (int i = 0; i < resourcesMax.Length; i++)
        {
            resourcesMax[i] = communeMembers.Length * 7;
        }
    }

    public bool AddTask (Task task)
    {
        // Set task not added as default
        bool taskAdded = false;

        // Check the taskqueue does not contain the task referenced
        if (!taskQueue.Contains(task))
        {
            // Add a task to the task queue with parameters given
            taskQueue.Add(task);
            
            // Task has been added successfully
            taskAdded = true;
        }

        return taskAdded;
    }

    public bool RemoveTask (Task task)
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
                    communeMember.GetComponent<CommuneMember>().targetTaskIndex = -1;
                    communeMember.GetComponent<CommuneMember>().targetTask = null;
                }
            }

            // Mark that task has been removed
            taskRemoved = true;
        }

        // Return whether or not the task has been removed successfully
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

    public void DisplayMessage (string message)
    {
        // Pause game
        paused.isOn = true;
        // Activate message panel
        messagePanel.SetActive(true);
        // Set message panel text
        messageTxt.text = message;
    }

    public void CloseMessage ()
    {
        // Resume game
        paused.isOn = false;
        // Close message panel
        messagePanel.SetActive(false);
    }

    public void NewMember ()
    {
        // Creates a new commune member
        GameObject newMember = Instantiate(communeMember);

        // Set name randomly
        newMember.GetComponent<CommuneMember>().name = names[(int)Mathf.Round(UnityEngine.Random.Range(0f, names.Length - 1f))];

        // Set abilities and enjoyment randomly
        newMember.GetComponent<CommuneMember>().abilities = SetSkills(newMember.GetComponent<CommuneMember>().abilities);
        newMember.GetComponent<CommuneMember>().enjoyment = SetSkills(newMember.GetComponent<CommuneMember>().enjoyment);

        // Re-get communeMembers
        communeMembers = GameObject.FindGameObjectsWithTag("CommuneMember");
    }

    public void RemoveMember (GameObject member)
    {
        // Destroy the passed member
        Destroy(member);
    }

    public int[] SetSkills (int[] skills)
    {
        // Set points to the length of skills
        int points = skills.Length;

        // While not all points have been spent
        while (points > 0)
        {
            // Loop through all skills in array
            for (int i = 0; i < skills.Length; i++)
            {
                // 50/50 chance to spend point
                if (UnityEngine.Random.Range(0f, 1f) > 0.5f && skills[i] < 2)
                {
                    skills[i] += 1;
                    points -= 1;
                }

                // Break out of loop if all points are spent
                if (points <= 0)
                {
                    break;
                }
            }
        }

        // Return skills now points have been spent
        return skills;
    }
}
