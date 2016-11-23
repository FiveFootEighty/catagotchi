using UnityEngine;
using System.Collections;

public class GrabbableObject : InteractionBase {
 
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

    public void OnGrab(GrabController controller)
    {
        this.controller = controller;
        isGrabbed = true;

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
            if (GetComponent<Collider>() != null && GetComponent<Collider>().material != null && 
                collision.gameObject.GetComponent<Collider>() != null && collision.gameObject.GetComponent<Collider>().material != null)
            {
                if (SoundEffectRegistry.instance().GetSoundCombination(GetComponent<Collider>().material.name, collision.gameObject.GetComponent<Collider>().material.name) != null)
                {
                    GetComponent<SoundEffect>().PlaySound(GetComponent<Collider>().material, collision.gameObject.GetComponent<Collider>().material, collision.relativeVelocity.magnitude / 6f);
                } else
                {
                    GetComponent<SoundEffect>().PlaySound(collision.relativeVelocity.magnitude / 6f);
                }
            } else
            {
                GetComponent<SoundEffect>().PlaySound(collision.relativeVelocity.magnitude / 6f);
            }
            
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
}
