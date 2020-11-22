using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildingTheCommune;

public class CommuneMember : MonoBehaviour
{
    // Task which the member is trying to do
    private Task targetTask = null;

    // Reference to scene communemanager
    private CommuneManager myCommuneManager;

    // Member stats
    public int[] abilities = {1};
    public int[] enjoyment = {1};
    
    private void Start()
    {
        // Get communemanager
        myCommuneManager = GameObject.FindWithTag("CommuneManager").GetComponent<CommuneManager>();
    }

    private void Update()
    {
        // Find task if there is no current task, otherwise, do current task
        if (targetTask == null)
        {
            targetTask = myCommuneManager.taskQueue[0];
        }
        else
        {
            // Move to task's target
            transform.Translate((targetTask.target.transform.position - transform.position).normalized);
        }
    }
}
