using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BuildingTheCommune;

public class ExtractableResource : MonoBehaviour
{
    // Determine what resource to give and how much
    [SerializeField] private int resourceType;
    [SerializeField] private int resourceQuant;

    // Determine what consumables to give and how much
    [SerializeField] private int consumableType;
    [SerializeField] private int consumableQuant;

    // Store ref to the communemanager
    private CommuneManager myCommuneManager;

    // Create a task for harvesting
    private Task myTask;

    private void Start()
    {
        // Find communemanager
        myCommuneManager = GameObject.FindWithTag("CommuneManager").GetComponent<CommuneManager>();

        // Set task for extraction
        myTask = new Task(0, 0, gameObject);
    }

    public void Clicked()
    {
        // On clicked remove/add task
        if(!myCommuneManager.RemoveTask(myTask)) 
        {
            myCommuneManager.AddTask(myTask);
        }
    }

    public void Extract() 
    {
        // Give resource & consumable in communemanager
        myCommuneManager.resourcesCount[resourceType] += resourceQuant;
        myCommuneManager.consumableNeeds[consumableType] += consumableQuant;
        Destroy(gameObject);
    }
}
