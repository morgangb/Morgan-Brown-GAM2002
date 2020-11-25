using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildingTheCommune;

public class CommuneMember : MonoBehaviour
{
    // Task which the member is trying to do
    public Task targetTask = null;

    // Reference to scene communemanager
    private CommuneManager myCommuneManager;

    // Member skill stats
    public int[] abilities = {1, 1, 1, 1};
    public int[] enjoyment = {1, 1, 1, 1};
    
    // Movement spd
    [SerializeField] private float moveSpd = 1f;

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
            // Get task from queue if there is a task in the queue, otherwise make own task
            if (myCommuneManager.taskQueue.Count > 0) 
            {
                targetTask = myCommuneManager.taskQueue[0];
                // Set targetTask's member to this, so that it knows who is doing it
                targetTask.member = gameObject;
            }
            else
            {

            }
        }
        else
        {
            //Move to task's target if far from task, otherwise, do task
            if (Vector2.Distance(targetTask.target.transform.position, transform.position) > 1f) 
            {
                transform.Translate((targetTask.target.transform.position - transform.position).normalized * moveSpd * Time.deltaTime);
            }
            else 
            {
                targetTask.difficulty -= (1f + abilities[targetTask.skill]) * 0.5f * Time.deltaTime;
                if (targetTask.difficulty <= 0f)
                {
                    targetTask.Complete();
                    myCommuneManager.RemoveTask(targetTask);
                    targetTask = null;
                }
            }
        }
    }
}
