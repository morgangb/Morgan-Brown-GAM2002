﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Last mouse position
    private Vector2 lastMousePos;

    // Speed at which to move when dragging
    [SerializeField] private float mouseMoveSpd = 0.05f;

    // Foundation to be placed
    public GameObject foundationPrefab = null;

    private void Update()
    {
        // Reset var by which to move
        var move = Vector2.zero;

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
                if(Input.GetMouseButtonDown(0))
                {
                    if (foundationPrefab == null && hit)
                    {
                        // Tell hit it's been clicked
                        hit.transform.gameObject.SendMessage("Clicked");
                    }
                    else if (foundationPrefab != null) 
                    {
                        Instantiate(foundationPrefab, new Vector3(hit.point.x, hit.point.y, 0f), Quaternion.identity);
                    }
                }

                // Move while the mouse button is down
                if (Input.GetMouseButton(1))
                {
                    move = (lastMousePos - mousePos2D) * mouseMoveSpd;
                }

                // Make last mouse pos current mouse pos
                lastMousePos = mousePos2D;
                break;
        }

        // Apply movement
        transform.Translate(move);
    }

    public void SetFoundationPrefab (GameObject _foundationPrefab)
    {
        // Set the foundation prefab to the foundation prefab given
        foundationPrefab = _foundationPrefab;
    }
}
