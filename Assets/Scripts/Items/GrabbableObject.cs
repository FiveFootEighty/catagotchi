using UnityEngine;
using System.Collections;

public class GrabbableObject : InteractionBase {

    protected Rigidbody rigidbody;

    public float velocityFactor = 7500f;
    protected Vector3 positionDelta;

    public float rotationFactor = 400f;
    protected Quaternion rotationDelta;
    
    public bool shouldHighlight;
    public Material highlightMaterial;
    protected Material[] savedMaterials;
    protected Material[] highlightMaterials;

    public bool shouldUseGhost;
    public Transform ghost;

    public bool useOtherTransform;
    public Transform otherTransform;

    [HideInInspector]
    public GrabController controller;
    [HideInInspector]
    public bool isGrabbed;
    [HideInInspector]
    public bool isHighlighted;

    protected Transform interationPoint;

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

        PostUpdate();
    }

    public virtual void PostUpdate()
    {

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

        AfterOnGrab();
    }

    public void OnUngrab()
    {
        controller = null;
        isGrabbed = false;

        AfterOnUnGrab();
    }

    public virtual void AfterOnGrab()
    {

    }

    public virtual void AfterOnUnGrab()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (GetComponent<SoundEffect>() != null)
        {
            GetComponent<SoundEffect>().PlaySound(collision.relativeVelocity.magnitude/10f);
        }
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
            if (shouldUseGhost)
            {
                ghost.gameObject.SetActive(true);
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).name != ghost.name)
                    {
                        transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    public void OnUnhighlight(GrabController controller)
    {
        if (controller == this.controller)
        {
            this.controller = null;
            isHighlighted = false;

            if (shouldHighlight)
            {
                GetComponent<Renderer>().materials = savedMaterials;
            }
            if (shouldUseGhost)
            {
                ghost.gameObject.SetActive(false);
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).name != ghost.name)
                    {
                        transform.GetChild(i).gameObject.SetActive(true);
                    }
                }
            }
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
