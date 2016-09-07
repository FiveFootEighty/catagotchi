using UnityEngine;
using System.Collections;

public class SelectableObject : MonoBehaviour {
     
    private Rigidbody rigidbody;

    private GrabController controller;
    private bool isGrabbed;
    private bool isHighlighted;

    public float velocityFactor = 5000f;
    private Vector3 positionDelta;

    public float rotationFactor = 400f;
    private Quaternion rotationDelta;

    private Transform interationPoint;
    

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody>();

        velocityFactor /= rigidbody.mass;
        rotationFactor /= rigidbody.mass;
    }
	
	// Update is called once per frame
	void Update () {

        if (isGrabbed && controller != null)
        {
            // move the object with the controller
            positionDelta = controller.transform.position - interationPoint.position;

            rigidbody.velocity = positionDelta * velocityFactor * Time.fixedDeltaTime;

            float angle;
            Vector3 axis;

            rotationDelta = controller.transform.rotation * Quaternion.Inverse(interationPoint.rotation);
            rotationDelta.ToAngleAxis(out angle, out axis);

            while (angle > 180)
            {
                angle -= 360;
            }

            rigidbody.angularVelocity = (Time.fixedDeltaTime * angle * axis) * rotationFactor;
        }
	}

    public void OnGrab(GrabController controller)
    {
        this.controller = controller;
        isGrabbed = true;

        Unfreeze();

        if (interationPoint == null)
        {
            // make this better
            interationPoint = new GameObject().transform;
        }

        interationPoint.position = controller.transform.position;
        interationPoint.rotation = controller.transform.rotation;
        interationPoint.SetParent(transform, true);
    }

    public void OnUngrab(GrabController controller)
    {
        this.controller = null;
        isGrabbed = false;
    }

    public void OnHighlight(GrabController controller)
    {
        this.controller = controller;
        isHighlighted = true;
    }

    public void OnUnhighlight(GrabController controller)
    {
        this.controller = null;
        isHighlighted = false;
    }

    public void Freeze()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }

    public void Unfreeze()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }
}
