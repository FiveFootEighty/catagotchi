using UnityEngine;
using System.Collections;

public class TrackedController : MonoBehaviour {

    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;

    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    public SteamVR_Controller.Device controller {  get {  return SteamVR_Controller.Input((int) trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;

    GameObject highlightedObject;
    GameObject selectedObject;

	void Start () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
	}
	
	void Update () {
	    if (controller != null)
        {
            if (controller.GetPressDown(triggerButton))
            {
                if (selectedObject != null)
                {
                    // drop object
                    //selectedObject.GetComponent<Rigidbody>().useGravity = true;
                    selectedObject.GetComponent<SelectableObject>().OnSelectedEnd(this);
                    selectedObject = null;
                }
                else
                {
                    // pick up object if one is entered
                    if (highlightedObject != null)
                    {
                        selectedObject = highlightedObject;
                        if (selectedObject.GetComponent<SelectableObject>().isHighlighted)
                        {
                            selectedObject.GetComponent<SelectableObject>().OnHighlightEnd(this);
                        }
                        if (selectedObject.GetComponent<SelectableObject>().isSelected) {
                            selectedObject.GetComponent<SelectableObject>().OnSelectedEnd(this);
                        }
                        selectedObject.GetComponent<SelectableObject>().OnSelectedBegin(this);
                        //selectedObject.GetComponent<Rigidbody>().useGravity = false;
                        highlightedObject = null;
                    }
                }

            }
        } else
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
                other.gameObject.GetComponent<SelectableObject>().OnHighlightBegin(this);
                highlightedObject = other.gameObject;
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
}
