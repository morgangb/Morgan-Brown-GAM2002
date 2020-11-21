using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ExtractableResource : MonoBehaviour
{
    // Determine what resource to give and how much
    [SerializeField] private int resourceType;
    [SerializeField] private int resourceQuant;

    // Store the communemanager
    private CommuneManager myCommuneManager;

    private void Start()
    {
        // Find communemanager
        myCommuneManager = GameObject.FindWithTag("CommuneManager").GetComponent<CommuneManager>();
    }

    private void Update()
    {
        
    }

    public void extract() 
    {
        // Give resource in communemanager
        myCommuneManager.resourcesCount[resourceType] += resourceQuant;
        Destroy(gameObject);
    }
}
