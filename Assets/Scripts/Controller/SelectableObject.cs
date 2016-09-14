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

    private Transform interactionPoint;

    public bool usePhysics;


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
            if (!usePhysics)
            {
                float time = 30f * Vector3.Distance(transform.position, controller.transform.position - interactionPoint.position) - 0.5f;
                
                if (time < 1)
                {
                    time = 1f;
                }

                GetComponent<Rigidbody>().position = new Vector3(
                    OtherSlide(transform.position.x, interactionPoint.position.x, time),
                    OtherSlide(transform.position.y, interactionPoint.position.y, time),
                    OtherSlide(transform.position.z, interactionPoint.position.z, time)
                    );

                transform.rotation = interactionPoint.rotation;
            } else
            {
                // move the object with the controller
                positionDelta = controller.transform.position - interactionPoint.position;

                rigidbody.velocity = positionDelta * velocityFactor * Time.fixedDeltaTime;

                float angle;
                Vector3 axis;

                rotationDelta = controller.transform.rotation * Quaternion.Inverse(interactionPoint.rotation);
                rotationDelta.ToAngleAxis(out angle, out axis);

                while (angle > 180)
                {
                    angle -= 360;
                }

                rigidbody.angularVelocity = (Time.fixedDeltaTime * angle * axis) * rotationFactor;
            }
        }
	}

    private float OtherSlide(float curVal, float targetVal, float speed)
    {
        float change = Time.deltaTime * speed;
        if (curVal < targetVal)
        {
            curVal += change;
            if (curVal > targetVal) curVal = targetVal;
        }
        else
        {
            curVal -= change;
            if (curVal < targetVal) curVal = targetVal;
        }
        return curVal;
    }

    private IEnumerator SlideTo(Vector3 positionStart, Vector3 positionEnd, float time)
    {
        float elapsedTime = 0;
        transform.position = positionStart;
        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(transform.position, positionEnd, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    

    public void OnGrab(GrabController controller)
    {
        this.controller = controller;
        isGrabbed = true;

        Unfreeze();

        if (interactionPoint == null)
        {
            // make this better
            interactionPoint = new GameObject().transform;
            interactionPoint.name = "InteractionPoint";
        }

        if (usePhysics)
        {
            interactionPoint.position = controller.transform.position;
            interactionPoint.rotation = controller.transform.rotation;

            interactionPoint.SetParent(transform, true);
        } else
        {
            interactionPoint.position = controller.transform.position - (controller.transform.position - transform.position);
            interactionPoint.rotation = transform.rotation;
            interactionPoint.SetParent(controller.transform, true);

            GetComponent<Rigidbody>().useGravity = false;
        }
    }

    public void OnUngrab(GrabController controller)
    {
        this.controller = null;
        isGrabbed = false;

        if (interactionPoint != null)
        {
            Destroy(interactionPoint.gameObject);
        }

        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }

    public void OnHighlight(GrabController controller)
    {
        this.controller = controller;
        isHighlighted = true;
    }

    public void OnUnhighlight(GrabController controller)
    {
        if (!isGrabbed)
        {
            this.controller = null;
        }
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
