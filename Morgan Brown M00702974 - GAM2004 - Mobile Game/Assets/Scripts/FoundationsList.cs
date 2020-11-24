using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoundationsList : MonoBehaviour
{
    // Whether or not the foundationslist is open
    private bool open = false;
    
    // The image of the expander to display
    private Image expander;

    // The rect transform to move in the UI Canvas
    private RectTransform rectTransform;

    // The images for open and closed
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closeSprite;

    // Speed at which to open / close
    [SerializeField] private float openSpd = 0.1f;

    // Store ref to CameraController for access
    private CameraController mainCameraController;

    private void Start()
    {
        // Get ref to image on this obj
        expander = GetComponent<Image>();

        // Get ref to recttransform on this obj
        rectTransform = GetComponent<RectTransform>();

        // Get ref to cameracontroller on main camera
        mainCameraController = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();
    }

    private void Update()
    {
        if (open)
        {
            // Set expander image to close
            expander.sprite = closeSprite;
            
            // Set position to open position
            rectTransform.anchoredPosition = new Vector2 (rectTransform.anchoredPosition.x, Mathf.Lerp(rectTransform.anchoredPosition.y, 64, openSpd));
        }
        else
        {
            // Set expander image to open
            expander.sprite = openSprite;

            // Set position to closed position
            rectTransform.anchoredPosition = new Vector2 (rectTransform.anchoredPosition.x, Mathf.Lerp(rectTransform.anchoredPosition.y, -340, openSpd));
        }
    }

    public void toggleOpen()
    {
        // Make open the opposite of open
        open = !open;
    }
}
