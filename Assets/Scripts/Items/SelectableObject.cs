using UnityEngine;
using System.Collections;

public class SelectableObject : MonoBehaviour {
     
    private Rigidbody rigidbody;

    private GrabController controller;
    private bool isGrabbed;
    private bool isHighlighted;

    // Both the velocityfactor and the rotationfactor are variables that need to be played with along with the mass of the object
    // to get just right. A velocity facotr that is too high wil cause stuttering and a Vf that is too low will cause too much lag as the
    // object tries to get back to it's interaction point.
    public float velocityFactor = 5000f;
    private Vector3 positionDelta;

    public float rotationFactor = 400f;
    private Quaternion rotationDelta;

    private Transform interationPoint;
    
    
	void Start () {
        rigidbody = GetComponent<Rigidbody>();

        velocityFactor /= rigidbody.mass;
        rotationFactor /= rigidbody.mass;
    }
	
	void Update () {

        if (isGrabbed && controller != null)
        {
            // calculate a velocity to apply to the object to get it to it's original interaction point. Then apply it to the rigidbody
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
            interationPoint = new GameObject().transform;
            interationPoint.name = "InteractionPoint";
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
