using UnityEngine;
using System.Collections;

public class PlebeianCameraController : MonoBehaviour {

    Vector3 previousMousePosition;

	void Update () {

        // zero == left mouse click
        // one == right mouse click
        // two == middle mouse click
        if (Input.GetMouseButtonDown(1))
        {
            previousMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(1))
        {
            Vector3 mousePositionDelta = previousMousePosition - Input.mousePosition;

            transform.Rotate(new Vector3(mousePositionDelta.y / 4f, 0, 0));
            transform.Rotate(new Vector3(0, mousePositionDelta.x / -4f, 0), Space.World);
            previousMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonDown(2))
        {
            previousMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(2))
        {
            Vector3 mousePositionDelta = previousMousePosition - Input.mousePosition;

            transform.Translate(mousePositionDelta/100f, Space.Self);
            previousMousePosition = Input.mousePosition;
        }
        transform.Translate(new Vector3(0, 0, Input.mouseScrollDelta.y / 10f), Space.Self);
    }
}
