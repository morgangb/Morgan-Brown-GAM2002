using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildingTheCommune {
    // Resources enum for finding resources quickly
    public enum RESOURCES
    {
        FOOD,
        WATER,
        WOOD,
        ROCK
    }

    // Task enum for finding tasks quickly
    public enum TASKS
    {
        EXTRACT,
        BUILD,
        USE
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
        WOODCUTTING,
        MINING,
        BUILDING,
        HAULING
    }

    public class Task : IEquatable<Task>
    {
        public Task()
        {

        }

        // This is an initializer that will allow a new task to be created as a copy of an existing task
        public Task(Task _task)
        {
            taskType = _task.taskType;
            skill = _task.skill;
            target = _task.target;
            difficulty = _task.difficulty;
        }

        // This is an initializer that will allow a new task to be created with a taskType and a target
        public Task(int _taskType, int _skill, float _difficulty, GameObject _target)
        {
            taskType = _taskType;
            skill = _skill;
            target = _target;
            difficulty = _difficulty;
        }

        // Type of task (see enum in CommuneManager)
        public int taskType;
        // Type of skill used
        public int skill;
        // Difficulty & time to completion, will count down
        public float difficulty;
        // Target of task
        public GameObject target { get; set; }
        // Commune member currently doing task
        public GameObject member { get; set; } = null;

        public void Complete()
        {
            // Switch and complete task based on taskType, target, etc.
            switch (taskType)
            {
                case 0:
                    target.SendMessage("Extract");
                    break;
                case 1:
                    target.SendMessage("Build");
                    break;
                case 2:
                    target.SendMessage("Use");
                    break;
                default:
                    Debug.Log("taskType not recognised");
                    break;
            }
        }

        // Bools for checking removed from queue
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Task objAsTask = obj as Task;
            if (objAsTask == null) return false;
            else return Equals(objAsTask);
        }
        public bool Equals(Task other)
        {
            if (other == null) return false;
            return (this.target.Equals(other.target));
        }
    }

    public class Bed 
    {
        public CommuneMember owner;

        public Bed()
        {
            
        }
    }
}