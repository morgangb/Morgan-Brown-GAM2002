using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildingTheCommune;

public class Clickable : MonoBehaviour
{
    // Store task to add / remove
    public Task myTask;

    // Store ref to the communemanager
    private CommuneManager myCommuneManager;

    // Store ref to taskMarker
    [SerializeField] private GameObject taskMarker;

    private void Start()
    {
        taskMarker.SetActive(false);
        // Find communemanager
        myCommuneManager = GameObject.FindWithTag("CommuneManager").GetComponent<CommuneManager>();
    }

    public void Clicked()
    {
        // On clicked remove/add task
        if(!myCommuneManager.RemoveTask(myTask)) 
        {
            taskMarker.SetActive(true);
            myCommuneManager.AddTask(myTask);
        }
        else {
            taskMarker.SetActive(false);
        }
    }
}
