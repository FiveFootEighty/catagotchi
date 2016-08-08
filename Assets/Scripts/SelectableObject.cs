using UnityEngine;
using System.Collections;

public class SelectableObject : MonoBehaviour {
    
    public Rigidbody rigidbody;

    public bool isSelected;
    public bool isHighlighted;

    public float velocityFactor = 5000f;
    private Vector3 positionDelta;
    private float rotationFactor = 400f;
    private Quaternion rotationDelta;
    private float angle;
    private Vector3 axis;

    private Transform interationPoint;

    private TrackedController attachedController;

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody>();
        interationPoint = new GameObject().transform;
        isSelected = false;

        velocityFactor /= rigidbody.mass;
        rotationFactor /= rigidbody.mass;
    }
	
	// Update is called once per frame
	void Update () {
	    if (isSelected && attachedController != null)
        {
            positionDelta = attachedController.transform.position - interationPoint.position;
            Debug.Log(positionDelta);
            this.rigidbody.velocity = positionDelta * velocityFactor * Time.fixedDeltaTime;

            rotationDelta = attachedController.transform.rotation * Quaternion.Inverse(interationPoint.rotation);
            rotationDelta.ToAngleAxis(out angle, out axis);

            if (angle > 180)
            {
                angle -= 360;
            }

            this.rigidbody.angularVelocity = (Time.fixedDeltaTime * angle * axis) * rotationFactor;
        }
	}

    public void OnHighlightBegin(TrackedController controller)
    {
        Debug.Log("OnHighlightBegin");

        attachedController = controller;
        isHighlighted = true;
    }

    public void OnHighlightEnd(TrackedController controller)
    {
        Debug.Log("OnHighlightEnd");

        isHighlighted = false;
    }

    public void OnSelectedBegin(TrackedController controller)
    {
        Debug.Log("OnBeginSelected");

        attachedController = controller;

        interationPoint.position = controller.transform.position;
        interationPoint.rotation = controller.transform.rotation;
        interationPoint.SetParent(transform, true);

        isSelected = true;
    }

    public void OnSelectedEnd(TrackedController controller)
    {
        if (controller == attachedController)
        {
            attachedController = null;
            isSelected = false;
        }
        Debug.Log("OnEndSelected");
        //controller.ClearSelectedObject();
        //isSelected = false;
    }
}
