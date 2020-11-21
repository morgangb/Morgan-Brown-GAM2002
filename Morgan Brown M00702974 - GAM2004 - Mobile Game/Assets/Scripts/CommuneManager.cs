using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// A class for managing commune functions and variables needed by all controllers
public class CommuneManager : MonoBehaviour
{
    // Needs enum for finding needs quickly
    public enum needs
    {
        food = 0,
        water = 1
    }

    // Needs variables
    [Header("Consumable Needs")]
    public int[] consumableNeeds = new int[2];
    [Header("Readouts")]
    private int beds = 0;
    public int time = 0;
    private int people = 0;
    
    // UI variables
    [Header("Consumable Needs UI")]
    [SerializeField] private TMP_Text[] consumableNeedsTxt = new TMP_Text[2];
    [Header("Readouts")]
    [SerializeField] private TMP_Text bedTxt;
    [SerializeField] private TMP_Text timeTxt;
    [SerializeField] private TMP_Text peopleTxt;
    
    // Make the CommuneManager a Singleton
    // Find any other instances of CommuneManager
    private static CommuneManager _instance;

    public static CommuneManager Instance 
    {
        get 
        {
            return _instance; 
        } 
    }

    private void Awake()
    {
        // Destroy this CommuneManager of another exists
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } 
        else 
        {
            _instance = this;
        }
    }

    private void Update()
    {
        // Set consumableNeedsTxt to consumableNeeds
        for (int i = 0; i < consumableNeeds.Length; i++)
        {
            consumableNeedsTxt[i].text = consumableNeeds[i].ToString();
        }
    }
}
