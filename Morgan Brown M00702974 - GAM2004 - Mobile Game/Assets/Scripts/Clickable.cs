using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildingTheCommune;

public class Clickable : MonoBehaviour
{
    // Determine how many of each resource to give for use/extract
    public int[] resources;

    // Store task to add / remove
    public Task myTask;

    // Store ref to the communemanager
    private CommuneManager myCommuneManager;

    // Store ref to taskMarker
    public GameObject taskMarker;

    private void Start()
    {
        // Initialise with task marker off
        taskMarker.SetActive(false);

        // Find communemanager
        myCommuneManager = GameObject.FindWithTag("CommuneManager").GetComponent<CommuneManager>();
    }

    public void Clicked()
    {
        // On clicked remove/add task for using
        if(!myCommuneManager.RemoveTask(new Task(myTask)) && myTask.difficulty > 0f)
        {
            taskMarker.SetActive(true);
            myCommuneManager.AddTask(new Task(myTask));
        }
        else 
        {
            taskMarker.SetActive(false);
        }
    }
}
