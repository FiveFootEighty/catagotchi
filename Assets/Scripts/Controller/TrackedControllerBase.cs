using UnityEngine;
using System.Collections;

public class TrackedControllerBase : MonoBehaviour {

    /*
     * To implement the TrackedControllerBase add this script to the Controller gameobject. Scripts to implement new controls
     * dont need to impement or extend this class but need to be placed on the same Controller game object and need to get
     * a reference to this class. In the new control class implement a listener class from this file, override the methods,
     * and add the new control script with the RegisterListener methods. Then, whenever a button action is fired from the physical
     * device, this base class will listen for it then notify all the appropriate listeners that have been registered. 
     */
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
    [HideInInspector]
    public GameObject steamVRObject;

    /// <summary>
    /// Listener interface for the Vive controller trigger button.
    /// 
    /// Implement OnTriggerPress, OnTriggerPressUp, and OnTriggerPressDown and
    /// register the listener with RegisterTriggerListener to receive callbacks.
    /// 
    /// To stop receiving callbacks UnregisterTriggerListener
    /// </summary>
    public interface TrackedControllerTriggerListener
    {
        void OnTriggerPress();
        void OnTriggerPressUp();
        void OnTriggerPressDown();
    }
    private ArrayList triggerListeners = new ArrayList();

    /// <summary>
    /// Listener interface for the Vive controller grip button.
    /// 
    /// Implement OnGripPress, OnGripPressUp, and OnGripPressDown and
    /// register the listener with RegisterGripListener to receive callbacks.
    /// 
    /// To stop receiving callbacks UnregisterGripListener
    /// </summary>
    public interface TrackedControllerGripListener
    {
        void OnGripPress();
        void OnGripPressUp();
        void OnGripPressDown();
    }
    private ArrayList gripListeners = new ArrayList();

    /// <summary>
    /// Listener interface for the Vive controller menu button.
    /// 
    /// Implement OnMenuPress, OnMenuPressUp, and OnMenuPressDown and
    /// register the listener with RegisterMenuListener to receive callbacks.
    /// 
    /// To stop receiving callbacks UnregisterMenuListener
    /// </summary>
    public interface TrackedControllerMenuListener
    {
        void OnMenuPress();
        void OnMenuPressUp();
        void OnMenuPressDown();
    }
    private ArrayList menuListeners = new ArrayList();

    /// <summary>
    /// Listener interface for the Vive controller trackpad button.
    /// 
    /// Implement OnTrackpadPress, OnTrackpadPressUp, and OnTrackpadPressDown and
    /// register the listener with RegisterTrackpadListener to receive callbacks.
    /// 
    /// To stop receiving callbacks UnregisterTrackpadListener
    /// </summary>
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

    
    /// <summary>
    /// Cause a haptic feedback pulse for x milliseconds on this controller.
    /// </summary>
    /// <param name="milliseconds"></param>
    public void Pulse(ushort milliseconds)
    {
        controller.TriggerHapticPulse(milliseconds);
    }




    /*
     *  Trigger callbacks
     */
     /// <summary>
     /// Register a trigger listener to listen for Vive controller trigger button events.
     /// </summary>
     /// <param name="listener"></param>
    public void RegisterTriggerListener(TrackedControllerTriggerListener listener)
    {
        triggerListeners.Add(listener);
    }
    /// <summary>
    /// Unregsiter a trigger listener. The listener parameter will no longer receive Vive controller trigger button events.
    /// </summary>
    /// <param name="listener"></param>
    public void UnregisterTriggerListener(TrackedControllerTriggerListener listener)
    {
        triggerListeners.Remove(listener);
    }




    /*
     *  Grip callbacks
     */
    /// <summary>
    /// Register a grip listener to listen for Vive controller grip button events.
    /// </summary>
    /// <param name="listener"></param>
    public void RegisterGripListener(TrackedControllerGripListener listener)
    {
        gripListeners.Add(listener);
    }
    /// <summary>
    /// Unregsiter a grip listener. The listener parameter will no longer receive Vive controller grip button events.
    /// </summary>
    /// <param name="listener"></param>
    public void UnregisterGripListener(TrackedControllerGripListener listener)
    {
        gripListeners.Remove(listener);
    }




    /*
     *  Menu callbacks
     */
    /// <summary>
    /// Register a menu listener to listen for Vive controller menu button events.
    /// </summary>
    /// <param name="listener"></param>
    public void RegisterMenuListener(TrackedControllerMenuListener listener)
    {
        menuListeners.Add(listener);
    }
    /// <summary>
    /// Unregsiter a menu listener. The listener parameter will no longer receive Vive controller menu button events.
    /// </summary>
    /// <param name="listener"></param>
    public void UnregisterMenuListener(TrackedControllerMenuListener listener)
    {
        menuListeners.Remove(listener);
    }




    /*
     *  Trackpad callbacks
     */
    /// <summary>
    /// Register a trackpad listener to listen for Vive controller trackpad button events.
    /// </summary>
    /// <param name="listener"></param>
    public void RegisterTrackpadListener(TrackedControllerTrackpadListener listener)
    {
        trackpadListeners.Add(listener);
    }
    /// <summary>
    /// Unregsiter a trackpad listener. The listener parameter will no longer receive Vive controller trackpad button events.
    /// </summary>
    /// <param name="listener"></param>
    public void UnregisterTrackpadListener(TrackedControllerTrackpadListener listener)
    {
        trackpadListeners.Remove(listener);
    }



    /*
     * Get the string literal for the given location. This is primaly for debugging the trackpad code.
     */
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

    /*
     * Get the int constant variable for the given vector2 position, likely taken from the tracked controller trackpad.
     */
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
