using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BuildingTheCommune;

[RequireComponent(typeof(Clickable))]
public class ExtractableResource : MonoBehaviour
{
    // Determine how extraction works
    [SerializeField] private int skill;
    [SerializeField] private float difficulty = 1f;

    // Store ref to the communemanager
    private CommuneManager myCommuneManager;

    // Store ref to clickable
    private Clickable myClickable;

    private void Start()
    {
        // Find communemanager
        myCommuneManager = GameObject.FindWithTag("CommuneManager").GetComponent<CommuneManager>();

        // Get ref to clickable
        myClickable = GetComponent<Clickable>();

        // Set task for extraction
        myClickable.myTask = new Task(0, skill, difficulty, gameObject);
    }

    public void Extract() 
    {
        // Give resource & consumable in communemanager
        myCommuneManager.ChangeResources(myClickable.resources, false);
        Destroy(gameObject);
    }
}
