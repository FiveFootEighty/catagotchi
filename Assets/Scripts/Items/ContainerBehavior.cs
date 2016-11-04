using UnityEngine;
using System.Collections;

public class ContainerBehavior : MonoBehaviour {

    [HideInInspector]
    public ArrayList containedObjects = new ArrayList();


    private void AddContainedObject(Transform transform)
    {
        foreach (Transform containedObject in containedObjects)
        {
            if (transform == containedObject)
            {
                return;
            }
        }
        containedObjects.Add(transform);
    }

    private void RemoveContainedObject(Transform transform)
    {
        for (int i = 0; i < containedObjects.Count; i++) {
            if (transform == (Transform)containedObjects[i])
            {
                containedObjects[i] = null;
                return;
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        AddContainedObject(collider.transform);
    }

    void OnTriggerExit(Collider collider)
    {
        RemoveContainedObject(collider.transform);
    }
}
