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
                // Get mouse position as Vector2
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                // Cast mouse to ray
                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                if(Input.GetMouseButtonDown(0) && hit) 
                {
                    hit.transform.gameObject.SendMessage("Clicked");
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
}
