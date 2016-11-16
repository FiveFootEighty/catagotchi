using UnityEngine;
using System.Collections;

public class LeverBehavior : GrabbableObject
{

    public HingeJoint hinge;
    public Transform hingeLocation;

    private float angle;
    private Vector3 previousControllerPos;

    private float startAngle = 0f;

    void FixedUpdate()
    {
        if (isGrabbed && controller != null)
        {
            if (previousControllerPos == Vector3.zero)
            {
                previousControllerPos = controller.transform.position;
            }

            Vector3 inputVector = hingeLocation.position - controller.transform.position;
            Quaternion parentRotation = Quaternion.Euler(new Vector3(hinge.transform.parent.transform.rotation.eulerAngles.x, -1 * hinge.transform.parent.transform.rotation.eulerAngles.y, hinge.transform.parent.transform.rotation.eulerAngles.z));
            inputVector = parentRotation * inputVector;
            angle = Mathf.Atan2(inputVector.x, inputVector.y) * Mathf.Rad2Deg;// hingeLocation.position.z - controller.transform.position.z, hingeLocation.position.y - controller.transform.position.y) * Mathf.Rad2Deg;
            angle *= -1;
            angle += 180;
            angle -= startAngle;
            while (angle < 180)
            {
                angle += 360;
            }
            while (angle > 180)
            {
                angle -= 360;
            }
            if (angle > hinge.limits.max)
            {
                angle = (hinge.limits.max);
            }
            else if (angle < hinge.limits.min)
            {
                angle = (hinge.limits.min);
            }
            angle += 23;
            
            float diff = angle - hinge.transform.rotation.eulerAngles.z;
            // rotate bottom vector by y angle of the parent object
            Vector3 axis = hinge.transform.parent.transform.rotation * new Vector3(0, 0, -1);
            hinge.transform.RotateAround(hingeLocation.position, axis, -diff);
        }
    }

    public override void AfterOnGrab()
    {
        Quaternion parentRotation = Quaternion.Euler(new Vector3(hinge.transform.parent.transform.rotation.eulerAngles.x, -1 * hinge.transform.parent.transform.rotation.eulerAngles.y, hinge.transform.parent.transform.rotation.eulerAngles.z));
        Vector3 inputVector = hingeLocation.position - controller.transform.position;
        inputVector = parentRotation * inputVector;
        startAngle = Mathf.Atan2(inputVector.x, inputVector.y) * Mathf.Rad2Deg * -1 + 180;
    }
}
