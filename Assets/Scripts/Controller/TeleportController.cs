using UnityEngine;
using System.Collections;

public class TeleportController : MonoBehaviour, TrackedControllerBase.TrackedControllerTrackpadListener {

    private TrackedControllerBase trackedControllerBase;

    // Teleporting feature fields
    public GameObject teleportReticle;
    private bool isTeleporting = false;
    private bool validTeleportSpot = false;

    private GrabController grabController;
    public GrabController otherGrabController;

    [HideInInspector]
    public GameObject head;
    
    void Start () {
        trackedControllerBase = GetComponentInParent<TrackedControllerBase>();
        trackedControllerBase.RegisterTrackpadListener(this);

        grabController = GetComponent<GrabController>();

        head = GameObject.Find("Camera (eye)");
    }

    public void OnTrackpadTouched(int location) { }
    public void OnTrackpadPressDown(int location) { }

    public void OnTrackpadPress(int location)
    {
        if (location == TrackedControllerBase.TRACKPAD_DOWN)
        {
            isTeleporting = true;

            if (!teleportReticle.activeSelf)
            {
                teleportReticle.SetActive(true);
            }

            Vector3 forward = transform.TransformDirection(Vector3.forward);

            RaycastHit floorHit;
            Debug.DrawRay(transform.position, forward * 2, Color.green);
            validTeleportSpot = false;
            if (Physics.Raycast(transform.position, forward, out floorHit, 10))
            {
                if (floorHit.collider.gameObject.name == "Floor")
                {
                    RaycastHit ceilingHit;
                    Debug.DrawRay(floorHit.point, Vector3.up * 2, Color.green);
                    if (!Physics.Raycast(floorHit.point, Vector3.up, out ceilingHit, 1))
                    {
                        validTeleportSpot = true;
                        teleportReticle.transform.position = new Vector3(floorHit.point.x, floorHit.point.y + 0.01f, floorHit.point.z);
                    }
                    else
                    {
                        teleportReticle.SetActive(false);
                    }
                }
                else
                {
                    teleportReticle.SetActive(false);
                }

            }
        }
    }

    public void OnTrackpadPressUp(int location)
    {
        if (location == TrackedControllerBase.TRACKPAD_DOWN && isTeleporting && validTeleportSpot)
        {
            // teleport there
            Vector3 distanceFromCenter = trackedControllerBase.steamVRObject.transform.position - head.transform.position;
            distanceFromCenter.y = 0;

           

            // god I hate this saved Parents thing so much
            
           
            

            /* ----------------- */
            trackedControllerBase.steamVRObject.transform.position = teleportReticle.transform.position + distanceFromCenter;
            /* ----------------- */


            
            isTeleporting = false;
            teleportReticle.SetActive(false);
        }
    }

    private ArrayList savedParents = new ArrayList();

    private void LockSelectedObject()
    {
        savedParents = new ArrayList();

        if (grabController.isGrabbed && grabController.selectedObject.GetComponent<ContainerBehavior>() != null)
        {
            ArrayList containedObjects = grabController.selectedObject.GetComponent<ContainerBehavior>().containedObjects;
            foreach (Transform containedObject in containedObjects)
            {
                if (containedObject != null)
                {
                    savedParents.Add(containedObject.parent);
                    containedObject.parent = trackedControllerBase.steamVRObject.transform;
                    if (containedObject.GetComponent<Rigidbody>() != null)
                    {
                        containedObject.GetComponent<Rigidbody>().isKinematic = true;
                    }
                }
            }
            savedParents.Add(grabController.selectedObject.transform.parent);
            grabController.selectedObject.transform.parent = trackedControllerBase.steamVRObject.transform;
            grabController.selectedObject.GetComponent<Rigidbody>().isKinematic = true;
        }

        if (otherGrabController.isGrabbed && otherGrabController.selectedObject.GetComponent<ContainerBehavior>() != null)
        {
            ArrayList containedObjects = otherGrabController.selectedObject.GetComponent<ContainerBehavior>().containedObjects;
            foreach (Transform containedObject in containedObjects)
            {
                if (containedObject != null)
                {
                    savedParents.Add(containedObject.parent);
                    containedObject.parent = trackedControllerBase.steamVRObject.transform;
                    if (containedObject.GetComponent<Rigidbody>() != null)
                    {
                        containedObject.GetComponent<Rigidbody>().isKinematic = true;
                    }
                }
            }
            savedParents.Add(otherGrabController.selectedObject.transform.parent);
            otherGrabController.selectedObject.transform.parent = trackedControllerBase.steamVRObject.transform;
            otherGrabController.selectedObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    private void UnlockSelectedObject()
    {
        int i = 0;
        if (grabController.isGrabbed)
        {
            if (grabController.selectedObject.GetComponent<ContainerBehavior>() != null)
            {
                ArrayList containedObjects = grabController.selectedObject.GetComponent<ContainerBehavior>().containedObjects;
                foreach (Transform containedObject in containedObjects)
                {
                    if (containedObject != null)
                    {
                        containedObject.parent = (Transform)savedParents[i++];
                        if (containedObject.GetComponent<Rigidbody>() != null)
                        {
                            containedObject.GetComponent<Rigidbody>().isKinematic = false;
                        }
                    }
                }
                grabController.selectedObject.transform.parent = (Transform)savedParents[i++];
                grabController.selectedObject.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
        if (otherGrabController.isGrabbed)
        {
            if (otherGrabController.selectedObject.GetComponent<ContainerBehavior>() != null)
            {
                ArrayList containedObjects = otherGrabController.selectedObject.GetComponent<ContainerBehavior>().containedObjects;
                foreach (Transform containedObject in containedObjects)
                {
                    if (containedObject != null)
                    {
                        containedObject.parent = (Transform)savedParents[i++];
                        if (containedObject.GetComponent<Rigidbody>() != null)
                        {
                            containedObject.GetComponent<Rigidbody>().isKinematic = false;
                        }
                    }
                }
                otherGrabController.selectedObject.transform.parent = (Transform)savedParents[i++];
                otherGrabController.selectedObject.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }
}
