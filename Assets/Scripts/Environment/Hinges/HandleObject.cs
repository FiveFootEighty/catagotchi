using UnityEngine;
using System.Collections;

public class HandleObject : GrabbableObject
{

    private Rigidbody rb;

    public HingeJoint hinge;
    public Transform hingeLocation;

    private float angle;
    private Vector3 previousControllerPos;

    private bool isLocked = true;
    private float mass;
    private float angularDrag;

    private float minInputAngle = 0;
    private float maxInputAngle = 360;
    private bool isMaxWrap = false;

    private float startAngle = 0f;

    public AudioClip openSound;
    public AudioClip closeSound;
    public AudioSource audioSource;

    // if the axis is == 1
    //      add the parent angle to the limits
    // if the axis is == -1
    //      subtract the parent angle from the limits

    void Start() {
        mass = hinge.transform.GetComponent<Rigidbody>().mass;
        angularDrag = hinge.transform.GetComponent<Rigidbody>().angularDrag;
        hinge.transform.GetComponent<Rigidbody>().maxAngularVelocity = float.MaxValue;

        rb = GetComponent<Rigidbody>();

        if (hinge.axis.y == 1)
        {
            minInputAngle = hinge.limits.min + hinge.transform.parent.rotation.eulerAngles.y;
            maxInputAngle = hinge.limits.max + hinge.transform.parent.rotation.eulerAngles.y;
        } else if (hinge.axis.y == -1)
        {
            minInputAngle = hinge.limits.min - hinge.transform.parent.rotation.eulerAngles.y;
            maxInputAngle = hinge.limits.max - hinge.transform.parent.rotation.eulerAngles.y;
        }
        minInputAngle = Limit(minInputAngle, 0, 360, 360);
        maxInputAngle = Limit(maxInputAngle, 0, 360, 360);

        if (maxInputAngle < minInputAngle)
        {
            isMaxWrap = true;
        }
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
            if (hinge.axis.y == 1)
            {
                angle -= 180;
                angle *= -1;
            }
            float parentAngle = hinge.transform.parent.rotation.eulerAngles.y;
            float minMaxAngle = Limit(angle, 0, 360, 360);
            if (isMaxWrap)
            {
                // need to check if in between max and min but closer to which one
                if (minMaxAngle > maxInputAngle && minMaxAngle < minInputAngle)
                {
                    float diffMin = Mathf.Abs(minMaxAngle - minInputAngle);
                    float diffMax = Mathf.Abs(minMaxAngle - maxInputAngle);

                    if (diffMin > 180)
                    {
                        diffMin = Mathf.Abs(minInputAngle - minMaxAngle + 360);
                    }
                    if (diffMax > 180)
                    {
                        diffMax = Mathf.Abs(maxInputAngle - minMaxAngle + 360);
                    }

                    if (diffMax < diffMin)
                    {
                        // closer to max
                        angle = maxInputAngle;
                        if (!isLocked)
                        {
                            LockHinge();
                        }
                    } else
                    {
                        // closer to min
                        angle = minInputAngle;
                        if (!isLocked)
                        {
                            LockHinge();
                        }
                    }
                }
                else
                {
                    if (isLocked)
                    {
                        UnlockHinge();
                    }
                }                
            } else
            {
                // need to check if more than max
                if (minMaxAngle > maxInputAngle)
                {

                    float diffMin = Mathf.Abs(minMaxAngle - minInputAngle);
                    float diffMax = Mathf.Abs(minMaxAngle - maxInputAngle);

                    if (diffMin > 180)
                    {
                        diffMin = Mathf.Abs(minInputAngle - minMaxAngle + 360);
                    }
                    if (diffMax > 180)
                    {
                        diffMax = Mathf.Abs(maxInputAngle - minMaxAngle + 360);
                    }
                    
                    if (diffMax < diffMin)
                    {
                        // closer to max
                        angle = maxInputAngle;
                        
                        if (!isLocked)
                        {
                            LockHinge();
                        }
                    }
                    else
                    {
                        // closer to min
                        angle = minInputAngle;
                        if (!isLocked)
                        {
                            LockHinge();
                        }
                    }
                    
                } else if (minMaxAngle < minInputAngle)
                {
                    angle = minInputAngle;
                    if (!isLocked)
                    {
                        LockHinge();
                    }
                }
                else
                {
                    if (isLocked)
                    {
                        UnlockHinge();
                    }
                }
            }

            angle *= -1;
            float diff = (hinge.transform.rotation.eulerAngles.y*(hinge.axis.y  * - 1)) - angle;
            hinge.transform.RotateAround(hingeLocation.position, hinge.axis, diff);
        }
    }

    private float Limit(float value, float min, float max, float correction)
    {
        while (value < min)
        {
            value += correction;
        }
        while (value > max)
        {
            value -= correction;
        }

        return value;
    }

    public override void AfterOnGrab()
    {
        startAngle = (Mathf.Atan2(hingeLocation.position.z - controller.transform.position.z, hingeLocation.position.x - controller.transform.position.x) * Mathf.Rad2Deg);
    }

    public override void AfterOnUnGrab()
    {
        
    }

    public void LockHinge()
    {
        isLocked = true;

        mass = hinge.transform.GetComponent<Rigidbody>().mass;
        angularDrag = hinge.transform.GetComponent<Rigidbody>().angularDrag;

        hinge.transform.GetComponent<Rigidbody>().mass = 5000;
        hinge.transform.GetComponent<Rigidbody>().angularDrag = 5000;

        if (audioSource != null)
        {
            audioSource.clip = closeSound;
            audioSource.Play();
        }
    }

    public void UnlockHinge()
    {
        isLocked = false;
        hinge.transform.GetComponent<Rigidbody>().mass = mass;
        hinge.transform.GetComponent<Rigidbody>().angularDrag = angularDrag;

        if (audioSource != null)
        {
            audioSource.clip = openSound;
            audioSource.Play();
        }
    }
}
