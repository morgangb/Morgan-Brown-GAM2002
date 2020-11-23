using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BuildingTheCommune;

[RequireComponent(typeof(Clickable))]
public class ExtractableResource : MonoBehaviour
{
    // Determine what resource to give and how much
    [SerializeField] private int resourceType;
    [SerializeField] private int resourceQuant;

    // Determine what consumables to give and how much
    [SerializeField] private int consumableType;
    [SerializeField] private int consumableQuant;

    // Determine how extraction works
    [SerializeField] private int skill;
    [SerializeField] private float difficulty = 1f;

    // Store ref to the communemanager
    private CommuneManager myCommuneManager;

    private void Start()
    {
        // Find communemanager
        myCommuneManager = GameObject.FindWithTag("CommuneManager").GetComponent<CommuneManager>();

        // Set task for extraction
        GetComponent<Clickable>().myTask = new Task(0, skill, difficulty, gameObject);
    }

    public void Extract() 
    {
        // Give resource & consumable in communemanager
        myCommuneManager.resourcesCount[resourceType] += resourceQuant;
        myCommuneManager.consumableNeeds[consumableType] += consumableQuant;
        Destroy(gameObject);
    }
}
