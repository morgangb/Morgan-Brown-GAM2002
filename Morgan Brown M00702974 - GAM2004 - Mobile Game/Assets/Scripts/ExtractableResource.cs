using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BuildingTheCommune;

[RequireComponent(typeof(Clickable))]
public class ExtractableResource : MonoBehaviour
{
    // Determine how extraction works
    public int skill;
    public float difficulty = 1f;

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

    public void Breed()
    {
        // 50% chance of spawning a copy of self nearby
        if(Random.Range(0f, 1f) >= 0.5f)
        {
            var clone = Instantiate(gameObject);
            clone.transform.Translate(new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f)));
        }
    }
}
