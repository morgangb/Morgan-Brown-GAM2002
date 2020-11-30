using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildingTheCommune;
using UnityEngine.UI;

[RequireComponent(typeof(Clickable))]
public class Building : MonoBehaviour
{
    // Difficulty; how long to build
    public float buildDifficulty = 1f;

    // Difficulty; how long to use
    public float useDifficulty = 1f;
    // Skill to use
    public int skill = 0;

    // How many resources will be used when building
    public int[] buildResources;

    // boolean that measures if this is built
    public bool built = false;

    // Marker that building is under construction
    [SerializeField] private GameObject buildMarker;

    // Clickable component on this gameobject
    private Clickable myClickable;

    // Store ref to the communemanager
    private CommuneManager myCommuneManager;

    // Beds (if there are any)
    public int bedsCount;
    private Bed[] beds;

    private void Start()
    {
        // Get ref to clickable
        myClickable = GetComponent<Clickable>();
        // Set clickable's task to building this gameObject at the specified difficulty
        myClickable.myTask = new Task(1, 2, buildDifficulty, gameObject);

        // Find communemanager
        myCommuneManager = GameObject.FindWithTag("CommuneManager").GetComponent<CommuneManager>();

        // Initialise beds with empty beds
        beds = new Bed[bedsCount];
    }

    private void Update()
    {
        // Activate buildMarker if building is under construction
        buildMarker.SetActive(!built);
    }

    public void Build()
    {
        // Build only if sufficient resources are had
        if (myCommuneManager.CheckResources(buildResources))
        {
            // Set that this building has been built
            built = true;

            // Set clickable's task to using this gameObject at the specified difficulty
            myClickable.myTask = new Task(2, skill, buildDifficulty, gameObject);
        
            // Remove used resources
            myCommuneManager.ChangeResources(buildResources, true);
        }
    }

    public void Use() 
    {
        // Give resource & consumable in communemanager
        myCommuneManager.ChangeResources(myClickable.resources, false);
    }
}