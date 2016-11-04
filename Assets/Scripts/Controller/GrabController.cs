using UnityEngine;
using System.Collections;

public class GrabController : MonoBehaviour, TrackedControllerBase.TrackedControllerTriggerListener
{

    [HideInInspector]
    public TrackedControllerBase trackedControllerBase;
    [HideInInspector]
    public GameObject selectedObject;
    [HideInInspector]
    public bool isHighlighted = false;
    [HideInInspector]
    public bool isGrabbed = false;
    public Vector3 velocity;
    Vector3 previousPosition = Vector3.zero;
    public Transform handModel;

    private Transform model;

    public Animator animator;

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
        animator.SetBool("ShouldGrab", false);

        if (isGrabbed)
        {
            UngrabObject();
        }
    }
    public void OnTriggerPressDown()
    {
        animator.SetBool("ShouldGrab", true);

        if (isHighlighted && !isGrabbed && selectedObject != null)
        {
            UnhighlightObject(selectedObject);
            GrabObject();
        }
    }





    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject != selectedObject && isHighlighted && !isGrabbed && collider.gameObject.GetComponent<GrabbableObject>() != null)
        {
            UnhighlightObject(selectedObject);
        }
        if (collider.gameObject.GetComponent<GrabbableObject>() != null && !isGrabbed)
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
        if (selectedObject.GetComponent<GrabbableObject>() != null)
        {
            if (selectedObject.GetComponent<GrabbableObject>().isGrabbed)
            {
                selectedObject.GetComponent<GrabbableObject>().controller.UngrabObject();
                selectedObject.GetComponent<GrabbableObject>().OnHighlight(this);
                selectedObject.GetComponent<GrabbableObject>().OnUnhighlight(this);
            }
            
            selectedObject.GetComponent<GrabbableObject>().OnGrab(this);
        }

        handModel.gameObject.SetActive(false);
    }

    void UngrabObject()
    {
        isGrabbed = false;
        if (selectedObject.GetComponent<GrabbableObject>() != null)
        {
            selectedObject.GetComponent<GrabbableObject>().OnUngrab();
        }
        selectedObject.GetComponent<Rigidbody>().velocity = velocity;
        selectedObject = null;

        handModel.gameObject.SetActive(true);
    }

    void HighlightObject(GameObject gameObject)
    {
        selectedObject = gameObject;
        isHighlighted = true;
        if (selectedObject.GetComponent<GrabbableObject>() != null)
        {
            selectedObject.GetComponent<GrabbableObject>().OnHighlight(this);
        }
    }

    void UnhighlightObject(GameObject gameObject)
    {
        isHighlighted = false;
        if (selectedObject.GetComponent<GrabbableObject>() != null)
        {
            selectedObject.GetComponent<GrabbableObject>().OnUnhighlight(this);
        }
    }
}
