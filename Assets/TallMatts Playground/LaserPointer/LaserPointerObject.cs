using UnityEngine;
using System.Collections;

public class LaserPointerObject : GrabbableObject {

    public Transform redDot;

    public override void PostUpdate()
    {
        base.PostUpdate();

        if (isGrabbed)
        {
            Vector3 up = transform.TransformDirection(Vector3.up);
            RaycastHit hit; Debug.DrawRay(transform.position, up * 2, Color.red);
            if (Physics.Raycast(transform.position, up, out hit, Mathf.Infinity))
            {
                if (!redDot.gameObject.activeSelf)
                {
                    redDot.gameObject.SetActive(true);
                }
                redDot.position = hit.point;
                redDot.rotation = Quaternion.Euler(hit.normal * 360);
                redDot.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            }
            else
            {
                if (!redDot.gameObject.activeSelf)
                {
                    redDot.gameObject.SetActive(false);
                }
            }
        }
    }

    public override void AfterOnGrab()
    {
        base.AfterOnGrab();

        redDot.gameObject.SetActive(true);
    }

    public override void AfterOnUnGrab()
    {
        base.AfterOnUnGrab();

        redDot.gameObject.SetActive(false);
    }
}
