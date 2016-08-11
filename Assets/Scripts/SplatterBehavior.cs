using UnityEngine;
using System.Collections;

public class SplatterBehavior : MonoBehaviour {

    public GameObject splatter;
    public bool hasSplattered = false;

    public void CreateSplatter(GameObject hitObject, Vector3 hitPosition, Vector3 hitRotation)
    {
        hasSplattered = true;
        Debug.Log(hitRotation + "!");
        GameObject splatterObject = (GameObject)Instantiate(splatter, hitPosition, Quaternion.Euler(hitRotation));
    }
}
