using UnityEngine;
using System.Collections;

public class LockBehavior : MonoBehaviour {

	void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponentInChildren<HandleObject>() != null)
        {
            collider.gameObject.GetComponentInChildren<HandleObject>().LockHinge();
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.GetComponentInChildren<HandleObject>() != null)
        {
            collider.gameObject.GetComponentInChildren<HandleObject>().UnlockHinge();
        }
    }
}
