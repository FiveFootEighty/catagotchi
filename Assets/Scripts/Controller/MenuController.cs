using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour, TrackedControllerBase.TrackedControllerGripListener
{

    private TrackedControllerBase trackedControllerBase;

    public Transform brochure;

    void Start () {
        trackedControllerBase = GetComponentInParent<TrackedControllerBase>();
        trackedControllerBase.RegisterGripListener(this);
    }

	void Update () {
	
	}

    public void OnGripPress()
    {

    }
    public void OnGripPressUp()
    {
        brochure.gameObject.SetActive(false);
    }
    public void OnGripPressDown()
    {
        brochure.gameObject.SetActive(true);
    }
}
