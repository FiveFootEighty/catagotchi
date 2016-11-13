using UnityEngine;
using System.Collections;

public class SoundEffectRegistry : MonoBehaviour {

    public AudioClip plasticTap;
    public AudioClip canTap;
    public AudioClip poopSplat;
    public AudioClip pateSplat;

    public AudioClip sandMoved;

    private static SoundEffectRegistry soundEffectRegistry;

    public static SoundEffectRegistry instance()
    {
        if (soundEffectRegistry == null)
        {
            // This is where the magic happens.
            //  FindObjectOfType(...) returns the first AManager object in the scene.
            soundEffectRegistry = FindObjectOfType(typeof(SoundEffectRegistry)) as SoundEffectRegistry;
        }

        // If it is still null, create a new instance
        if (soundEffectRegistry == null)
        {
            GameObject obj = new GameObject("SoundEffectRegistry");
            soundEffectRegistry = obj.AddComponent(typeof(SoundEffectRegistry)) as SoundEffectRegistry;
        }

        return soundEffectRegistry;
    }
}
