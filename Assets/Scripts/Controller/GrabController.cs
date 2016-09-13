using UnityEngine;
using System.Collections;

public class GrabController : MonoBehaviour, TrackedControllerBase.TrackedControllerTriggerListener
{

    private TrackedControllerBase trackedControllerBase;

    [HideInInspector]
    public GameObject selectedObject;
    [HideInInspector]
    public bool isHighlighted = false;
    [HideInInspector]
    public bool isGrabbed = false;
    public Vector3 velocity;
    Vector3 previousPosition = Vector3.zero;

    // Use this for initialization
    void Start () {
        trackedControllerBase = GetComponentInParent<TrackedControllerBase>();
        trackedControllerBase.RegisterTriggerListener(this);

        isGrabbed = false;
        isHighlighted = false;
    }

    void Update()
    {
        if (previousPosition != Vector3.zero)
        {
            velocity = (transform.position - previousPosition) / Time.deltaTime;
        }
        previousPosition = transform.position;
    }

    public void OnTriggerPress()
    {

    }
    public void OnTriggerPressUp()
    {
        if (isGrabbed)
        {
            UngrabObject();
        }
    }
    public void OnTriggerPressDown()
    {
        if (isHighlighted && !isGrabbed && selectedObject != null)
        {
            UnhighlightObject(selectedObject);
            GrabObject();
        }
    }





    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<SelectableObject>() != null)
        {
            // can select this one  
            HighlightObject(collider.gameObject);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject == selectedObject && isHighlighted && !isGrabbed)
        {
            UnhighlightObject(collider.gameObject);
        }
    }





    void GrabObject()
    {
        isHighlighted = false;
        isGrabbed = true;
        selectedObject.GetComponent<SelectableObject>().OnGrab(this);
    }

    void UngrabObject()
    {
        isGrabbed = false;
        selectedObject.GetComponent<SelectableObject>().OnUngrab(this);
        selectedObject.GetComponent<Rigidbody>().velocity = velocity;
        selectedObject = null;
    }

    void HighlightObject(GameObject gameObject)
    {
        selectedObject = gameObject;
        isHighlighted = true;
        selectedObject.GetComponent<SelectableObject>().OnHighlight(this);
    }

    void UnhighlightObject(GameObject gameObject)
    {
        isHighlighted = false;
        selectedObject.GetComponent<SelectableObject>().OnUnhighlight(this);
    }
}
