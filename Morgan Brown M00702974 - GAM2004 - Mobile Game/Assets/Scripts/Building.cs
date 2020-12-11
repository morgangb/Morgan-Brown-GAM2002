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

    // Clicktime; how long has the click been held, and what was it last frame
    private float clickTime = 0f;
    private float lastClickTime = 0f;

    // Difficulty; how long to use
    public float useDifficulty = 1f;
    // Skill to use
    public int skill = 0;

    // How many resources will be used when building
    public int[] buildResources;

    // boolean that measures if this is built
    public bool built = false;

    // Measures if the building is cooling down; if above 0, cannot be used
    public float coolDown = 0f;

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

        // Find communemanager
        myCommuneManager = GameObject.FindWithTag("CommuneManager").GetComponent<CommuneManager>();

        // Initialise beds with empty beds
        beds = new Bed[bedsCount];

        // if this is already built set the task to use, and if not, set it to build
        if (built)
        {
            // Set clickable's task to using this gameObject at the specified difficulty
            myClickable.myTask = new Task(2, skill, useDifficulty, gameObject);
        }
        else
        {
            // Set clickable's task to building this gameObject at the specified difficulty
            myClickable.myTask = new Task(1, 2, buildDifficulty, gameObject);
        }
    }

    private void Update()
    {
        if (clickTime == lastClickTime)
        {
            clickTime = 0f;
        }

        if (clickTime >= 1f)
        {
            myCommuneManager.RemoveTask(myClickable.myTask);
            Destroy(gameObject);
        }

        lastClickTime = clickTime;

        // Decrease coolDown
        if (coolDown > 0f)
        {
            coolDown -= Time.deltaTime;
        }

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
            myClickable.myTask = new Task(2, skill, useDifficulty, gameObject);
        
            // Remove used resources
            myCommuneManager.ChangeResources(buildResources, true);
        }
    }

    public void Use() 
    {
        // Give resource & consumable in communemanager
        myCommuneManager.ChangeResources(myClickable.resources, false);

        // Set coolDown
        coolDown = useDifficulty * 2f;
    }

    public void ClickHeld()
    {
        clickTime += Time.deltaTime;
    }
}