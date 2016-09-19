using UnityEngine;
using System.Collections;

public class TrashCanBehaviour : MonoBehaviour {

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<SelectableObject>() != null)
        {
            Destroy(collider.gameObject);
        }
    }
}
