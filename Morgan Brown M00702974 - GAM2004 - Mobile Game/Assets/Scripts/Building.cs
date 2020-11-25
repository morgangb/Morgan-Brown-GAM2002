using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildingTheCommune;
using UnityEngine.UI;

[RequireComponent(typeof(Clickable))]
public class Building : MonoBehaviour
{
    // Difficulty; how long to build
    [SerializeField] private float buildDifficulty = 1f;
    // Difficulty; how long to use
    [SerializeField] private float useDifficulty = 1f;
    // Skill to use
    [SerializeField] private int skill = 0;

    // boolean that measures if this is built
    private bool built = false;

    // Marker that building is under construction
    [SerializeField] private GameObject buildMarker;

    // Clickable component on this gameobject
    private Clickable myClickable;

    // Store ref to the communemanager
    private CommuneManager myCommuneManager;

    // Determine what resource to give and how much
    [SerializeField] private int resourceType;
    [SerializeField] private int resourceQuant;

    // Determine what consumables to give and how much
    [SerializeField] private int consumableType;
    [SerializeField] private int consumableQuant;

    private void Start()
    {
        // Get ref to clickable
        myClickable = GetComponent<Clickable>();
        // Set clickable's task to building this gameObject at the specified difficulty
        myClickable.myTask = new Task(1, 2, buildDifficulty, gameObject);
        // Find communemanager
        myCommuneManager = GameObject.FindWithTag("CommuneManager").GetComponent<CommuneManager>();
    }

    private void Update()
    {
        // Activate buildMarker if building is under construction
        buildMarker.SetActive(!built);
    }

    public void Build()
    {
        // Set that this building has been built
        built = true;
        // Set clickable's task to using this gameObject at the specified difficulty
        myClickable.myTask = new Task(2, skill, buildDifficulty, gameObject);
    }

    public void Use() 
    {
        // Give resource & consumable in communemanager
        myCommuneManager.resourcesCount[resourceType] += resourceQuant;
        myCommuneManager.consumableNeeds[consumableType] += consumableQuant;
    }
}