using UnityEngine;
using System.Collections;

public class TapSound : MonoBehaviour {

    private AudioSource source;
    public AudioClip[] sounds;

    private int previousSound = -1;
    private int previousPreviousSound = -1;

    public float minThreshold;
    public float volumeCieling;

    // Use this for initialization
    void Start () {
        source = GetComponent<AudioSource>();
    }

    public void MakeSound(float intensity)
    {
        if (intensity < minThreshold)
        {
            return;
        }
        if (intensity > volumeCieling)
        {
            intensity = volumeCieling;
        }
        int sound = previousSound;
        while (sound == previousSound || sound == previousPreviousSound)
        {
            sound = Random.Range(0, sounds.Length);
        }
        previousPreviousSound = previousSound;
        previousSound = sound;

        source.clip = sounds[sound];
        source.volume = intensity;
        source.Play();
    }
}
