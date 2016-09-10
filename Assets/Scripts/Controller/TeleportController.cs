using UnityEngine;
using System.Collections;

public class TeleportController : MonoBehaviour, TrackedControllerBase.TrackedControllerTrackpadListener {

    private TrackedControllerBase trackedControllerBase;

    // Teleporting feature fields
    public GameObject teleportReticle;
    private bool isTeleporting = false;
    private bool validTeleportSpot = false;

    // Use this for initialization
    void Start () {
        trackedControllerBase = GetComponentInParent<TrackedControllerBase>();
        trackedControllerBase.RegisterTrackpadListener(this);
    }
	
	// Update is called once per frame
	void Update () {

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
            //teleport there
            trackedControllerBase.steamVRObject.transform.position = teleportReticle.transform.position;
            isTeleporting = false;
            teleportReticle.SetActive(false);
        }
    }
}
