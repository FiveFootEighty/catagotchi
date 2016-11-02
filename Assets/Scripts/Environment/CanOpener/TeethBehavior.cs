using UnityEngine;
using System.Collections;

public class TeethBehavior : MonoBehaviour {

    public Transform lever;
    public float topY;
    public float bottomY;

    public float topAngle;
    public float bottomAngle;

    private float m = 0;
    private float b = 0;
    public CanOpenerBehavior canOpener;

	void Start () {
        // y = mx + b
        // topY = m(topAngle) + b
        // b = topY - m (topAngle)
        // b = bottomY - m (bottomAngle)
        // topY - m (topAngle) = bottomY - m (bottomAngle)
        // topY - bottomY = m (topAngle) - m (bottomAngle)
        // topY - bottomY = m (topAngle - bottomAngle)
        m = (topY - bottomY) / (topAngle - bottomAngle);
        b = topY - m * topAngle;

        canOpener = transform.parent.GetComponentInChildren<CanOpenerBehavior>();
    }
    
    void Update () {
        float newY = m * lever.rotation.eulerAngles.z + b;
        if (newY < topY && newY > bottomY)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
        }
	}

    void OnTriggerEnter(Collider collider)
    {
        
        if (collider.transform == canOpener.placedCan && transform.localPosition.y < (bottomY + 0.25f * (topY - bottomY)))
        {
            if (canOpener.placedCan.Find("Top") != null)
            {
                Destroy(canOpener.placedCan.Find("Top").gameObject);
                canOpener.placedCan.Find("Pate").gameObject.SetActive(true);
                canOpener.placedCan.GetComponent<CanObject>().opened = true;

                canOpener.OnCanPuntured();
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.transform == canOpener.placedCan && canOpener.placedCan.Find("Top") == null)
        {
            canOpener.OnCanOpened();
        }
    }
}
