using UnityEngine;
using System.Collections;

public class PateObject : GrabbableObject {

    private Transform pate;
    private CanOpenerBehavior canOpenerBehavior;

    public bool opened = false;
    
	void Start () {
        rigidbody = GetComponent<Rigidbody>();

        velocityFactor /= rigidbody.mass;
        rotationFactor /= rigidbody.mass;

        pate = transform.Find("Pate");
	}
	
	public override void PostUpdate() {
        if (opened && pate != null && ((transform.rotation.eulerAngles.x > 150 && transform.rotation.eulerAngles.x < 210) || (transform.rotation.eulerAngles.z > 150 && transform.rotation.eulerAngles.z < 210)))// && pate.gameObject.GetComponent<Rigidbody>() == null)
        {
            if (pate.GetComponent<Rigidbody>() == null)
            {
                pate.gameObject.AddComponent<Rigidbody>();
                pate.GetComponent<Rigidbody>().isKinematic = true;
            }

            pate.transform.Translate(new Vector3(0, 0.005f, 0), Space.Self);
        }
    }

    public override void AfterOnUnGrab()
    {
        base.AfterOnUnGrab();
        if (canOpenerBehavior != null)
        {
            Transform ghostCan = canOpenerBehavior.OnPateUnGrab();
            transform.position = ghostCan.position;
            transform.rotation = ghostCan.rotation;
        }
    }

    public void SetCanOpenerBehavior(CanOpenerBehavior canOpenerBehavior)
    {
        this.canOpenerBehavior = canOpenerBehavior;
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.transform == pate)
        {

            pate.GetComponent<MeshCollider>().enabled = true;
            pate.GetComponent<Rigidbody>().isKinematic = false;
            pate.parent = null;
            pate = null;
        }
    }
}
