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
            if (true)
            {
                positionDelta = attachedController.transform.position - interationPoint.position;
                
                Vector3 objectVelocity = positionDelta * velocityFactor * Time.fixedDeltaTime;
                objectVelocity = new Vector3(Mathf.Clamp(objectVelocity.x, -2, 2), Mathf.Clamp(objectVelocity.y, -2, 2), Mathf.Clamp(objectVelocity.z, -2, 2));

                rigidbody.velocity = objectVelocity;

                rotationDelta = attachedController.transform.rotation * Quaternion.Inverse(interationPoint.rotation);
                rotationDelta.ToAngleAxis(out angle, out axis);

                if (angle > 180)
                {
                    angle -= 360;
                }

                rigidbody.angularVelocity = (Time.fixedDeltaTime * angle * axis) * rotationFactor;
            }
        }
	}

    public void OnHighlightBegin(TrackedController controller)
    {
        attachedController = controller;
        isHighlighted = true;
    }

    public void OnHighlightEnd(TrackedController controller)
    {
        attachedController = null;
        isHighlighted = false;
    }

    public void OnSelectedBegin(TrackedController controller)
    {
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
    }

    void OnCollisionEnter(Collision collision)
    {
        // if you have a splatter
        if (GetComponent<SplatterBehavior>() != null)
        {
            // if you havent splatterd yet
            if (collision.gameObject.tag == "Wall" && !GetComponent<SplatterBehavior>().hasSplattered)
            {
                // splatter
                ContactPoint contact = collision.contacts[0];
                GetComponent<SplatterBehavior>().CreateSplatter(collision.gameObject, contact.point, collision.gameObject.transform.rotation.eulerAngles);
            }
        }
    }
}
