using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector2 lastMousePos;
    [SerializeField] private float mouseMoveSpd = 0.05f;

    private void Update()
    {
        var move = Vector2.zero;

        // Switch input by platform
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                break;
            default:
                // Move while the mouse button is down
                if (Input.GetMouseButton(1))
                {
                    move = (lastMousePos - new Vector2(Input.mousePosition.x, Input.mousePosition.y)) * mouseMoveSpd;
                }
                // Make last mouse pos current mouse pos
                lastMousePos = Input.mousePosition;
                break;
        }

        // Apply movement
        transform.Translate(move);
    }
}
