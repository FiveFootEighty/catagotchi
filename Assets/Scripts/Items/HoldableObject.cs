using UnityEngine;
using System.Collections;

public class HoldableObject : GrabbableObject
{

    protected Rigidbody rigidbody;
    
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        rigidbody.maxAngularVelocity = float.MaxValue;

        AfterStart();
    }

    public virtual void AfterStart()
    {

    }

    void FixedUpdate()
    {
        if (rigidbody != null && isGrabbed && controller != null)
        {
            float maxDistanceDelta = 10f;

            Quaternion rotationDelta;
            Vector3 positionDelta;

            float angle;
            Vector3 axis;

            rotationDelta = controller.transform.rotation * Quaternion.Inverse(interationPoint.rotation);
            positionDelta = controller.transform.position - interationPoint.position;

            rotationDelta.ToAngleAxis(out angle, out axis);

            angle = (angle > 180 ? angle -= 360 : angle);

            if (angle != 0)
            {
                Vector3 angularTarget = angle * axis;
                rigidbody.angularVelocity = Vector3.MoveTowards(rigidbody.angularVelocity, angularTarget, maxDistanceDelta);
            }

            Vector3 velocityTarget = positionDelta / Time.fixedDeltaTime;
            rigidbody.velocity = Vector3.MoveTowards(rigidbody.velocity, velocityTarget, maxDistanceDelta);
        }

        PostUpdate();
    }

    public virtual void PostUpdate()
    {

    }
}
