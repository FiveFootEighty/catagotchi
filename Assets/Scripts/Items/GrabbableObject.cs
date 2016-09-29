using UnityEngine;
using System.Collections;

public class GrabbableObject : InteractionBase {

    private Rigidbody rigidbody;

    public float velocityFactor = 5000f;
    private Vector3 positionDelta;

    public float rotationFactor = 400f;
    private Quaternion rotationDelta;
    
    public bool shouldHighlight;
    public Material highlightMaterial;
    private Material[] savedMaterials;
    private Material[] highlightMaterials;

    public bool useOtherTransform;
    public Transform otherTransform;

    private GrabController controller;
    private bool isGrabbed;
    private bool isHighlighted;

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
        if (!isGrabbed)
        {
            this.controller = controller;
            isHighlighted = true;

            if (shouldHighlight)
            {
                if (savedMaterials == null || highlightMaterials == null)
                {
                    savedMaterials = GetComponent<Renderer>().materials;
                    highlightMaterials = new Material[savedMaterials.Length];
                    for (int i = 0; i < savedMaterials.Length; i++)
                    {
                        highlightMaterials[i] = highlightMaterial;
                    }
                }
                GetComponent<Renderer>().materials = highlightMaterials;
            }
        }
    }

    public void OnUnhighlight(GrabController controller)
    {
        this.controller = null;
        isHighlighted = false;
        
        if (shouldHighlight)
        {
            GetComponent<Renderer>().materials = savedMaterials;
        }
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
