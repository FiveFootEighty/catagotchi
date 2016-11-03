using UnityEngine;
using System.Collections;

public class CanOpenerBehavior : MonoBehaviour {

    public Transform placedCan;
    private Transform ghostCan;
    
	void Start () {
        ghostCan = transform.Find("GhostCan");
    }

    public Transform OnPateUnGrab()
    {
        ghostCan.gameObject.SetActive(false);
        return ghostCan;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<PateObject>() != null && !collider.gameObject.GetComponent<PateObject>().opened)
        {
            if (placedCan == null)
            {
                placedCan = collider.transform;
                placedCan.GetComponent<PateObject>().SetCanOpenerBehavior(this);
                ghostCan.gameObject.SetActive(true);
            }
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.GetComponent<PateObject>() != null && !collider.gameObject.GetComponent<PateObject>().opened)
        {
            if (placedCan == null)
            {
                placedCan = collider.transform;
                placedCan.GetComponent<PateObject>().SetCanOpenerBehavior(this);
                ghostCan.gameObject.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.transform == placedCan)
        {
            placedCan.GetComponent<PateObject>().SetCanOpenerBehavior(null);
            placedCan = null;
            ghostCan.gameObject.SetActive(false);
        }
    }
}
