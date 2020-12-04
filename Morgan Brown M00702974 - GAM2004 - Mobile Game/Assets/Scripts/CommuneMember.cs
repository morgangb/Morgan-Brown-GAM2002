using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildingTheCommune;
using TMPro;

public class CommuneMember : MonoBehaviour
{
    // Name of this commune member
    public string name;
    
    // Task which the member is trying to do
    public Task targetTask = null;

    // Reference to scene communemanager
    private CommuneManager myCommuneManager;

    // Member skill stats
    public int[] abilities = {1, 1, 1, 1};
    public int[] enjoyment = {1, 1, 1, 1};

    // Stat displays
    [SerializeField] private GameObject statsDisplay;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text satisfactionText;
    [SerializeField] private TMP_Text[] abilitiesText;
    [SerializeField] private TMP_Text[] enjoymentText;
    
    // Member satisfaction level (between -100 and 100)
    public float satisfaction = 0;
    
    // Movement spd
    [SerializeField] private float moveSpd = 1f;

    private void Start()
    {
        // Get communemanager
        myCommuneManager = GameObject.FindWithTag("CommuneManager").GetComponent<CommuneManager>();
    }

    private void Update()
    {
        // Find task if there is no current task, otherwise, do current task
        if (targetTask == null)
        {
            // Get task from queue if there is a task in the queue
            if (myCommuneManager.taskQueue.Count > 0) 
            {                
                // Look through task queue for tasks; do the most enjoyable one that's earliest in the queue or the most necessary one
                foreach (Task task in myCommuneManager.taskQueue)
                {
                    // Check that the task has noone assigned to it
                    if (task.member == null) {
                        // Decide what to do based on enjoyment
                        switch (enjoyment[task.skill])
                        {
                            // Do non-enjoyable tasks only if necessary
                            case 0:
                                for (int i = 0; i < myCommuneManager.resourcesNeeded.Length; i++)
                                {
                                    if (myCommuneManager.resourcesNeeded[i] > 0 && task.target.GetComponent<Clickable>().resources[i] > 0)
                                    {
                                        targetTask = task;
                                    }
                                }
                                break;
                            // Do neutral & enjoyable tasks if assigned
                            default:
                                targetTask = task;
                                break;
                        }
                    }

                    // Once a task has been found, break the loop
                    if (targetTask != null)
                    {
                        break;
                    }
                }
            }

            // If targetTask is still null then no task has been found; make own task
            if (targetTask == null)
            {
                // Keep arrays of buildings and extractable resources 
                GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
                GameObject[] extractables = GameObject.FindGameObjectsWithTag("Extractable");

                // Find an enjoyable skill (we do this twice, once for high satisfaction, once for mid)
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < enjoyment.Length; k++)
                    {
                        if (enjoyment[k] == 2 - j)
                        {
                            // Building - find a non-built building and build it
                            if (k == 2 - j)
                            {
                                foreach (GameObject building in buildings)
                                {
                                    if (!building.GetComponent<Building>().built)
                                    {
                                        targetTask = new Task(1, 2, building.GetComponent<Building>().buildDifficulty, building);
                                        break;
                                    }
                                }
                            }
                            // Otherwise just use / extract whatever object
                            else
                            {
                                // Use building if there is a usable building
                                foreach (GameObject building in buildings)
                                {
                                    if (building.GetComponent<Building>().built && building.GetComponent<Building>().skill == k && building.GetComponent<Building>().useDifficulty > 0f && building.GetComponent<Building>().coolDown <= 0f)
                                    {
                                        targetTask = new Task(2, k, building.GetComponent<Building>().useDifficulty, building);
                                        break;
                                    }
                                }
                                
                                // Extract resource if there is an extractable resource and a building is not being used
                                if (targetTask == null)
                                {
                                    foreach (GameObject extractable in extractables)
                                    {
                                        if (extractable.GetComponent<ExtractableResource>().skill == k)
                                        {
                                            targetTask = new Task(0, k, extractable.GetComponent<ExtractableResource>().difficulty, extractable);
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        if (targetTask != null)
                        {
                            break;
                        }
                    }
                }
            }

            if (targetTask != null) 
            {
                // Set targetTask's member to this gameObject so that it knows who's doing it
                targetTask.member = gameObject;
            }
        }
        else
        {
            //Move to task's target if far from task, otherwise, do task
            if (Vector2.Distance(targetTask.target.transform.position, transform.position) > 1f) 
            {
                transform.Translate((targetTask.target.transform.position - transform.position).normalized * moveSpd * Time.deltaTime);
            }
            else 
            {
                // Do task
                targetTask.difficulty -= (1f + abilities[targetTask.skill]) * 0.5f * Time.deltaTime;

                // Change satisfaction
                satisfaction += (enjoyment[targetTask.skill] - 1f) * Time.deltaTime;

                // If task is less than 0, it's done
                if (targetTask.difficulty <= 0f)
                {
                    targetTask.Complete();
                    myCommuneManager.RemoveTask(targetTask);
                    targetTask = null;
                }
            }
        }

        // Set stat texts
        nameText.text = name;
        satisfactionText.text = Mathf.Round(satisfaction).ToString();
        
        for (int i = 0; i < enjoymentText.Length; i++)
        {
            enjoymentText[i].text = enjoyment[i].ToString();
        }

        for (int j = 0; j < abilitiesText.Length; j++)
        {
            abilitiesText[j].text = abilities[j].ToString();
        }
    }

    public void Clicked()
    {
        // On clicked toggle stats
        statsDisplay.SetActive(!statsDisplay.active);
    }
}
