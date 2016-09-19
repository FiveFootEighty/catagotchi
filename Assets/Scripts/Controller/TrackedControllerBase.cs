using UnityEngine;
using System.Collections;

public class TrackedControllerBase : MonoBehaviour {

    public const int TRACKPAD_NONE = -1;
    public const int TRACKPAD_CENTER = 0;
    public const int TRACKPAD_RIGHT = 1;
    public const int TRACKPAD_UP_RIGHT = 2;
    public const int TRACKPAD_UP = 3;
    public const int TRACKPAD_UP_LEFT = 4;
    public const int TRACKPAD_LEFT = 5;
    public const int TRACKPAD_DOWN_LEFT = 6;
    public const int TRACKPAD_DOWN = 7;
    public const int TRACKPAD_DOWN_RIGHT = 8;

    private const float TRACKPAD_THRESHHOLD_RADIUS = 0.35f;

    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    private Valve.VR.EVRButtonId menuButton = Valve.VR.EVRButtonId.k_EButton_ApplicationMenu;
    private Valve.VR.EVRButtonId axis0 = Valve.VR.EVRButtonId.k_EButton_Axis0;

    [HideInInspector]
    public SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    [HideInInspector]
    public SteamVR_TrackedObject trackedObj;

    public GameObject steamVRObject;

    public interface TrackedControllerTriggerListener
    {
        void OnTriggerPress();
        void OnTriggerPressUp();
        void OnTriggerPressDown();
    }
    private ArrayList triggerListeners = new ArrayList();

    public interface TrackedControllerGripListener
    {
        void OnGripPress();
        void OnGripPressUp();
        void OnGripPressDown();
    }
    private ArrayList gripListeners = new ArrayList();

    public interface TrackedControllerMenuListener
    {
        void OnMenuPress();
        void OnMenuPressUp();
        void OnMenuPressDown();
    }
    private ArrayList menuListeners = new ArrayList();

    public interface TrackedControllerTrackpadListener
    {
        void OnTrackpadTouched(int location);
        void OnTrackpadPress(int location);
        void OnTrackpadPressUp(int location);
        void OnTrackpadPressDown(int location);
    }
    private ArrayList trackpadListeners = new ArrayList();

    void Start() {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        steamVRObject = GameObject.Find("[CameraRig]");
    }

    // Update is called once per frame
    void Update () {

        if (controller != null)
        {
            /*
             *  Trigger
             */
            if (controller.GetPress(triggerButton))
            {
                foreach (TrackedControllerTriggerListener listener in triggerListeners)
                {
                    listener.OnTriggerPress();
                }
            }
            if (controller.GetPressDown(triggerButton))
            {
                foreach (TrackedControllerTriggerListener listener in triggerListeners)
                {
                    listener.OnTriggerPressDown();
                }
            }
            if (controller.GetPressUp(triggerButton))
            {
                foreach (TrackedControllerTriggerListener listener in triggerListeners)
                {
                    listener.OnTriggerPressUp();
                }
            }

            /*
             *  Grip
             */
            if (controller.GetPress(gripButton))
            {
                foreach (TrackedControllerGripListener listener in gripListeners)
                {
                    listener.OnGripPress();
                }
            }
            if (controller.GetPressDown(gripButton))
            {
                foreach (TrackedControllerGripListener listener in gripListeners)
                {
                    listener.OnGripPressDown();
                }
            }
            if (controller.GetPressUp(gripButton))
            {
                foreach (TrackedControllerGripListener listener in gripListeners)
                {
                    listener.OnGripPressUp();
                }
            }

            /*
             *  Menu
             */
            if (controller.GetPress(menuButton))
            {
                foreach (TrackedControllerMenuListener listener in menuListeners)
                {
                    listener.OnMenuPress();
                }
            }
            if (controller.GetPressDown(menuButton))
            {
                foreach (TrackedControllerMenuListener listener in menuListeners)
                {
                    listener.OnMenuPressDown();
                }
            }
            if (controller.GetPressUp(menuButton))
            {
                foreach (TrackedControllerMenuListener listener in menuListeners)
                {
                    listener.OnMenuPressUp();
                }
            }

            /*
             *  Trackpad
             */
            if (GetTrackPadLocation(controller.GetAxis(axis0)) != TRACKPAD_NONE)
            {
                foreach (TrackedControllerTrackpadListener listener in trackpadListeners)
                {
                    listener.OnTrackpadTouched(GetTrackPadLocation(controller.GetAxis(axis0)));
                }
            }
            if (controller.GetPress(axis0)) {
                foreach (TrackedControllerTrackpadListener listener in trackpadListeners)
                {
                    listener.OnTrackpadPress(GetTrackPadLocation(controller.GetAxis(axis0)));
                }
            }
            if (controller.GetPressDown(axis0))
            {
                foreach (TrackedControllerTrackpadListener listener in trackpadListeners)
                {
                    listener.OnTrackpadPressDown(GetTrackPadLocation(controller.GetAxis(axis0)));
                }
            }
            if (controller.GetPressUp(axis0))
            {
                foreach (TrackedControllerTrackpadListener listener in trackpadListeners)
                {
                    listener.OnTrackpadPressUp(GetTrackPadLocation(controller.GetAxis(axis0)));
                }
            }
        }
    }

    

    public void Pulse(ushort milliseconds)
    {
        controller.TriggerHapticPulse(milliseconds);
    }

    /*
     *  Trigger callbacks
     */
    public void RegisterTriggerListener(TrackedControllerTriggerListener listener)
    {
        triggerListeners.Add(listener);
    }

    public void UnregisterTriggerListener(TrackedControllerTriggerListener listener)
    {
        triggerListeners.Remove(listener);
    }

    /*
     *  Grip callbacks
     */
    public void RegisterGripListener(TrackedControllerGripListener listener)
    {
        gripListeners.Add(listener);
    }

    public void UnregisterGripListener(TrackedControllerGripListener listener)
    {
        gripListeners.Remove(listener);
    }

    /*
     *  Menu callbacks
     */
    public void RegisterMenuListener(TrackedControllerMenuListener listener)
    {
        menuListeners.Add(listener);
    }

    public void UnregisterMenuListener(TrackedControllerMenuListener listener)
    {
        menuListeners.Remove(listener);
    }

    /*
     *  Track Pad callbacks
     */
    public void RegisterTrackpadListener(TrackedControllerTrackpadListener listener)
    {
        trackpadListeners.Add(listener);
    }

    public void UnregisterTrackpadListener(TrackedControllerTrackpadListener listener)
    {
        trackpadListeners.Remove(listener);
    }

    protected string GetTrackPadLocationName(int location)
    {
        switch (location)
        {
            case TRACKPAD_CENTER:
                return "TRACKPAD_CENTER";
            case TRACKPAD_UP:
                return "TRACKPAD_UP";
            case TRACKPAD_DOWN:
                return "TRACKPAD_DOWN";
            case TRACKPAD_LEFT:
                return "TRACKPAD_LEFT";
            case TRACKPAD_RIGHT:
                return "TRACKPAD_RIGHT";
            case TRACKPAD_UP_LEFT:
                return "TRACKPAD_UP_LEFT";
            case TRACKPAD_UP_RIGHT:
                return "TRACKPAD_UP_RIGHT";
            case TRACKPAD_DOWN_LEFT:
                return "TRACKPAD_DOWN_LEFT";
            case TRACKPAD_DOWN_RIGHT:
                return "TRACKPAD_DOWN_RIGHT";
        }
        return "TRACKPAD_NONE";
    }

    private int GetTrackPadLocation(Vector2 position)
    {
        if (position != null)
        {
            if (position.magnitude > TRACKPAD_THRESHHOLD_RADIUS)
            {
                float angle = Mathf.Rad2Deg * Mathf.Atan2(position.y, position.x);
                float quadrant = angle / 45f;   
                
                if (quadrant <= 0.5f && quadrant >= -0.5)
                {
                    // right
                    return TRACKPAD_RIGHT;
                }
                else if (quadrant <= 1.5f && quadrant >= 0.5)
                {
                    // up right
                    return TRACKPAD_UP_RIGHT;
                }
                else if (quadrant <= 2.5f && quadrant >= 1.5)
                {
                    // up
                    return TRACKPAD_UP;
                }
                else if (quadrant <= 3.5f && quadrant >= 2.5)
                {
                    // up left
                    return TRACKPAD_UP_LEFT;
                }
                else if (quadrant >= 3.5f || quadrant <= -3.5)
                {
                    // left
                    return TRACKPAD_LEFT;
                }
                else if (quadrant >= -3.5f && quadrant <= -2.5)
                {
                    // down left
                    return TRACKPAD_DOWN_LEFT;
                }
                else if (quadrant >= -2.5f && quadrant <= -1.5)
                {
                    // down
                    return TRACKPAD_DOWN;
                }
                else if (quadrant >= -1.5f && quadrant <= -0.5)
                {
                    // down right
                    return TRACKPAD_DOWN_RIGHT;
                }

            }
            else
            {
                if (position.magnitude == 0)
                {
                    return TRACKPAD_NONE;
                } else
                {
                    return TRACKPAD_CENTER;
                }
            }
        }

        return TRACKPAD_NONE;
    }


}
