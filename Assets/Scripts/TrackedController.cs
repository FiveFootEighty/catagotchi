using UnityEngine;
using System.Collections;

public class TrackedController : MonoBehaviour {

    private const int TRACKPAD_NONE = -1;
    private const int TRACKPAD_UP = 0;
    private const int TRACKPAD_DOWN = 1;
    private const int TRACKPAD_LEFT = 2;
    private const int TRACKPAD_RIGHT = 3;
    private const int TRACKPAD_CENTER = 4;

    private const float TRACKPAD_THRESHHOLD_MAJOR = 0.7f;
    private const float TRACKPAD_THRESHHOLD_MINOR = 0.35f;

    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    private Valve.VR.EVRButtonId axis0 = Valve.VR.EVRButtonId.k_EButton_Axis0;

    public SteamVR_Controller.Device controller {  get {  return SteamVR_Controller.Input((int) trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;

    public GameObject steamVRObject;

    // Selecting objects fields
    private GameObject highlightedObject;
    private GameObject selectedObject;

    // Teleporting feature fields
    public GameObject teleportReticle;
    private bool isTeleporting = false;
    private bool validTeleportSpot = false;

	void Start () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
	}
	
	void Update () {
	    if (controller != null)
        {
            if (controller.GetPressDown(triggerButton))
            {
                    if (selectedObject == null)
                    {
                        // pick up object if one is entered
                        if (highlightedObject != null)
                        {
                            selectedObject = highlightedObject;
                            if (selectedObject.GetComponent<SelectableObject>().isHighlighted)
                            {
                                selectedObject.GetComponent<SelectableObject>().OnHighlightEnd(this);
                            }
                            if (selectedObject.GetComponent<SelectableObject>().isSelected)
                            {
                                selectedObject.GetComponent<SelectableObject>().OnSelectedEnd(this);
                            }
                            selectedObject.GetComponent<SelectableObject>().OnSelectedBegin(this);
                            highlightedObject = null;
                        }
                    }

            }
            else if (controller.GetPressUp(triggerButton))
            {
                if (selectedObject != null)
                {
                    // drop object
                    selectedObject.GetComponent<SelectableObject>().OnSelectedEnd(this);
                    selectedObject = null;
                }
            }

            //Debug.Log(GetTrackPadLocation(controller.GetAxis(axis0).x, controller.GetAxis(axis0).y));
            if (controller.GetPress(axis0) && GetTrackPadLocation(controller.GetAxis(axis0).x, controller.GetAxis(axis0).y) == TRACKPAD_DOWN)
            {
                isTeleporting = true;
                // turn on teleport feature
                if (!teleportReticle.activeSelf)
                {
                    teleportReticle.SetActive(true);
                }
                // raycast outward
                // if you hit a floor object
                // put the reticle at that point

                Vector3 forward = transform.TransformDirection(Vector3.forward);

                RaycastHit floorHit;
                Debug.DrawRay(transform.position, forward * 2, Color.green);
                validTeleportSpot = false;
                if (Physics.Raycast(transform.position, forward, out floorHit, 10))
                {
                    //Debug.Log("hit: " + hit.collider.gameObject.name);
                    if (floorHit.collider.gameObject.name == "Floor")
                    {
                        RaycastHit ceilingHit;
                        Debug.DrawRay(floorHit.point, Vector3.up * 2, Color.green);
                        if (Physics.Raycast(floorHit.point, Vector3.up, out ceilingHit, 10))
                        {
                            if (ceilingHit.collider.gameObject.name == "Ceiling")
                            {
                                validTeleportSpot = true;
                                teleportReticle.transform.position = new Vector3( floorHit.point.x, floorHit.point.y+0.01f, floorHit.point.z);

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
                    else
                    {
                        teleportReticle.SetActive(false);
                    }
                }
            }
            else
            {
                if (controller.GetPressUp(axis0) && GetTrackPadLocation(controller.GetAxis(axis0).x, controller.GetAxis(axis0).y) == TRACKPAD_DOWN)
                {
                    if (isTeleporting)
                    {
                        if (validTeleportSpot)
                        {
                            // teleport there
                            steamVRObject.transform.position = teleportReticle.transform.position;
                            isTeleporting = false;
                            teleportReticle.SetActive(false);
                        }
                    }
                }
            }
        }
        else
        {
            Debug.Log("Controller not initialized");
        }

        

        if (selectedObject != null)
        {
            //selectedObject.transform.position = transform.position;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // if you dont already have an object selected, highlight an object
        if (selectedObject == null)
        {
            if (other.tag == "SelectableObject")
            {
                // if the object isnt already selected by another controller
                if (!other.gameObject.GetComponent<SelectableObject>().isSelected)
                {
                    other.gameObject.GetComponent<SelectableObject>().OnHighlightBegin(this);
                    highlightedObject = other.gameObject;
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // if you exit an object and it's the one taht is entered, unenter it
        if (highlightedObject == other.gameObject) {
            highlightedObject.GetComponent<SelectableObject>().OnHighlightEnd(this);
            highlightedObject = null;
        }
    }

    public void ClearSelectedObject()
    {
        selectedObject = null;
    }

    private int GetTrackPadLocation(float x, float y)
    {

        if (x > TRACKPAD_THRESHHOLD_MAJOR)
        {
            if (y > -TRACKPAD_THRESHHOLD_MINOR && y < TRACKPAD_THRESHHOLD_MINOR)
            {
                return TRACKPAD_LEFT;
            }
        }
        if (x < -TRACKPAD_THRESHHOLD_MAJOR)
        {
            if (y > -TRACKPAD_THRESHHOLD_MINOR && y < TRACKPAD_THRESHHOLD_MINOR)
            {
                return TRACKPAD_RIGHT;
            }
        }
        if (y > TRACKPAD_THRESHHOLD_MAJOR)
        {
            if (x > -TRACKPAD_THRESHHOLD_MINOR && x < TRACKPAD_THRESHHOLD_MINOR)
            {
                return TRACKPAD_UP;
            }
        }
        if (y < -TRACKPAD_THRESHHOLD_MAJOR)
        {
            if (x > -TRACKPAD_THRESHHOLD_MINOR && x < TRACKPAD_THRESHHOLD_MINOR)
            {
                return TRACKPAD_DOWN;
            }
        }
        if (x > -TRACKPAD_THRESHHOLD_MINOR && x < TRACKPAD_THRESHHOLD_MINOR)
        {
            if (y > -TRACKPAD_THRESHHOLD_MINOR && y < TRACKPAD_THRESHHOLD_MINOR)
            {
                return TRACKPAD_CENTER;
            }
        }

        return TRACKPAD_NONE;
    }
}
