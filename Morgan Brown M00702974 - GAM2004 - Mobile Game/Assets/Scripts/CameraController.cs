using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    // Origin of mouse for panning
    private Vector2 mousePosOrigin;

    // Speed at which to move when dragging
    [SerializeField] private float mouseMoveSpd = 1f;
    
    // Speed at which to zoom
    [SerializeField] private float mouseZoomSpd = 1f;

    // Foundation to be placed
    public GameObject foundationPrefab = null;

    // Image for foundation currently held
    [SerializeField] private Image foundationImage;

    // Ref to camera component on this gameObject
    private Camera myCam;

    private void Start()
    {
        // Get a reference to camera component on this gameobject
        myCam = GetComponent<Camera>();
    }

    private void Update()
    {
        // Reset var by which to move
        var move = Vector3.zero;

        // Switch input by platform
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                break;
            default:
                // Get mouse position as Vector2
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                // Cast mouse to ray
                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                if (Input.GetMouseButtonDown(0))
                {
                    // Get mouse position at moment that input is received
                    mousePosOrigin = mousePos2D;

                    if (foundationPrefab == null && hit)
                    {
                        // Tell hit it's been clicked
                        hit.transform.gameObject.SendMessage("Clicked");
                    }
                    else if (foundationPrefab != null) 
                    {
                        // Instantiate foundationPrefab at position clicked
                        Instantiate(foundationPrefab, new Vector3(mousePos.x, mousePos.y, 0f), Quaternion.identity);
                        // Clear foundation prefab
                        ClearFoundationPrefab();
                    }
                }

                if (Input.GetMouseButton(0))
                {
                    // Move while mouse button is down
                    transform.Translate((mousePosOrigin - mousePos2D) * mouseMoveSpd);
                
                    // Send message that mouse is held to destroy buildings
                    hit.transform.gameObject.SendMessage("ClickHeld");
                }

                // Zoom based on scroll wheel input
                myCam.orthographicSize = Mathf.Clamp(myCam.orthographicSize - (Input.mouseScrollDelta.y * mouseZoomSpd), 3, 25);

                break;
        }
        
        if (foundationPrefab != null) {
            foundationImage.enabled = true;
            foundationImage.sprite = foundationPrefab.GetComponent<SpriteRenderer>().sprite;
        }
        else
        {
            foundationImage.enabled = false;
        }
    }

    public void SetFoundationPrefab (GameObject _foundationPrefab)
    {
        // Set the foundation prefab to the foundation prefab given
        foundationPrefab = _foundationPrefab;
    }

    public void ClearFoundationPrefab ()
    {
        // Clear foundation prefab by setting it to null
        foundationPrefab = null;
    }
}