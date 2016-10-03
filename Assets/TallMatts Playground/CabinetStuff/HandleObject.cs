using UnityEngine;
using System.Collections;

public class HandleObject : GrabbableObject
{

    public HingeJoint hinge;
    public Transform hingeLocation;

    private float angle;
    private Vector3 previousControllerPos;

    private bool isLocked = false;
    private float mass;

    void Start () {
        mass = hinge.transform.GetComponent<Rigidbody>().mass;
    }
    
    void Update()
    {

        if (isGrabbed && controller != null)
        {
            if (previousControllerPos == Vector3.zero)
            {
                previousControllerPos = controller.transform.position;
            }

            angle = Mathf.Atan2(hingeLocation.position.z - controller.transform.position.z, hingeLocation.position.x - controller.transform.position.x) * Mathf.Rad2Deg;
            angle -= 90;

            while(angle < 180)
            {
                angle += 360;
            }
            while (angle > 180)
            {
                angle -= 360;
            }

            if (angle > hinge.limits.max - hinge.transform.parent.rotation.eulerAngles.y)
            {
                angle = (hinge.limits.max - hinge.transform.parent.rotation.eulerAngles.y);
                LockHinge();
            }
            else if (angle < hinge.limits.min - hinge.transform.parent.rotation.eulerAngles.y)
            {
                angle = (hinge.limits.min - hinge.transform.parent.rotation.eulerAngles.y);
                LockHinge();
            }
            else
            {
                UnlockHinge();
            }
            
            angle *= -1;

            float diff = hinge.transform.rotation.eulerAngles.y - angle;

            hinge.transform.RotateAround(hingeLocation.position, hinge.axis, diff);
        }
    }

    private void LockHinge()
    {
        isLocked = true;
        hinge.transform.GetComponent<Rigidbody>().mass = 5000;
    }

    private void UnlockHinge()
    {
        isLocked = false;
        hinge.transform.GetComponent<Rigidbody>().mass = mass;
    }
}
