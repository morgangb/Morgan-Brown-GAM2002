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
    public int targetTaskIndex = -1;

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

    // Days unsated (number of days a need has not been met)
    public int daysUnsated = 0;
    
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
        if (targetTaskIndex == -1)
        {
            // Get task from queue if there is a task in the queue
            if (myCommuneManager.taskQueue.Count > 0) 
            {
                // Look through task queue for tasks; do the most enjoyable one that's earliest in the queue or the most necessary one
                for (int i = 0; i < myCommuneManager.taskQueue.Count; i++)
                {
                    // Check that the task has noone assigned to it
                    if (myCommuneManager.taskQueue[i].member == null) {
                        // Decide what to do based on enjoyment
                        switch (enjoyment[myCommuneManager.taskQueue[i].skill])
                        {
                            // Do non-enjoyable tasks only if necessary
                            case 0:
                                for (int j = 0; j < myCommuneManager.resourcesNeeded.Length; j++)
                                {
                                    if (myCommuneManager.resourcesNeeded[j] > 0 && myCommuneManager.taskQueue[i].target.GetComponent<Clickable>().resources[j] > 0)
                                    {
                                        targetTaskIndex = i;
                                    }
                                }
                                break;
                            // Do neutral & enjoyable tasks if assigned
                            default:
                                targetTaskIndex = i;
                                break;
                        }
                    }

                    // Task has been found; break out of this loop
                    if (targetTaskIndex != -1)
                    {
                        targetTask = myCommuneManager.taskQueue[targetTaskIndex];
                        break;
                    }
                }
            }

            // If targetTask is still -1 then no task has been found; make own task
            if (targetTaskIndex == -1)
            {
                // Keep arrays of buildings and extractable resources 
                GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
                GameObject[] extractables = GameObject.FindGameObjectsWithTag("Extractable");

                // Find an enjoyable skill (we do this twice, once for high satisfaction, once for mid)
                for (int i = 0; i < 2; i++)
                {
                    // Loop through skills' enjoyments
                    for (int j = 0; j < enjoyment.Length; j++)
                    {
                        // If the enjoyment of the focused skill corresponds to the focused enjoyment
                        if (enjoyment[j] == 2 - i)
                        {
                            // Find a non-built building and build that building if the focused skill is building
                            if (j == 2)
                            {
                                // Loop through buildings
                                foreach (GameObject building in buildings)
                                {
                                    // The building hasn't been built
                                    if (!building.GetComponent<Building>().built)
                                    {
                                        // Try to make a task, if successful, break out of loop (since a task has been found, there's no need to continue)
                                        if (MakeTask(1, 2, building.GetComponent<Building>().buildDifficulty, building))
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // Use building if there is a usable building
                                foreach (GameObject building in buildings)
                                {
                                    if (building.GetComponent<Building>().built && building.GetComponent<Building>().skill == j && building.GetComponent<Building>().useDifficulty > 0f && building.GetComponent<Building>().coolDown <= 0f && myCommuneManager.CheckResources(building.GetComponent<Building>().buildResources))
                                    {
                                        // Try to make a task, if successful, break out of loop (since a task has been found, there's no need to continue)
                                        if (MakeTask(2, j, building.GetComponent<Building>().useDifficulty, building))
                                        {
                                            break;
                                        }
                                    }
                                }
                                
                                // Extract resource if there is an extractable resource and a building is not being used
                                if (targetTaskIndex == -1)
                                {
                                    // Loop through extractables
                                    foreach (GameObject extractable in extractables)
                                    {
                                        // If skill to extract matches focused skill
                                        if (extractable.GetComponent<ExtractableResource>().skill == j)
                                        {
                                            // Try to make a task, if successful, break out of loop (since a task has been found, there's no need to continue)
                                            if (MakeTask(0, j, extractable.GetComponent<ExtractableResource>().difficulty, extractable))
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // Break out of loop if a target task has been made successfully
                        if (targetTaskIndex != -1)
                        {
                            break;
                        }
                    }

                    // Break out of loop if a target task has been made successfully
                    if (targetTaskIndex != -1)
                    {
                        break;
                    }
                }
            }

            if (targetTask != null) 
            {
                // Set targetTask's member to this gameObject so that it knows who's doing it
                targetTask.member = gameObject;
                myCommuneManager.taskQueue[targetTaskIndex].member = gameObject;
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
                    targetTaskIndex = -1;
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

        // Leave commune if satisfaction is below -100
        if (satisfaction <= -100f || daysUnsated > 2)
        {
            myCommuneManager.RemoveMember(gameObject);
        }
    }

    public bool MakeTask(int taskType, int skill, float difficulty, GameObject target)
    {
        // Generate task according to input
        targetTask = new Task(taskType, skill, difficulty, target);

        // Add task to list or ditch if it already is present
        if (myCommuneManager.AddTask(targetTask))
        {
            targetTaskIndex = myCommuneManager.taskQueue.Count - 1;
            return true;
        }
        else
        {
            targetTask = null;
            return false;
        }
    }

    public void Clicked()
    {
        // On clicked toggle stats
        statsDisplay.SetActive(!statsDisplay.active);
    }
}
