using UnityEngine;
using System.Collections;

public class TrashCanBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<SelectableObject>() != null)
        {
            Destroy(collider.gameObject);
        }
    }
}
