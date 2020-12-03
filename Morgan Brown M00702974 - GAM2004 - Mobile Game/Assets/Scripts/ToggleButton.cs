using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    // Bool that measures if this toggle is on or off
    public bool isOn;

    // Own image
    private Image myImage;

    // Images for on and off
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;

    private void Start()
    {
        // Get image at start-up
        myImage = GetComponent<Image>();
    }

    public void Toggle()
    {
        // Toggle isOn to the opposite
        isOn = !isOn;

        // Change sprite accordingly
        if (isOn) 
        {
            myImage.sprite = onSprite;
        }
        else
        {
            myImage.sprite = offSprite;
        }
    }
}
