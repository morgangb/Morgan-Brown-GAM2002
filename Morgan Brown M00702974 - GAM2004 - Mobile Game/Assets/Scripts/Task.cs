using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildingTheCommune {
    // Needs enum for finding needs quickly
    public enum NEEDS
    {
        FOOD,
        WATER
    }

    // Resources enum for finding resources quickly
    public enum RESOURCES
    {
        WOOD,
        ROCK
    }

    // Task enum for finding tasks quickly
    public enum TASKS
    {
        EXTRACT
    }

    // Levels enum for measuring the diff levels
    public enum LEVELS
    {
        LOW,
        MID,
        HIGH
    }

    // Skills that are used for tasks and by members
    public enum SKILLS
    {
        WOODCUTTING
    } 

    public class Task
    {
        // This is an initializer that will allow a new task to be created with a taskType and a target
        public Task(int _taskType, int _skill, GameObject _target)
        {
            taskType = _taskType;
            skill = _skill;
            target = _target;
        }

        // Type of task (see enum in CommuneManager)
        public int taskType;
        // Type of skill used
        public int skill;
        // Target of task
        public GameObject target;
        // Commune member currently doing task
        public GameObject member = null;
    }
}